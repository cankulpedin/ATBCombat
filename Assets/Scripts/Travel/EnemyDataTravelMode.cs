using System;

[Serializable]
public class EnemyDataTravelMode
{
    public float speed;

    public enum EnemyState
    {
        Idle,
        Chasing,
        Returning
    }

    public EnemyState currentState;
}
