using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    private float m_currentSize;

    // Start is called before the first frame update
    void Start()
    {
        m_currentSize = GetComponent<UnityEngine.Camera>().orthographicSize; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }
}
