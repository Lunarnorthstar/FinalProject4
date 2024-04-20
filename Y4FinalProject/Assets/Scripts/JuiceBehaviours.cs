using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
//using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;
public class JuiceBehaviours : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public bool canBob;
    public float currentSpeed;
    public AnimationCurve bobSpeedMutliplier;
    public float currentBobSpeed;
    public float lerpSpeed;

    public bool isOnGround_;

    public Animator walkAni;

    public float verticalVelocity;
    public float lowImpactThreshold;
    public float highImpactTheshold;
    bool isDueForImpact;

    [Space]
    public float defaultFov;
    public AnimationCurve fovChange;
    public float fovchangeSpeed;
    public CinemachineVirtualCamera cam;

    [Space]
    public Transform particleSpawnPoint;
    public ParticleSystem speedParticles;
    public AnimationCurve speedParticleMulti;
    public float speedParticleAMount;

    public GameObject landImpact;
    public GameObject jumpParticle;

    [Space]
    public Toggle headBobToggle;
    public MeshRenderer powerUpIndicator;
    public Material powerupIndicatorGood;
    public Material powerupIndicatorNoGood;

    void Start()
    {
        //cam = Camera.main;

        //load settings
        if (headBobToggle)
        {
            if (PlayerPrefs.HasKey("headBob"))
            {
                if (PlayerPrefs.GetInt("headBob") == 1)

                    headBobToggle.isOn = true;
                else
                    headBobToggle.isOn = true;
            }
            else
            {
                PlayerPrefs.SetInt("headBob", 1);
                headBobToggle.isOn = true;
            }
        }
    }

    void Update()
    {
        //update settings
        if (PlayerPrefs.GetInt("headBob") == 1)
            canBob = true;
        else
            canBob = false;


        if (headBobToggle.isOn)

            PlayerPrefs.SetInt("headBob", 1);
        else
            PlayerPrefs.SetInt("headBob", 0);

        //set parameters
        currentSpeed = playerMovement.HorizontalVelocityf;
        lerpSpeed = playerMovement.horizontalVelocityLerp;
        isOnGround_ = playerMovement.isOnGround;
        verticalVelocity = playerMovement.verticalVelocity;

        //set bobSpeed
        if (canBob)
            currentBobSpeed = bobSpeedMutliplier.Evaluate(currentSpeed);
        else currentBobSpeed = 0;

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
            walkAni.SetBool("jump", false);
            isDueForImpact = true;
        }

        //fov
        float fovDiff = fovChange.Evaluate(lerpSpeed);
        cam.m_Lens.FieldOfView = Mathf.Lerp(
            cam.m_Lens.FieldOfView,
            defaultFov + fovDiff,
            fovchangeSpeed);

        //cam.fieldOfView = defaultFov;

        //particles
        var emission = speedParticles.emission;
        emission.rateOverTime = speedParticleMulti.Evaluate(lerpSpeed) * speedParticleAMount;
    }

    void playImpact()
    {

        if (Mathf.Abs(verticalVelocity) >= highImpactTheshold)
        {
            AudioManager.instance.GenerateSound(AudioReference.instance.landHard, Vector3.zero);
            //walkAni.SetTrigger("hard Landing");
            GameObject landingParticleTemp = GameObject.Instantiate(landImpact, particleSpawnPoint.position, Quaternion.identity);
            Destroy(landingParticleTemp, 1f);
        }
        else if (Mathf.Abs(verticalVelocity) >= lowImpactThreshold)
        {
            AudioManager.instance.GenerateSound(AudioReference.instance.landMedium, Vector3.zero);
            walkAni.SetTrigger("medium Landing");

            GameObject landingParticleTemp = GameObject.Instantiate(landImpact, particleSpawnPoint.position, Quaternion.identity);
            Destroy(landingParticleTemp, 1f);
        }
        else
        {
            AudioManager.instance.GenerateSound(AudioReference.instance.landSoft, Vector3.zero);
            walkAni.SetTrigger("soft Landing");

            GameObject landingParticleTemp = GameObject.Instantiate(landImpact, particleSpawnPoint.position, Quaternion.identity);
            Destroy(landingParticleTemp, 1f);
        }

    }

    public void playPowerupAni(bool ready)
    {
        if (ready)
            powerUpIndicator.material = powerupIndicatorGood;
        else
            powerUpIndicator.material = powerupIndicatorNoGood;

        walkAni.SetTrigger("powerup");
    }

    public void playJumpAnimation()
    {
        walkAni.SetBool("jump", true);
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
