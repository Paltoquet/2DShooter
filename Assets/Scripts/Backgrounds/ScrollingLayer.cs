using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System;

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

    private List<SpriteRenderer> m_sprites;
    SpriteRenderer m_leftSprite;
    SpriteRenderer m_rightSprite;
    protected float m_leftBound;
    protected float m_rightBound;

    // Start is called before the first frame update
    protected virtual void OnEnable()
    {
        m_sprites = new List<SpriteRenderer>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            SpriteRenderer r = child.GetComponent<SpriteRenderer>();
            m_sprites.Add(r);
        }

        m_sprites = m_sprites.OrderBy(
            t => t.transform.position.x
        ).ToList();
    }

    public void MoveLayer(Vector2 cameraMovement)
    {
        Vector2 displacement = cameraMovement * displacementRatio;
        Vector2 translation = displacement - cameraMovement;
        this.gameObject.transform.Translate(new Vector3(translation.x, translation.y, 0.0f));
    }

    public void checkForHiddenLayer(float leftLimit, float rightLimit)
    {

        updateBounds();

        while(rightLimit > m_rightBound)
        {
            //Move Sprite to the right
            float leftSpriteHalfWidth = m_leftSprite.bounds.size.x / 2;
            float rightSpriteHalfWidth = m_rightSprite.bounds.size.x / 2;
            float offset = leftSpriteHalfWidth + rightSpriteHalfWidth;
            m_sprites.Remove(m_leftSprite);
            m_sprites.Add(m_leftSprite);
            m_leftSprite.transform.position = m_rightSprite.transform.position + new Vector3(offset, 0, 0);
            updateBounds();
        }

        while(leftLimit < m_leftBound)
        {
            float leftSpriteHalfWidth = m_leftSprite.bounds.size.x / 2;
            float rightSpriteHalfWidth = m_rightSprite.bounds.size.x / 2;
            float offset = leftSpriteHalfWidth + rightSpriteHalfWidth;
            m_sprites.Remove(m_rightSprite);
            m_sprites.Insert(0, m_rightSprite);
            m_rightSprite.transform.position = m_leftSprite.transform.position - new Vector3(offset, 0, 0);
            updateBounds();
        }
    }

    private void updateBounds()
    {
        if (m_sprites.Count > 0)
        {
            m_leftSprite = m_sprites[0];
            m_rightSprite = m_sprites[m_sprites.Count() - 1];
            float leftSpriteHalfWidth = m_leftSprite.bounds.size.x / 2;
            float rightSpriteHalfWidth = m_rightSprite.bounds.size.x / 2;
            m_leftBound = m_leftSprite.transform.position.x - leftSpriteHalfWidth;
            m_rightBound = m_rightSprite.transform.position.x + rightSpriteHalfWidth;
        }
    }
}
