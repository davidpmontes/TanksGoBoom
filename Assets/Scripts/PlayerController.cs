using System.Diagnostics;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] protected GameObject vehicleModel;
    [SerializeField] protected GameObject playerIconPrefab;
    [SerializeField] protected PlayerScriptableObject pso;

    protected GameObject turretRotateable;
    protected GameObject turret;
    protected GameObject weapons;
    protected GameObject body;
    protected GameObject rotateable;
    protected GameObject normal;
    protected GameObject groundNormalRaycastOrigin;

    protected AudioSource engineAudioSource;
    protected AudioSource turretAudioSource;

    //protected CharacterController cc;
    protected Rigidbody rb;
    protected CanvasManager canvasManager;

    protected float life;

    //protected bool isGrounded;

    protected GameObject targetingReticleTarget;

    protected float targetYaw;
    protected float targetPitch;
    public Vector3 playerVelocity;
    public Vector3 targetPlayerVelocity;

    protected bool getNearestEnemyInput;
    protected bool changeCameraInput;
    protected Vector2 leftStickInput;
    protected Vector2 rightStickInput;
    protected bool buttonNorthPressedInput;
    protected bool buttonNorthReleasedInput;
    protected float leftShoulderPressedInput;
    protected float rightShoulderPressedInput;


    protected Quaternion bodyRotation;

    protected const float GRAVITY = -9.81f;
    public bool isNPC;

    delegate void InputReceiver();
    InputReceiver GetInput;
    private TankAI tankAI;

    protected GameObject currRadarTarget;
    private LayerMask radarLayerMask;

    private UnityEngine.InputSystem.PlayerInput playerInput;

    Collider[] hitColliders = new Collider[100];

    private void Awake()
    {
        playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        radarLayerMask = LayerMask.GetMask("enemyVehicle");
        life = pso.maxLife;
        if (isNPC)
        {
            tankAI = GetComponent<TankAI>();
            Init();
        }
    }

    public virtual void Init()
    {
        if (isNPC)
        {
            GetInput = NPCInput;
        }
        else
        {
            GetInput = PlayerInput;
        }
        //cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        canvasManager = FindObjectOfType<CanvasManager>();
        targetingReticleTarget = Utils.FindChildByNameRecursively(transform, "TargetingReticleTarget");

        turretRotateable = Utils.FindChildByNameRecursively(transform, "TurretRotateable");
        turret = Utils.FindChildByNameRecursively(transform, "Turret");
        weapons = Utils.FindChildByNameRecursively(transform, "Weapons");
        body = Utils.FindChildByNameRecursively(transform, "Body");
        rotateable = Utils.FindChildByNameRecursively(transform, "Rotateable");
        normal = Utils.FindChildByNameRecursively(transform, "Normal");
        groundNormalRaycastOrigin = Utils.FindChildByNameRecursively(transform, "GroundNormalRaycastOrigin");

        body.GetComponent<MeshFilter>().mesh = vehicleModel.GetComponent<MeshFilter>().mesh;
        body.GetComponent<MeshRenderer>().sharedMaterials = vehicleModel.GetComponent<MeshRenderer>().sharedMaterials;

        var turretModel = vehicleModel.transform.GetChild(0).gameObject;
        turret.transform.localPosition = turretModel.transform.localPosition;
        turret.transform.localRotation = turretModel.transform.localRotation;
        if (turretModel.TryGetComponent(out MeshFilter meshFilter))
        {
            turret.GetComponent<MeshFilter>().mesh = meshFilter.mesh;
        }

        if (turretModel.TryGetComponent(out MeshRenderer meshRenderer))
        {
            turret.GetComponent<MeshRenderer>().sharedMaterial = meshRenderer.sharedMaterial;
        }

        var weaponsModel = turretModel.transform.GetChild(0).gameObject;
        weapons.transform.localPosition = weaponsModel.transform.localPosition;
        weapons.transform.localRotation = weaponsModel.transform.localRotation;
        weapons.GetComponent<MeshFilter>().mesh = weaponsModel.GetComponent<MeshFilter>().mesh;
        weapons.GetComponent<MeshRenderer>().sharedMaterial = weaponsModel.GetComponent<MeshRenderer>().sharedMaterial;

        var playerIcon = Instantiate(playerIconPrefab, transform);
        playerIcon.transform.localPosition = Vector3.zero;
        playerIcon.transform.localRotation = Quaternion.identity;
        playerIcon.GetComponent<MapIcon>().StartTracking(transform, turretRotateable.transform);

        var primaryWeapon = Utils.FindChildByNameRecursively(transform, "PrimaryWeapon").transform.GetChild(0).gameObject;
        var secondaryWeapon = Utils.FindChildByNameRecursively(transform, "SecondaryWeapon").transform.GetChild(0).gameObject;
        GetComponent<WeaponsController>().Init(isNPC, primaryWeapon, secondaryWeapon);

        var weaponContents = Utils.FindChildByNameRecursively(transform, "WeaponContents");
        weaponContents.transform.SetParent(weapons.transform);

        Destroy(vehicleModel);

        engineAudioSource = Utils.FindChildByNameRecursively(transform, "Audio_TankEngine").GetComponent<AudioSource>();
        turretAudioSource = Utils.FindChildByNameRecursively(transform, "Audio_TankTurret").GetComponent<AudioSource>();
    }

    public void SetStartingPosition(Vector3 worldPosition)
    {
        enabled = false;
        rb.isKinematic = true;
        //cc.enabled = false;
        transform.position = worldPosition;
        //cc.enabled = true;
        rb.isKinematic = false;
        enabled = true;
    }

    void Update()
    {
        GetInput();
        ChangeCamera();
        Targets();
    }

    protected virtual void FixedUpdate()
    {
        HandleGround();
        MoveVehicle();
        MoveTurret();
        MoveWeapons();
        UpdateAnimator();
        UpdateHUD();
        UpdateAudio();
    }

    public void TargetReportingDestroyed(GameObject target)
    {
        currRadarTarget = null;
        canvasManager.SetRadarTargetVisible(false);
    }

    protected virtual GameObject GetNearestTarget()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, 100, hitColliders, radarLayerMask);

        GameObject nearestTarget = null;

        float nearestSqrDis = float.MaxValue;

        for (int i = 0; i < numColliders; i++)
        {
            Vector3 offset = hitColliders[i].gameObject.transform.position - transform.position;
            if (offset.sqrMagnitude < nearestSqrDis)
            {
                nearestSqrDis = offset.sqrMagnitude;
                nearestTarget = hitColliders[i].gameObject;
            }
        }

        return nearestTarget;
    }

    protected virtual void Targets()
    {
        if (getNearestEnemyInput)
        {
            currRadarTarget = GetNearestTarget();
        }

        if (currRadarTarget)
        {
            canvasManager.UpdateRadarTarget(currRadarTarget.transform.position);
            canvasManager.SetRadarTargetVisible(true);
        }
        else
        {
        }
    }

    protected abstract void HandleGround();
    protected abstract void MoveVehicle();
    protected abstract void MoveTurret();
    protected abstract void MoveWeapons();
    protected abstract void UpdateAnimator();

    protected virtual void UpdateHUD()
    {
        canvasManager.SetSpeedIndicator(Mathf.FloorToInt(playerVelocity.magnitude * 10f));
        canvasManager.UpdateTargetingReticle(targetingReticleTarget.transform.position);
    }

    protected abstract void UpdateAudio();

    private void NPCInput()
    {
        leftStickInput = tankAI.leftStickInput;
        rightStickInput = tankAI.rightStickInput;
    }

    private void PlayerInput()
    {
        leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
        rightStickInput = playerInput.actions["Look"].ReadValue<Vector2>();
        //changeCameraInput = Gamepad.current.selectButton.wasPressedThisFrame;
        //buttonNorthPressedInput = Gamepad.current.buttonNorth.wasPressedThisFrame;
        //buttonNorthReleasedInput = Gamepad.current.buttonNorth.wasReleasedThisFrame;
        //getNearestEnemyInput = Gamepad.current.buttonEast.wasPressedThisFrame;
        leftShoulderPressedInput = playerInput.actions["LeftShoulder"].ReadValue<float>();
        rightShoulderPressedInput = playerInput.actions["RightShoulder"].ReadValue<float>();
    }

    public void DamageReceived(Vector3 fromDirection, float power, float damage)
    {
        life -= damage;
        CameraMovement.Instance.Shake();
        canvasManager.UpdateLifeIndicator(life / pso.maxLife);
    }

    private void ChangeCamera()
    {
        if (changeCameraInput)
        {
            changeCameraInput = false;
            CameraMovement.Instance.ToggleCamera();
        }
    }
}