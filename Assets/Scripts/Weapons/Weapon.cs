﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject character;
    public GameObject bulletPrefab;
    public Transform weaponTip;
    public SpriteRenderer muzzleFlashRend;

    public float damage = 2.0f;
    public float hitForce = 20.0f;
    public LayerMask damageableLayer;
    public float range = 40.0f;

    public float maxRecoil;
    public float recoilFallOff = 0.05f;
    public float maxHorizontalDistance;
    public float maxVerticalDistance;
    public float xOffset;
    public float yOffset;

    private SpriteRenderer m_weaponRender;
    private WeaponParams m_weaponParams;

    private Transform leftGrip;
    private Transform rightGrip;

    private bool m_isFliped;
    private float m_angle;
    private float m_currentRecoilPercentage = 0.0f;
    private float m_nextShot;
    private Vector2 m_shootingDirection;
    private Vector3 m_originalTransform;
    private float m_muzzleFlashDuration = 0.2f;

    void OnEnable()
    {
        m_weaponRender = GetComponent<SpriteRenderer>();
        leftGrip = transform.Find("LeftGrip");
        rightGrip = transform.Find("RightGrip");

        m_isFliped = false;
        m_shootingDirection = new Vector2(0.0f, 0.0f);
        muzzleFlashRend.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        m_originalTransform = this.gameObject.transform.localPosition;
    }

    void Update()
    {
        updateRecoil();
    }

    // Update is called once per frame
    public void updateOrientation(Vector2 direction)
    {
        m_shootingDirection = direction;
        int vertical = (int)Input.GetAxis("Vertical");

        Vector2 facingDirection = new Vector2(1.0f, 0.0f);
        m_angle = Vector2.SignedAngle(facingDirection, direction) * Mathf.Deg2Rad;

        float facingAngle = Mathf.Abs(m_angle) > Mathf.PI / 2 ? -1 * (Mathf.PI - m_angle) : m_angle;
        float absFacingAngle = Mathf.Abs(facingAngle);
        float rot = Mathf.Rad2Deg * facingAngle;
        bool shouldFlip = m_isFliped ? Mathf.Abs(m_angle) <= Mathf.PI / 2 : Mathf.Abs(m_angle) > Mathf.PI / 2;

        if (shouldFlip)
        {
            List<GameObject> toFlip = new List<GameObject>();
            //toFlip.Add(gameObject);
            toFlip.Add(character);
            Flip(toFlip);
        }

        float previousXOffset = xOffset;
        float previousYOffset = yOffset;
        xOffset = maxHorizontalDistance * (absFacingAngle / (Mathf.PI / 2));
        yOffset = maxVerticalDistance * (absFacingAngle / (Mathf.PI / 2));
        Vector3 offset = new Vector3(xOffset - previousXOffset, yOffset - previousYOffset, 0.0f);
        offset.x = m_isFliped ? -offset.x : offset.x;
        //transform.position += offset;

        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rot));

        if (vertical > 0)
        {
            this.gameObject.transform.position += new Vector3(0, 0.05f, 0);
        }
        else if (vertical < 0)
        {
            this.gameObject.transform.position += new Vector3(0, -0.05f, 0);
        }
    }

    public void requestShoot(Vector2 direction)
    {
        if (m_weaponParams.fireRate == 0.0f)
        {
            Shoot(direction);
        }
        else
        {
            if(Time.time > m_nextShot)
            {
                m_nextShot = Time.time + 1 / m_weaponParams.fireRate;
                Shoot(direction);
            }
        }
    }
    
    public void updateRecoil()
    {
        m_currentRecoilPercentage = m_currentRecoilPercentage > recoilFallOff ? m_currentRecoilPercentage - recoilFallOff : 0.0f;
        if(m_currentRecoilPercentage > 1.0f)
        {
            m_currentRecoilPercentage = 1.0f;
        }
        float offset = maxRecoil * m_currentRecoilPercentage;
        Vector2 shootingDirection = m_isFliped ? new Vector2(-1.0f * m_shootingDirection.x, m_shootingDirection.y) : m_shootingDirection;
        Vector2 currentRecoil = -1.0f * shootingDirection * offset;
        this.gameObject.transform.localPosition = m_originalTransform;
        this.gameObject.transform.localPosition += new Vector3(currentRecoil.x, currentRecoil.y, 0.0f);
    }

    void Shoot(Vector2 direction)
    {
        m_currentRecoilPercentage += m_weaponParams.recoil;
        Vector2 firePos = new Vector2(weaponTip.position.x, weaponTip.position.y);
        Vector2 mouse = Input.mousePosition;

        //RaycastHit2D hit = Physics2D.Raycast(firePos, direction, range, damageableLayer);
        //Debug.DrawRay(firePos, direction * range, Color.yellow, 1f);
        
        DrawBullet(direction);
    }

    void DrawBullet(Vector2 direction)
    {
        var nbProjectiles = m_weaponParams.nbProjectiles;
        var damage = m_weaponParams.damage;
        var hitForce = m_weaponParams.hitForce;
        var prefab = m_weaponParams.bulletPrefab;

        for(int projectile = 0; projectile < nbProjectiles; projectile++)
        {
            var randomAngle = Random.Range(-m_weaponParams.projectileRadius, m_weaponParams.projectileRadius);
            var currentAngle = m_angle + randomAngle;
            Quaternion rot = Quaternion.Euler(0, 0, currentAngle * Mathf.Rad2Deg);
            GameObject obj = Instantiate(prefab, weaponTip.position, rot);
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.setDirection(new Vector3(direction.x, direction.y, 0.0f));
            bullet.damageableLayer = damageableLayer;
            bullet.damage = damage;
            bullet.hitForce = hitForce;
        }

        StartCoroutine(MuzzleFlash());
    }

    void Flip(List<GameObject> gameObjects)
    {
        m_isFliped = !m_isFliped;
        foreach (GameObject obj in gameObjects) {
            Vector3 localScale = obj.transform.localScale;
            localScale.x = localScale.x * -1;
            obj.transform.localScale = localScale;
        }
    }

    public void setCurrentWeapon(WeaponParams weapon)
    {
        m_weaponParams = weapon;
        m_weaponRender.sprite = m_weaponParams.sprite;
        leftGrip.localPosition = new Vector3(m_weaponParams.leftHandleOffset.x, m_weaponParams.leftHandleOffset.y, leftGrip.position.z);
        rightGrip.localPosition = new Vector3(m_weaponParams.rightHandleOffset.x, m_weaponParams.rightHandleOffset.y, leftGrip.position.z);
    }

    public WeaponParams getCurrentWeapon()
    {
        return m_weaponParams;
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlashRend.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        muzzleFlashRend.gameObject.transform.Rotate(Random.Range(0, 2) * 180, 0, 0);

        yield return new WaitForSeconds(m_muzzleFlashDuration);

        muzzleFlashRend.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
}
