using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class FMODChecker
{
    [System.Obsolete]
    static FMODChecker()
    {
        // Check if FMOD is present by trying to find an FMOD class
        bool isFMODPresent = System.Type.GetType("FMODUnity.RuntimeManager, FMODUnity") != null;

        // Get current scripting define symbols
        string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

        if (isFMODPresent)
        {
            if (!symbols.Contains("FMOD_INSTALLED"))
            {
                symbols += ";FMOD_INSTALLED";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
            }
        }
        else
        {
            if (symbols.Contains("FMOD_INSTALLED"))
            {
                symbols = symbols.Replace("FMOD_INSTALLED;", "").Replace("FMOD_INSTALLED", "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
            }
        }
    }
}