using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurns
{
    public string Attacker; // name of attacker
    
    public GameObject AttackerGameObject;
    public GameObject TargetGameObject; // attacker's target

    // which attack is performed
}
