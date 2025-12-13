using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject homePanel; // ch?a nút Start, HowToPlay, Rules, Exit
    [SerializeField] private GameObject howToPlayPanel; // h??ng d?n cách ch?i
    [SerializeField] private GameObject rulesPanel; // lu?t ch?i

    [Header("Game")]
    [SerializeField] private int gameSceneBuildIndex = 1; // index scene gameplay trong Build Settings

    void Start()
    {
        // ??m b?o th?i gian bình th??ng khi vào menu
        Time.timeScale = 1f;
        ShowHome();
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneBuildIndex);
    }

    public void ShowHome()
    {
        if (homePanel != null) homePanel.SetActive(true);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (rulesPanel != null) rulesPanel.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        if (homePanel != null) homePanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(true);
        if (rulesPanel != null) rulesPanel.SetActive(false);
    }

    public void ShowRules()
    {
        if (homePanel != null) homePanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (rulesPanel != null) rulesPanel.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
