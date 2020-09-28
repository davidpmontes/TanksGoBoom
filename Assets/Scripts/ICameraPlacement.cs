using UnityEngine;

public interface ICameraPlacement
{
    GameObject GetCameraPositionFirstPerson();
    GameObject GetCameraPositionThirdPerson();
    GameObject GetCameraFocusFirstPoint();
    GameObject GetCameraFocusThirdPoint();
}
