using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float baseHealth = 100.0f;
    public Sprite final;

    private SpriteRenderer m_renderer;
    private float m_currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        m_currentHealth = baseHealth;
        m_renderer = GetComponent<SpriteRenderer>();
    }

    public void Damage(float damage)
    {
        m_currentHealth = m_currentHealth - damage;
        if(m_currentHealth < 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject, 1);
        m_renderer.sprite = final;
    }

}
