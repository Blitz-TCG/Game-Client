//using UnityEditor;
//using UnityEngine;
//using System; // Add this for the 'Type' class

//#if UNITY_EDITOR
//[CustomEditor(typeof(CardDetails))]
//public class CardDetailsEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        CardDetails cardDetails = (CardDetails)target;

//        EditorGUI.BeginChangeCheck();
//        base.OnInspectorGUI();
//        if (EditorGUI.EndChangeCheck())
//        {
//            serializedObject.ApplyModifiedProperties();

//            // Repaint Inspector to reflect changes
//            Repaint();
//        }

//        EditorGUILayout.Space();

//        switch (cardDetails.ability)
//        {
//            case CardAbility.Clone:
//                cardDetails.showMultiplierAndFailureChance = EditorGUILayout.Toggle("Show Multiplier and Failure Chance", cardDetails.showMultiplierAndFailureChance);
//                break;
//            case CardAbility.Meteor:
//                cardDetails.showHealth = EditorGUILayout.Toggle("Show Health", cardDetails.showHealth);
//                break;
//            case CardAbility.Evolve:
//                cardDetails.showDamage = EditorGUILayout.Toggle("Show Damage", cardDetails.showDamage);
//                break;
//            default:
//                break;
//        }
//    }

//}
//#endif
