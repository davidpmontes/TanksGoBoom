using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private GameObject rotateable;
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject[] wheels;
    [SerializeField] private GameObject groundNormalRaycastOrigin;

    private CharacterController controller;

    private const float GRAVITY = -9.81f;

    private CameraConfigs cameraConfigs;
    private Vector3 moveDirection;
    public float speed = 15;
    private int currIdx = 0;
    private float distance;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currIdx = Random.Range(0, waypoints.Length);
        cameraConfigs = new CameraConfigs();
        cameraConfigs.minRadius = 65;
        cameraConfigs.maxRadius = 70;
        cameraConfigs.rearLocal = new Vector3(0, 20f, -65f);
    }

    private void Update()
    {
        NavigateWaypoints();
        Normalize();
        RotateWheels();
    }

    private void NavigateWaypoints()
    {
        distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                    new Vector3(waypoints[currIdx].transform.position.x, 0, waypoints[currIdx].transform.position.z));

        if (distance < 3)
        {
            currIdx = Random.Range(0, waypoints.Length);
        }

        var direction = (waypoints[currIdx].transform.position - transform.position).normalized;
        direction.y = 0;

        rotateable.transform.rotation = Quaternion.Slerp(rotateable.transform.rotation, Quaternion.LookRotation(direction), 2 * Time.deltaTime);

        moveDirection.x = rotateable.transform.forward.x * speed * Time.deltaTime;
        moveDirection.y += GRAVITY * Time.deltaTime * Time.deltaTime;
        moveDirection.z = rotateable.transform.forward.z * speed * Time.deltaTime;

        controller.Move(moveDirection);
    }

    private void Normalize()
    {
        if (Physics.Raycast(groundNormalRaycastOrigin.transform.position, Vector3.down, out RaycastHit hit))
        {
            Quaternion grndTilt = Quaternion.FromToRotation(Vector3.up, hit.normal);
            normal.transform.rotation = Quaternion.RotateTowards(normal.transform.rotation, grndTilt * Quaternion.Euler(0, rotateable.transform.rotation.eulerAngles.y, 0), 50 * Time.deltaTime);
        }
    }

    private void RotateWheels()
    {
        foreach(GameObject wheel in wheels)
        {
            wheel.transform.Rotate(Vector3.right, 10 * speed * Time.deltaTime, Space.Self);
        }
    }
}
