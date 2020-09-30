using UnityEngine;

public class CameraMap : MonoBehaviour
{
    public static CameraMap Instance { get; private set; }

    [SerializeField] private GameObject vehicle = default;

    public void Init()
    {
        Instance = this;
    }

    public void SetVehicle(GameObject newVehicle)
    {
        vehicle = newVehicle;
    }

    private void LateUpdate()
    {
        MoveMapCamera();
    }

    private void MoveMapCamera()
    {
        transform.position = new Vector3(vehicle.transform.position.x,
                                         200,
                                         vehicle.transform.position.z);
        transform.rotation = vehicle.transform.rotation;
    }
}
