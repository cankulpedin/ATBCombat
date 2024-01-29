public class Ice : BaseMagicAction
{
    public Ice()
    {
        MagicType = MagicTypes.Fire;
        Damage = 30f;
        MPCost = 10f;
        MinIntRequirment = 2;
        StatusAccumulation = 3f;
    }
}
