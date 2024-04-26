using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardDataBaseExporter : MonoBehaviour
{
    public CardDataBase cardDataBase;

    [ContextMenu("Export Selected Card Details to JSON")]
    public void ExportToJson()
    {
        List<SimpleCardDetail> simpleDetails = new List<SimpleCardDetail>();

        foreach (var cardDetail in cardDataBase.cardDetails)
        {
            simpleDetails.Add(new SimpleCardDetail
            {
                Id = cardDetail.id,
                ErgoTokenId = cardDetail.ergoTokenId,
                CardName = cardDetail.cardName
            });
        }

        string json = JsonUtility.ToJson(new SimpleHolder { cardDetails = simpleDetails }, true);
        // Specify your path here
        string directoryPath = @"C:\Users\black\Downloads\Blitz_Dev\Firebase Content\firestore_database";
        string fileName = "selectedCardDatabase.json";
        string filePath = Path.Combine(directoryPath, fileName);

        // Ensure the directory exists
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(filePath, json);
        Debug.Log("Exported selected card details to JSON at: " + filePath);
    }

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
