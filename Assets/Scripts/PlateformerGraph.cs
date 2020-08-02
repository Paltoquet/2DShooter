using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Serialization;

// Inherit our new graph from the base graph type
[JsonOptIn]
public class PlateformerPointNode : PointNode
{

    public NodeData data;

    public PlateformerPointNode(AstarPath astar) : base(astar)
    {
    }

    public PlateformerPointNode(AstarPath astar, NodeData nodeData): base(astar)
    {
        data = nodeData;
    }

    public void setData(NodeData nodeData)
    {
        data = nodeData;
    }

    public static bool shouldJump(PlateformerPointNode current, PlateformerPointNode next)
    {
        bool result = false;
        var jumpableNodes = current.data.jumpableNode;
        foreach(var node in jumpableNodes)
        {
            if(node.GetComponent<NodeData>().m_id == next.data.m_id){
                result = true;
            }
        }
        return result;
    }
}

[JsonOptIn]
public class PlateformerGraph : PointGraph
{
    private Dictionary<int, PlateformerPointNode> m_nodes;
    public Vector3 center;

    protected override PointNode[] CreateNodes(int count)
    {
        var nodes = new PlateformerPointNode[count];

        for (int i = 0; i < nodeCount; i++) nodes[i] = new PlateformerPointNode(active);
        return nodes;
    }

    public override void GetNodes(System.Action<GraphNode> action)
    {
        if (nodes == null) return;
        for (int i = 0; i < nodes.Length; i++)
        {
            // Call the delegate
            action(nodes[i]);
        }
    }

    protected override IEnumerable<Progress> ScanInternal()
    {

        //Creating the nodes
        nodeCount = root.childCount;
        nodes = CreateNodes(nodeCount);
        m_nodes = new Dictionary<int, PlateformerPointNode>(nodeCount);

        int c = 0;
        foreach (Transform child in root)
        {
            PlateformerPointNode pNode = (PlateformerPointNode)(nodes[c]);
            nodes[c].position = (Int3)child.position;
            nodes[c].Walkable = true;
            nodes[c].gameObject = child.gameObject;
            pNode.setData(child.gameObject.GetComponent<NodeData>());
            var data = child.gameObject.GetComponent<NodeData>();
            m_nodes[pNode.data.getId()] = pNode;
            c++;
        }

        //Creating the connections
        foreach (KeyValuePair<int, PlateformerPointNode> nodeIt in m_nodes)
        {
            PlateformerPointNode node = nodeIt.Value;
            NodeData nodeData = node.data;
            var neighbours = nodeData.neighbours;

            var connections = new Connection[neighbours.Length];
            c = 0;
            foreach (GameObject obj in neighbours)
            {
                NodeData n = obj.GetComponent<NodeData>();
                connections[c].node = m_nodes[n.getId()];
                connections[c].cost = (uint)(node.position - connections[c].node.position).costMagnitude; //could add if jumpable
                c++;
            }
            node.connections = connections;
        }

        yield break;
    }
}
