using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MotionController
{

    public Camera cam;
    public Weapon weapon;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        base.updatePosition();
        Vector2 mouse = Input.mousePosition;
        Vector3 originalPosition = transform.position;
        Vector2 originalPixelPosition = cam.WorldToScreenPoint(originalPosition);
        Vector2 direction = (mouse - new Vector2(originalPixelPosition.x, originalPixelPosition.y)).normalized;
        weapon.updateOrientation(direction);

        if (Input.GetButton("Fire1"))
        {
            weapon.requestShoot(direction);
        }
    }

    protected override void ComputeInputVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && m_grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if(Input.GetButtonUp("Jump"))
        {
            if(velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        m_targetVelocity = move * maxSpeed;

        bool isMoving = m_targetVelocity.magnitude != 0.0f;
        m_animator.SetBool("moving", isMoving);
    }
}
