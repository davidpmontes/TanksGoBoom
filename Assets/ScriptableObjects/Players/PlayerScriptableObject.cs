using UnityEngine;

[CreateAssetMenu(fileName = "Data",
                 menuName = "ScriptableObjects/PlayerScriptableObject",
                 order = 1)]

public class PlayerScriptableObject : ScriptableObject
{
    public float maxLife;
    public float mass;
    public float moveSpeed;
    public float moveAcceleration;

    public string vehicleName;

    [TextArea(6, 6)]
    public string vehicleDescription;

    [Range(0, 10)] public int speedValue;
    [Range(0, 10)] public int armorValue;
    [Range(0, 10)] public int attackValue;

    public string pilotName;
    [Range(0, 115)] public int pilotAge;
    public Sprite pilotCountryFlag;
}