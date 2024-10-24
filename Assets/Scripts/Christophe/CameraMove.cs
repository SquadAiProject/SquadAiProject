using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player; 
    public Vector3 offset;    

    private void Start()
    {
        player = GameObject.Find("Player");
        
        offset = transform.position - player.transform.position;
    }

    private void Update()
    {
        if(player)
            transform.position = player.transform.position + offset;
    }
    
}
