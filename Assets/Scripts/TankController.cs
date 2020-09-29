using UnityEngine;

public class TankController : PlayerController 
{
    //private Animator animator;

    private const float MAX_BARREL_PITCH = 28f;
    private const float MIN_BARREL_PITCH = -10f;
    private const float TURRET_YAW_SPEED = 100f;
    private const float BARREL_PITCH_SPEED = 100f;

    private const float MAX_ENGINE_PITCH = 0.5f;

    public override void Init()
    {
        base.Init();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    protected override void HandleGround()
    {
        //isGrounded = cc.isGrounded;

        //if (isGrounded && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = -0.01f;
        //}
    }

    protected override void MoveVehicle()
    {
        targetPlayerVelocity = Vector3.MoveTowards(targetPlayerVelocity,
                                                   new Vector3(leftStickInput.x, 0, leftStickInput.y) * pso.maxSpeed,
                                                   pso.maxAcceleration * Time.deltaTime);

        playerVelocity = turretRotateable.transform.forward * targetPlayerVelocity.z * Time.deltaTime +
                         turretRotateable.transform.right * targetPlayerVelocity.x * Time.deltaTime +
                         Vector3.up * playerVelocity.y;


        var viewingVector = playerVelocity;
        viewingVector.y = 0;
        if (viewingVector != Vector3.zero)
            bodyRotation = Quaternion.LookRotation(viewingVector, Vector3.up);

        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, bodyRotation, 10 * Time.deltaTime);

        rb.AddForce(playerVelocity * 1000000, ForceMode.Force);
        //playerVelocity.y += GRAVITY * Time.deltaTime * Time.deltaTime;
        
        //cc.Move(playerVelocity);
    }

    protected override void MoveTurret()
    {
        targetYaw += rightStickInput.x * TURRET_YAW_SPEED * Time.deltaTime;
        if (targetYaw > 360) targetYaw -= 360;
        if (targetYaw < 0) targetYaw += 360;

        turretRotateable.transform.localRotation = Quaternion.Euler(0, targetYaw, 0);
    }

    protected override void MoveWeapons()
    {
        targetPitch = Mathf.Clamp(targetPitch + (-rightStickInput.y * BARREL_PITCH_SPEED * Time.deltaTime), -MAX_BARREL_PITCH, -MIN_BARREL_PITCH);
        weapons.transform.localRotation = Quaternion.Euler(targetPitch, 0, 0);
    }

    protected override void UpdateAnimator()
    {

    }

    protected override void UpdateAudio()
    {
        engineAudioSource.pitch = 1 + MAX_ENGINE_PITCH * (playerVelocity.magnitude / pso.maxSpeed);
        turretAudioSource.volume = Mathf.Abs(rightStickInput.x);
    }

    //private void Normalize()
    //{
    //    if (Physics.Raycast(groundNormalRaycastOrigin.transform.position, Vector3.down, out RaycastHit hit))
    //    {
    //        Quaternion grndTilt = Quaternion.FromToRotation(Vector3.up, hit.normal);
    //        normal.transform.rotation = Quaternion.RotateTowards(normal.transform.rotation,
    //                                                             grndTilt * Quaternion.Euler(0, rotateable.transform.rotation.eulerAngles.y, 0),
    //                                                             50 * Time.deltaTime);
    //    }
    //}
}