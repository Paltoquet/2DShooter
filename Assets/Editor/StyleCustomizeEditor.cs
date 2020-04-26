using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Anima2D;
using UnityEditor;

[CustomEditor(typeof(StyleCustomizer))]
public class StyleCustomizerEditor : Editor
{

    // OnInspector GUI
    public override void OnInspectorGUI()
    {
        // ---------------------------------------------------------- Avaible MeshIds ----------------------------------------------------------
        StyleCustomizer data = (StyleCustomizer)target;


        base.OnInspectorGUI();

        if (GUILayout.Button("Parse Folder"))
        {
            data.parseRessourceFolder();
            data.changeHat();
        }

        /*GUILayout.Space(10f);


        GUILayout.Label("Available assets", EditorStyles.boldLabel);

        displaySpriteList("Head Datas:", ref data.headDatas, ref showHeadDatas);
        displaySpriteList("Body Datas:", ref data.bodyDatas, ref showBodyDatas);
        displaySpriteList("Pelvis Datas:", ref data.pelvisDatas, ref showPelvisDatas);

        if (GUILayout.Button("Parse Folder"))
        {
            
        }*/

        /*showHeadDatas = EditorGUILayout.Foldout(showHeadDatas, "Head Datas:");
        if (showHeadDatas) {
            var availableHeads = data.headDatas;
        
            for (int i=0; i<availableHeads.Count; i++)
            {
                if (availableHeads[i] == null)
                {
                    availableHeads[i] = new SpriteMeshData();
                }
            }

            int currentHeadSize = Mathf.Max(0, EditorGUILayout.DelayedIntField("size", availableHeads.Count));
            while (currentHeadSize < availableHeads.Count)
                availableHeads.RemoveAt(availableHeads.Count - 1);
            while (currentHeadSize > availableHeads.Count)
                availableHeads.Add(new SpriteMeshData());


            for (int i = 0; i < availableHeads.Count; i++)
            {

                EditorGUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                availableHeads[i].name = EditorGUILayout.TextField("Name: ", availableHeads[i].name);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                availableHeads[i].mesh = (SpriteMesh)EditorGUILayout.ObjectField("Mesh: ", availableHeads[i].mesh, typeof(SpriteMesh), true);
                GUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                GUILayout.Space(10f);
            }*/
    }

    private void displaySpriteList(string listName, ref List<SpriteMeshDataDescription> datas, ref bool showData)
    {
        showData = EditorGUILayout.Foldout(showData, listName);

        if (showData)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i] == null)
                {
                    datas[i] = new SpriteMeshDataDescription("" , "");
                }
            }

            int currentListSize = Mathf.Max(0, EditorGUILayout.DelayedIntField("size", datas.Count));
            while (currentListSize < datas.Count)
                datas.RemoveAt(datas.Count - 1);
            while (currentListSize > datas.Count)
                datas.Add(new SpriteMeshDataDescription("", ""));


            for (int i = 0; i < datas.Count; i++)
            {

                EditorGUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                datas[i].fullName = EditorGUILayout.TextField("Name: ", datas[i].fullName);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                //datas[i].mesh = (SpriteMesh)EditorGUILayout.ObjectField("Mesh: ", datas[i].mesh, typeof(SpriteMesh), true);
                GUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                GUILayout.Space(10f);
            }
        }
    }
}
