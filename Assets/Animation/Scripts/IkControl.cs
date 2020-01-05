using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkControl : MonoBehaviour
{
    public Transform rightHandTarget;
    public Transform leftHandtarget;
    public bool ikActive;

    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (m_animator) {
            if (ikActive) {
                m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                m_animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                m_animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandtarget.position);
            }
            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}
