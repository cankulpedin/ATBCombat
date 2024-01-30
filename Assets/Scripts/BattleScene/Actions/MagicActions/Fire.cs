public class Fire : BaseMagicAction
{
    public Fire() {
        MagicType = MagicTypes.Fire;
        Damage = 40f;
        MPCost = 10f;
        MinIntRequirment = 2;
    }
}
