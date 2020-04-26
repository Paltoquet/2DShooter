using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Anima2D;

[System.Serializable]
public enum BodyOrientation
{
    Left,
    Right
}

[System.Serializable]
public enum BodyPart
{
    Head,
    Hood,
    Torso,
    Pelvis,
    LShoulder,
    RShoulder,
    LElbow,
    RElbow,
    LWrist,
    RWrist,
    LLeg,
    RLeg,
    LBoot,
    RBoot
}

public class SpriteMeshDataDescription
{
    public string fullName;
    public string assetPath;

    public SpriteMeshDataDescription(string fName, string path)
    {
        fullName = fName;
        assetPath = path;
    }
}

public class StyleCollection : MonoBehaviour
{
    private string jsonPath = "Resouces/SpriteMesh/SpriteCollection.json";

    private Dictionary<BodyPart, List<SpriteMeshDataDescription>> m_meshDescriptions;
    private Dictionary<BodyPart, AssetBundle> m_meshBundles;
    private Dictionary<BodyPart, CustomizerData> m_meshDatas;
    private Dictionary<BodyPart, string> m_bodyPartKey;


    // -------------------------------------------------------- Constructor --------------------------------------------------------
    public StyleCollection()
    {
        m_bodyPartKey = new Dictionary<BodyPart, string>();


        m_bodyPartKey[BodyPart.Head]        = bodyPartToString(BodyPart.Head).ToLower();
        m_bodyPartKey[BodyPart.Hood]        = bodyPartToString(BodyPart.Hood).ToLower();
        m_bodyPartKey[BodyPart.Torso]       = bodyPartToString(BodyPart.Torso).ToLower();
        m_bodyPartKey[BodyPart.Pelvis]      = bodyPartToString(BodyPart.Pelvis).ToLower();
        m_bodyPartKey[BodyPart.LShoulder]   = bodyPartToString(BodyPart.LShoulder).ToLower();
        m_bodyPartKey[BodyPart.RShoulder]   = bodyPartToString(BodyPart.RShoulder).ToLower();
        m_bodyPartKey[BodyPart.LElbow]      = bodyPartToString(BodyPart.LElbow).ToLower();
        m_bodyPartKey[BodyPart.RElbow]      = bodyPartToString(BodyPart.RElbow).ToLower();
        m_bodyPartKey[BodyPart.LWrist]      = bodyPartToString(BodyPart.LWrist).ToLower();
        m_bodyPartKey[BodyPart.RWrist]      = bodyPartToString(BodyPart.RWrist).ToLower();
        m_bodyPartKey[BodyPart.LLeg]        = bodyPartToString(BodyPart.LLeg).ToLower();
        m_bodyPartKey[BodyPart.RLeg]        = bodyPartToString(BodyPart.RLeg).ToLower();
        m_bodyPartKey[BodyPart.LBoot]       = bodyPartToString(BodyPart.LBoot).ToLower();
        m_bodyPartKey[BodyPart.RBoot]       = bodyPartToString(BodyPart.RBoot).ToLower();
    }

