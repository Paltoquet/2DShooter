using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MotionController
{

    public Camera cam;
    public Weapon weapon;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private int m_currentWeaponSlot = 0;

    private Animator m_animator;
    private List<WeaponParams> m_weapons;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_animator = GetComponent<Animator>();

        m_weapons = new List<WeaponParams>();
        m_weapons.Add(WeaponCollection.Instance.getParams("riffle"));
        //m_weapons.Add(WeaponCollection.Instance.getParams("shotgun"));

        setCurrentWeapon();
    }

    void Update()
    {
        base.updatePosition();
        Vector2 mouse = Input.mousePosition;
        Vector3 originalPosition = weapon.transform.position;
        Vector2 originalPixelPosition = cam.WorldToScreenPoint(originalPosition);
        Vector2 direction = (mouse - new Vector2(originalPixelPosition.x, originalPixelPosition.y)).normalized;
        weapon.updateOrientation(direction);

        if (Input.GetButton("Fire1"))
        {
            weapon.requestShoot(mouse);
        }

        if(Input.mouseScrollDelta.y != 0)
        {
            int delta = Input.mouseScrollDelta.y > 0 ? 1 : -1;
            m_currentWeaponSlot += delta;
            if(m_currentWeaponSlot > m_weapons.Count)
            {
                m_currentWeaponSlot = 0;
            } else if (m_currentWeaponSlot < 0)
            {
                m_currentWeaponSlot = m_weapons.Count - 1;
            }
            setCurrentWeapon();
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

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("found " + col.gameObject);
        if(col.gameObject.tag == "Pickable")
        {
            Pickable picked = col.gameObject.GetComponent<Pickable>();
            string weaponName = picked.weapon;
            bool hasWeapons = false;
            foreach(var weapon in m_weapons)
            {
                if(weapon.name == weaponName)
                {
                    hasWeapons = true;
                    break;
                }
            }

            Debug.Log("found a weapon " + weaponName);

            if (!hasWeapons)
            {
                m_weapons.Add(WeaponCollection.Instance.getParams(weaponName));
                m_currentWeaponSlot = m_weapons.Count - 1;
                setCurrentWeapon();
            }

            Destroy(col.gameObject);
        }
    }

    protected void setCurrentWeapon()
    {
        if (m_currentWeaponSlot < m_weapons.Count) {
            var currentWeapon = m_weapons[m_currentWeaponSlot];
            weapon.setCurrentWeapon(currentWeapon);
        }
    }
        
}
