using UnityEngine;
using UnityEngine.InputSystem;

public class BomberController : MonoBehaviour
{
    [SerializeField] private GameObject modelContainer = default;
    [SerializeField] private GameObject yaw = default;
    [SerializeField] private GameObject pitch = default;
    [SerializeField] private GameObject roll = default;
    [SerializeField] private float flySpeed = default;

    private float minFlySpeed = 0f;
    private float maxFlySpeed = 50;

    private const float PITCH_ACCEL = 40;
    private const float TURN_ACCEL = 40;
    private const float MAX_BANK_ANGLE = 40;
    private const float THRUSTER_ACCEL = 400;

    private float horizontalRaw;
    private float verticalRaw;
    private float leftTriggerRaw;
    private float rightTriggerRaw;
    private float leftShoulderRaw;
    private float rightShoulderRaw;

    private float yValue;
    private float xValue;
    private float zValue;

    private CharacterController cc;
    private bool changeCameraInput;
    private CameraConfigs cameraConfigs;

    private void Awake()
    {
        cc = GetComponentInChildren<CharacterController>();
        cameraConfigs = new CameraConfigs();
        cameraConfigs.minRadius = 65;
        cameraConfigs.maxRadius = 70;
        cameraConfigs.rearLocal = new Vector3(0, 20f, -65f);
    }

    void Update()
    {
        GetInput();
        ChangeCamera();
    }

    void FixedUpdate()
    {
        //UpdateYawPitchRollRealistic();
        UpdateYawPitchRollArcade();
        UpdateSpeed();
        MoveVehicle();
    }

    public Vector3 GetCCVelocity()
    {
        return cc.velocity;
    }

    private void GetInput()
    {
        horizontalRaw = Gamepad.current.leftStick.x.ReadValue();
        verticalRaw = Gamepad.current.leftStick.y.ReadValue();

        // 0 => up, 1 => held down
        leftTriggerRaw = Gamepad.current.leftTrigger.ReadValue();
        rightTriggerRaw = Gamepad.current.rightTrigger.ReadValue();

        leftShoulderRaw = Gamepad.current.leftShoulder.ReadValue();
        rightShoulderRaw = Gamepad.current.rightShoulder.ReadValue();

        if (Gamepad.current.selectButton.wasPressedThisFrame)
            changeCameraInput = true;
    }

    //private void UpdateYawPitchRollRealistic()
    //{
    //    xValue = Mathf.MoveTowards(xValue, verticalRaw, 10 * Mathf.Abs(xValue - verticalRaw) * Time.deltaTime);

    //    yValue = Mathf.MoveTowards(yValue, -leftShoulderRaw + rightShoulderRaw,
    //                               10 * Mathf.Abs(yValue - (-leftShoulderRaw + rightShoulderRaw)) * Time.deltaTime);
    //    zValue = Mathf.MoveTowards(zValue, horizontalRaw, 15 * Mathf.Abs(zValue - horizontalRaw) * Time.deltaTime);

    //    roll.transform.Rotate(xValue * 2f, yValue * 1f, -zValue * 3f, Space.Self);
    //}

    private void UpdateYawPitchRollArcade()
    {
        xValue = Mathf.MoveTowards(xValue, verticalRaw, PITCH_ACCEL * Mathf.Abs(xValue - verticalRaw) * Time.deltaTime * Time.deltaTime);
        //yValue = Mathf.MoveTowards(yValue, -leftShoulderRaw + rightShoulderRaw, 10 * Mathf.Abs(yValue - (-leftShoulderRaw + rightShoulderRaw)) * Time.deltaTime);

        var maxTurn = horizontalRaw * 1;
        zValue = Mathf.MoveTowards(zValue, maxTurn, TURN_ACCEL * Mathf.Abs(zValue - maxTurn) * Time.deltaTime * Time.deltaTime);

        yaw.transform.Rotate(0, zValue, 0, Space.Self);
        pitch.transform.Rotate(xValue * 2f, 0, 0, Space.Self);
        roll.transform.localRotation = Quaternion.Euler(0, 0, -zValue * MAX_BANK_ANGLE);
    }

    private void UpdateSpeed()
    {
        flySpeed = Mathf.MoveTowards(flySpeed, maxFlySpeed, THRUSTER_ACCEL * rightTriggerRaw * Time.deltaTime * Time.deltaTime);
        flySpeed = Mathf.MoveTowards(flySpeed, minFlySpeed, THRUSTER_ACCEL * leftTriggerRaw * Time.deltaTime * Time.deltaTime);
        CameraMovement.Instance.UpdateRadius((flySpeed - minFlySpeed) / (maxFlySpeed - minFlySpeed));
    }

    private void MoveVehicle()
    {
        cc.Move(roll.transform.forward * flySpeed * Time.deltaTime);
    }

    private void ChangeCamera()
    {
        if (changeCameraInput)
        {
            changeCameraInput = false;
            CameraMovement.Instance.ToggleCamera();
        }
    }

    public void GetCameraConfigs()
    {

    }
}
