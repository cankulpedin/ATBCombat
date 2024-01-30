using UnityEngine;

[System.Serializable]
public class BattleTurn
{
    public string TurnOwnerName;
    public GameObject TurnOwnerGameObject;
    public GameObject targetGameObject;

    public ActionTypes actionType;
    public BaseMagicAction MagicAction;
}
