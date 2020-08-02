using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData : MonoBehaviour
{
    static int counter;

    public bool jumpable;
    public GameObject[] neighbours;
    public GameObject[] jumpableNode;
    public int m_id;

    public NodeData() : base()
    {
        m_id = counter;
        counter++;
    }

    public int getId()
    {
        return m_id;
    }
}
