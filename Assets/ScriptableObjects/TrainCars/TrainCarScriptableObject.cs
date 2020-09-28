using UnityEngine;

[CreateAssetMenu(fileName = "Data",
                 menuName = "ScriptableObjects/TrainCarScriptableObject",
                 order = 1)]

public class TrainCarScriptableObject : ScriptableObject
{
    public float startingLife;
    public float finalExplosionForce;
}