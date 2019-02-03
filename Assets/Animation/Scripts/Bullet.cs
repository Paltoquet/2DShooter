﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int speed = 250;
    public float lifeDuration = 1.0f;

    private Vector3 m_direction;


    private void OnEnable()
    {
        StartCoroutine(AutoDestroy(lifeDuration));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    IEnumerator AutoDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public void setDirection(Vector3 direction)
    {
        m_direction = direction;
    }
}
