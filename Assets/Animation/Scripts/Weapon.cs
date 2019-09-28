using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject character;
    public GameObject bulletPrefab;
    public Transform weaponTip;
    public SpriteRenderer muzzleFlashRend;

    public float fireRate = 0.5f;
    public float damage = 2.0f;
    public float hitForce = 20.0f;
    public LayerMask damageableLayer;
    public float range = 100.0f;

    public float maxHorizontalDistance;
    public float maxVerticalDistance;
    public float xOffset;
    public float yOffset;

    private bool m_isFliped;
    private float m_angle;
    private float m_nextShot;
    private float m_muzzleFlashDuration = 0.2f;

    // Start is called before the first frame update
    void Awake()
    {
        m_isFliped = false;
        muzzleFlashRend.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    public void updateOrientation(Vector2 direction)
    {
        int vertical = (int)Input.GetAxis("Vertical");

        Vector2 facingDirection = new Vector2(1.0f, 0.0f);
        m_angle = Vector2.SignedAngle(facingDirection, direction) * Mathf.Deg2Rad;

        float facingAngle = Mathf.Abs(m_angle) > Mathf.PI / 2 ? -1 * (m_angle - (m_angle - (Mathf.PI / 2)) * 2) : m_angle;
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

        //Debug.Log("coucou " + m_angle + " " + (m_angle % (Mathf.PI / 2)));

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
        if (fireRate == 0.0f)
        {
            Shoot(direction);
        }
        else
        {
            if(Time.time > m_nextShot)
            {
                m_nextShot = Time.time + 1 / fireRate;
                Shoot(direction);
            }
        }
    }

    void Shoot(Vector2 direction)
    {
        Vector2 firePos = new Vector2(weaponTip.position.x, weaponTip.position.y);
        Vector2 mouse = Input.mousePosition;

        RaycastHit2D hit = Physics2D.Raycast(firePos, direction, range, damageableLayer);
        //Debug.DrawRay(firePos, direction * range, Color.yellow, 1f);
        if(hit.collider != null)
        {
            Debug.Log("finded");
            GameObject enemy = hit.collider.gameObject;
            HealthController health = enemy.GetComponent<HealthController>();
            Rigidbody2D body = enemy.GetComponent<Rigidbody2D>();
            health.Damage(damage);
            if(body != null)
            {
                body.AddForce(-hit.normal * hitForce);
            }
        }
        DrawBullet(direction);
    }

    void DrawBullet(Vector2 direction)
    {
        Quaternion rot = Quaternion.Euler(0, 0, m_angle * Mathf.Rad2Deg);

        GameObject obj = Instantiate(bulletPrefab, weaponTip.position, rot);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.setDirection(new Vector3(direction.x, direction.y, 0.0f));

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

    IEnumerator MuzzleFlash()
    {
        muzzleFlashRend.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        muzzleFlashRend.gameObject.transform.Rotate(Random.Range(0, 2) * 180, 0, 0);

        yield return new WaitForSeconds(m_muzzleFlashDuration);

        muzzleFlashRend.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
}
