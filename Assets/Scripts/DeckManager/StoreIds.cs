[System.Serializable]
public class StoreIdsAvailable 
{
    public int[] cardIdAvailable;
    public int hideCardsAvailable;
    public string filterCardsAvailable;

    public int hideGameboards;
    public string filterGameboards;
    public int hideGeneralPfp;
    public string filterGeneralPfp;

    public StoreIdsAvailable(int[] cardIdAvailable, int hideCardsAvailable, string filterCardsAvailable, int hideGameboards, string filterGameboards, int hideGeneralPfp, string filterGeneralPfp)
    {
        this.cardIdAvailable = cardIdAvailable;
        this.filterCardsAvailable = filterCardsAvailable;
        this.hideCardsAvailable = hideCardsAvailable;
        this.hideGameboards = hideGameboards;
        this.filterGameboards = filterGameboards;
        this.hideGeneralPfp = hideGeneralPfp;
        this.filterGeneralPfp = filterGeneralPfp;

    }
}
public class StoreIdsCurrent
{
    public int[] cardIdCurrent;
    public string filterCardsCurrent;
    public string gameboardCurrent;
    public string generalPfpCurrent;

    public string deckTitle;
    public string deckBody;

    public int deckId;
    public int deckGeneral;

    public StoreIdsCurrent(int[] cardIdCurrent, string filterCardsCurrent, string gameboardCurrent, string generalPfpCurrent, string deckTitle, string deckBody, int deckId, int deckGeneral)
    {
        this.cardIdCurrent = cardIdCurrent;
        this.filterCardsCurrent = filterCardsCurrent;
        this.gameboardCurrent = gameboardCurrent;
        this.generalPfpCurrent = generalPfpCurrent;
        this.deckTitle = deckTitle;
        this.deckBody = deckBody;
        this.deckId = deckId;
        this.deckGeneral = deckGeneral;
    }
}