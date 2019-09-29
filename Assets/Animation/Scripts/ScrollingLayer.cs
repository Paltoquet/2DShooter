using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingLayer : MonoBehaviour
{

    //try to apply perspective distorsion 
    //If you apply a displacement Delta on a object close to the camera at distance d
    //The Layer which is at a distance d' of the camera should be applied a displacement Delta'
    //Delta' = Delta * d'/d
    //Now this displacement should be scaled to the actual size of the layer 
    //If the local size of the image is l but in the world its L
    //The displacement become Delta' * l / L
    //displacementRatio becomes : d' / d * l / L

    public float displacementRatio;

    public void MoveLayer(Vector2 cameraMovement)
    {
        Vector2 displacement = cameraMovement * displacementRatio;
        Vector2 translation = displacement - cameraMovement;
        this.gameObject.transform.Translate(new Vector3(translation.x, translation.y, 0.0f));
    }
}
