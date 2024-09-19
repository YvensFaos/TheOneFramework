using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : PlayerActivatable
{
    public DialogueRunner dialogueRunner;
    public string dialogueNodeName;

    private void Awake()
    {
        Collider selfCollider = GetComponent<Collider>();
        if (selfCollider.isTrigger) return;
        Debug.LogWarning($"{gameObject.name} Dialogue Trigger is using a non-trigger collider. It will be forcibly set to trigger.");
        selfCollider.isTrigger = true;
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