using UnityEngine;

public class HelicopterController : PlayerController
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

    protected override void MoveTurret()
    {
    }

    protected override void MoveVehicle()
    {
        float targetVerticalVelocity = 0.0f;
        targetVerticalVelocity += -leftShoulderPressedInput * 0.5f + rightShoulderPressedInput * 0.5f;

        targetPlayerVelocity = (turret.transform.forward * leftStickInput.y +
                               Vector3.up * targetVerticalVelocity +
                               turret.transform.right * leftStickInput.x) * pso.moveSpeed;

        body.transform.localRotation = Quaternion.AngleAxis(-leftStickInput.x * MAX_BODY_TILT, turret.transform.forward) *
                                       Quaternion.AngleAxis(leftStickInput.y * MAX_BODY_TILT, turret.transform.right);

        var forceDirection = (targetPlayerVelocity - rb.velocity).normalized;
        var velocityMagnitude = (targetPlayerVelocity - rb.velocity).magnitude;

        rb.AddForce(forceDirection * 0.5f * rb.mass * velocityMagnitude * velocityMagnitude, ForceMode.Force);
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
