using UnityEngine;

public class WalkerController : PlayerController
{
    [SerializeField] private GameObject turretYaw;
    [SerializeField] private GameObject turretPitch;
    [SerializeField] private GameObject leftJet;
    [SerializeField] private GameObject rightJet;
    [SerializeField] private GameObject ccJet;
    [SerializeField] private GameObject cwJet;
    [SerializeField] private AudioClip landingClip;

    [SerializeField] private AudioSource leftJetsAudiosource;
    [SerializeField] private AudioSource rightJetsAudiosource;
    [SerializeField] private AudioSource cwJetsAudiosource;
    [SerializeField] private AudioSource ccJetsAudiosource;
    [SerializeField] private AudioSource landingAudiosource;

    private Animator animator;

    private bool isJetting;
    private float airborneTime;
    private float jetsTimeRemaining;

    private float forwardVelocity;

    private const float JETS_STARTING_TIME = 2f;
    private const float JETS_ACCELERATION = 22f;
    private const float WALKING_FORWARD_ACCELERATION = 3f;
    private const float FLYING_FORWARD_ACCELERATION = 1f;
    private const float MAX_TURRET_YAW = 90f;
    private const float MAX_TURRET_PITCH = 45f;
    private const float TURRET_YAWPITCH_SPEED = 50f;
    private const float MAX_WALKING_SPEED_FORWARD = 10f;
    private const float MAX_WALKING_SPEED_REVERSE = -5f;
    private const float MAX_ROTATE_SPEED = 40f;

    private const string IS_JETTING = "isJetting";
    private const string IS_GROUNDED = "isGrounded";
    private const string DIRECTION = "direction";
    private const string HAS_FORWARD_VELOCITY = "hasForwardVelocity";

    public override void Init()
    {
        base.Init();

        animator = GetComponentInChildren<Animator>();
        leftJet.SetActive(false);
        rightJet.SetActive(false);
        ccJet.SetActive(false);
        cwJet.SetActive(false);
        jetsTimeRemaining = JETS_STARTING_TIME;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        RotatePlayer();
        ApplyJets();
    }

    private void ApplyJets()
    {
        if (buttonNorthPressedInput)
        {
            leftJetsAudiosource.Play();
            rightJetsAudiosource.Play();
            isJetting = true;
            animator.SetBool(IS_JETTING, true);
            leftJet.SetActive(true);
            rightJet.SetActive(true);
        }


        if (isJetting)
        {
            jetsTimeRemaining -= Time.deltaTime;
            if (jetsTimeRemaining < 0)
            {
                buttonNorthReleasedInput = true;
            }
        }
        else
        {
            jetsTimeRemaining = Mathf.MoveTowards(jetsTimeRemaining, JETS_STARTING_TIME, 0.5f * Time.deltaTime);
        }
        canvasManager.SetJetsIndicator(jetsTimeRemaining / JETS_STARTING_TIME);

        if (buttonNorthReleasedInput)
        {
            leftJetsAudiosource.Stop();
            rightJetsAudiosource.Stop();
            isJetting = false;
            animator.SetBool(IS_JETTING, false);
            leftJet.SetActive(false);
            rightJet.SetActive(false);
        }

    }

    protected override void Targets()
    {

    }

    protected override void HandleGround()
    {
        isGrounded = cc.isGrounded;

        if (!isGrounded)
        {
            airborneTime += Time.deltaTime;
        }
        else
        {
            if (airborneTime > 1f)
            {
                landingAudiosource.PlayOneShot(landingClip);
            }
            airborneTime = 0f;
            cwJetsAudiosource.Stop();
            ccJetsAudiosource.Stop();
            ccJet.SetActive(false);
            cwJet.SetActive(false);
        }

        animator.SetBool(IS_GROUNDED, isGrounded);

        if (!isJetting && isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.01f;
        }
    }

