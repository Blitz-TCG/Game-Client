[System.Serializable]
public class MatchData
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
    Masquerades,
    OldKingdom,
    Tinkerers,
    DarkMatter, 
    Fairytales
}
