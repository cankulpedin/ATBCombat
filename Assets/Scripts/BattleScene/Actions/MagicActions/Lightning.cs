public class Lightning : BaseMagicAction
{
    public Lightning()
    {
        MagicType = MagicTypes.Fire;
        Damage = 50f;
        MPCost = 10f;
        MinIntRequirment = 2;
    }
}
