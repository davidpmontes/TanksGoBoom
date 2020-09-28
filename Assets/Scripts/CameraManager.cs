using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject currentVehicle;

    private ICameraPlacement iCameraPlacement;
    private GameObject cameraPosition;
    private GameObject cameraFocusPoint;
    private bool isFirst;

    public void Start()
    {
        iCameraPlacement = currentVehicle.GetComponent<ICameraPlacement>();
        isFirst = false;
        SetCamera();
    }

    private void SetCamera()
    {
        if (isFirst)
        {
            cameraPosition = iCameraPlacement.GetCameraPositionFirstPerson();
            cameraFocusPoint = iCameraPlacement.GetCameraFocusFirstPoint();
        }
        else
        {
            cameraPosition = iCameraPlacement.GetCameraPositionThirdPerson();
            cameraFocusPoint = iCameraPlacement.GetCameraFocusThirdPoint();
        }
    }

    public void CycleCamera()
    {
        isFirst = !isFirst;
        SetCamera();
    }

    private void LateUpdate()
    {
        var distance = Vector3.Distance(transform.position, cameraPosition.transform.position);

        //transform.position = Vector3.MoveTowards(transform.position, cameraPosition.transform.position, 5 * distance * Time.deltaTime);
        transform.position = cameraPosition.transform.position;
        transform.LookAt(cameraFocusPoint.transform.position, Vector3.up);
    }
}
