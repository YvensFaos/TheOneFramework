using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : PlayerActivatable
{
    public DialogueRunner dialogueRunner;
    public string dialogueNodeName;

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