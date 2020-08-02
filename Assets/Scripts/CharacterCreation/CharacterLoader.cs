using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{

    public StyleCollection collection;
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

    private Dictionary<BodyPart, string> m_meshIds;
    private string defaultPart;

    public CharacterLoader()
    {
        m_meshIds = new Dictionary<BodyPart, string>();
        defaultPart = "rogue";
    }

    // Start is called before the first frame update
    void Start()
    {
        collection = StyleCollection.getInstance();

        foreach (var bodyPartAvailable in collection.getAvailableBodyParts())
        {
            var key = StyleCollection.bodyPartToString(bodyPartAvailable);
            var currentId = PlayerPrefs.GetString(key);
            if(currentId == "")
            {
                currentId = defaultPart;
            }
            m_meshIds[bodyPartAvailable] = currentId;
        }

        loadSprite(Hood,            BodyPart.Hood);
        loadSprite(Head,            BodyPart.Head);
        loadSprite(Torso,           BodyPart.Torso);
        loadSprite(Pelvis,          BodyPart.Pelvis);
        loadSprite(leftShoulder,    BodyPart.LShoulder);
        loadSprite(rightShoulder,   BodyPart.RShoulder);
        loadSprite(leftElbow,       BodyPart.LElbow);
        loadSprite(rightElbow,      BodyPart.RElbow);
        loadSprite(leftWrist,       BodyPart.LWrist);
        loadSprite(rightWrist,      BodyPart.RWrist);
        loadSprite(leftLeg,         BodyPart.LLeg);
        loadSprite(rightLeg,        BodyPart.RLeg);
        loadSprite(leftBoot,        BodyPart.LBoot);
        loadSprite(rightBoot,       BodyPart.RBoot);
    }

    void loadSprite(GameObject source, BodyPart bodyPart)
    {
        var meshId = m_meshIds[bodyPart];
        var currentMesh = collection.getSpriteMesh(bodyPart, meshId);
        if (currentMesh != null)
        {
            changeSpriteMesh(source, currentMesh);
        }
        collection.updateCustomizerData(bodyPart, meshId);
    }

    void changeSpriteMesh(GameObject current, SpriteMesh mesh)
    {
        var spriteMeshInstance = current.GetComponent<SpriteMeshInstance>();
        var spriteMeshRenderer = current.GetComponent<SkinnedMeshRenderer>();

        spriteMeshInstance.spriteMesh = mesh;
        spriteMeshRenderer.sharedMesh = mesh.sharedMesh;
    }
}
