using System.Collections;
using UnityEngine;

public struct CameraConfigs
{
    public float minRadius;
    public float maxRadius;
    public Vector3 rearLocal;
}

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance { get; private set; }

    [SerializeField] private GameObject vehicle = default;
    [SerializeField] private GameObject shake = default;
    [SerializeField] private GameObject mapCamera = default;

    private GameObject firstPersonCameraPosition;
    private GameObject thirdPersonCameraPosition;

    private float radius;
    private float minRadius = 65;
    private float maxRadius = 70;

    delegate void MoveCamera();
    MoveCamera moveVehicleCamera;

    public void Init()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        moveVehicleCamera.Invoke();
        MoveMapCamera();
    }

    public void SetVehicle(GameObject newVehicle)
    {
        vehicle = newVehicle;
    }

    private void MoveMapCamera()
    {
        mapCamera.transform.position = new Vector3(vehicle.transform.position.x,
                                                   100,
                                                   vehicle.transform.position.z);
    }

    private void MoveFirstPerson()
    {
        transform.position = firstPersonCameraPosition.transform.position;
        transform.rotation = firstPersonCameraPosition.transform.rotation;
    }

    private void MoveThirdPerson()
    {
        transform.position = thirdPersonCameraPosition.transform.position;
        transform.rotation = thirdPersonCameraPosition.transform.rotation;
    }

    public void UpdateRadius(float speedPercent)
    {
        if (moveVehicleCamera == MoveFirstPerson)
        {
            Camera.main.transform.localPosition = Vector3.zero;
            return;
        }

        radius = minRadius + speedPercent * (maxRadius - minRadius);
        var position = Camera.main.transform.localPosition;
        position.z = -radius;
        Camera.main.transform.localPosition = position;
    }

    public void ForwardView()
    {
        Camera.main.transform.localPosition = new Vector3(0, 0, 0f);
        Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void LeftView()
    {
        Camera.main.transform.localPosition = new Vector3(-0.3f, 0, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(0, 90, 0);
    }

    public void RearView()
    {
        Camera.main.transform.localPosition = new Vector3(0, 7, -14.5f);
        Camera.main.transform.localRotation = Quaternion.Euler(10, 0, 0);
    }

    public void RightView()
    {
        Camera.main.transform.localPosition = new Vector3(0.3f, 0, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(0, -90, 0);
    }

    public void TopView()
    {
        Camera.main.transform.localPosition = new Vector3(0, 0.3f, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public void ToggleCamera()
    {
        if (moveVehicleCamera == MoveFirstPerson)
        {
            SetThirdPersonCamera();
        }
        else
        {
            SetFirstPersonCamera();
        }
    }

    public void SetFirstPersonCamera()
    {
        ForwardView();
        firstPersonCameraPosition = Utils.FindChildByNameRecursively(vehicle.transform, "CameraPositionFirstPerson");
        moveVehicleCamera = MoveFirstPerson;
    }

    public void SetThirdPersonCamera()
    {
        RearView();
        thirdPersonCameraPosition = Utils.FindChildByNameRecursively(vehicle.transform, "CameraPositionThirdPerson");
        moveVehicleCamera = MoveThirdPerson;
    }

    public void Shake()
    {
        shake.transform.localPosition = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        float endTime = Time.time + 1;

        while(Time.time < endTime)
        {
            shake.transform.localPosition = Random.onUnitSphere * (endTime - Time.time);
            yield return null;
        }

        shake.transform.localPosition = Vector3.zero;
    }
}
