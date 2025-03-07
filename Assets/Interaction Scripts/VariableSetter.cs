using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class VariableSetter : PlayerActivatable
{
    public DialogueRunner dialogueSystem;

    public string identifier;
    public OperationType operation;

    public int value;

    public enum OperationType
    {
        Add,
        Subtract,
        Set
    }

    private VariableStorageBehaviour variableStorage;

    [System.Obsolete]
    void Start() 
    {
        if (dialogueSystem == null) {
            dialogueSystem = FindFirstObjectByType<DialogueRunner>();
        }
        if (dialogueSystem != null) {
            variableStorage = dialogueSystem.GetComponent<InMemoryVariableStorage>();
        }
        if (identifier != "") {
            if (identifier.Substring(0, 1) != "$") {
                identifier = "$" + identifier;
            }
        }
    }

    override protected void OnActivate()
    {
        if (variableStorage != null && !string.IsNullOrEmpty(identifier))
        {
            float variableValue = 0f;
            variableStorage.TryGetValue(identifier, out variableValue);

            switch (operation)
            {
                case OperationType.Add:
                    variableValue += value;
                    break;
                case OperationType.Subtract:
                    variableValue -= value;
                    break;
                case OperationType.Set:
                    variableValue = value;
                    break;
            }

            variableStorage.SetValue(identifier, variableValue);
        }
    }

    override protected bool IsActivated() 
    {
        return false;
    }

}
