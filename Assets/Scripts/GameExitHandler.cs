using UnityEngine;

/// <summary>
/// Script cho phép thoát game b?ng phím t?t
/// ??t script này vào m?t GameObject b?t k? trong scene
/// </summary>
public class GameExitHandler : MonoBehaviour
{
    [Header("Phím Thoát")]
    [Tooltip("Phím ?? thoát game")]
    public KeyCode exitKey = KeyCode.Escape;
    
    [Header("Cài ??t")]
    [Tooltip("Có hi?n h?p tho?i xác nh?n không?")]
    public bool showConfirmDialog = false;
    
    [Tooltip("Có cho phép Alt+F4 không?")]
    public bool allowAltF4 = true;
    
    [Header("Fullscreen Toggle")]
    [Tooltip("Phím ?? b?t/t?t fullscreen")]
    public KeyCode toggleFullscreenKey = KeyCode.F11;
    
    [Tooltip("Cho phép toggle fullscreen b?ng Alt+Enter")]
    public bool allowAltEnter = true;

    void Update()
    {
        // F11 ?? toggle fullscreen
        if (Input.GetKeyDown(toggleFullscreenKey))
        {
            ToggleFullscreen();
        }
        
        // Alt + Enter ?? toggle fullscreen
        if (allowAltEnter && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Return))
        {
            ToggleFullscreen();
        }
        
        // Nh?n ESC ?? thoát
        if (Input.GetKeyDown(exitKey))
        {
            if (showConfirmDialog)
            {
                // Có th? thêm UI xác nh?n ? ?ây
                Debug.Log("B?n có mu?n thoát game? (Nh?n ESC l?n n?a ?? xác nh?n)");
                
                // Thoát ngay n?u nh?n 2 l?n
                if (Input.GetKeyDown(exitKey))
                {
                    ExitGame();
                }
            }
            else
            {
                ExitGame();
            }
        }
        
        // Alt + F4 (Windows)
        if (allowAltF4 && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.F4))
        {
            ExitGame();
        }
    }

    /// <summary>
    /// Thoát game
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("?ang thoát game...");
        
        #if UNITY_EDITOR
            // Trong Unity Editor, d?ng ch? ?? Play
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Trong build, thoát ?ng d?ng
            Application.Quit();
        #endif
    }
    
    /// <summary>
    /// B?t/t?t fullscreen
    /// </summary>
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log($"Fullscreen: {(Screen.fullScreen ? "B?t" : "T?t")}");
    }
}
