using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardDataBaseImporter : MonoBehaviour
{
    public CardDataBase cardDataBase;

    [ContextMenu("Import Selected Card Details from JSON")]
    public void ImportFromJson()
    {
        // Specify your path here
        string directoryPath = @"C:\Users\black\Downloads\Blitz_Dev\Firebase Content\firestore_database";
        string fileName = "unity_cards_data.json";
        string filePath = Path.Combine(directoryPath, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SimpleHolder simpleHolder = JsonUtility.FromJson<SimpleHolder>(json);

            foreach (var simpleDetail in simpleHolder.cardDetails)
            {
                var existingDetail = cardDataBase.cardDetails.Find(detail => detail.id == simpleDetail.Id);
                if (existingDetail != null)
                {
                    existingDetail.ergoTokenId = simpleDetail.ErgoTokenId;
                    // existingDetail.cardName = simpleDetail.CardName; // Uncomment if you wish to overwrite the name
                }
                else
                {
                    // Handle the case where the detail doesn't exist in your current database
                }
            }

            Debug.Log("Imported selected card details from JSON at: " + filePath);
        }
        else
        {
            Debug.LogError("JSON file not found at " + filePath);
        }
    }

    // These definitions must match the ones used in the export script.
    [System.Serializable]
    private class SimpleCardDetail
    {
        public int Id;
        public string ErgoTokenId;
        public string CardName;
    }

    [System.Serializable]
    private class SimpleHolder
    {
        public List<SimpleCardDetail> cardDetails;
    }
}
