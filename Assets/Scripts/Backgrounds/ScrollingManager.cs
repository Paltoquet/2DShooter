using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingManager : MonoBehaviour
{

    public List<ScrollingLayer> m_layers;
    public Camera m_camera;

    private List<MovingLayer> m_movingLayers;
    private Vector3 m_oldCameraPosition;
    private float m_viewPortWidth;
    private float m_leftBound;
    private float m_rightBound;

    // Start is called before the first frame update
    void Start()
    {
        m_oldCameraPosition = m_camera.transform.position;
        m_viewPortWidth = m_camera.orthographicSize * 2 * m_camera.aspect;
        foreach(var scrollingLayer in m_layers)
        {

        }
        updateBounds();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 updatedCameraPosition = m_camera.transform.position;
        Vector2 displacement = updatedCameraPosition - m_oldCameraPosition;
        if (displacement != Vector2.zero) {
            foreach (var layer in m_layers)
            {
                layer.MoveLayer(displacement);
            }
            m_oldCameraPosition = updatedCameraPosition;
            updateBounds();
        }
    }

    private void updateBounds()
    {
        m_leftBound = m_oldCameraPosition.x  - m_viewPortWidth / 2;
        m_rightBound = m_oldCameraPosition.x + m_viewPortWidth / 2;
        foreach (var layer in m_layers)
        {
            layer.checkForHiddenLayer(m_leftBound, m_rightBound);
        }
    }
}
