using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public float turningSpeed;
    public float upRange = 5f;
    public float downRange = 5f;
    
    private bool cameraIsLockedOn = false;
    private float currentUpDownRotation;
    private bool triggerWasPressedLastFrame;

    public Transform camPivot;
    public Transform enemy;
    public Sprite[] lockonImage;
    public Image lockonSpot;
    public Image outerGlow;
    public Text lockonText;
    
    void Update()
    {
        if (GameManager.instance.stopPlayer || GameManager.instance.gamePaused)
            return;

        if (!cameraIsLockedOn)
        {
            HandleFreeCameraControls();
            lockonSpot.sprite = lockonImage[0];
            outerGlow.color = Color.white;
            lockonText.text = "Lock on";
        }
        else
        {
            HandleLockOnCamera();
            lockonSpot.sprite = lockonImage[1];
            outerGlow.color = Color.black;
            lockonText.text = "Lock off";
        }

        // Check GetAxisDown - create a bool and set it to false only when the axis returns a Zero value
        if (Input.GetAxis("LeftTrigger") > 0)
        {
            if (!triggerWasPressedLastFrame)
                cameraIsLockedOn = !cameraIsLockedOn;

            triggerWasPressedLastFrame = true;
        }
        else
            triggerWasPressedLastFrame = false;
    }

    void HandleFreeCameraControls()
    {
        //Camera look up and down and clamp
        float mouseXInput = Input.GetAxis("RightJoystickX");
        float mouseYInput = Input.GetAxis("RightJoystickY");

        currentUpDownRotation = currentUpDownRotation + mouseYInput;
        currentUpDownRotation = Mathf.Clamp(currentUpDownRotation, -upRange, downRange);

        //Camera look sideways
        camPivot.Rotate(0f, mouseXInput * turningSpeed, 0f, Space.World);
        camPivot.transform.eulerAngles = new Vector3(currentUpDownRotation, camPivot.transform.eulerAngles.y);
    }

    private void HandleLockOnCamera()
    {
        float rotateSpeed = 0.1f;

        Vector3 enemyPos = enemy.position + Vector3.up * 23f;
        Vector3 myPos = camPivot.position;
        Vector3 distance = enemyPos - myPos;

        Quaternion targetRotation = Quaternion.LookRotation(distance.normalized, Vector3.up);
        Vector3 targetEuler = targetRotation.eulerAngles;
        float pitch = targetEuler.x;

        while (pitch < 0)
            pitch += 360f;

        if (pitch > downRange && pitch < (360f - upRange))
        {
            if (Mathf.Abs(pitch - downRange) > Mathf.Abs(pitch - upRange))
                pitch = 360 - upRange;
            else
                pitch = downRange;
        }

        targetEuler.x = pitch;
        targetEuler.z = 0f;
        
        targetRotation.eulerAngles = targetEuler;

        camPivot.rotation = Quaternion.Slerp(camPivot.rotation, targetRotation, rotateSpeed);
    }
}