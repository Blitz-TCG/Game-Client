using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFieldPosition : MonoBehaviour
{
    RectTransform rectTransform;
    Vector2[] sizes = new Vector2[]
        {
            new Vector2(1705f, 805f),
            new Vector2(1705f, 805f),
            new Vector2(2565f, 1295f),
            new Vector2(2740f, 1310f),
            new Vector2(2850f, 1350f),
            new Vector2(2770f, 1310f),
            new Vector2(3150f, 1315f)
        };
    Vector2[] positions = new Vector2[]
        {
            new Vector2(0f, -437f),
            new Vector2(0f, -437f),
            new Vector2(0f, -435f),
            new Vector2(0f, -435f),
            new Vector2(-3f, -410f),
            new Vector2(0f, -430f),
            new Vector2(6f, -420f)
        };


    private void Start()
    {
        
    }

    public void SetObjectSize(int deckId)
    {
        Debug.Log(deckId + " deck id ");
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = sizes[deckId];
    }

    public void SetObjectPosition(int deckId, string fieldPos)
    {
        //Vector3 newPosition = new Vector3(posX, posY, transform.localPosition.z);
        rectTransform = GetComponent<RectTransform>();
        Vector3 newPosition = new Vector3(positions[deckId][0], fieldPos == "down" ? positions[deckId][1] : -positions[deckId][1], rectTransform.localPosition.z);
        rectTransform.localPosition = newPosition;
    }
}
