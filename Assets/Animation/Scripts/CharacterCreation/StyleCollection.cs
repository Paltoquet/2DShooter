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

public class StyleCollection
{
    private string jsonPath = "Resouces/SpriteMesh/SpriteCollection.json";

    private static StyleCollection m_instance = null;

    private List<BodyPart> m_availableBodyParts;
    private Dictionary<BodyPart, AssetBundle> m_meshBundles;
    private Dictionary<BodyPart, List<SpriteMeshDataDescription>> m_meshDescriptions;
    private Dictionary<BodyPart, CustomizerData> m_meshDatas;
    private Dictionary<BodyPart, string> m_bodyPartKey;


    // -------------------------------------------------------- Constructor --------------------------------------------------------
    public StyleCollection()
    {
        m_bodyPartKey = new Dictionary<BodyPart, string>();

        m_availableBodyParts = new List<BodyPart>();

        m_availableBodyParts.Add(BodyPart.Head);
        m_availableBodyParts.Add(BodyPart.Hood);
        m_availableBodyParts.Add(BodyPart.Torso);
        m_availableBodyParts.Add(BodyPart.Pelvis);
        m_availableBodyParts.Add(BodyPart.LShoulder);
        m_availableBodyParts.Add(BodyPart.RShoulder);
        m_availableBodyParts.Add(BodyPart.LElbow);
        m_availableBodyParts.Add(BodyPart.RElbow);
        m_availableBodyParts.Add(BodyPart.LWrist);
        m_availableBodyParts.Add(BodyPart.RWrist);
        m_availableBodyParts.Add(BodyPart.LLeg);
        m_availableBodyParts.Add(BodyPart.RLeg);
        m_availableBodyParts.Add(BodyPart.LBoot);
        m_availableBodyParts.Add(BodyPart.RBoot);

        foreach (var bodyPart in m_availableBodyParts)
        {
            m_bodyPartKey[bodyPart] = bodyPartToString(bodyPart).ToLower();
        }
    }

    public static StyleCollection getInstance()
    {
        if(m_instance == null)
        {
            m_instance = new StyleCollection();
            m_instance.buildAssetsDescription();
            m_instance.buildAssets();
        }
        return m_instance;
    }

    // -------------------------------------------------------- public methods --------------------------------------------------------
    public void buildAssetsDescription()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        m_meshDescriptions = new Dictionary<BodyPart, List<SpriteMeshDataDescription>>();
        m_meshBundles = new Dictionary<BodyPart, AssetBundle>();
        string bundleAssetDirectory = Path.Combine(Application.dataPath, "AssetBundles", "spritemesh");
        
        foreach(var bodyPart in m_availableBodyParts)
        {
            m_meshDescriptions[bodyPart] = loadDescriptions(bodyPart, bundleAssetDirectory);
        }

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

        foreach(var bodyPart in m_availableBodyParts)
        {
            m_meshDatas[bodyPart] = loadAssets(bodyPart);
        }
    }

    public void unloadAllAssets()
    {

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

    public List<BodyPart> getAvailableBodyParts()
    {
        return m_availableBodyParts;
    }

    public SpriteMesh getSpriteMesh(BodyPart bodyPart, string meshId)
    {
        var availableSprites = m_meshDatas[bodyPart];
        return availableSprites.getMesh(meshId);
    }

    public void updateCustomizerData(BodyPart bodyPart, string meshId)
    {
        var availableDatas = m_meshDatas[bodyPart];
        availableDatas.update(meshId);
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
