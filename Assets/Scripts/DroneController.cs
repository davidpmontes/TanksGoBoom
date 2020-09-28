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
        targetPlayerVelocity = Vector3.MoveTowards(targetPlayerVelocity,
                                           new Vector3(leftStickInput.x,
                                                       rightStickInput.y,
                                                       leftStickInput.y) * pso.moveSpeed,
                                           pso.moveAcceleration * Time.deltaTime);

        playerVelocity = turret.transform.forward * targetPlayerVelocity.z * Time.deltaTime +
                         turret.transform.right * targetPlayerVelocity.x * Time.deltaTime +
                         Vector3.up * targetPlayerVelocity.y * Time.deltaTime;

        body.transform.localRotation = Quaternion.AngleAxis(-leftStickInput.x * MAX_BODY_TILT, turret.transform.forward) *
                                       Quaternion.AngleAxis(leftStickInput.y * MAX_BODY_TILT, turret.transform.right);

        cc.Move(playerVelocity);
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
        engineAudioSource.pitch = 1 + MAX_ENGINE_PITCH * (playerVelocity.magnitude / pso.moveSpeed);
        turretAudioSource.volume = Mathf.Abs(rightStickInput.x);
    }
}
