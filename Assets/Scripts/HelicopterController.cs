using UnityEngine;

public class HelicopterController : PlayerController
{
    private const float TURRET_YAW_SPEED = 115f;
    private const float MAX_BODY_TILT = 30f;

    private const float MAX_ENGINE_PITCH = 0.5f;

    [SerializeField] private GameObject tailRotor;

    public override void Init()
    {
        base.Init();
    }

    protected override void Targets()
    {

    }
    protected override void HandleGround()
    {
    }

    protected override void MoveTurret()
    {
        //targetYaw += rightStickInput.x * TURRET_YAW_SPEED * Time.deltaTime;
        //if (targetYaw > 360) targetYaw -= 360;
        //if (targetYaw < 0) targetYaw += 360;

        //turret.transform.localRotation = Quaternion.Euler(0, targetYaw, 0);
    }

    protected override void MoveVehicle()
    {
        var targetRotation = Quaternion.Euler(leftStickInput.y * MAX_BODY_TILT, 0, -rightStickInput.x * MAX_BODY_TILT);

        body.transform.localRotation = Quaternion.RotateTowards(body.transform.localRotation,
                                                                targetRotation,
                                                                Quaternion.Angle(body.transform.localRotation, targetRotation) * Time.fixedDeltaTime);
        
        rb.AddForceAtPosition(-rightStickInput.x * tailRotor.transform.right * 100000f, tailRotor.transform.position, ForceMode.Force);

        rb.drag = 1 + rb.velocity.magnitude / pso.maxSpeed;
        rb.angularDrag = 1 + rb.angularVelocity.magnitude / pso.maxAngularSpeed;

        rb.AddForce((turret.transform.forward * leftStickInput.y +
                    Vector3.up * rightStickInput.y +
                    turret.transform.right * leftStickInput.x) * 500000f,
                    ForceMode.Force);
    }

    protected override void MoveWeapons()
    {
    }

    protected override void UpdateAnimator()
    {
    }

    protected override void UpdateAudio()
    {
    }
}