    private void RotatePlayer()
    {
        if (airborneTime > 0.2f)
        {
            if (leftStickInput.x < 0)
            {
                cwJet.SetActive(false);
                ccJet.SetActive(true);

                cwJetsAudiosource.Stop();

                if (!ccJetsAudiosource.isPlaying)
                    ccJetsAudiosource.Play();
            }
            else
            {
                ccJet.SetActive(false);
                cwJet.SetActive(true);

                ccJetsAudiosource.Stop();

                if (!cwJetsAudiosource.isPlaying)
                    cwJetsAudiosource.Play();
            }
        }

        if (Mathf.Abs(leftStickInput.x) < 0.5f)
        {
            ccJet.SetActive(false);
            cwJet.SetActive(false);
            cwJetsAudiosource.Stop();
            ccJetsAudiosource.Stop();
        }

        transform.Rotate(Vector3.up, leftStickInput.x * MAX_ROTATE_SPEED * Time.deltaTime, Space.Self);
    }

    protected override void MoveVehicle()
    {
        if (isJetting)
        {
            forwardVelocity = Mathf.Clamp(forwardVelocity + leftStickInput.y * FLYING_FORWARD_ACCELERATION * Time.deltaTime, MAX_WALKING_SPEED_REVERSE, MAX_WALKING_SPEED_FORWARD);

            playerVelocity = new Vector3(transform.forward.x * forwardVelocity * Time.deltaTime,
                                         playerVelocity.y,
                                         transform.forward.z * forwardVelocity * Time.deltaTime);

            playerVelocity.y += JETS_ACCELERATION * Time.deltaTime * Time.deltaTime;
        }
        else if (isGrounded)
        {
            forwardVelocity = Mathf.Clamp(forwardVelocity + leftStickInput.y * WALKING_FORWARD_ACCELERATION * Time.deltaTime, MAX_WALKING_SPEED_REVERSE, MAX_WALKING_SPEED_FORWARD);

            if (Mathf.Abs(forwardVelocity) < 1 && Mathf.Abs(leftStickInput.y) < 0.1f)
            {
                forwardVelocity = 0f;
            }

            playerVelocity = new Vector3(transform.forward.x * forwardVelocity * Time.deltaTime,
                                         playerVelocity.y,
                                         transform.forward.z * forwardVelocity * Time.deltaTime);
        }

        playerVelocity.y += GRAVITY * Time.deltaTime * Time.deltaTime;
        cc.Move(playerVelocity);
    }

    protected override void MoveTurret()
    {
        targetYaw = Mathf.Clamp(targetYaw + (rightStickInput.x * TURRET_YAWPITCH_SPEED * Time.deltaTime), -MAX_TURRET_YAW, MAX_TURRET_YAW);
        targetPitch = Mathf.Clamp(targetPitch + (-rightStickInput.y * TURRET_YAWPITCH_SPEED * Time.deltaTime), -MAX_TURRET_PITCH, MAX_TURRET_PITCH);

        turretYaw.transform.localRotation = Quaternion.Euler(0, 0, targetYaw);
        turretPitch.transform.localRotation = Quaternion.Euler(targetPitch, 0, 0);
    }

    protected override void MoveWeapons()
    {
    }

    protected override void UpdateAnimator()
    {
        if (isJetting)
        {
        }
        else
        {
            float normalizedSpeed = 0f;

            if (forwardVelocity > 0f)
            {
                normalizedSpeed = forwardVelocity / MAX_WALKING_SPEED_FORWARD;
            }
            else if (forwardVelocity < 0f)
            {
                normalizedSpeed = -forwardVelocity / MAX_WALKING_SPEED_REVERSE;
            }

            animator.SetFloat(DIRECTION, normalizedSpeed);

            animator.SetBool(HAS_FORWARD_VELOCITY, !Mathf.Approximately(forwardVelocity, 0f));
        }
    }

    protected override void UpdateHUD()
    {
        canvasManager.SetSpeedIndicator(Mathf.FloorToInt(forwardVelocity));
        canvasManager.SetTurretIndicatorPosition(targetYaw / MAX_TURRET_YAW);
    }

    protected override void UpdateAudio()
    {
    }
}