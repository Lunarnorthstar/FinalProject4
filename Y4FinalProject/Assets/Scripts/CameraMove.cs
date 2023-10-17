using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera mainCam;

    PlayerManager playerManager;
    PlayerMovement playerMovement;

    public Transform Player;
    public float Mousesens;
    //public float Smoothing;
    Vector2 MouseLook;
    Vector2 SmoothV;

    float CamRotY;

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

        float CamRotX = Input.GetAxis("Mouse X") * Mousesens;

        CamRotY += Input.GetAxis("Mouse Y") * Mousesens;

        CamRotY = Mathf.Clamp(CamRotY, -80, 80);

        Player.transform.Rotate(0, CamRotX, 0);

        transform.rotation = Quaternion.Euler(-CamRotY, transform.eulerAngles.y, 0);
    }
}
