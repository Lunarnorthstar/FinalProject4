using Unity.Mathematics;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera mainCam;

    public bool shouldHoldCam;

    public PlayerManager playerManager;
    public PlayerMovement playerMovement;
    public Animator ani;

    public Transform Player;
    public float Mousesens;
    public float mouseSmooth;
    //public float Smoothing;
    Vector2 MouseLook;
    Vector2 SmoothV;

    float CamRotY;
    float CamRotX;

    float sCamRotX;
    float sCamRotY;

    [Header("Camera Settings")]
    public float defaultFOV;
    public float sprintFOV;
    public float fovSmooth;

    void Start()
    {
        InvokeRepeating("moveCam", 0, 0.01f);
        // playerManager = transform.parent.parent.GetComponent<PlayerManager>();
        //  playerMovement = transform.parent.parent.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        //the fov smoothly lerps between what is is, and a wider view based on speed

        if (playerMovement.isSprinting)
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, sprintFOV, fovSmooth * Time.deltaTime);
        }
        else
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, defaultFOV, fovSmooth * Time.deltaTime);
        }
    }


    void moveCam()
    {
        if (shouldHoldCam && playerMovement.ClimbLookTarget == null) return; //if is in animation then lock cam
        // else if (playerMovement.ClimbLookTarget != null && playerMovement.isHangingOnWall)//if isnt in animation but is tryna climb
        // {
        //     Transform target = playerMovement.ClimbLookTarget;//assign the target

        //     //move the target to directly in front (no unwanted left to right camera movement)
        //    // target.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        //    // transform.LookAt(target.position);

        //     //Vector3 directionVector = target.position - transform.right;//get a vector pointing towards the target (should be up and down)
        //     //transform.rotation = quaternion.Euler(directionVector);
        //     //directionVector.Normalize();

        //     // float rotAmount = Vector3.Cross(directionVector, transform.forward).z;


        //     // Quaternion lookRot = quaternion.LookRotation(directionVector, Vector3.up);
        //     // transform.rotation = lookRot;//
        //     //Quaternion.Lerp(transform.rotation, lookRot, 0.2f);
        //     //transform.rotation = quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, 0, 0));
        //     return;
        // }

        if (!playerManager.isMouseLocked) return;

        CamRotX += Input.GetAxis("Mouse X") * Mousesens;
        CamRotY += Input.GetAxis("Mouse Y") * Mousesens;

        CamRotY = Mathf.Clamp(CamRotY, -80, 80);

        sCamRotX = Mathf.Lerp(sCamRotX, CamRotX, mouseSmooth);
        sCamRotY = Mathf.Lerp(sCamRotY, CamRotY, mouseSmooth);

        Player.transform.rotation = Quaternion.Euler(0, sCamRotX, 0);

        transform.rotation = Quaternion.Euler(-sCamRotY, transform.eulerAngles.y, 0);
    }
}
