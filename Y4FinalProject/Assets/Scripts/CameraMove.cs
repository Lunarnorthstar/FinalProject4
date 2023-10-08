using UnityEngine;

public class CameraMove : MonoBehaviour
{
    PlayerManager playerManager;

    public Transform Player;
    public float Mousesens;
    //public float Smoothing;
    Vector2 MouseLook;
    Vector2 SmoothV;

    float CamRotY;

    void Start()
    {
        InvokeRepeating("moveCam", 0, 0.01f);
        playerManager = transform.parent.GetComponent<PlayerManager>();
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
