[System.Serializable]
public abstract class BaseMagicAction
{
    public MagicTypes MagicType;
    
    public float Damage; // base damage
    public float MPCost;

    public int MinIntRequirment;

    public float StatusAccumulation; 
}
