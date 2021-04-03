using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwapMultipleObjects))]
public class SwapMultipleObjectsEditor : Editor
{

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SwapMultipleObjects swapObjects = (SwapMultipleObjects)target;

        if (GUILayout.Button("swap objects")) {
            swapObjects.SwapObjects();
        }
    }


}
