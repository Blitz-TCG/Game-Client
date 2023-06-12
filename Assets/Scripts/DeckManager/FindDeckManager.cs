using UnityEngine;

public class FindDeckManager : MonoBehaviour
{
    DeckManager deckManager;
    public Card cardNormalPrefab;
    public static Transform doubleClickParent;
    public GameObject cardClicked;
    void Start()
    {
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
    }

    public void NormalCardClick()
    {
        doubleClickParent = transform.parent.parent.parent.parent;
        string doubleClickCardType = doubleClickParent.name.Split(" ")[0];
        deckManager.OnClickSingleCard(cardClicked, cardNormalPrefab, doubleClickCardType);
    }
}
