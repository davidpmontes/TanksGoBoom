using UnityEngine;

public class VehicleSelectorMenu : Menu<VehicleSelectorMenu>
{
    [SerializeField] private GameObject[] vehicles;

    private float CHANGE_VEHICLE_THRESHOLD = 0.5f;
    private int currIdx;
    private bool isCameraDoneMoving;
    private float duration = 0.4f;

    protected override void Awake()
    {
        base.Awake();

        foreach (GameObject vehicle in vehicles)
        {
            vehicle.GetComponent<VehicleSlot>().Init();
        }
    }

    public static void Show()
    {
        Open();
        Instance.currIdx = 0;

    }

    public static void Hide()
    {
        Close();
    }

    public override void OnBackPressed()
    {
        //MenuManager.Instance.OnCancelVehicle();
    }

    public override void OnEnterPressed()
    {
        //MenuManager.Instance.OnSelectVehicle();
    }

    public override void OnLeftPressed()
    {
        //if (!isCameraDoneMoving)
        //    return;

        //if (value.x < -CHANGE_VEHICLE_THRESHOLD)
        //{
        //    if (currIdx > 0)
        //    {
        //        vehicles[currIdx].GetComponent<VehicleSlot>().StopSelection(duration);
        //        currIdx -= 1;
        //        SelectVehicle();
        //    }
        //}
        //else if (value.x > CHANGE_VEHICLE_THRESHOLD)
        //{
        //    if (currIdx < vehicles.Length - 1)
        //    {
        //        vehicles[currIdx].GetComponent<VehicleSlot>().StopSelection(duration);
        //        currIdx += 1;
        //        SelectVehicle();
        //    }
        //}
    }

    public override void OnRightPressed()
    {
    }

    public void SelectVehicle()
    {
        vehicles[currIdx].GetComponent<VehicleSlot>().StartSelection(duration);
        isCameraDoneMoving = false;
        CameraVehicleSelection.Instance.MoveToNextVehicle(vehicles[currIdx].GetComponent<VehicleSlot>().GetCameraPosition(), duration);
        MainMenuCanvasManager.Instance.UpdateStatistics(vehicles[currIdx].GetComponent<VehicleSlot>().GetPlayerScriptableObject());
    }

    public void CameraDoneMoving()
    {
        isCameraDoneMoving = true;
    }
}
