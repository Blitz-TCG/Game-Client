using System.Collections;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;

public class databaseExampleDeleteAfterReview : MonoBehaviour
{
    //this is pseudocode for you to put into your matchmaking script(s) where appropropriate to update match and user data after Skirmish matches are completed
    //you worked on the 'MatchData' script previously, so either move this content over into it after review or repurpose this script as needed

    public static databaseExampleDeleteAfterReview instance;

    //match data -------- these 'match data' elements are where I need your help, these need to point to the relevant match variables at the end of a match
    string gameMode;
    int winnerMmrChange;
    int loserMmrChange;
    int winnerXp;
    int loserXp;
    string winnerDeck;
    string loserDeck;
    int matchLength;
    int winnerTurnsTaken;
    int loserTurnsTaken;
    string matchStatus;
    //--------------------------------------------------------

    //testing variables
    string winnerHardSet;
    string loserHardSet;
    private DatabaseReference dbReference;

    //winner data --- do not touch
    int xpOpenWinner;
    int mmrOpenWinner;
    int totalTimePlayedOpenWinner;
    int totalTurnsTakenOpenWinner;
    int margoDeckWinOpenWinner;
    int margoDeckLossOpenWinner;
    int miosDeckWinOpenWinner;
    int miosDeckLossOpenWinner;
    int nasseDeckWinOpenWinner;
    int nasseDeckLossOpenWinner;
    int voidDeckWinOpenWinner;
    int voidDeckLossOpenWinner;
    int tootDeckWinOpenWinner;
    int tootDeckLossOpenWinner;


    //loser data --- do not touch
    int xpOpenLoser;
    int mmrOpenLoser;
    int totalTimePlayedOpenLoser;
    int totalTurnsTakenOpenLoser;
    int margoDeckWinOpenLoser;
    int margoDeckLossOpenLoser;
    int miosDeckWinOpenLoser;
    int miosDeckLossOpenLoser;
    int nasseDeckWinOpenLoser;
    int nasseDeckLossOpenLoser;
    int voidDeckWinOpenLoser;
    int voidDeckLossOpenLoser;
    int tootDeckWinOpenLoser;
    int tootDeckLossOpenLoser;

    //general data --- do not touch
    int margoWinner;
    int margoLoser;
    int miosWinner;
    int miosLoser;
    int nasseWinner;
    int nasseLoser;
    int voidWinner;
    int voidLoser;
    int tootWinner;
    int tootLoser;

