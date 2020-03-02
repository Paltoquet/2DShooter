using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCustomizerData : MonoBehaviour
{

    public List<string> availableMesheIds;
    public Dictionary<BodyOrientation, Dictionary<BodyPart, SpriteMesh>> availableMeshes;

    public MultiCustomizerData()
    {
        availableMesheIds = new List<string>();
        availableMeshes = new Dictionary<BodyOrientation, Dictionary<BodyPart, SpriteMesh>>();
        availableMeshes.Add(BodyOrientation.Left, new Dictionary<BodyPart, SpriteMesh>());
        availableMeshes.Add(BodyOrientation.Right, new Dictionary<BodyPart, SpriteMesh>());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
