using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public enum PlayerType
    {
        drone,
        tank,
        walker
    }

    [SerializeField] private GameObject playerDronePrefab;
    [SerializeField] private GameObject playerTankPrefab;
    [SerializeField] private GameObject playerWalkerPrefab;
    [SerializeField] private GameObject player1SpawnPoint;

    private GameObject player1;

    private void Awake()
    {
        Instance = this;
        Utils.FindChildByNameRecursively(transform, "CameraSystems").GetComponent<CameraMovement>().Init();
        Utils.FindChildByNameRecursively(transform, "JukeBox").GetComponent<JukeBox>().Init();
        Utils.FindChildByNameRecursively(transform, "Canvas").GetComponent<CanvasManager>().Init();
    }

    public void StartBackgroundMusic()
    {
        JukeBox.Instance.StartMusic();
    }

    public GameObject GetPlayer1()
    {
        return player1;
    }

    public void SpawnPlayer(PlayerType type)
    {
        CameraMovement.Instance.enabled = false;
        if (player1)
            Destroy(player1);

        if (type == PlayerType.drone)
            player1 = Instantiate(playerDronePrefab);
        else if (type == PlayerType.tank)
            player1 = Instantiate(playerTankPrefab);
        else if (type == PlayerType.walker)
            player1 = Instantiate(playerWalkerPrefab);

        player1.GetComponent<PlayerController>().Init();
        player1.GetComponent<PlayerController>().SetStartingPosition(player1SpawnPoint.transform.position);

        CameraMovement.Instance.SetVehicle(player1);
        CameraMovement.Instance.SetThirdPersonCamera();
        CameraMovement.Instance.enabled = true;
    }
}