    // -------------------------------------------------------- abstract methods --------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //var fileContent = File.ReadAllText(path);
    }

    // -------------------------------------------------------- public methods --------------------------------------------------------
    public void buildAssetsDescription()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        m_meshDescriptions = new Dictionary<BodyPart, List<SpriteMeshDataDescription>>();
        m_meshBundles = new Dictionary<BodyPart, AssetBundle>();
        string bundleAssetDirectory = Path.Combine(Application.dataPath, "AssetBundles", "spritemesh");
        
        m_meshDescriptions[BodyPart.Head]       = loadDescriptions(BodyPart.Head,      bundleAssetDirectory);
        m_meshDescriptions[BodyPart.Hood]       = loadDescriptions(BodyPart.Hood,      bundleAssetDirectory);
        m_meshDescriptions[BodyPart.Torso]      = loadDescriptions(BodyPart.Torso,     bundleAssetDirectory);
        m_meshDescriptions[BodyPart.Pelvis]     = loadDescriptions(BodyPart.Pelvis,    bundleAssetDirectory);
        m_meshDescriptions[BodyPart.LShoulder]  = loadDescriptions(BodyPart.LShoulder, bundleAssetDirectory);
        m_meshDescriptions[BodyPart.RShoulder]  = loadDescriptions(BodyPart.RShoulder, bundleAssetDirectory);
        m_meshDescriptions[BodyPart.LElbow]     = loadDescriptions(BodyPart.LElbow,    bundleAssetDirectory);
        m_meshDescriptions[BodyPart.RElbow]     = loadDescriptions(BodyPart.RElbow,    bundleAssetDirectory);
        m_meshDescriptions[BodyPart.LWrist]     = loadDescriptions(BodyPart.LWrist,    bundleAssetDirectory);
        m_meshDescriptions[BodyPart.RWrist]     = loadDescriptions(BodyPart.RWrist,    bundleAssetDirectory);
        m_meshDescriptions[BodyPart.LLeg]       = loadDescriptions(BodyPart.LLeg,      bundleAssetDirectory);
        m_meshDescriptions[BodyPart.RLeg]       = loadDescriptions(BodyPart.RLeg,      bundleAssetDirectory);
        m_meshDescriptions[BodyPart.LBoot]      = loadDescriptions(BodyPart.LBoot,     bundleAssetDirectory);
        m_meshDescriptions[BodyPart.RBoot]      = loadDescriptions(BodyPart.RBoot,     bundleAssetDirectory);

        /*List<SpriteMeshDataDescription> sprites = new List<SpriteMeshDataDescription>();
        var setting = new JsonSerializerSettings();
        setting.Formatting = Formatting.Indented;

        var resourcesFolderPath = Application.dataPath;
        var spriteMeshFolder = "SpriteMesh";
        var spriteMeshFolderPath = Path.Combine(resourcesFolderPath, "Resources", spriteMeshFolder);

        if (Directory.Exists(spriteMeshFolderPath))
        {
            string[] subdirectories = Directory.GetDirectories(spriteMeshFolderPath);

            foreach(var subdirectory in subdirectories)
            {
                string displayName = getLastPartOfUri(subdirectory);
                string[] files = Directory.GetFiles(subdirectory);
                foreach(var file in files)
                {
                    if (file.EndsWith("asset"))
                    {
                        int PathBeguining = file.IndexOf(spriteMeshFolder);
                        string filePath = file.Substring(PathBeguining);
                        string fullName = getLastPartOfUri(file);
                        SpriteMeshDataDescription sprite = new SpriteMeshDataDescription(file, displayName, file);
                        Debug.Log("spriteDescription :" + displayName + ", name " + fullName + ", path " + filePath);
                        sprites.Add(sprite);
                    }
                }
            }

            var spriteDescription = sprites[0];
            SpriteMesh mesh = Resources.Load<SpriteMesh>(spriteDescription.assetPath);

            Debug.Log("All directories " + subdirectories);
        }*/
    }

    public void buildAssets()
    {
        m_meshDatas = new Dictionary<BodyPart, CustomizerData>();

        m_meshDatas[BodyPart.Head]      = loadAssets(BodyPart.Head);
        m_meshDatas[BodyPart.Hood]      = loadAssets(BodyPart.Hood);
        m_meshDatas[BodyPart.Torso]     = loadAssets(BodyPart.Torso);
        m_meshDatas[BodyPart.Pelvis]    = loadAssets(BodyPart.Pelvis);
        m_meshDatas[BodyPart.LShoulder] = loadAssets(BodyPart.LShoulder);
        m_meshDatas[BodyPart.RShoulder] = loadAssets(BodyPart.RShoulder);
        m_meshDatas[BodyPart.LElbow]    = loadAssets(BodyPart.LElbow);
        m_meshDatas[BodyPart.RElbow]    = loadAssets(BodyPart.RElbow);
        m_meshDatas[BodyPart.LWrist]    = loadAssets(BodyPart.LWrist);
        m_meshDatas[BodyPart.RWrist]    = loadAssets(BodyPart.RWrist);
        m_meshDatas[BodyPart.LLeg]      = loadAssets(BodyPart.LLeg);
        m_meshDatas[BodyPart.RLeg]      = loadAssets(BodyPart.RLeg);
        m_meshDatas[BodyPart.LBoot]     = loadAssets(BodyPart.LBoot);
        m_meshDatas[BodyPart.RBoot]     = loadAssets(BodyPart.RBoot);
    }

    public List<SpriteMeshDataDescription> loadDescriptions(BodyPart bodyPart, string bundleAssetDirectory)
    {
        string bundleName = m_bodyPartKey[bodyPart];
        List<SpriteMeshDataDescription> result = new List<SpriteMeshDataDescription>();

        var bundle = AssetBundle.LoadFromFile(Path.Combine(bundleAssetDirectory, bundleName));
        m_meshBundles[bodyPart] = bundle;
        if (bundle == null) {
            Debug.Log("Failed to load AssetBundle " + bundleName);

        } else {
            var assets = bundle.GetAllAssetNames();
            foreach (var name in assets) {
                var meshDescription = new SpriteMeshDataDescription(getLastPartOfUri(name), name);
                result.Add(meshDescription);
            }
        }
        return result;
    }

    public CustomizerData loadAssets(BodyPart bodyPart)
    {
        List<string> availableAssetNames = new List<string>();
        List<SpriteMesh> availableSpriteMeshes = new List<SpriteMesh>();

        var availableDescriptions = m_meshDescriptions[bodyPart];
        var assetBundle = m_meshBundles[bodyPart];

        foreach (var description in availableDescriptions)
        {
            var assetName = description.fullName;
            var spriteMesh = assetBundle.LoadAsset<SpriteMesh>(assetName);
            if(spriteMesh == null) {
                Debug.Log("Failed to load asset " + assetName);
            }
            availableAssetNames.Add(assetName);
            availableSpriteMeshes.Add(spriteMesh);
        }

        CustomizerData result = new CustomizerData(availableAssetNames, availableSpriteMeshes);
        return result;
    }

    public CustomizerData getCustomizerData(BodyPart bodyPart)
    {
        return m_meshDatas[bodyPart];
    }
    // -------------------------------------------------------- private Methods --------------------------------------------------------
    public static string bodyPartToString(BodyPart bodyPart)
    {
        switch (bodyPart)
        {
            case BodyPart.Hood:
                return "Hood";
            case BodyPart.Head:
                return "Head";
            case BodyPart.Torso:
                return "Torso";
            case BodyPart.Pelvis:
                return "Pelvis";
            case BodyPart.LShoulder:
                return "LeftShoulder";
            case BodyPart.RShoulder:
                return "RightShoulder";
            case BodyPart.LElbow:
                return "LeftElbow";
            case BodyPart.RElbow:
                return "RightElbow";
            case BodyPart.LWrist:
                return "LeftWrist";
            case BodyPart.RWrist:
                return "RightWrist";
            case BodyPart.LLeg:
                return "LeftLeg";
            case BodyPart.RLeg:
                return "RightLeg";
            case BodyPart.LBoot:
                return "LeftBoot";
            case BodyPart.RBoot:
                return "RightBoot";
        }
        return "Unknown";
    }

    // Assert the path shouldn' contain a . (please)
    public string getLastPartOfUri(string path)
    {
        string result = "";
        var universalPath = path.Replace(@"\", "/");
        int pathIndex = universalPath.LastIndexOf("/");
        if (pathIndex >= 0)
        {
            result = universalPath.Substring(pathIndex + 1);
            int extensionIndex = result.LastIndexOf(".");
            if(extensionIndex >= 0)
            {
                result = result.Substring(0, extensionIndex);
            }
        }
        return result;
    }
}
