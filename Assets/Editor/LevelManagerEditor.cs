using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelManager myTarget = (LevelManager)target;

        if (GUILayout.Button("Spawn Helicopter"))
        {
            myTarget.SpawnPlayer(LevelManager.PlayerType.helicopter);
        }

        if (GUILayout.Button("Spawn Drone"))
        {
            myTarget.SpawnPlayer(LevelManager.PlayerType.drone);
        }

        if (GUILayout.Button("Spawn Tank"))
        {
            myTarget.SpawnPlayer(LevelManager.PlayerType.tank);
        }

        if (GUILayout.Button("Spawn Walker"))
        {
            myTarget.SpawnPlayer(LevelManager.PlayerType.walker);
        }

        if (GUILayout.Button("Start Music"))
        {
            myTarget.StartBackgroundMusic();
        }
    }
}