    private void Awake() //for testing
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
    }

    public void Start() //for testing purposes
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void MatchEnd()
    {
        //On MatchEnd, StartCoroutine to push match data to Firebase for each user
        StartCoroutine(MatchDataUpdates());
    }

    public IEnumerator MatchDataUpdates()
    {
        yield return StartCoroutine(PushMatchDataToFirebase()); //first update the match data itself
        yield return StartCoroutine(LoadPlayerDataFromFirebase()); //then load each user's current data
        yield return StartCoroutine(PushPlayerDataToFirebase()); //then update the specific player data for each user
    }

    public IEnumerator PushMatchDataToFirebase() //these are the values I need all pointed to the dynamic values generated and stored during a match
    {
        //I hard set some of these varibles for testing purposes, but they should all be using the real match data.
        //FirebaseUser winner = FirebaseManager.instance.user; //update this to the winning user
        //FirebaseUser loser = FirebaseManager.instance.user; //update this to the losing user

        winnerHardSet = "00ZrYBVWIzPHWIYTXTiNKig7P8z1";
        loserHardSet = "0VbXHY9ASnbBs8pMlNYMnGZz0wg1";
        gameMode = "open"; //for Skirmish matches, this should always be set to 'open', for Blitz matches (coming later), it will be set to the 'season#'.
        winnerDeck = "Masquerades"; //update this with the the actual deck the winning player used during the match
        loserDeck = "The Old Kingdom"; //update this with the the actual deck the loser player used during the match
        matchLength = 180; //should be updated dynamically based on the total match length in seconds
        winnerTurnsTaken = 15; //should be updated dynamically based on the total turns taken in a match
        loserTurnsTaken = 14; //should be updated dynamically based on the total turns taken in a match
        matchStatus = "normal"; //should be updated dynamically based on the how a match ended

        if (matchStatus != "draw") //'playerQuit', 'playerDisconnect', and 'playerForfeit' should all cause the leaving player to lose and hence the 'normal' rules apply.
        {
            winnerXp = 100; //set to 100 for now
            loserXp = 25; //set to 25 for now
            winnerMmrChange = 25; //for now, the winner should gain 25 mmr
            loserMmrChange = -25; //for now, the loser should lose 25 mmr

        }
        else if (matchStatus == "draw")
        {
            winnerXp = 50;
            loserXp = 50;
            winnerMmrChange = 0; //if a match ends in a draw, then neither player should get any mmr
            loserMmrChange = 0;
        }

        //reminder for me, use winner.UserId notation instead of hard setting the user ID here.
        MatchEndData PlayerData = new MatchEndData(gameMode, winnerHardSet, loserHardSet, winnerDeck, loserDeck, winnerXp, loserXp, winnerMmrChange, loserMmrChange, matchLength, winnerTurnsTaken, loserTurnsTaken, matchStatus);
        string jsonString = JsonUtility.ToJson(PlayerData);

        var matchData = dbReference.Child("matchData").Push().SetRawJsonValueAsync(jsonString);
        yield return new WaitUntil(predicate: () => matchData.IsCompleted);
        if (matchData.IsFaulted)
        {
            Debug.LogError("Failed to send match data to Firebase"); ;
        }
        else if (matchData.IsCompleted)
        {
            Debug.Log("Match data sent to Firebase");
        }
    }

    public IEnumerator LoadPlayerDataFromFirebase()
    {
        var winnerLoad = FirebaseDatabase.DefaultInstance.GetReference("open").Child(winnerHardSet).GetValueAsync(); //update this user to be the winning user
        yield return new WaitUntil(predicate: () => winnerLoad.IsCompleted);
        if (winnerLoad.IsFaulted)
        {
            Debug.Log("Unable to load winning player's data");
        }
        else if (winnerLoad.IsCompleted)
        {
            Debug.Log("Loaded winning player's data successfully");

            DataSnapshot snapshot = winnerLoad.Result;

            if (snapshot.Value == null)
            {
                Debug.Log("creating wining player data tree");
                StartCoroutine(CreateLosingPlayerData());
            }
            else
            {
                Debug.Log("winning player data tree already created, loading current values.");
                var fullJSON = JsonConvert.DeserializeObject(snapshot.GetRawJsonValue());
                PlayerMatchData deckData = JsonUtility.FromJson<PlayerMatchData>(fullJSON.ToString());

                xpOpenWinner = deckData.xpOpen;
                mmrOpenWinner = deckData.mmrOpen;
                totalTimePlayedOpenWinner = deckData.totalTimePlayedOpen;
                totalTurnsTakenOpenWinner = deckData.totalTurnsTakenOpen;
                margoDeckWinOpenWinner = deckData.margoDeckWinOpen;
                margoDeckLossOpenWinner = deckData.margoDeckLossOpen;
                miosDeckWinOpenWinner = deckData.miosDeckWinOpen;
                miosDeckLossOpenWinner = deckData.miosDeckLossOpen;
                nasseDeckWinOpenWinner = deckData.nasseDeckWinOpen;
                nasseDeckLossOpenWinner = deckData.nasseDeckLossOpen;
                voidDeckWinOpenWinner = deckData.voidDeckWinOpen;
                voidDeckLossOpenWinner = deckData.voidDeckLossOpen;
                tootDeckWinOpenWinner = deckData.tootDeckWinOpen;
                tootDeckLossOpenWinner = deckData.tootDeckLossOpen;
            }
        }

        var loserLoad = FirebaseDatabase.DefaultInstance.GetReference("open").Child(loserHardSet).GetValueAsync(); //update this user to be the losing user
        yield return new WaitUntil(predicate: () => loserLoad.IsCompleted);
        if (loserLoad.IsFaulted)
        {
            Debug.Log("Unable to load losing player's data");
        }
        else if (loserLoad.IsCompleted)
        {
            Debug.Log("Loaded losing player's data successfully");

            DataSnapshot snapshot = loserLoad.Result;

            if (snapshot.Value == null)
            {
                Debug.Log("creating losing player data tree");
                StartCoroutine(CreateWinningPlayerData());
            }
            else
            {
                Debug.Log("losing player data tree already created, loading current values.");
                var fullJSON = JsonConvert.DeserializeObject(snapshot.GetRawJsonValue());
                PlayerMatchData deckData = JsonUtility.FromJson<PlayerMatchData>(fullJSON.ToString());

                xpOpenLoser = deckData.xpOpen;
                mmrOpenLoser = deckData.mmrOpen;
                totalTimePlayedOpenLoser = deckData.totalTimePlayedOpen;
                totalTurnsTakenOpenLoser = deckData.totalTurnsTakenOpen;
                margoDeckWinOpenLoser = deckData.margoDeckWinOpen;
                margoDeckLossOpenLoser = deckData.margoDeckLossOpen;
                miosDeckWinOpenLoser = deckData.miosDeckWinOpen;
                miosDeckLossOpenLoser = deckData.miosDeckLossOpen;
                nasseDeckWinOpenLoser = deckData.nasseDeckWinOpen;
                nasseDeckLossOpenLoser = deckData.nasseDeckLossOpen;
                voidDeckWinOpenLoser = deckData.voidDeckWinOpen;
                voidDeckLossOpenLoser = deckData.voidDeckLossOpen;
                tootDeckWinOpenLoser = deckData.tootDeckWinOpen;
                tootDeckLossOpenLoser = deckData.tootDeckLossOpen;
            }
        }
    }

    public IEnumerator CreateWinningPlayerData()
    {
        PlayerMatchData winningPlayerData = new PlayerMatchData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        string jsonStringData = JsonConvert.SerializeObject(winningPlayerData);

        var createWinningPlayerData = dbReference.Child("open").Child(winnerHardSet).SetRawJsonValueAsync(jsonStringData); //update this user to be the winning user
        yield return new WaitUntil(predicate: () => createWinningPlayerData.IsCompleted);
        if (createWinningPlayerData.IsFaulted)
        {
            Debug.LogError("Losing player failed to be created.");
        }
        else if (createWinningPlayerData.IsCompleted)
        {
            Debug.Log("Losing player was successfully created.");
        }
    }

    public IEnumerator CreateLosingPlayerData()
    {
        PlayerMatchData losingPlayerData = new PlayerMatchData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        string jsonStringData = JsonConvert.SerializeObject(losingPlayerData);

        var createLosingPlayerData = dbReference.Child("open").Child(loserHardSet).SetRawJsonValueAsync(jsonStringData); //update this user to be the losing user
        yield return new WaitUntil(predicate: () => createLosingPlayerData.IsCompleted);
        if (createLosingPlayerData.IsFaulted)
        {
            Debug.LogError("Losing player failed to be created.");
        }
        else if (createLosingPlayerData.IsCompleted)
        {
            Debug.Log("Losing player was successfully created.");
        }
    }


    public IEnumerator PushPlayerDataToFirebase()
    {
        DeckChecks(); //determine which decks lost and which won per user

        PlayerMatchData winningPlayerData = new PlayerMatchData(xpOpenWinner + winnerXp, mmrOpenWinner + winnerMmrChange, totalTimePlayedOpenWinner + matchLength, totalTurnsTakenOpenWinner + winnerTurnsTaken,
            margoDeckWinOpenWinner + margoWinner, margoDeckLossOpenWinner, miosDeckWinOpenWinner + miosWinner, miosDeckLossOpenWinner, nasseDeckWinOpenWinner + nasseWinner, nasseDeckLossOpenWinner,
            voidDeckWinOpenWinner + voidWinner, voidDeckLossOpenWinner, tootDeckWinOpenWinner + tootWinner, tootDeckLossOpenWinner);

        PlayerMatchData losingPlayerData = new PlayerMatchData(xpOpenLoser + loserXp, mmrOpenLoser + loserMmrChange, totalTimePlayedOpenLoser + matchLength, totalTurnsTakenOpenLoser + loserTurnsTaken,
            margoDeckWinOpenLoser, margoDeckLossOpenLoser + margoLoser, miosDeckWinOpenLoser, miosDeckLossOpenLoser + miosLoser, nasseDeckWinOpenLoser, nasseDeckLossOpenLoser + nasseLoser,
            voidDeckWinOpenLoser, voidDeckLossOpenLoser + voidLoser, tootDeckWinOpenLoser, tootDeckLossOpenLoser + tootLoser);

        string jsonStringWinner = JsonConvert.SerializeObject(winningPlayerData);
        string jsonStringLoser = JsonConvert.SerializeObject(losingPlayerData);

        var pushWinnerData = dbReference.Child("open").Child(winnerHardSet).SetRawJsonValueAsync(jsonStringWinner); //set to winning user ID
        yield return new WaitUntil(predicate: () => pushWinnerData.IsCompleted);
        if (pushWinnerData.IsFaulted)
        {
            Debug.LogError("Winning user data update failed."); ;
        }
        else if (pushWinnerData.IsCompleted)
        {
            Debug.Log("Winning user data updated successfully");
        }

        var pushLoserData = dbReference.Child("open").Child(loserHardSet).SetRawJsonValueAsync(jsonStringLoser); //set to loser user ID
        yield return new WaitUntil(predicate: () => pushLoserData.IsCompleted);
        if (pushLoserData.IsFaulted)
        {
            Debug.LogError("Losing user data update failed."); ;
        }
        else if (pushLoserData.IsCompleted)
        {
            Debug.Log("Losing user data updated successfully");
        }

    }

    public void DeckChecks()
    {
        //winner
        if (winnerDeck == "Masquerades")
        {
            margoWinner = 1;
        }
        else
        {
            margoWinner = 0;
        }

        if (winnerDeck == "The Old Kingdom")
        {
            miosWinner = 1;
        }
        else
        {
            miosWinner = 0;
        }

        if (winnerDeck == "Fairytales")
        {
            nasseWinner = 1;
        }
        else
        {
            nasseWinner = 0;
        }

        if (winnerDeck == "Dark Matter")
        {
            voidWinner = 1;
        }
        else
        {
            voidWinner = 0;
        }

        if (winnerDeck == "Tinkerers")
        {
            tootWinner = 1;
        }
        else
        {
            tootWinner = 0;
        }

        //loser

        if (loserDeck == "Masquerades")
        {
            margoLoser = 1;
        }
        else
        {
            margoLoser = 0;
        }

        if (loserDeck == "The Old Kingdom")
        {
            miosLoser = 1;
        }
        else
        {
            miosLoser = 0;
        }

        if (loserDeck == "Fairytales")
        {
            nasseLoser = 1;
        }
        else
        {
            nasseLoser = 0;
        }

        if (loserDeck == "Dark Matter")
        {
            voidLoser = 1;
        }
        else
        {
            voidLoser = 0;
        }

        if (loserDeck == "Tinkerers")
        {
            tootLoser = 1;
        }
        else
        {
            tootLoser = 0;
        }
    }

}
public class MatchEndData
{
    public string gameMode;
    public string winner;
    public string loser;
    public string winnerDeck;
    public string loserDeck;
    public int winnerXp;
    public int loserXp;
    public int winnerMmrChange;
    public int loserMmrChange;
    public int matchLength;
    public int winnerTurnsTaken;
    public int loserTurnsTaken;
    public string matchStatus;


