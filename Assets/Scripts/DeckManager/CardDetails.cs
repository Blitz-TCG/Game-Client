using UnityEngine;


[System.Serializable]
public class CardDetails
{
    public int id;
    public string ergoTokenId;
    public long ergoTokenAmount;
    public string cardName;
    public string cardDescription;
    public int attack;
    public int HP;
    public int gold;
    public int XP;
    //public int race;
    public int fieldLimit;
    public string levelRequired;
    public string clan;
    public CardClass cardClass;
    public Sprite cardImage;
    public Sprite cardFrame;
}