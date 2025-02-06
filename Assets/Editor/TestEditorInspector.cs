using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestData))]
public class TestEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
