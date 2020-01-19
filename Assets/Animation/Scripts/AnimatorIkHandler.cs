using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class AnimatorIkHandler : MonoBehaviour
{

    public Bone2D leftHand;
    public Bone2D rightHand;

    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        m_animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.transform.position);
        m_animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
