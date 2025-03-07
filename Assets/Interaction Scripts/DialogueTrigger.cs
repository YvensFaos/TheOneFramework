using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;
using Yarn;
using System.Reflection;

public class DialogueTrigger : PlayerActivatable
{
    public DialogueRunner dialogueRunner;
    
    public string dialogueNodeName;

    [System.Obsolete]
    void Start()
    {
        if (dialogueRunner == null) {
            dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        }

    }

    override protected void OnActivate()
    {    
        if (dialogueRunner != null) {
            if (dialogueRunner.IsDialogueRunning) {                
                dialogueRunner.Stop();
            }
            dialogueRunner.StartDialogue(dialogueNodeName);
        } else {
            Debug.LogWarning("DialogueRunner component not assigned!");
        }

    }

    override protected bool IsActivated() {
        if (dialogueRunner != null) {
            return dialogueRunner.IsDialogueRunning;
        }
        return base.IsActivated();
    }

}
