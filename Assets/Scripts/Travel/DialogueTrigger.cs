using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public float interactionRange = 2f; // Adjust this value as needed
    public Dialogue dialogue;
    private bool playerInRange = false;

    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue);
    }
}
