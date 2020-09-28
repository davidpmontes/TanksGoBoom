using UnityEngine;

[CreateAssetMenu(fileName = "Data",
                 menuName = "ScriptableObjects/WeaponScriptableObject",
                 order = 1)]

public class WeaponScriptableObject : ScriptableObject
{
    public float mass;
    public float damage;
    public float speed;
    public float spreadSpead;
    public float maxTime;
}