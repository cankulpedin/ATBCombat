using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ConsumableName", menuName = "Items/Create new consumable")]
public class Consumable: ScriptableObject
{
    public string label;
    public string description;
}
