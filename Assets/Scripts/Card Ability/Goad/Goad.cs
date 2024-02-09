using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Goad : Card
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Goad))]
    public class DerivedClassEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("id"));
            ////EditorGUILayout.PropertyField(serializedObject.FindProperty("multiplier"));
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("baseGoldValue"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    public override void UseAbility(bool isMaster)
    {
        base.UseAbility();
    }
}
