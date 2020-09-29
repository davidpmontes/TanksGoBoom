using UnityEngine;

public class DroneController : PlayerController
{
    private const float TURRET_YAW_SPEED = 115f;
    private const float MAX_BODY_TILT = 15f;

    private const float MAX_ENGINE_PITCH = 0.5f;

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

    protected override void MoveVehicle()
    {
        float targetVerticalVelocity = 0.0f;
        targetVerticalVelocity += (rightShoulderPressedInput - leftShoulderPressedInput) * 0.5f;

        targetPlayerVelocity = (turret.transform.forward * leftStickInput.y +
                               Vector3.up * targetVerticalVelocity +
                               turret.transform.right * leftStickInput.x) * pso.maxSpeed;

        //body.transform.localRotation = Quaternion.AngleAxis(-leftStickInput.x * MAX_BODY_TILT, turret.transform.forward) *
        //                               Quaternion.AngleAxis(leftStickInput.y * MAX_BODY_TILT, turret.transform.right);
        body.transform.localRotation = Quaternion.RotateTowards(body.transform.localRotation,
                                                                Quaternion.AngleAxis(-leftStickInput.x * MAX_BODY_TILT, turret.transform.forward) *
                                                                Quaternion.AngleAxis(leftStickInput.y * MAX_BODY_TILT, turret.transform.right), 50 * Time.fixedDeltaTime);

        var forceDirection = (targetPlayerVelocity - rb.velocity).normalized;
        var velocityMagnitude = (targetPlayerVelocity - rb.velocity).magnitude;

        rb.AddForce(forceDirection * 0.5f * rb.mass * velocityMagnitude * velocityMagnitude, ForceMode.Force);
    }

    protected override void MoveTurret()
    {
        targetYaw += rightStickInput.x * TURRET_YAW_SPEED * Time.deltaTime;
        if (targetYaw > 360) targetYaw -= 360;
        if (targetYaw < 0) targetYaw += 360;

        turret.transform.localRotation = Quaternion.Euler(0, targetYaw, 0);
    }

    protected override void MoveWeapons()
    {
    }

    protected override void UpdateAnimator()
    {
    }

    protected override void UpdateAudio()
    {
        engineAudioSource.pitch = 1 + MAX_ENGINE_PITCH * (playerVelocity.magnitude / pso.maxSpeed);
        turretAudioSource.volume = Mathf.Abs(rightStickInput.x);
    }
}
