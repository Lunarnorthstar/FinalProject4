using Unity.Mathematics;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera mainCam;

    PlayerManager playerManager;
    PlayerMovement playerMovement;

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
    public AnimationCurve FovMuliplier;
    public float FoV;
    public float fovSmoothing;

    void Start()
    {
        InvokeRepeating("moveCam", 0, 0.01f);
        playerManager = transform.parent.parent.GetComponent<PlayerManager>();
        playerMovement = transform.parent.parent.GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        //the fov smoothly lerps between what is is, and a wider view based on speed
        // mainCam.fieldOfView = Mathf.Lerp(
        //     mainCam.fieldOfView,
        //      FoV * FovMuliplier.Evaluate(playerMovement.HorizontalVelocityf),
        //      fovSmoothing);
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

        transform.rotation = Quaternion.Euler(-sCamRotY, transform.eulerAngles.y, 0);
    }
}
