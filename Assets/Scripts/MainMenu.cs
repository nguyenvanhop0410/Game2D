using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject homePanel; // chứa nút Start, HowToPlay, Rules, Exit
    [SerializeField] private GameObject howToPlayPanel; // hướng dẫn cách chơi
    [SerializeField] private GameObject rulesPanel; // luật chơi

    [Header("Game")]
    [SerializeField] private int gameSceneBuildIndex = 1; // index scene gameplay trong Build Settings

    void Start()
    {
        // Đảm bảo thời gian bình thường khi vào menu
        Time.timeScale = 1f;
        ShowHome(false); // Không phát âm thanh khi khởi tạo lần đầu
    }

    public void StartGame()
    {
        // Phát âm thanh click nút
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
            // Chuyển sang nhạc gameplay
            AudioManager.Instance.PlayGameplayMusic();
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneBuildIndex);
    }

    public void ShowHome()
    {
        ShowHome(true); // Khi gọi từ button thì có âm thanh
    }

    private void ShowHome(bool playSound)
    {
        if (playSound && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (homePanel != null) homePanel.SetActive(true);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (rulesPanel != null) rulesPanel.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (homePanel != null) homePanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(true);
        if (rulesPanel != null) rulesPanel.SetActive(false);
    }

    public void ShowRules()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (homePanel != null) homePanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (rulesPanel != null) rulesPanel.SetActive(true);
    }

    public void ExitGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Debug.Log("Thoát game...");
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
