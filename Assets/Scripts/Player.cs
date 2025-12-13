using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int currentEnergy = 100;
    [SerializeField] private int score = 0;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f; // units per second
    [SerializeField] private bool allowDiagonal = true;
    [SerializeField] private float inputDeadZone = 0.05f;
    
    [Header("Safety Settings")]
    [SerializeField] private Vector2 spawnPosition = new Vector2(0, 0);
    [SerializeField] private float fallThreshold = -5f;
    
    [Header("Debug")]
    [SerializeField] private bool showCollisionDebug = false;
    [SerializeField] private bool showMovementDebug = false;

    [Header("UI")]
    [SerializeField] private GameObject questionsCanvas; // Canvas_Questions to show when touching chest
    [SerializeField] private string chestTag = "Chest";
    [SerializeField] private UnityEngine.UI.Slider energyBar; // thanh năng lượng hiển thị
    [SerializeField] private UnityEngine.UI.Image energyFillImage; // Image của Fill để đổi màu động
    [SerializeField] private GameObject gameOverPanel; // Panel Game Over
    [SerializeField] private GameObject trapWarningPanel; // Panel thông báo trap
    [SerializeField] private float trapWarningDuration = 2f; // Thời gian hiển thị cảnh báo trap
    [SerializeField] private GameObject successRewardPanel; // Panel chúc mừng thu thập rương
    [SerializeField] private float successRewardDuration = 2.5f; // Thời gian hiển thị chúc mừng
    [SerializeField] private GameObject wrongAnswerPanel; // Panel thông báo trả lời sai
    [SerializeField] private float wrongAnswerDuration = 2f; // Thời gian hiển thị thông báo sai
    [Header("End Game")]
    [SerializeField] private Transform statueTarget; // Vị trí tượng để dịch chuyển tới khi hoàn thành
    [SerializeField] private GameObject winPanel; // Panel chiến thắng
    [SerializeField] private GameObject retryPanel; // Panel yêu cầu chơi lại
    [SerializeField] private int winEnergyThreshold = 70; // Ngưỡng năng lượng để thắng
    [SerializeField] private string statueTag = "Statue"; // Tag của tượng để kích hoạt kết thúc khi chạm
    [SerializeField] private GameObject notEnoughPanel; // Panel cảnh báo chưa đủ rương
    [SerializeField] private float notEnoughDuration = 2f; // thời gian hiển thị cảnh báo

    // Theo dõi tiến độ thu thập rương câu hỏi
    private int totalQuestionChests = 0;
    private int collectedQuestionChests = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private float moveInputX;
    private float moveInputY;
    private Vector2 movement;
    private bool facingRight = true;
    private float debugTimer = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Setup Rigidbody2D trong Awake để chạy trước mọi thứ
        if (rb != null)
        {
            // Tắt Interpolation tạm thời để tránh smooth từ (0,0) khi khởi tạo
            rb.interpolation = RigidbodyInterpolation2D.None;
            
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            // Đồng bộ Rigidbody2D position với Transform ngay lập tức
            rb.position = transform.position;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Lưu vị trí spawn ban đầu SAU KHI sync Rigidbody
        spawnPosition = transform.position;
        
        // Bật lại Interpolation sau 1 frame để tránh smooth từ vị trí cũ
        if (rb != null)
        {
            StartCoroutine(EnableInterpolationAfterFrame());
        }
        else
        {
            Debug.LogError("✗ Player thiếu Rigidbody2D component!");
        }
        
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("✗ Player thiếu Collider2D component!");
        }
        
        if (animator == null)
        {
            Debug.LogWarning("⚠ Chưa có Animator component trên Player!");
        }

        // Khởi tạo thanh năng lượng UI nếu có
        if (energyBar != null)
        {
            energyBar.minValue = 0;
            energyBar.maxValue = maxEnergy;
            energyBar.value = currentEnergy;
            // Nếu chưa gán thủ công, cố tìm Image Fill từ slider
            if (energyFillImage == null && energyBar.fillRect != null)
            {
                energyFillImage = energyBar.fillRect.GetComponent<UnityEngine.UI.Image>();
            }
            UpdateEnergyColor();
        }

        // Đếm tổng số rương câu hỏi có thể tương tác (không tính rương trap)
        var allChests = Object.FindObjectsOfType<ChestQuestion>(true);
        totalQuestionChests = 0;
        foreach (var chest in allChests)
        {
            if (chest != null && chest.gameObject.activeInHierarchy)
            {
                // Chỉ tính những chest có thể tương tác
                var col2d = chest.GetComponent<Collider2D>();
                if (chest.chestType == ChestQuestion.ChestType.Question && chest.canInteract && (col2d == null || col2d.enabled))
                {
                    totalQuestionChests++;
                    Debug.Log($"Đếm chest: {chest.gameObject.name} (type={chest.chestType}, colliderEnabled={(col2d==null?"none":col2d.enabled.ToString())})");
                }
            }
        }
        Debug.Log($"Tổng số rương cần thu thập: {totalQuestionChests}");
    }

    void Update()
    {
        if (rb != null && rb.gravityScale != 0f)
        {
            rb.gravityScale = 0f;
        }
        
        // Bỏ kiểm tra rơi khỏi map để không teleport hoặc dừng game
        
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveInputX, moveInputY);
        if (!allowDiagonal)
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) movement.y = 0f; else movement.x = 0f;
        }
        movement = (movement.magnitude < inputDeadZone) ? Vector2.zero : movement.normalized;

        // Tắt debug movement khi chơi

        if (moveInputX > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInputX < 0 && facingRight)
        {
            Flip();
        }
        
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Di chuyển top-down mượt mà, không bị kẹt khi va chạm
            Vector2 targetPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
            // Giữ velocity = 0 để tránh các lực dư ảnh hưởng
            rb.velocity = Vector2.zero;
        }
    }
    
    void UpdateAnimation()
    {
        if (animator != null)
        {
            bool isMoving = movement.magnitude > 0.01f;
            animator.SetBool("IsRun", isMoving);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (showCollisionDebug)
        {
            Debug.Log($"✓ Player va chạm: {collision.gameObject.name}");
        }

        // Show question canvas when colliding with chest
        if (collision.gameObject.CompareTag(chestTag))
        {
            ShowQuestionsCanvas();
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        // Có thể thêm xử lý tùy ý khi đứng sát vật cản
        if (showCollisionDebug)
        {
            Debug.Log($"[Stay] {collision.gameObject.name} normal count={collision.contactCount}");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(chestTag))
        {
            ShowQuestionsCanvas();
        }
        // Nếu chạm tượng, kiểm tra điều kiện kết thúc
        if (other.CompareTag(statueTag))
        {
            Debug.Log($"Chạm tượng. collected={collectedQuestionChests}, total={totalQuestionChests}");
            if (totalQuestionChests > 0 && collectedQuestionChests >= totalQuestionChests)
            {
                HandleAllChestsCollected();
            }
            else
            {
                Debug.Log("Chưa thu thập hết rương có câu hỏi, chưa thể kết thúc.");
                if (notEnoughPanel != null)
                {
                    StartCoroutine(ShowNotEnoughWarning());
                }
            }
        }
    }

    // Hiển thị panel cảnh báo chưa đủ rương trong thời gian ngắn
    private IEnumerator ShowNotEnoughWarning()
    {
        notEnoughPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(notEnoughDuration);
        notEnoughPanel.SetActive(false);
    }

    private void ShowQuestionsCanvas()
    {
        if (questionsCanvas != null)
        {
            questionsCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Questions canvas chưa được gán trên Player");
        }
    }

    // API: Gọi từ ChestQuestion để trừ năng lượng hoặc cộng điểm
    public void ApplyDamage(int amount, bool isTrap = false)
    {
        currentEnergy = Mathf.Max(0, currentEnergy - amount);
        if (energyBar != null) energyBar.value = currentEnergy;
        UpdateEnergyColor();
        Debug.Log($"Player mất {amount} năng lượng. Còn lại: {currentEnergy}");
        
        // Hiển thị cảnh báo nếu là trap
        if (isTrap && trapWarningPanel != null)
        {
            StartCoroutine(ShowTrapWarning());
        }
        
        // Kiểm tra nếu năng lượng = 0 thì dừng game
        if (currentEnergy <= 0)
        {
            GameOver();
        }
    }
    
    // Hiển thị cảnh báo trap trong thời gian ngắn
    private IEnumerator ShowTrapWarning()
    {
        trapWarningPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(trapWarningDuration);
        trapWarningPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Player +{amount} điểm. Tổng điểm: {score}");
        // TODO: cập nhật UI điểm nếu có
    }
    
    // API: Gọi từ ChestQuestion để cộng năng lượng
    public void RestoreEnergy(int amount)
    {
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + amount);
        if (energyBar != null) energyBar.value = currentEnergy;
        UpdateEnergyColor();
        Debug.Log($"Player hồi {amount} năng lượng. Hiện tại: {currentEnergy}/{maxEnergy}");
    }
    
    // API: Hiển thị thông báo thu thập rương thành công
    public void ShowSuccessReward()
    {
        if (successRewardPanel != null)
        {
            StartCoroutine(ShowSuccessRewardCoroutine());
        }
    }
    
    // Hiển thị chúc mừng trong thời gian ngắn
    private IEnumerator ShowSuccessRewardCoroutine()
    {
        successRewardPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(successRewardDuration);
        successRewardPanel.SetActive(false);
    }
    
    // API: Hiển thị thông báo trả lời sai
    public void ShowWrongAnswer()
    {
        if (isGameOver || currentEnergy <= 0) return; // Không hiển thị khi đã Game Over
        if (wrongAnswerPanel != null)
        {
            StartCoroutine(ShowWrongAnswerCoroutine());
        }
    }
    
    // Hiển thị thông báo sai trong thời gian ngắn
    private IEnumerator ShowWrongAnswerCoroutine()
    {
        wrongAnswerPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(wrongAnswerDuration);
        wrongAnswerPanel.SetActive(false);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPosition, 0.5f);
        Gizmos.DrawLine(spawnPosition + Vector2.left * 0.3f, spawnPosition + Vector2.right * 0.3f);
        Gizmos.DrawLine(spawnPosition + Vector2.up * 0.3f, spawnPosition + Vector2.down * 0.3f);
        
        // Vẽ velocity direction
        if (rb != null && Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + rb.velocity);
        }
    }

    // Đổi màu thanh năng lượng theo ngưỡng
    private void UpdateEnergyColor()
    {
        if (energyFillImage == null) return;
        float pct = (float)currentEnergy / Mathf.Max(1, maxEnergy);
        if (pct <= 0.5f)
        {
            energyFillImage.color = Color.red;
        }
        else
        {
            energyFillImage.color = Color.green;
        }
    }
    
    // Coroutine bật Interpolation sau 1 frame
    private IEnumerator EnableInterpolationAfterFrame()
    {
        yield return new WaitForFixedUpdate(); // Chờ 1 physics frame
        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    // Gọi từ ChestQuestion khi trả lời đúng và rương biến mất
    public void OnChestCollected()
    {
        collectedQuestionChests++;
        // Không kết thúc ngay lập tức. Chỉ kết thúc khi người chơi chạm tượng.
        Debug.Log($"Đã thu thập: {collectedQuestionChests}/{totalQuestionChests}");
    }

    private void HandleAllChestsCollected()
    {
        Debug.Log("✓ Đã thu thập đủ rương. Xử lý kết thúc...");
        // Di chuyển người chơi tới tượng nếu có cấu hình
        if (statueTarget != null)
        {
            if (rb != null)
            {
                rb.position = statueTarget.position;
                rb.velocity = Vector2.zero;
            }
            transform.position = statueTarget.position;
        }

        // Hiển thị kết quả dựa vào năng lượng
        bool isWin = currentEnergy >= winEnergyThreshold;
        if (winPanel == null && retryPanel == null)
        {
            Debug.LogError("winPanel và retryPanel chưa được gán trong Inspector!");
        }
        if (winPanel != null)
        {
            winPanel.SetActive(isWin);
            Debug.Log($"Hiển thị winPanel = {isWin}");
        }
        if (retryPanel != null)
        {
            retryPanel.SetActive(!isWin);
            Debug.Log($"Hiển thị retryPanel = {!isWin}");
        }

        // Tắt các panel khác
        if (questionsCanvas != null) questionsCanvas.SetActive(false);
        if (trapWarningPanel != null) trapWarningPanel.SetActive(false);
        if (successRewardPanel != null) successRewardPanel.SetActive(false);
        if (wrongAnswerPanel != null) wrongAnswerPanel.SetActive(false);

        StopAllCoroutines();
        Time.timeScale = 0; // Dừng game khi kết thúc
    }
    
    // Game Over khi hết năng lượng
    private void GameOver()
    {
        Debug.Log("=== GAME OVER ===");
        isGameOver = true;
        
        // Tắt tất cả các panel thông báo khác
        if (questionsCanvas != null) questionsCanvas.SetActive(false);
        if (trapWarningPanel != null) trapWarningPanel.SetActive(false);
        if (successRewardPanel != null) successRewardPanel.SetActive(false);
        if (wrongAnswerPanel != null) wrongAnswerPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (retryPanel != null) retryPanel.SetActive(false);
        if (notEnoughPanel != null) notEnoughPanel.SetActive(false);
        
        // Dừng tất cả Coroutine đang chạy để ngăn panel tự bật lại
        StopAllCoroutines();
        
        // Hiển thị panel Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        Time.timeScale = 0; // Dừng game
    }
    
    // Gọi từ nút Restart
    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1; // Bật lại thời gian
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}
