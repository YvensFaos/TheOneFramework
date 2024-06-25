using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : PlayerActivatable
{
    public DialogueRunner dialogueRunner;

    public string dialogueNodeName;

    override protected void OnActivate()
    {
        if (dialogueRunner != null) {
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
