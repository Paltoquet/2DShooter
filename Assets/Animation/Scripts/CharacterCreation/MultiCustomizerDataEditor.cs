using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Anima2D;

[CustomEditor(typeof(MultiCustomizerData))]
public class MultiCustomizerDataEditor : Editor
{

    private int m_leftBodyPartIndex = 0;
    private int m_rightBodyPartIndex = 0;

    // OnInspector GUI
    public override void OnInspectorGUI()
    {
        BodyPart[] availableBodyParts = { BodyPart.Head, BodyPart.Torso, BodyPart.Pelvis, BodyPart.Shoulder, BodyPart.Elbow, BodyPart.Wrist, BodyPart.Leg, BodyPart.Boot };
        string[] bodyPartOptions = new string[availableBodyParts.Length];
        for(int i = 0; i < availableBodyParts.Length; i++){
            bodyPartOptions[i] = StyleCustomizer.toString(availableBodyParts[i]);
        }

        MultiCustomizerData data = (MultiCustomizerData) target;

        GUILayout.Space(10f);
        GUILayout.Label("Available sets", EditorStyles.boldLabel);

        var availableMeshIds = data.availableMesheIds;
        int currentMeshIdsSize = Mathf.Max(0, EditorGUILayout.DelayedIntField("size", availableMeshIds.Count));
        while (currentMeshIdsSize < availableMeshIds.Count)
            availableMeshIds.RemoveAt(availableMeshIds.Count - 1);
        while (currentMeshIdsSize > availableMeshIds.Count)
            availableMeshIds.Add(null);

        for (int i = 0; i < availableMeshIds.Count; i++)
        {
            availableMeshIds[i] = (string)EditorGUILayout.TextField(availableMeshIds[i]);
        }

        GUILayout.Space(10f);
        GUILayout.Label("Left Body Parts", EditorStyles.boldLabel);
        int currentLeftMeshSize = Mathf.Max(0, EditorGUILayout.DelayedIntField("nb parts", data.availableMeshes[BodyOrientation.Left].Count));

        for (int i = 0; i < currentLeftMeshSize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            BodyPart bodyKey = availableBodyParts[0];
            SpriteMesh BodyType = new SpriteMesh();

            m_leftBodyPartIndex = EditorGUILayout.Popup(m_leftBodyPartIndex, bodyPartOptions);
            bodyKey = availableBodyParts[m_leftBodyPartIndex];
            BodyType = (SpriteMesh)EditorGUILayout.ObjectField(BodyType, typeof(SpriteMesh), true);
            data.availableMeshes[BodyOrientation.Left][bodyKey] = BodyType;
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10f);
        GUILayout.Label("Right Body Parts", EditorStyles.boldLabel);

        base.DrawDefaultInspector();
    }
}
