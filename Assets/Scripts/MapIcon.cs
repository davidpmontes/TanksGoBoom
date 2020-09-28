using UnityEngine;

public class MapIcon : MonoBehaviour
{
    private Transform positionTrack;
    private Transform rotationTrack;

    private void Awake()
    {
        transform.SetParent(Utils.FindChildByNameRecursively(GameObject.Find("LevelManager").transform, "StaticMapIcons").transform);
        enabled = false;
    }

    public void StartTracking(Transform position, Transform rotation)
    {
        positionTrack = position;
        rotationTrack = rotation;
        enabled = true;
    }

    void Update()
    {
        if (positionTrack == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = positionTrack.position;
            transform.rotation = Quaternion.Euler(0, rotationTrack.rotation.eulerAngles.y, 0);
        }
    }
}
