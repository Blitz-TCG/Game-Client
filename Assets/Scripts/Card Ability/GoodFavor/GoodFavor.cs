using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class GoodFavor : Card
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GoodFavor))]
    public class DerivedClassEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //DrawDefaultInspector();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("id"));
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("multiplier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("baseGoldValue"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    public int multiplier = 1;
    public int baseGoldValue = 25;

    public void SetAbility(int multiplayerValue = 1, int goldValue = 25)  // for setting up the here you can use ability like that 
      // for 2x gold  just you need to change the multiplayer value.
      // If you want custom gold value than use SetAbility like that multiplayerValue = 1 and goldValue =as you want  ie SetAbility(1,35)
    {
        multiplier = multiplayerValue;
        baseGoldValue = multiplayerValue * goldValue; // Adjust this calculation based on your requirements
    }

    public void StartTurn(bool isMaster)
    {
        Debug.Log("StartTurn called " + baseGoldValue);
        GainGold(baseGoldValue, isMaster);

    }

    public override void UseAbility(bool isMaster)
    {
        base.UseAbility();
        Debug.Log("Ability 1 used with value: " + baseGoldValue);
        StartTurn(isMaster);
    }


    private void GainGold(int amount, bool master)
    {
        Debug.Log("gain gold called " + master + " amt " + amount);
        if(master)
        {
            int totalPlayerGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["masterGold"];
            totalPlayerGold += amount;
            PhotonNetwork.CurrentRoom.CustomProperties["masterGold"] = totalPlayerGold;
            Debug.Log(totalPlayerGold + " total PlayerGold gold ");
            Gold.instance.SetGold(totalPlayerGold);
        }
        else
        {
            int totalClientGold = (int)PhotonNetwork.CurrentRoom.CustomProperties["clientGold"];
            totalClientGold += amount;
            PhotonNetwork.CurrentRoom.CustomProperties["clientGold"] = totalClientGold;
            Debug.Log(totalClientGold + " total client gold ");
            Gold.instance.SetGold(totalClientGold);
        }

        Debug.Log($"Gained {amount} gold. Total gold: {gold}");
    }
}
