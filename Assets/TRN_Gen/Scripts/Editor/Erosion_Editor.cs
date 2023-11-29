using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TRN_Erosion))]
public class Erosion_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //TRN_Erosion simulation = (TRN_Erosion)target;

        //if(GUILayout.Button("Run Simulation"))
        //{
        //    simulation.Run();
        //}

        //if (GUILayout.Button("Save Heightmap"))
        //{
        //    simulation.SaveHeight();
        //}

        //if (GUILayout.Button("Restore Heightmap"))
        //{
        //    simulation.RestoreHeight();
        //}
    }
}
