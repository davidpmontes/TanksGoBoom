using System.Collections;
using UnityEngine;

public class VehicleSlot : MonoBehaviour
{
    [SerializeField] private PlayerScriptableObject pso;
    private GameObject vehicleModel;
    private GameObject cameraPosition;
    private Quaternion unselectedRotation = Quaternion.Euler(0, 180, 0);
    private Vector3 selectedLocalPosition = new Vector3(0, 1.2f - 0.39f, -7f + 1.36f);

    public void Init()
    {
        cameraPosition = transform.GetChild(0).gameObject;
        vehicleModel = transform.GetChild(1).gameObject;
        vehicleModel.transform.localRotation = unselectedRotation;
    }

    public PlayerScriptableObject GetPlayerScriptableObject()
    {
        return pso;
    }

    public GameObject GetCameraPosition()
    {
        return cameraPosition;
    }

    public void StartSelection(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(StartSelectionCR(duration));
        StartCoroutine(RotateClockwise());
    }

    public void StopSelection(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(StopSelectionCR(duration));
    }

    private IEnumerator StartSelectionCR(float duration)
    {
        Vector3 startPosition = vehicleModel.transform.localPosition;
        float endTime = Time.time + duration;
        float t = 0;

        while(Time.time < endTime)
        {
            vehicleModel.transform.localPosition = Vector3.Lerp(startPosition, selectedLocalPosition, t);
            yield return null;
            t = (duration - (endTime - Time.time)) / duration;
        }
        vehicleModel.transform.localPosition = selectedLocalPosition;
    }

    private IEnumerator StopSelectionCR(float duration)
    {
        Quaternion startRotation = vehicleModel.transform.localRotation;
        Vector3 startPosition = vehicleModel.transform.localPosition;
        float endTime = Time.time + duration;
        float t = 0;

        while (Time.time < endTime)
        {
            vehicleModel.transform.localRotation = Quaternion.Lerp(startRotation, unselectedRotation, t);
            vehicleModel.transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, t);
            yield return null;
            t = (duration - (endTime - Time.time)) / duration;
        }
        vehicleModel.transform.localRotation = unselectedRotation;
        vehicleModel.transform.localPosition = Vector3.zero;
    }

    private IEnumerator RotateClockwise()
    {
        while(true)
        {
            vehicleModel.transform.Rotate(0, 50 * Time.deltaTime, 0, Space.Self);
            yield return null;
        }
    }
}
