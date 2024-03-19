using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = false;
    public Vector3 vaultPoint;
    public float handDistance = 0.2f;

    public float IKTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)//If you have an animator
        {
            if (ikActive)//If IK is active
            {
                //Right Hand
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, vaultPoint);
                    //animator.SetIKRotation(AvatarIKGoal.RightHand, vaultObject.rotation);
                    
                    //Left Hand
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, vaultPoint);
                    //animator.SetIKRotation(AvatarIKGoal.LeftHand, vaultObject.rotation);
            }
            else //If IK is not active
            {
                //Left Hand
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                
                //Left Hand
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (ikActive)
        {
            IKTime -= Time.deltaTime;
            if (IKTime <= 0)
            {
                ikActive = false;
            }
        }
    }
}
