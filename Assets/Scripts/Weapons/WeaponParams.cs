using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WeaponParams
{
    public string name;
    public float fireRate = 30.0f;
    public float damage = 2.0f;
    public float hitForce = 20.0f;
    public float range = 40.0f;
    public float recoil = 0.2f;
    public int nbProjectiles = 1;
    public float projectileRadius = 0.0f;
    public Vector2 leftHandleOffset;
    public Vector2 rightHandleOffset;
    public Sprite sprite;
    public GameObject bulletPrefab;

    public WeaponParams(string n, float ft, float dmg, float force, float rg, float rc, Vector2 leftOffset, Vector2 rightOffset)
    {
        name = n;
        fireRate = ft;
        damage = dmg;
        hitForce = force;
        range = rg;
        recoil = rc;
        leftHandleOffset = leftOffset;
        rightHandleOffset = rightOffset;
    }
}
