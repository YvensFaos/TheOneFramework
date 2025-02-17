using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Collections.Generic;

[InitializeOnLoad]
public static class DependencyChecker
{
    static List<string> requirements = new List<string>{
        "com.unity.cinemachine",
        "com.unity.ai.navigation",
        //"com.unity.textmeshpro",
        "com.unity.inputsystem",
        "com.unity.render-pipelines.universal",
        "dev.yarnspinner.unity",
    };

    static ListRequest Request;

    static DependencyChecker()
    {
        Request = Client.List(true);    // List all packages, including built-in ones
        EditorApplication.update += Progress;
    }

    static void Progress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
            {
                foreach (string requirement in requirements) {
                    bool isFound = false;
                    foreach (var installed in Request.Result) {
                        if (installed.name == requirement) {
                            isFound = true;
                        }
                    }
                    if (!isFound) {
                        Debug.LogWarning("Package " + requirement + " is not installed. Please install it via the Package Manager.");
                    }
                }

            }
            else if (Request.Status >= StatusCode.Failure)
            {
                Debug.Log(Request.Error.message);
            }

            EditorApplication.update -= Progress;
        }
    }
}
