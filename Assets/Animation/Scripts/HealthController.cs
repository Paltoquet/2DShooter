using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float baseHealth = 100.0f;
    protected float m_currentHealth;
    protected bool m_isDead = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_currentHealth = baseHealth;
    }

    public void Damage(float damage)
    {
        m_currentHealth = m_currentHealth - damage;
        if(m_currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        m_isDead = true;
    }

}
