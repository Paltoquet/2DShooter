using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour
{

    public GameObject player;
    public float speed = 0.01f;
    public float velocityLimit = 0.01f;

    private Rigidbody2D m_rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = gameObject.transform.position;
        Vector3 playerPos = player.transform.position;
        Vector3 direction = (playerPos - position).normalized;

        transform.position = transform.position + (direction * speed);
    }
}
