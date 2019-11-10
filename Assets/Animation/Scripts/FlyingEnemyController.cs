using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : HealthController
{

    public GameObject player;
    public Sprite final;
    public Transform pickable;

    public float speed = 0.01f;
    public float velocityLimit = 0.01f;

    private SpriteRenderer m_renderer;
    private Rigidbody2D m_rigidBody;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_renderer = GetComponent<SpriteRenderer>();
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

    protected override void Die()
    {
        if (!m_isDead) {
            Instantiate(pickable, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject, 1);
            m_renderer.sprite = final;
            m_isDead = true;
        }
    }
}
