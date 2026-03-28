using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EditorUIScaler
{
    static EditorUIScaler()
    {
        ApplyScaleBasedOnResolution();
    }

    [MenuItem("Tools/Apply UI Scaling")]
    public static void ApplyScaleBasedOnResolution()
    {
        // 1080p typically needs 100% (1.0), 3.2k often needs 150-200% (1.5 - 2.0)
        float screenHeight = Screen.currentResolution.height;
        float targetScale = 1.0f;

        if (screenHeight > 1500) // Identifying the 3.2k screen
        {
            targetScale = 1.5f; 
        }
        else // Identifying the 1080p screen
        {
            targetScale = 1.0f;
        }

        // Setting the internal Unity Preference for UI Scaling
        // Note: This matches the "UI Scaling" setting in Preferences
        EditorPrefs.SetFloat("UnityEditor.UIScalingValue", targetScale);
        
        // Some versions require UseCustomScaling to be true
        EditorPrefs.SetBool("UnityEditor.UseCustomUIScaling", true);
        
        Debug.Log($"[UIScaler] Screen Height: {screenHeight}. Applied UI Scale: {targetScale * 100}%");
    }
}