using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DialogueRunner))]
public class DialogKeyAdvance : MonoBehaviour
{
    // Drag and drop the DialogueRunner component here in the Inspector
    private DialogueRunner dialogueRunner;

    void Start()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
    }

    void Update()
    {
        // Check if a dialogue is currently running
        if (dialogueRunner.IsDialogueRunning)
        {
            // Use the Keyboard class to check for the 'E' key being pressed down
            if (Input.GetKeyDown(KeyCode.E) || Keyboard.current[Key.E].wasPressedThisFrame)
            {
                // Request the next line from the DialogueRunner
                dialogueRunner.RequestNextLine();
            }
        }
    }
}