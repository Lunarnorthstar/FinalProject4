using Unity.Mathematics;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera mainCam;

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
        if (!playerManager.isMouseLocked) return;

        CamRotX += Input.GetAxis("Mouse X") * Mousesens;
        CamRotY += Input.GetAxis("Mouse Y") * Mousesens;

        CamRotY = Mathf.Clamp(CamRotY, -80, 80);

        sCamRotX = Mathf.Lerp(sCamRotX, CamRotX, mouseSmooth);
        sCamRotY = Mathf.Lerp(sCamRotY, CamRotY, mouseSmooth);

        Player.transform.rotation = Quaternion.Euler(0, sCamRotX, 0);

        transform.localRotation = Quaternion.Euler(-sCamRotY, transform.localEulerAngles.y, 0);
    }
}
