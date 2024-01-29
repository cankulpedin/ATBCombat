[System.Serializable]
public class CharacterData
{
    public string name;

    public float HP;
    public float MP;
    public float baseDamage;

    public float BaseCooldown = 3f;

    public int Vitaliy;
    public int Stamina;
    public int Intelligence;
    public int Strenght;
    public int Luck;

    public int ElementalResistance;
    public int HolyResistance;

    public enum Status
    {
        Normal,
        Slowed,
        Poisoned,
        Sleep,
        Dead
    }

    public Status CharacterStatus;

    public struct StatusAccumulationLimits
    {
        public float Poison;
        public float Frost;
        public float Terror;
    }

    public float Poison = 0f;
    public float Frost = 0f;
    public float Terror = 0f;
}
