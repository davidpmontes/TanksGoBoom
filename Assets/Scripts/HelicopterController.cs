using UnityEngine;

public class HelicopterController : PlayerController
{
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
    }

    protected override void MoveVehicle()
    {
        // Constants
        var torqueMultiplier = 150000f;
        var lateralMultiplier = 300000f;
        var forwardMultiplier = 550000f;
        var altitudeMultiplier = 400000f;
        var maxAltitude = 120f;

        // the higher you fly, the harder it is to keep flying higher
        var altitudeResistance = rightStickInput.y > 0 ? Mathf.Cos(transform.position.y * Mathf.PI / (maxAltitude * 2)) : 1;

        // calculate four forces
        var forwardForce = turret.transform.forward * leftStickInput.y * forwardMultiplier;
        var upwardForce = Vector3.up * rightStickInput.y * altitudeResistance * altitudeMultiplier;
        var lateralForce = turret.transform.right * leftStickInput.x * lateralMultiplier;
        var torqueForce = -rightStickInput.x * tailRotor.transform.right * torqueMultiplier;

        // body tilting effect
        var targetRotation = Quaternion.Euler(leftStickInput.y * MAX_BODY_TILT, 0, -rightStickInput.x * MAX_BODY_TILT);
        var maxDegreesDelta = Quaternion.Angle(body.transform.localRotation, targetRotation) * Time.fixedDeltaTime;
        body.transform.localRotation = Quaternion.RotateTowards(body.transform.localRotation, targetRotation, maxDegreesDelta);
        
        // apply drag
        rb.drag = 1 + rb.velocity.magnitude / pso.maxSpeed;
        rb.angularDrag = 1 + rb.angularVelocity.magnitude / pso.maxAngularSpeed;

        // apply four forces
        rb.AddForce(forwardForce + upwardForce + lateralForce, ForceMode.Force);
        rb.AddForceAtPosition(torqueForce, tailRotor.transform.position, ForceMode.Force);
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
