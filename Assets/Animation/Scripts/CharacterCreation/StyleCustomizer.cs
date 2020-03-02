using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public enum BodyOrientation
{
    Left,
    Right
}

public enum BodyPart
{
    Head,
    Torso,
    Pelvis,
    Shoulder,
    Elbow,
    Wrist,
    Leg,
    Boot
}

public class StyleCustomizer : MonoBehaviour
{

    public GameObject Hat;
    public GameObject Torso;
    public GameObject Pelvis;
    public GameObject LeftElbow;
    public GameObject RightElbow;

    public List<SpriteMesh> availableHats;
    public List<SpriteMesh> availableTorsos;
    public List<SpriteMesh> availablePelvis;
    public List<SpriteMesh> availablesLeftShoulders;
    public List<SpriteMesh> availableRightShoulders;

    public CustomizerData headDatas;
    public CustomizerData torsoDatas;
    public CustomizerData pelvisDatas;

    public int currentHatIndex;
    public int currentTorsoIndex;
    public int currentPelvisIndex;

    // Start is called before the first frame update
    void Start()
    {
        headDatas.meshDataChanged().AddListener(changeHat);
        torsoDatas.meshDataChanged().AddListener(changeTorso);
        pelvisDatas.meshDataChanged().AddListener(changePelvis);
    }

    private void changeHat()
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

    void changeSpriteMesh(GameObject current, SpriteMesh mesh)
    {
        var spriteMeshInstance = current.GetComponent<SpriteMeshInstance>();
        var spriteMeshRenderer = current.GetComponent<SkinnedMeshRenderer>();

        spriteMeshInstance.spriteMesh = mesh;
        spriteMeshRenderer.sharedMesh = mesh.sharedMesh;
    }

    public static string toString(BodyPart bodyPart)
    {
        switch (bodyPart)
        {
            case BodyPart.Head:
                return "Head";
            case BodyPart.Torso:
                return "Torso";
            case BodyPart.Pelvis:
                return "Pelvis";
            case BodyPart.Shoulder:
                return "Shoulder";
            case BodyPart.Elbow:
                return "Elbow";
            case BodyPart.Wrist:
                return "Wrist";
            case BodyPart.Leg:
                return "Leg";
            case BodyPart.Boot:
                return "Boot";
        }
        return "Unknown";
    }
}
