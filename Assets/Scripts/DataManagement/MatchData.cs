using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public class MatchData //look at my other script and help point me to the actual match values
{
    public string matchid;
    public GameMode gameMode;
    public string winner;
    public string loser;
    public DeckGeneral winnerDeck;
    public DeckGeneral loserDeck;
    public int winnerXp;
    public int loserXp;
    public int winnerMmrChange;
    public int loserMmrChange;
    public float matchLength;
    public int totalTurns;
    public MatchStatus matchStatus;

    public MatchData(string id, GameMode mode, string winnerId, string loserId, DeckGeneral winnerDeckGeneral, DeckGeneral loserDeckGeneral, int winnerXP, int loserXP, int winnerMmrValue, int loserMmrValue, float matchLengthValue, int turns, MatchStatus currentStatus)
    {
        matchid = id;
        this.gameMode = mode;
        this.winner = winnerId;
        this.loser = loserId;
        this.winnerDeck = winnerDeckGeneral;
        this.loserDeck = loserDeckGeneral;
        this.winnerXp = winnerXP;
        this.loserXp = loserXP;
        this.winnerMmrChange = winnerMmrValue;
        this.loserMmrChange = loserMmrValue;
        this.matchLength = matchLengthValue;
        this.totalTurns = turns;
        this.matchStatus = currentStatus;
    }
}

public enum MatchStatus
{
    PlayerQuit,
    PlayerDisconnect,
    PlayerForfeit,
    Normal,
    Unknown
}

public enum GameMode
{
    Ranked,
    OpenToPlay
}

public enum DeckGeneral
{
    GreenField,
    PurpleField,
    Masquerades,
    OldKingdom,
    Tinkerers,
    DarkMatter, 
    Fairytales, 
    Unknown
}
