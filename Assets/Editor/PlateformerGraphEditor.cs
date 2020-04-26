using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;
using UnityEditor;

[CustomGraphEditor(typeof(PlateformerGraph), "PlateformerGraph")]
public class PlateformerGraphEditor : GraphEditor
{
    // Here goes the GUI
    public override void OnInspectorGUI(NavGraph target)
    {
        var graph = target as PlateformerGraph;
        graph.root = EditorGUILayout.ObjectField("root", graph.root, typeof(Transform), true) as Transform;
    }
}