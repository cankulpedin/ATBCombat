using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyData
{
    public string name;

    public float hp;
    public float mana;

    public float hitPoint;

    public enum Status
    {
        ALIVE,
        PARALYZED,
        DEAD
    }
    public Status status;
}
