using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestQuestion : MonoBehaviour
{
    public enum ChestType { Question, Trap }

    [Header("Question Data")]
    public string question = "Câu hỏi của bạn?";
    public string[] answers = new string[4];
    public int correctIndex = 0; // Index của đáp án đúng (0-3)
    public int scoreOnCorrect = 10; // điểm thưởng khi trả lời đúng
    public int energyRewardOnCorrect = 15; // năng lượng được cộng khi trả lời đúng
    public int damageOnWrongAnswer = 10; // năng lượng bị trừ khi trả lời sai
    
    
    [Header("Rewards")]
    public GameObject rewardPrefab; // Phần thưởng khi trả lời đúng
    public Transform spawnPoint; // Vị trí spawn phần thưởng
    
    [Header("Settings")]
    public ChestType chestType = ChestType.Question;
    public bool canInteract = true;
    public bool useTrigger = false; // Dùng trigger thay vì distance check
    public float interactionRange = 2f;
    public int damageOnTrap = 20; // lượng năng lượng bị trừ nếu là trap
    
    private bool hasBeenOpened = false;
    private GameObject player;
    private bool playerInRange = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Không tìm thấy Player! Hãy gán tag 'Player' cho Player GameObject.");
        }
        
        // Kiểm tra Collider setup
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && useTrigger)
        {
            if (!col.isTrigger)
            {
                Debug.LogWarning($"[{gameObject.name}] Use Trigger = true nhưng Collider không phải Trigger! Đang set...");
                col.isTrigger = true;
            }
        }
    }

    void Update()
    {
        // Kiểm tra player có ở gần và nhấn phím tương tác không
        if (canInteract && !hasBeenOpened && player != null)
        {
            if (!useTrigger)
            {
                // Distance-based interaction (cũ)
                float distance = Vector2.Distance(transform.position, player.transform.position);
                playerInRange = distance <= interactionRange;
            }
            // Nếu dùng trigger, playerInRange được set trong OnTriggerEnter/Exit
            
            
            if (playerInRange)
            {
                // Nhấn E hoặc Enter để mở chest
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                {
                    OpenChest();
                }
            }
        }
    }
    
    void OpenChest()
    {
        // Phát âm thanh mở rương
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayChestOpen();
        }
        
        if (chestType == ChestType.Question)
        {
            // Ensure QuestionUI instance exists
            if (QuestionUI.Instance == null)
            {
                var found = Object.FindObjectOfType<QuestionUI>(true);
                if (found != null)
                {
                    QuestionUI.Instance = found;
                    Debug.Log("✓ Tìm thấy QuestionUI trong scene và gán Instance.");
                }
            }

            if (QuestionUI.Instance != null)
            {
                Debug.Log($"[{gameObject.name}] Mở câu hỏi: {question}");
                QuestionUI.Instance.ShowQuestion(this);
                hasBeenOpened = true;
            }
            else
            {
                Debug.LogError($"[{gameObject.name}] QuestionUI.Instance không tồn tại! Hãy đặt GameObject có script QuestionUI trong scene và gán các UI references.");
            }
        }
        else // Trap
        {
            if (player != null)
            {
                var playerComp = player.GetComponent<Player>();
                if (playerComp != null)
                {
                    playerComp.ApplyDamage(damageOnTrap, true); // đánh dấu là trap để hiện cảnh báo
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy Player component để trừ năng lượng");
                }
            }
            hasBeenOpened = true;
            // Trap: ẩn rương sau khi kích hoạt
            gameObject.SetActive(false);
        }
    }
    
    // Trigger-based interaction
    void OnTriggerEnter2D(Collider2D other)
    {
        if (useTrigger && other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log($"[{gameObject.name}] Player vào vùng tương tác. Nhấn E để mở.");
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (useTrigger && other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log($"[{gameObject.name}] Player rời khỏi vùng tương tác.");
        }
    }

    // Collision-based interaction for non-trigger collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useTrigger && collision.collider.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log($"[{gameObject.name}] Player chạm vào chest. Nhấn E để mở.");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!useTrigger && collision.collider.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    
    
    // Được gọi khi trả lời đúng
    public void AnswerCorrect()
    {
        Debug.Log($"[{gameObject.name}] ✓ Trả lời đúng!");
        
        // Phát âm thanh thu thập rương
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayChestCollect();
        }
        
        // Spawn phần thưởng
        if (rewardPrefab != null)
        {
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up;
            Instantiate(rewardPrefab, spawnPos, Quaternion.identity);
        }

        // Cộng điểm và năng lượng cho player
        if (player != null)
        {
            var playerComp = player.GetComponent<Player>();
            if (playerComp != null)
            {
                playerComp.AddScore(scoreOnCorrect);
                playerComp.RestoreEnergy(energyRewardOnCorrect);
                playerComp.ShowSuccessReward();
            }
        }
        
        // Ẩn hoặc phá hủy chest
        gameObject.SetActive(false);

        // Báo cho Player đã thu thập một rương câu hỏi
        var playerComp2 = player != null ? player.GetComponent<Player>() : null;
        if (playerComp2 != null)
        {
            playerComp2.OnChestCollected();
        }
    }
    
    
    // Được gọi khi trả lời sai
    public void AnswerWrong()
    {
        Debug.Log($"[{gameObject.name}] ✗ Trả lời sai!");
        
        // Trừ năng lượng player
        if (player != null)
        {
            var playerComp = player.GetComponent<Player>();
            if (playerComp != null)
            {
                playerComp.ApplyDamage(damageOnWrongAnswer);
                playerComp.ShowWrongAnswer();
            }
        }
        
        // Reset để có thể thử lại
        hasBeenOpened = false;
    }
    
    
    // Visualize interaction range trong Scene view
    void OnDrawGizmosSelected()
    {
        if (!useTrigger)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