    public MatchEndData(string gameMode, string winner, string loser, string winnerDeck, string loserDeck, int winnerXp, int loserXp, int winnerMmrChange,
        int loserMmrChange, int matchLength, int winnerTurnsTaken, int loserTurnsTaken, string matchStatus)
    {
        this.gameMode = gameMode;
        this.winner = winner;
        this.loser = loser;
        this.winnerDeck = winnerDeck;
        this.loserDeck = loserDeck;
        this.winnerXp = winnerXp;
        this.loserXp = loserXp;
        this.winnerMmrChange = winnerMmrChange;
        this.loserMmrChange = loserMmrChange;
        this.matchLength = matchLength;
        this.winnerTurnsTaken = winnerTurnsTaken;
        this.loserTurnsTaken = loserTurnsTaken;
        this.matchStatus = matchStatus;

    }
}
public class PlayerMatchData
{
    public int xpOpen;
    public int mmrOpen;
    public int totalTimePlayedOpen;
    public int totalTurnsTakenOpen;
    public int margoDeckWinOpen;
    public int margoDeckLossOpen;
    public int miosDeckWinOpen;
    public int miosDeckLossOpen;
    public int nasseDeckWinOpen;
    public int nasseDeckLossOpen;
    public int voidDeckWinOpen;
    public int voidDeckLossOpen;
    public int tootDeckWinOpen;
    public int tootDeckLossOpen;

