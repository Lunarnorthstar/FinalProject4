using Unity.Mathematics;
//using UnityEditor.Callbacks;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public Vector2 joyCamera;

    float CamRotY;
    public float CamRotX;

    float sCamRotX;
    float sCamRotY;

    [Header("Camera Settings")]
    public float defaultFOV;
    public float fovSmooth;
    public float defaultRot;
    public float targetFov;

    void Start()
    {
        InvokeRepeating("moveCam", 0, 0.01f);
        //changeFov(0);
        // playerManager = transform.parent.parent.GetComponent<PlayerManager>();
        //  playerMovement = transform.parent.parent.GetComponent<PlayerMovement>();
    }


    void Update()
    {
        // mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, targetFov, fovSmooth * Time.deltaTime);
    }


    void moveCam()
    {
        if (!playerManager.isMouseLocked) return;

        CamRotX += Input.GetAxis("Mouse X") + joyCamera.x * Mousesens;
        CamRotY += Input.GetAxis("Mouse Y") + joyCamera.y * Mousesens;

        CamRotY = Mathf.Clamp(CamRotY, -80, 80);

        sCamRotX = Mathf.Lerp(sCamRotX, CamRotX, mouseSmooth);
        sCamRotY = Mathf.Lerp(sCamRotY, CamRotY, mouseSmooth);

        Player.transform.rotation = Quaternion.Euler(0, sCamRotX + defaultRot, 0);

        transform.localRotation = Quaternion.Euler(-sCamRotY, transform.localEulerAngles.y, 0);
    }
}
