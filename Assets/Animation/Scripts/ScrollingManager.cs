using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingManager : MonoBehaviour
{

    public List<ScrollingLayer> m_layers;
    public Camera m_camera;

    private Vector3 m_oldCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_oldCameraPosition = m_camera.transform.position;
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
        }
    }
}
