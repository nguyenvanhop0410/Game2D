using UnityEditor;
using UnityEngine;

public class BuildScript
{
    // Menu item để build nhanh từ Unity Editor
    [MenuItem("Build/Build Windows")]
    public static void BuildWindows()
    {
        // Cấu hình build
        string buildPath = "Builds/Windows/Game2D.exe";
        
        // Lấy tất cả scenes trong Build Settings
        string[] scenes = GetScenePaths();
        
        // Build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };
        
        // Bắt đầu build
        Debug.Log("Bắt đầu build game...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log($"Build hoàn tất! File: {buildPath}");
    }
    
    
    // Lấy danh sách scenes từ Build Settings
    private static string[] GetScenePaths()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        string[] scenePaths = new string[scenes.Length];
        
        for (int i = 0; i < scenes.Length; i++)
        {
            scenePaths[i] = scenes[i].path;
        }
        
        return scenePaths;
    }
}
