using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class JuiceBehaviours : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public float currentSpeed;
    public AnimationCurve bobSpeedMutliplier;
    public float currentBobSpeed;

    public bool isOnGround_;

    public Animator walkAni;

    public float verticalVelocity;
    public float lowImpactThreshold;
    public float highImpactTheshold;
    bool isDueForImpact;

    void Update()
    {
        //set parameters
        currentSpeed = playerMovement.HorizontalVelocityf;
        isOnGround_ = playerMovement.isOnGround;
        verticalVelocity = playerMovement.verticalVelocity;

        //set bobSpeed
        currentBobSpeed = bobSpeedMutliplier.Evaluate(currentSpeed);

        //if on the ground set the bob speed to the bob speed
        if (isOnGround_)
            walkAni.SetFloat("walkSpeed", currentBobSpeed);
        else
            walkAni.SetFloat("walkSpeed", 0);

        if (currentSpeed > 0.1f)
            walkAni.SetBool("moving", true);
        else
            walkAni.SetBool("moving", false);

        if (isOnGround_ && isDueForImpact)
        {
            isDueForImpact = false;
            playImpact();
        }
        if (!isOnGround_ && !isDueForImpact)
        {
            isDueForImpact = true;
        }
    }

    void playImpact()
    {
        if (Mathf.Abs(verticalVelocity) >= highImpactTheshold)
        {
            AudioManager.instance.GenerateSound(AudioReference.instance.landHard, Vector3.zero);
            walkAni.SetTrigger("hard Landing");
        }
        else if (Mathf.Abs(verticalVelocity) >= lowImpactThreshold)
        {
            AudioManager.instance.GenerateSound(AudioReference.instance.landMedium, Vector3.zero);
            walkAni.SetTrigger("medium Landing");
        }
        else
        {
            AudioManager.instance.GenerateSound(AudioReference.instance.landSoft, Vector3.zero);
            walkAni.SetTrigger("soft Landing");
        }
        Debug.Log(Mathf.Abs(verticalVelocity));

    }

    public void playfootStep()
    {
        AudioManager.instance.GenerateSound(AudioReference.instance.walk, Vector3.zero);
    }

    public void playSlideSound()
    {
        AudioManager.instance.GenerateSound(AudioReference.instance.slide, Vector3.zero);
    }
}
