using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WeaponName", menuName = "Items/Create new weapon")]
public class Weapon: ScriptableObject
{
    public string label;
    public string description;
}
