using UnityEngine;

[CreateAssetMenu(fileName = "Data",
                 menuName = "ScriptableObjects/CivilianVehiclesScriptableObject",
                 order = 1)]

public class CivilianVehiclesScriptableObject : ScriptableObject
{
    public float startingLife;
    public float finalExplosionForce;
}