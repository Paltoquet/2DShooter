using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class IkFollow : MonoBehaviour
{

    public Bone2D target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = target.transform.position;
    }
}
