using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public float horizontalSpeed;

    private Animator m_animator;
    private Rigidbody2D m_body;

    private bool m_isMoving;
    private Vector2 m_previousPosition;
    private Vector2 m_currentPosition;
    private Vector2 m_velocity;

    // Start is called before the first frame update
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_body = GetComponent<Rigidbody2D>();

        m_currentPosition = m_body.position;
        m_previousPosition = m_body.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        int horizontal = (int)Input.GetAxis("Horizontal");
        m_isMoving = Mathf.Abs(horizontal) > 0;
        Vector2 nextMovement = new Vector2(horizontal * horizontalSpeed, 0.0f);

        m_previousPosition = m_body.position;
        m_currentPosition = m_previousPosition + nextMovement;
        m_velocity = (m_currentPosition - m_previousPosition) / Time.deltaTime;

        m_body.MovePosition(m_currentPosition);
        m_animator.SetBool("moving", m_isMoving);
    }
}
