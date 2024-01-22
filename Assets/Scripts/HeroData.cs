using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HeroData 
{
    public string name;

    public float hp;
    public float mana;

    public float hitPoint;

    public int vitality;
    public int dexterity;
    public int intelligence;

    public enum Status
    {
        ALIVE,
        PARALYZED,
        DEAD
    }
    public Status status;
}
