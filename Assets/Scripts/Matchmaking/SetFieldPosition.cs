using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFieldPosition : MonoBehaviour
{
    RectTransform rectTransform;

    Vector2[] BottomRightPositions = new Vector2[]
        {
            new Vector2(1365f, -845f),
            new Vector2(1365f, -845f),
            new Vector2(1295f, -1077f),
            new Vector2(1295f, -1077f),
            new Vector2(1400, -1077f),
            new Vector2(1355f, -1077f),
            new Vector2(1600f, -1100f)
        };

    Vector2[] BottomRightSizes = new Vector2[]
        {
            new Vector2(1370f, 1005f),
            new Vector2(1370f, 1005f),
            new Vector2(1300f, 1110f),
            new Vector2(1300f, 1300f),
            new Vector2(1405f, 1120f),
            new Vector2(1360f, 1330f),
            new Vector2(1605f, 1320f)
        };

    Vector2[] TopRightPositions = new Vector2[]
       {
            new Vector2(1365f, -155f),
            new Vector2(1365f, -155f),
            new Vector2(1295f, -215f),
            new Vector2(1400f, -625f),
            new Vector2(1475f, -265f),
            new Vector2(1400f, -260f),
            new Vector2(1625f, -255f)
       };

    Vector2[] TopRightSizes = new Vector2[]
        {
            new Vector2(1370f, 985f),
            new Vector2(1370f, 985f),
            new Vector2(1300f, 1285f),
            new Vector2(1425f, 1700f),
            new Vector2(1480f, 1340f),
            new Vector2(1405f, 1335f),
            new Vector2(1630f, 1330f)
        };

    Vector2[] TopLeftPositions = new Vector2[]
       {
            new Vector2(0f, -155f),
            new Vector2(0f, -155f),
            new Vector2(0f, -50f),
            new Vector2(0f, -235f),
            new Vector2(0f, -50f),
            new Vector2(0f, -270f),
            new Vector2(0f, -235f)
       };

    Vector2[] TopLeftSizes = new Vector2[]
        {
            new Vector2(1345f, 985f),
            new Vector2(1345f, 985f),
            new Vector2(1256f, 1120f),
            new Vector2(1255f, 1305f),
            new Vector2(1400f, 1120f),
            new Vector2(1360f, 1370f),
            new Vector2(1620f, 1310f)
        };

    Vector2[] BottomLeftPositions = new Vector2[]
        {
            new Vector2(0f, -845f),
            new Vector2(0f, -845f),
            new Vector2(0f, -1077f),
            new Vector2(0f, -1077f),
            new Vector2(0f, -1077f),
            new Vector2(0f, -1077f),
            new Vector2(0f, -1077f)
        };

    Vector2[] BottomLeftSizes = new Vector2[]
        {
            new Vector2(1345f, 1005f),
            new Vector2(1345f, 1005f),
            new Vector2(1265f, 1285f),
            new Vector2(1395f, 1700f),
            new Vector2(1400f, 1325f),
            new Vector2(1400f, 1330f),
            new Vector2(1620f, 1310f),
        };

    

    public void SetObjectSize(int deckId, int layerIndex)
    {
        Debug.Log(deckId + " deck id " + layerIndex + " layer index");
        rectTransform = GetComponent<RectTransform>();
        if(layerIndex == 0)
        {
            rectTransform.sizeDelta = BottomRightSizes[deckId];
        }
        else if(layerIndex == 1)
        {
            rectTransform.sizeDelta = TopRightSizes[deckId];
        }
        else if(layerIndex == 2)
        {
            rectTransform.sizeDelta = TopLeftSizes[deckId];
        }
        else if(layerIndex == 3)
        {
            rectTransform.sizeDelta = BottomLeftSizes[deckId];
        }
        
    }

    public void SetObjectPosition(int deckId, int layerIndex)
    {
        //Vector3 newPosition = new Vector3(posX, posY, transform.localPosition.z);
        rectTransform = GetComponent<RectTransform>();
        Vector2 positions = Vector2.zero;
        if (layerIndex == 0)
        {
            positions = BottomRightPositions[deckId];
        }
        else if(layerIndex == 1)
        {
            positions = TopRightPositions[deckId];
        }
        else if (layerIndex == 2)
        {
            positions = TopLeftPositions[deckId];
        }
        else if (layerIndex == 3)
        {
            positions = BottomLeftPositions[deckId];
        }
        Vector3 newPosition = new Vector3(positions.x, positions.y , rectTransform.localPosition.z);
        rectTransform.localPosition = newPosition;
    }
}

[System.Serializable]
public class Sprites
{
    public List<Sprite> sprites;
}