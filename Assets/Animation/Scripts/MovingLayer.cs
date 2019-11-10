using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLayer : ScrollingLayer
{
    public float horizontalSpeed;
    public Camera camera;

    private float m_viewPortWidth;

    protected override void Start()
    {
        base.Start();
        m_viewPortWidth = camera.orthographicSize * 2 * camera.aspect; ;
    }

    // Update is called once per frame
    void Update()
    {
        float displacement = horizontalSpeed * Time.deltaTime;
        this.gameObject.transform.Translate(new Vector3(displacement, 0.0f, 0.0f));
        float leftBound = camera.transform.position.x - m_viewPortWidth / 2;
        float rightBound = camera.transform.position.x + m_viewPortWidth / 2;
        checkForHiddenLayer(leftBound, rightBound);
    }
}
