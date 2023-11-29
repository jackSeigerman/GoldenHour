using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TRN_Generator))]
public class TRN_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TRN_Generator terrain = (TRN_Generator)target;

        if (GUILayout.Button("Generate"))
        {
            terrain.spawnPrefabs = true;
            terrain.Generate();
        }

        if (GUILayout.Button("Generate Random"))
        {
            terrain.seed = Random.value * 50000f;
            terrain.spawnPrefabs = true;
            terrain.Generate();
        }

        if (GUILayout.Button("Generate Heightmap"))
        {
            terrain.spawnPrefabs = false;
            terrain.Generate();
        }

        if (GUILayout.Button("Generate Heightmap Random"))
        {
            terrain.seed = Random.value * 50000f;
            terrain.spawnPrefabs = false;
            terrain.Generate();
        }

        if (GUILayout.Button("Spawn Prefabs"))
        {
            terrain.SpawnTrees();
        }

        if (GUILayout.Button("Spawn Details"))
        {
            terrain.SpawnDetailTextures();
        }
    }
}
