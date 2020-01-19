using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int speed = 250;
    public float lifeDuration = 1.0f;
    public float damage;
    public float hitForce;
    public LayerMask damageableLayer;
    public ContactFilter2D contactFilter;

    private Vector3 m_direction;
    private BoxCollider2D m_collider;

    private Vector3 m_oldPosition;
    private Vector3 m_currentPosition;


    private void OnEnable()
    {
        m_collider = GetComponent<BoxCollider2D>();
        StartCoroutine(AutoDestroy(lifeDuration));

        m_oldPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    void FixedUpdate()
    {
        m_currentPosition = gameObject.transform.position;
        checkHit();
    }

    void checkHit()
    {
        Vector2 direction = m_currentPosition - m_oldPosition;
        RaycastHit2D hit = Physics2D.Raycast(m_oldPosition, direction, direction.magnitude, damageableLayer);
        if(hit.collider != null)
        {
            Debug.Log("found enemy " + hit.collider.gameObject);
            GameObject enemy = hit.collider.gameObject;
            HealthController health = enemy.GetComponent<HealthController>();
            if (health)
            {
                Rigidbody2D body = enemy.GetComponent<Rigidbody2D>();
                health.Damage(damage);
                if (body != null)
                {
                    body.AddForce(-hit.normal * hitForce);
                }
            }

            Destroy(gameObject);
        }
    }

    IEnumerator AutoDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public void setDirection(Vector3 direction)
    {
        m_direction = direction;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.name + " : " + gameObject.tag);
        if (col.gameObject.tag.Equals("Sauv"))
        {
            Destroy(gameObject);
        }
    }
}
