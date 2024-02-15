using UnityEngine;

public enum Outcome
{
    NULL,
    Good,
    Bad,
    Get_Item_BroadSword
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [System.Serializable]
    public class DialogueNode
    {
        public string npcName;
        [TextArea(3, 10)]
        public string npcLine;
        public Response[] responses;
    }

    [System.Serializable]
    public class Response
    {
        public string playerLine;
        public Outcome outcome;
        public Outcome outcomeToRemove = Outcome.NULL;
        public int nextDialogueNodeIndex;
    }

    public DialogueNode[] dialogueNodes;
}
