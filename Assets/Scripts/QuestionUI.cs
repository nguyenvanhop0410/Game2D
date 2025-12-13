using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour
{
    public static QuestionUI Instance;

    [Header("UI references")]
    public GameObject panel;                 // QuestionPanel
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] answerTexts;    // 4 text fields
    public Button[] answerButtons;           // 4 buttons
    [Header("Behavior")]
    public bool hideOnStart = true;          // if false, panel stays visible on play

    private ChestQuestion currentChest;

    void Awake()
    {
        // Singleton pattern - Đảm bảo chỉ có 1 instance
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("✓ QuestionUI Instance đã được khởi tạo!");
        }
        else
        {
            Debug.LogWarning("⚠ Đã có QuestionUI Instance, destroying duplicate...");
            Destroy(gameObject);
            return;
        }

        // Kiểm tra references
        if (panel == null)
        {
            Debug.LogError("✗ Panel chưa được gán trong QuestionUI!");
        }
        else
        {
            // Ẩn panel ban đầu nếu được cấu hình
            if (hideOnStart)
            {
                panel.SetActive(false);
                Debug.Log("✓ Panel đã được ẩn ban đầu");
            }
        }
        
        if (questionText == null)
        {
            Debug.LogError("✗ Question Text chưa được gán!");
        }
        
        if (answerTexts == null || answerTexts.Length != 4)
        {
            Debug.LogError("✗ Answer Texts chưa đủ 4 elements!");
        }
        
        if (answerButtons == null || answerButtons.Length != 4)
        {
            Debug.LogError("✗ Answer Buttons chưa đủ 4 elements!");
        }
    }

    // Gọi khi muốn show câu hỏi của 1 chest
    public void ShowQuestion(ChestQuestion chest)
    {
        if (chest == null)
        {
            Debug.LogError("✗ Chest is null!");
            return;
        }

        currentChest = chest;

        // Fill UI
        if (questionText != null)
        {
            questionText.text = chest.question;
            Debug.Log($"✓ Hiển thị câu hỏi: {chest.question}");
        }

        for (int i = 0; i < answerTexts.Length; i++)
        {
            if (i < chest.answers.Length && answerTexts[i] != null)
            {
                answerTexts[i].text = chest.answers[i];
                
                if (answerButtons[i] != null)
                {
                    answerButtons[i].gameObject.SetActive(true);
                    answerButtons[i].interactable = true;
                }
            }
            else
            {
                // ẩn nút nếu chest chỉ có 3 đáp án
                if (answerButtons[i] != null)
                {
                    answerButtons[i].gameObject.SetActive(false);
                }
            }
        }

        if (panel != null)
        {
            // Bảo đảm Canvas cha được bật
            var canvas = GetComponent<Canvas>();
            if (canvas != null && !canvas.enabled)
            {
                canvas.enabled = true;
            }

            // Hiện panel và đảm bảo CanvasGroup (nếu có) hiển thị
            panel.SetActive(true);
            var cg = panel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.blocksRaycasts = true;
                cg.interactable = true;
            }

            Debug.Log($"✓ Panel đã được hiển thị | active={panel.activeInHierarchy}");
        }
    }

    // Được gán cho các nút: ChooseAnswer(index)
    public void ChooseAnswer(int index)
    {
        if (currentChest == null)
        {
            Debug.LogWarning("⚠ currentChest is null!");
            return;
        }

        bool correct = index == currentChest.correctIndex;

        Debug.Log($"Chọn đáp án {index}: {(correct ? "✓ ĐÚNG" : "✗ SAI")}");

        if (correct)
        {
            currentChest.AnswerCorrect();
        }
        else
        {
            currentChest.AnswerWrong();
        }

        // Ẩn panel sau khi chọn
        if (panel != null)
        {
            panel.SetActive(false);
        }

        // reset current
        currentChest = null;
    }
}
