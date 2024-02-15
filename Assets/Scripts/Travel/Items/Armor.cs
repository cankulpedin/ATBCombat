using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ArmorName", menuName = "Items/Create new armor")]
public class Armor: ScriptableObject
{
    public string label;
    public string description;

}
