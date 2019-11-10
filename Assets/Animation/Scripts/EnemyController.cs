using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class EnemyController : MotionController
{

    public GameObject player;
    public float speed = 0.01f;
    public float velocityLimit = 0.01f;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    public Weapon weapon;

    private EnemyBehaviour m_behaviour;
    private Animator m_animator;
    private Seeker m_seeker;

    private Vector2 m_currentDirection;
    private bool m_shouldJump = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_behaviour = GetComponent<EnemyBehaviour>();
        m_animator = GetComponent<Animator>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_seeker = GetComponent<Seeker>();

        m_behaviour.init(this.gameObject, this, weapon, player, m_seeker);
    }

    void Update()
    {
        m_currentDirection = new Vector2(0.0f, 0.0f);
        m_behaviour.updateBehaviour();
        base.updatePosition();
    }

    protected override void ComputeInputVelocity()
    {
        Vector2 lookAt = m_currentDirection.normalized;
        Vector2 direction = m_currentDirection.x != 0.0f ? Mathf.Sign(m_currentDirection.x) * new Vector2(1.0f, 0.0f) : m_currentDirection;
        m_targetVelocity = direction * maxSpeed;

        if(lookAt.y > 0.2 && m_grounded && m_shouldJump)
        {
            m_shouldJump = false;
            velocity.y = jumpTakeOffSpeed;
        }

        bool isMoving = m_targetVelocity.magnitude != 0.0f;
        m_animator.SetBool("moving", isMoving);
    }

    public void setCurrentDirection(Vector2 direction)
    {
        m_currentDirection = direction;
    }

    public void setShouldJump(bool jump)
    {
        m_shouldJump = jump;
    }
}
