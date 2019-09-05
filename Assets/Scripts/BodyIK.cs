using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class BodyIK : MonoBehaviour
{
    [SerializeField]
    private float leftHandPosWeight, leftHandRotWeight,
        rightHandPosWeight, rightHandRotWeight;

    [SerializeField]
    private Transform leftHandObj, rightHandObj;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.Update(0);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(leftHandObj != null)
        { 
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandPosWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
        }

        if (rightHandObj != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandPosWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandRotWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
        }
    }
}
