using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    private GameObject primary;
    private GameObject secondary;

    private bool primaryWeaponInput;
    private bool secondaryWeaponInput;
    private bool isFiringPrimary;

    private UnityEngine.InputSystem.PlayerInput playerInput;

    delegate void InputReceiver();
    InputReceiver GetInput;

    private TankAI tankAI;

    private void Awake()
    {
        playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
    }

    public void Init(bool isNPC, GameObject primaryWeapon, GameObject secondaryWeapon)
    {
        if (isNPC)
        {
            tankAI = GetComponent<TankAI>();
            GetInput = NPCInput;
        }
        else
        {
            GetInput = PlayerInput;
        }

        primary = primaryWeapon;
        secondary = secondaryWeapon;
    }

    private void Update()
    {
        GetInput();
        PrimaryWeapon();
        SecondaryWeapon();
    }

    private void NPCInput()
    {

    }

    private void PlayerInput()
    {
        primaryWeaponInput = playerInput.actions["FirePrimary"].ReadValue<float>() > 0.9f ? true : false;
        secondaryWeaponInput = playerInput.actions["FireSecondary"].ReadValue<float>() > 0.9f ? true : false;
    }

    private void PrimaryWeapon()
    {
        if (isFiringPrimary ^ primaryWeaponInput)
        {
            if (primaryWeaponInput)
            {
                primary.GetComponent<IWeaponSystem>().StartFiring();
            }
            else
            {
                primary.GetComponent<IWeaponSystem>().StopFiring();
            }
            isFiringPrimary = !isFiringPrimary;
        }
    }

    private void SecondaryWeapon()
    {
        if (secondaryWeaponInput)
        {
            secondary.GetComponent<IWeaponSystem>().StartFiring();
        }
    }
}
