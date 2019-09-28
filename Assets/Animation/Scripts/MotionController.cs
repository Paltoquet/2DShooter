using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour
{

    public float gravityModifier = 0.1f;
    public float minGroundNormalY = 0.65f;

    protected Rigidbody2D m_rigidBody;
    protected Vector2 m_targetVelocity;
    protected Vector2 velocity;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected bool m_grounded;
    protected Vector2 m_groundNormal;
    protected const float m_shellRadius = 0.01f;
    protected const float m_minMoveDistance = 0.001f;
    protected ContactFilter2D m_contactFilter;

    void OnEnable()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        m_contactFilter.useTriggers = false;
        m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        m_contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    protected void updatePosition()
    {
        m_targetVelocity = Vector2.zero;
        ComputeInputVelocity();
    }

    protected virtual void ComputeInputVelocity()
    {

    }

    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = m_targetVelocity.x;

        m_grounded = false;

        Vector2 moveAlongGround = new Vector2(m_groundNormal.y, -m_groundNormal.x);
        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;
        Movement(move, true);

    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if(distance > m_minMoveDistance)
        {
            int count = m_rigidBody.Cast(move, m_contactFilter, hitBuffer, distance + m_shellRadius);
            hitBufferList.Clear();

            for(int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for(int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 normal = hitBufferList[i].normal;
                if (normal.y > minGroundNormalY)
                {
                    m_grounded = true;
                    if (yMovement)
                    {
                        m_groundNormal = normal;
                        normal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, normal);
                if(projection < 0)
                {
                    velocity = velocity - projection * normal;
                }

                float modifyDistance = hitBufferList[i].distance - m_shellRadius;
                distance = modifyDistance < distance ? modifyDistance : distance;
            }
        }

        //Debug.Log("grounded: " + m_grounded + " direction: " + move + " distance: " + distance + " velocity: " + velocity);
        m_rigidBody.position = m_rigidBody.position + move.normalized * distance;
    }
}
