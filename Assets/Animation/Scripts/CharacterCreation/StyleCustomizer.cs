using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class StyleCustomizer : MonoBehaviour
{
    private StyleCollection collection;

    public GameObject Hat;
    public GameObject Torso;
    public GameObject Pelvis;
    public GameObject leftShoulder;
    public GameObject leftElbow;
    public GameObject rightShoulder;
    public GameObject rightElbow;

    public CustomizerData headDatas;
    public CustomizerData torsoDatas;
    public CustomizerData pelvisDatas;
    //public MultiCustomizerData shoulderDatas;
    //public MultiCustomizerData elbowDatas;
    //public MultiCustomizerData handDatas;

    public CustomizerFieldController headCustomizerFieldController;
    public CustomizerFieldController torsoCustomizerFieldController;
    public CustomizerFieldController pelvisCustomizerFieldController;

    public int currentHatIndex;
    public int currentTorsoIndex;
    public int currentPelvisIndex;

    public StyleCustomizer()
    {
        collection = new StyleCollection();
    }

    private void Awake()
    {
        collection.buildAssetsDescription();
        collection.buildAssets();

        headDatas = collection.getCustomizerData(BodyPart.Head);
        torsoDatas = collection.getCustomizerData(BodyPart.Torso);
        pelvisDatas = collection.getCustomizerData(BodyPart.Pelvis);

        headCustomizerFieldController.customizerData = headDatas;
        torsoCustomizerFieldController.customizerData = torsoDatas;
        pelvisCustomizerFieldController.customizerData = pelvisDatas;
    }

    // Start is called before the first frame update
    void Start()
    {
        headDatas.meshDataChanged().AddListener(changeHat);
        torsoDatas.meshDataChanged().AddListener(changeTorso);
        pelvisDatas.meshDataChanged().AddListener(changePelvis);
        //shoulderDatas.meshDataChanged().AddListener(changeShoulders);
        //elbowDatas.meshDataChanged().AddListener(changeElbows);
        //handDatas.meshDataChanged().AddListener(changePelvis);
    }

    public void parseRessourceFolder()
    {
        collection = new StyleCollection();
        collection.buildAssetsDescription();
    }

    public void changeHat()
    {
        changeSpriteMesh(Hat, headDatas.getCurrentMesh());
    }

    private void changeTorso()
    {
        changeSpriteMesh(Torso, torsoDatas.getCurrentMesh());
    }

    private void changePelvis()
    {
        changeSpriteMesh(Pelvis, pelvisDatas.getCurrentMesh());
    }

    private void changeShoulders()
    {
        /*var leftShoulderData = shoulderDatas.getCurrentMesh(BodyOrientation.Left);
        var rightShoulderData = shoulderDatas.getCurrentMesh(BodyOrientation.Right);
        changeSpriteMesh(leftShoulder, leftShoulderData);
        changeSpriteMesh(rightShoulder, rightShoulderData);*/
    }

    private void changeElbows()
    {
        /*var leftElbowData = elbowDatas.getCurrentMesh(BodyOrientation.Left);
        var rightElbowData = elbowDatas.getCurrentMesh(BodyOrientation.Right);
        changeSpriteMesh(leftElbow, leftElbowData);
        changeSpriteMesh(rightElbow, rightElbowData);*/
    }

    void changeSpriteMesh(GameObject current, SpriteMesh mesh)
    {
        var spriteMeshInstance = current.GetComponent<SpriteMeshInstance>();
        var spriteMeshRenderer = current.GetComponent<SkinnedMeshRenderer>();

        spriteMeshInstance.spriteMesh = mesh;
        spriteMeshRenderer.sharedMesh = mesh.sharedMesh;
    }
}
