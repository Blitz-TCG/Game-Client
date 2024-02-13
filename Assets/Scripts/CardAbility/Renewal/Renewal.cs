using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Renewal : Card
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Renewal))]
    public class DerivedClassEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("id"));
            ////EditorGUILayout.PropertyField(serializedObject.FindProperty("multiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("healthAmount"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    public int healthAmount = 0;

    public int UseAbility(Card card, int maxHP)
    {
        Debug.Log(" use renewal called " + card.name);
        return card.HealCard(healthAmount, maxHP);
    }
}

