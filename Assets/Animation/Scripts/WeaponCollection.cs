using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WeaponCollection : MonoBehaviour
{
    public List<WeaponParams> Weapons;

    public Dictionary<string, WeaponParams> m_datas;
    public static WeaponCollection Instance;

    public WeaponCollection()
    {
        Weapons = new List<WeaponParams>();
        m_datas = new Dictionary<string, WeaponParams>();

        var riffleParams = new WeaponParams("riffle", 30.0f, 30.0f, 0.02f, 100, 0.25f, new Vector2(-1.0f, -0.45f), new Vector2(0.2f, -0.4f));
        var shotgunParams = new WeaponParams("shotgun", 10.0f, 75.0f, 0.02f, 30, 0.8f, new Vector2(-1.0f, -0.45f), new Vector2(1.2f, -0.12f));

        Weapons.Add(riffleParams);
        Weapons.Add(shotgunParams);
    }

    public static WeaponCollection getInstance()
    {
        return Instance;
    }

    public void Awake()
    {
        Instance = this;

        foreach(var weapon in Weapons)
        {
            m_datas[weapon.name] = weapon;
        }
    }

    public WeaponParams getParams(string weapon)
    {
        return m_datas[weapon];
    }
}