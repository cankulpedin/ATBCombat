using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;


    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z - 10f);
    }
}
