using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    // Data to pass between scenes
    public string enemyId;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
