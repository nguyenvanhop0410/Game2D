using UnityEngine;

/// <summary>
/// Script debug ?? ki?m tra cài ??t Camera
/// Attach vào Main Camera và ch?y game ?? xem log
/// </summary>
public class CameraDebugger : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        
        if (cam == null)
        {
            Debug.LogError("? Không có Camera component!");
            return;
        }
        
        Debug.Log("=== CAMERA DEBUG INFO ===");
        Debug.Log($"Clear Flags: {cam.clearFlags}");
        Debug.Log($"Background Color: {cam.backgroundColor}");
        Debug.Log($"Culling Mask: {cam.cullingMask}");
        Debug.Log($"Depth: {cam.depth}");
        Debug.Log($"Render Mode: {cam.renderingPath}");
        Debug.Log($"HDR: {cam.allowHDR}");
        Debug.Log($"MSAA: {cam.allowMSAA}");
        
        // Ki?m tra Color Space
        Debug.Log($"?? Color Space: {QualitySettings.activeColorSpace}");
        
        // Ki?m tra có Post-Processing không
        var postProcessing = FindObjectsOfType<MonoBehaviour>();
        foreach (var comp in postProcessing)
        {
            if (comp.GetType().Name.Contains("Post"))
            {
                Debug.Log($"?? Tìm th?y Post-Processing: {comp.GetType().Name} trên {comp.gameObject.name}");
            }
        }
        
        // Ki?m tra có Canvas che ph? không
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (var canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                Debug.Log($"??? Canvas Overlay tìm th?y: {canvas.gameObject.name}");
                
                // Ki?m tra Image components
                var images = canvas.GetComponentsInChildren<UnityEngine.UI.Image>();
                foreach (var img in images)
                {
                    if (img.color.a > 0.1f && img.gameObject.activeInHierarchy)
                    {
                        Debug.Log($"   ?? Image: {img.gameObject.name} | Color: {img.color} | Active: {img.enabled}");
                    }
                }
            }
        }
        
        Debug.Log("======================");
    }
}
