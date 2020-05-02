using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class StyleCustomizer : MonoBehaviour
{
    private StyleCollection collection;

    public GameObject Hood;
    public GameObject Head;
    public GameObject Torso;
    public GameObject Pelvis;
    public GameObject leftShoulder;
    public GameObject leftElbow;
    public GameObject leftWrist;
    public GameObject rightShoulder;
    public GameObject rightElbow;
    public GameObject rightWrist;
    public GameObject leftLeg;
    public GameObject leftBoot;
    public GameObject rightLeg;
    public GameObject rightBoot;

    public CustomizerFieldController hoodCustomizerFieldController;
    public CustomizerFieldController headCustomizerFieldController;
    public CustomizerFieldController armCustomizerFieldController;
    public CustomizerFieldController torsoCustomizerFieldController;
    public CustomizerFieldController pelvisCustomizerFieldController;
    public CustomizerFieldController legCustomizerFieldController;

    private CustomizerData m_hoodDatas;
    private CustomizerData m_headDatas;

    private CustomizerData m_leftShoulderData;
    private CustomizerData m_leftElbowData;
    private CustomizerData m_leftWristData;
    private CustomizerData m_rightShoulderData;
    private CustomizerData m_rightElbowData;
    private CustomizerData m_rightWristData;

    private CustomizerData m_torsoDatas;
    private CustomizerData m_pelvisDatas;

    private CustomizerData m_leftLegData;
    private CustomizerData m_leftBootData;
    private CustomizerData m_rightLegData;
    private CustomizerData m_rightBootData;

    public int currentHatIndex;
    public int currentTorsoIndex;
    public int currentPelvisIndex;

    public StyleCustomizer()
    {

    }

    private void Awake()
    {
        collection = StyleCollection.getInstance();

        m_hoodDatas = collection.getCustomizerData(BodyPart.Hood);
        m_headDatas = collection.getCustomizerData(BodyPart.Head);
        m_torsoDatas = collection.getCustomizerData(BodyPart.Torso);

        m_leftShoulderData = collection.getCustomizerData(BodyPart.LShoulder);
        m_leftElbowData = collection.getCustomizerData(BodyPart.LElbow);
        m_leftWristData = collection.getCustomizerData(BodyPart.LWrist);
        m_rightShoulderData = collection.getCustomizerData(BodyPart.RShoulder);
        m_rightElbowData = collection.getCustomizerData(BodyPart.RElbow);
        m_rightWristData = collection.getCustomizerData(BodyPart.RWrist);

        m_pelvisDatas = collection.getCustomizerData(BodyPart.Pelvis);

        m_leftLegData = collection.getCustomizerData(BodyPart.LLeg);
        m_leftBootData = collection.getCustomizerData(BodyPart.LBoot);
        m_rightLegData = collection.getCustomizerData(BodyPart.RLeg);
        m_rightBootData = collection.getCustomizerData(BodyPart.RBoot);

        hoodCustomizerFieldController.addCustomizerData(m_hoodDatas);
        headCustomizerFieldController.addCustomizerData(m_headDatas);
        armCustomizerFieldController.addCustomizerData(m_leftShoulderData);
        armCustomizerFieldController.addCustomizerData(m_leftElbowData);
        armCustomizerFieldController.addCustomizerData(m_leftWristData);
        armCustomizerFieldController.addCustomizerData(m_rightShoulderData);
        armCustomizerFieldController.addCustomizerData(m_rightElbowData);
        armCustomizerFieldController.addCustomizerData(m_rightWristData);
        torsoCustomizerFieldController.addCustomizerData(m_torsoDatas);
        pelvisCustomizerFieldController.addCustomizerData(m_pelvisDatas);
        legCustomizerFieldController.addCustomizerData(m_leftLegData);
        legCustomizerFieldController.addCustomizerData(m_leftBootData);
        legCustomizerFieldController.addCustomizerData(m_rightLegData);
        legCustomizerFieldController.addCustomizerData(m_rightBootData);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_hoodDatas.meshDataChanged().AddListener(changeHood);
        m_headDatas.meshDataChanged().AddListener(changeHat);

        m_leftShoulderData.meshDataChanged().AddListener(changeLeftShoulder);
        m_leftElbowData.meshDataChanged().AddListener(changeLeftElbow);
        m_leftWristData.meshDataChanged().AddListener(changeLeftWrist);

        m_rightShoulderData.meshDataChanged().AddListener(changeRightShoulder);
        m_rightElbowData.meshDataChanged().AddListener(changeRightElbow);
        m_rightWristData.meshDataChanged().AddListener(changeRightWrist);

        m_torsoDatas.meshDataChanged().AddListener(changeTorso);
        m_pelvisDatas.meshDataChanged().AddListener(changePelvis);

        m_leftLegData.meshDataChanged().AddListener(changeLeftLeg);
        m_leftBootData.meshDataChanged().AddListener(changeLeftBoot);
        m_rightLegData.meshDataChanged().AddListener(changeRightLeg);
        m_rightBootData.meshDataChanged().AddListener(changeRightBoot);
    }

    public void parseRessourceFolder()
    {
        
    }

    private void OnDestroy()
    {
        foreach (var bodyPartAvailable in collection.getAvailableBodyParts())
        {
            var key = StyleCollection.bodyPartToString(bodyPartAvailable);
            var spriteMeshDatas = collection.getCustomizerData(bodyPartAvailable);
            var currentId = spriteMeshDatas.getCurrentMeshId();
            PlayerPrefs.SetString(key, currentId);
        }
        collection.unloadAllAssets();
    }


    // -------------------------------------------------------- private methods --------------------------------------------------------

    private void initializeDatas()
    {

    }

    private void changeHood()
    {
        changeSpriteMesh(Hood, m_hoodDatas.getCurrentMesh());
    }

    private void changeHat()
    {
        changeSpriteMesh(Head, m_headDatas.getCurrentMesh());
    }

    private void changeTorso()
    {
        changeSpriteMesh(Torso, m_torsoDatas.getCurrentMesh());
    }

    private void changePelvis()
    {
        changeSpriteMesh(Pelvis, m_pelvisDatas.getCurrentMesh());
    }

    private void changeLeftShoulder()
    {
        changeSpriteMesh(leftShoulder, m_leftShoulderData.getCurrentMesh());
    }

    private void changeLeftElbow()
    {
        changeSpriteMesh(leftElbow, m_leftElbowData.getCurrentMesh());
    }

    private void changeLeftWrist()
    {
        changeSpriteMesh(leftWrist, m_leftWristData.getCurrentMesh());
    }

    private void changeRightShoulder()
    {
        changeSpriteMesh(rightShoulder, m_rightShoulderData.getCurrentMesh());
    }

    private void changeRightElbow()
    {
        changeSpriteMesh(rightElbow, m_rightElbowData.getCurrentMesh());
    }

    private void changeRightWrist()
    {
        changeSpriteMesh(rightWrist, m_rightWristData.getCurrentMesh());
    }

    private void changeLeftLeg()
    {
        changeSpriteMesh(leftLeg, m_leftLegData.getCurrentMesh());
    }

    private void changeLeftBoot()
    {
        changeSpriteMesh(leftBoot, m_leftBootData.getCurrentMesh());
    }

    private void changeRightLeg()
    {
        changeSpriteMesh(rightLeg, m_rightLegData.getCurrentMesh());
    }

    private void changeRightBoot()
    {
        changeSpriteMesh(rightBoot, m_rightBootData.getCurrentMesh());
    }

    void changeSpriteMesh(GameObject current, SpriteMesh mesh)
    {
        var spriteMeshInstance = current.GetComponent<SpriteMeshInstance>();
        var spriteMeshRenderer = current.GetComponent<SkinnedMeshRenderer>();

        spriteMeshInstance.spriteMesh = mesh;
        spriteMeshRenderer.sharedMesh = mesh.sharedMesh;
    }
}
