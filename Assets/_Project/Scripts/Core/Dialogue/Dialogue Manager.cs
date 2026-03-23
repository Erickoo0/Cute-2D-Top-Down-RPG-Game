using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    
    [Header("Reference Data")] 
    [SerializeField] private Image dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private TextMeshProUGUI dialogueBody;
    [SerializeField] private Image dialoguePortrait;

    private string[] dialogueLines;
    private int dialogueLineIndex;

    //private int inputTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.unityLogger.Log("Multiple DialogueManagers detected. Disabling script.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void ControlDialogue(string name, string[] body, Sprite portrait)
    {
        // Enable the dialogue panel if it's not already active'
        if (!dialoguePanel.gameObject.activeSelf)
        {
            SetDialogue(name, body, portrait);
        }
        // Disable the dialogue panel if it's already active'
        else
        {
            // Transition to the next line
            if (dialogueLineIndex < dialogueLines.Length - 1)
            {
                dialogueLineIndex++;
                dialogueBody.text = dialogueLines[dialogueLineIndex];
            } else CloseDialogue();
        }

    }
    
    private void SetDialogue(string name, string[] body, Sprite portrait)
    {
        // Set the name, portrait, and index
        dialogueName.text = name;
        dialoguePortrait.sprite = portrait;
        dialogueLineIndex = 0;
        
        // Set the body
        dialogueLines = body;
        dialogueBody.text = dialogueLines[dialogueLineIndex];
        dialoguePanel.gameObject.SetActive(true);
        
        // Pause the game
        PauseManager.SetPause(true);
    }
    
    private void CloseDialogue()
    {
        dialoguePanel.gameObject.SetActive(false);
        
        // Unpause the game
        PauseManager.SetPause(false);
    }
}
