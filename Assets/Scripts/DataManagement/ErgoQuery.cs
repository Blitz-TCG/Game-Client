using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ErgoQuery : MonoBehaviour
{
    public static ErgoQuery instance;
    public CardIdChecks cardIdChecks;

    TokenData data;
    public List<string> tokenIDs = new List<string>();
    public List<long> tokenAmounts = new List<long>();
    public RawImage image;
    public List<long> timer = new List<long>();

    public int[][] cardIdCurrentStore = new int[5][];
    public List<int>[] cardIdCurrentList = new List<int>[5];
    private int currentSize;

    public string[] deckBodyStore = new string[5];
    public int[] deckGeneralStore = new int[5];
    public int[] deckIdStore = new int[5];
    public string[] deckTitleStore = new string[5];
    public string[] filterCardsCurrentStore = new string[5];
    public string[] gameboardCurrentStore = new string[5];
    public string[] generalPfpCurrentStore = new string[5];

    public int[][] cardIdAvailableStore = new int[5][];
    public List<int>[] cardIdAvailableList = new List<int>[5];
    public string[] filterCardsAvailableStore = new string[5];
    public string[] filterGameboardsStore = new string[5];
    public string[] filterGeneralPfpStore = new string[5];
    public int[] hideCardsAvailableStore = new int[5];
    public int[] hideGameboardsStore = new int[5];
    public int[] hideGeneralPfpStore = new int[5];

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }
    public IEnumerator LoadErgoTokens() //public enumerator and then call when logging in
    {
        /*        for (int i = 0; i < 1; i++) //can get rid of this eventually - just to see what average loading times of different urls are
                {*/
        //var watch = new System.Diagnostics.Stopwatch();
        //watch.Start();

        var client = new HttpClient();
        // API URL
        //client.BaseAddress = new Uri("https://api.ergoplatform.com/");
        client.BaseAddress = new Uri("https://ergo-explorer.anetabtc.io/");
        //client.BaseAddress = new Uri("https://ergo-explorer.tosidrop.io/");


        // Header config
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        yield return StartCoroutine(FirebaseWalletAddress((string returnedWalletId) =>
        {
            if (returnedWalletId.Replace("\"", "") == "none" || returnedWalletId.Replace("\"", "") == "")
            {
                Debug.Log("No tokens were loaded");
                Debug.Log(returnedWalletId.Replace("\"", ""));

                StartCoroutine(LoadDeckData());
            }
            else
            {
                HttpResponseMessage response = client.GetAsync("api/v1/addresses/" + returnedWalletId.Replace("\"", "") + "/balance/confirmed").Result;

                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsStringAsync().Result;

                    var fullJSON = JsonConvert.DeserializeObject(dataObjects);
                    Debug.Log(fullJSON);

                    data = JsonUtility.FromJson<TokenData>(dataObjects);

                    tokenIDs = new List<string>();
                    tokenAmounts = new List<long>();

                    foreach (var token in data.tokens)
                    {
                        /*                            Debug.Log(token.decimals);
                                                    Debug.Log(Mathf.Pow(10, token.decimals));*/
                        long tokenDecimals = Convert.ToInt64(Mathf.Pow(10, token.decimals));
                        tokenIDs.Add(token.tokenId);
                        tokenAmounts.Add(token.amount / tokenDecimals);
                    }

                    Debug.Log(returnedWalletId.Replace("\"", ""));

                    StartCoroutine(LoadDeckData());
                }
            }

        }));
    }

    IEnumerator FirebaseWalletAddress(Action<string> onCallback)
    {
        var userWalletId = FirebaseDatabase.DefaultInstance.GetReference("users").Child(FirebaseManager.instance.user.UserId).Child("wallet").GetValueAsync();
        yield return new WaitUntil(predicate: () => userWalletId.IsCompleted); //todo maybe add some exception checking

        DataSnapshot userWalletResult = userWalletId.Result;

        if (userWalletId.Exception != null) //basic error checking, unsure if it actually does anything here of use
        {
            onCallback.Invoke("");
            Debug.Log("failed to load firebase wallet ID");
        }
        else if (userWalletResult.Exists) //check if username is already on a user's friends list
        {
            if (userWalletResult.GetRawJsonValue().ToString() == "{}") //sometimes the jason is returning an empty bracket json when a username does not exist, should be returning nothing
            {
                onCallback.Invoke("");
                Debug.Log("{} issue when loading wallet ID");
            }
            else
            {
                onCallback.Invoke(userWalletResult.GetRawJsonValue().ToString());
            }
        }
        else
        {
            onCallback.Invoke("");
        }
    }

    public IEnumerator LoadDeckData()
    {
        var deckLoad = FirebaseDatabase.DefaultInstance.GetReference("decks").Child(FirebaseManager.instance.user.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => deckLoad.IsCompleted);
        if (deckLoad.IsFaulted)
        {
            Debug.Log("Unable to load deck data");
            FirebaseManager.instance.LoadingAnimationLoginOff();
            FirebaseManager.instance.loginOutputTextError.text = "Unable to load deck data";
            FirebaseManager.instance.loginOutputTextSuccess.text = "";
        }
        else if (deckLoad.IsCompleted)
        {
            Debug.Log("Loaded deck data successfully");

            DataSnapshot dataSnapshot = deckLoad.Result;

            var fullJSON = JsonConvert.DeserializeObject(dataSnapshot.GetRawJsonValue());
            DeckData deckData = JsonUtility.FromJson<DeckData>(fullJSON.ToString());

            CurrentCards[] currentCardsArray = new CurrentCards[5];
            AvailableCards[] availableCardsArray = new AvailableCards[5];

            currentCardsArray[0] = deckData.CurrentCards1;
            currentCardsArray[1] = deckData.CurrentCards2;
            currentCardsArray[2] = deckData.CurrentCards3;
            currentCardsArray[3] = deckData.CurrentCards4;
            currentCardsArray[4] = deckData.CurrentCards5;

            availableCardsArray[0] = deckData.AvailableCards1;
            availableCardsArray[1] = deckData.AvailableCards2;
            availableCardsArray[2] = deckData.AvailableCards3;
            availableCardsArray[3] = deckData.AvailableCards4;
            availableCardsArray[4] = deckData.AvailableCards5;

            for (int count = 0; count <= 4; count++)
            {
                if (currentCardsArray[count].cardIdCurrent != "")
                {
                    DeckDataStorage(currentCardsArray[count], availableCardsArray[count], count);
                }
                Debug.Log(count);
            }
            GameManager.instance.ChangeScene(1);
        }
    }

    public void DeckDataStorage(CurrentCards currentCards, AvailableCards availableCards, int count)
    {
        cardIdCurrentList[count] = currentCards.cardIdCurrent.Split(',').Select(s => int.Parse(s)).ToList();
        currentSize = cardIdCurrentList[count].Count;
        deckBodyStore[count] = currentCards.deckBody;
        deckGeneralStore[count] = currentCards.deckGeneral;
        deckIdStore[count] = currentCards.deckId;
        deckTitleStore[count] = currentCards.deckTitle;
        filterCardsCurrentStore[count] = currentCards.filterCardsCurrent;
        gameboardCurrentStore[count] = currentCards.gameboardCurrent;
        generalPfpCurrentStore[count] = currentCards.generalPfpCurrent;

        cardIdAvailableList[count] = availableCards.cardIdAvailable.Split(',').Select(s => int.Parse(s)).ToList();
        filterCardsAvailableStore[count] = availableCards.filterCardsAvailable;
        filterGameboardsStore[count] = availableCards.filterGameboards;
        filterGeneralPfpStore[count] = availableCards.filterGeneralPfp;
        hideCardsAvailableStore[count] = availableCards.hideCardsAvailable;
        hideGameboardsStore[count] = availableCards.hideGameboards;
        hideGeneralPfpStore[count] = availableCards.hideGeneralPfp;

        int n = 0;
        for (int i = 0; i < currentSize; i++)
        {
            if (cardIdChecks.cardToErgoTokenId.ContainsKey(cardIdCurrentList[count][i - n]))
            {
                if (!tokenIDs.Contains(cardIdChecks.cardToErgoTokenId[cardIdCurrentList[count][i - n]]))
                {
                    Debug.Log(cardIdChecks.cardToErgoTokenId[cardIdCurrentList[count][i - n]]);
                    cardIdAvailableList[count].Add(cardIdCurrentList[count][i - n]);
                    cardIdCurrentList[count].Remove(cardIdCurrentList[count][i - n]);

                    n++;
                }
            }
        }

        cardIdCurrentStore[count] = cardIdCurrentList[count].ToArray();
        cardIdAvailableStore[count] = cardIdAvailableList[count].ToArray();
    }

    [System.Serializable]
    public class DeckData
    {
        public AvailableCards AvailableCards1;
        public AvailableCards AvailableCards2;
        public AvailableCards AvailableCards3;
        public AvailableCards AvailableCards4;
        public AvailableCards AvailableCards5;

        public CurrentCards CurrentCards1;
        public CurrentCards CurrentCards2;
        public CurrentCards CurrentCards3;
        public CurrentCards CurrentCards4;
        public CurrentCards CurrentCards5;
    }

    [System.Serializable]
    public class CurrentCards
    {
        public string cardIdCurrent;
        public string deckBody;
        public int deckGeneral;
        public int deckId;
        public string deckTitle;
        public string filterCardsCurrent;
        public string gameboardCurrent;
        public string generalPfpCurrent;
    }

    [System.Serializable]
    public class AvailableCards
    {
        public string cardIdAvailable;
        public string filterCardsAvailable;
        public string filterGameboards;
        public string filterGeneralPfp;
        public int hideCardsAvailable;
        public int hideGameboards;
        public int hideGeneralPfp;
    }

    [Serializable]
    public class TokenData
    {
        public List<Tokens> tokens;
    }

    [Serializable]
    public class Tokens
    {
        public string tokenId;
        public long amount;
        public long decimals;
        public string name;
        public string tokenType;

        public Tokens(string tokenId, long amount, long decimals, string name, string tokenType)
        {
            this.tokenId = tokenId;
            this.amount = amount;
            this.decimals = decimals;
            this.name = name;
            this.tokenType = tokenType;
        }
    }
}