    public PlayerMatchData(int xpOpen, int mmrOpen, int totalTimePlayedOpen, int totalTurnsTakenOpen, int margoDeckWinOpen, int margoDeckLossOpen,
        int miosDeckWinOpen, int miosDeckLossOpen, int nasseDeckWinOpen, int nasseDeckLossOpen, int voidDeckWinOpen, int voidDeckLossOpen,
        int tootDeckWinOpen, int tootDeckLossOpen)
    {
        this.xpOpen = xpOpen;
        this.mmrOpen = mmrOpen;
        this.totalTimePlayedOpen = totalTimePlayedOpen;
        this.totalTurnsTakenOpen = totalTurnsTakenOpen;
        this.margoDeckWinOpen = margoDeckWinOpen;
        this.margoDeckLossOpen = margoDeckLossOpen;
        this.miosDeckWinOpen = miosDeckWinOpen;
        this.miosDeckLossOpen = miosDeckLossOpen;
        this.nasseDeckWinOpen = nasseDeckWinOpen;
        this.nasseDeckLossOpen = nasseDeckLossOpen;
        this.voidDeckWinOpen = voidDeckWinOpen;
        this.voidDeckLossOpen = voidDeckLossOpen;
        this.tootDeckWinOpen = tootDeckWinOpen;
        this.tootDeckLossOpen = tootDeckLossOpen;
    }

}

