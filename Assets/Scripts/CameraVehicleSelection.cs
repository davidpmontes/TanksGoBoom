using System.Collections;
using UnityEngine;

public class CameraVehicleSelection : MonoBehaviour
{
    public static CameraVehicleSelection Instance { get; private set; }

    public void Init()
    {
        Instance = this;
    }

    public void MoveToNextVehicle(GameObject nextPosition, float duration)
    {
        StartCoroutine(MoveDirection(nextPosition, duration));
    }

    private IEnumerator MoveDirection(GameObject nextPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float endTime = Time.time + duration;
        float t = 0;

        while (Time.time < endTime)
        {
            transform.position = Vector3.Lerp(startPosition, nextPosition.transform.position, t);
            yield return null;
            t = (duration - (endTime - Time.time)) / duration;
        }
        transform.position = nextPosition.transform.position;
        VehicleSelectorMenu.Instance.CameraDoneMoving();
    }
}
