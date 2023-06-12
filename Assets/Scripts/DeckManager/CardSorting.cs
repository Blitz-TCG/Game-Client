using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSorting : MonoBehaviour
{
    public DeckManager deckManager;

    [Header("Current Cards")]
    public GameObject[] currentCardsSortArray;
    public int[] currentCardsSortArrayLevel;
    public GameObject[] currentCardsSortArrayLevelObject;
    public GameObject currentCardsSort;
    public Transform currentCardsSortTransform;
    public GameObject currentCardsUI;
    public GameObject currentCardsSortPopup;
    public GameObject currentABCselected;
    public GameObject currentCBAselected;
    public GameObject current123selected;
    public GameObject current321selected;

    [Header("Available Cards")]
    public Image[] cardImages;
    public GameObject[] availableCardsSortArrayAll;
    public GameObject[] availableCardsSortArrayMatched;
    public GameObject[] availableCardsSortArrayUnmatched;
    public GameObject[] availableCardsHideArrayUnmatched;
    public GameObject[] availableMatchedSortObject;
    public GameObject[] availableUnmatchedSortObject;
    public GameObject availableCardsSort;
    public Transform availableCardsSortTransform;
    public GameObject availableCardsSortPopup;
    public GameObject availableABCselected;
    public GameObject availableCBAselected;
    public GameObject available123selected;
    public GameObject available321selected;

    [Header("Gameboards")]
    public Image[] gameboardImages;
    public TMP_Text[] gameboardText;
    public GameObject[] gameboardSortArrayAll;
    public GameObject[] gameboardSortArrayMatched;
    public GameObject[] gameboardSortArrayUnmatched;
    public GameObject[] gameboardHideArrayUnmatched;
    public GameObject[] gameboardMatchedSortObject;
    public GameObject[] gameboardUnmatchedSortObject;
    public GameObject gameboardSort;
    public Transform gameboardSortTransform;
    public GameObject gameboardSortPopup;
    public bool gameboardsNameBool;
    public GameObject gameboardABCselected;
    public GameObject gameboardCBAselected;
    public GameObject[] gameboardsHideArrayUnmatched;
    public GameObject hideUnmatchedButtonGameboard;

    [Header("Misc")]
    public bool currentNameBool = true;
    public bool currentNameLevelBool = true;
    public bool availableNameBool = true;
    public bool availableNameLevelBool = true;
    public GameObject hideUnmatchedButton;

    private Color enableColor = new Vector4(1f, 1f, 1f, 1f);

    public void SortPopupCurrent()
    {
        if (currentCardsSortPopup.activeSelf)
        {
            currentCardsSortPopup.SetActive(false);
        }
        else if (!currentCardsSortPopup.activeSelf)
        {
            currentCardsSortPopup.SetActive(true);
        }
    }
    public void SortPopupAvailable()
    {
        if (!deckManager.gameboardUI.activeSelf)
        {
            if (availableCardsSortPopup.activeSelf)
            {
                availableCardsSortPopup.SetActive(false);
            }
            else if (!availableCardsSortPopup.activeSelf)
            {
                availableCardsSortPopup.SetActive(true);
            }
        }
        else if (deckManager.gameboardUI.activeSelf)
        {
            if (gameboardSortPopup.activeSelf)
            {
                gameboardSortPopup.SetActive(false);
            }
            else if (!gameboardSortPopup.activeSelf)
            {
                gameboardSortPopup.SetActive(true);
            }
        }
    }
    public void SortChildrenByNameCurrent()
    {

        currentCardsSortTransform = currentCardsSort.transform;
        currentCardsSortArray = new GameObject[currentCardsSortTransform.childCount];

        int j = currentCardsSortTransform.childCount;
        Debug.Log(j);

        for (int i = 0; j > i; i++) //saving cards into an array
        {
            currentCardsSortArray[i] = currentCardsSortTransform.GetChild(i).gameObject;
        }

        if (currentNameBool) //abc
        {
            currentCardsSortArray = currentCardsSortArray.OrderBy(go => go.name).ToArray();
            for (int i = 0; j > i; i++) //sorts cards by name
            {
                currentCardsSortTransform.transform.Find(currentCardsSortArray[i].name).SetSiblingIndex(i);
            }

            deckManager.filterCurrent = "currentABC";
            CurrrentFilterSelection(deckManager.filterCurrent);
        }
        else if (!currentNameBool) //cba
        {
            currentCardsSortArray = currentCardsSortArray.OrderBy(go => go.name).ToArray();
            currentCardsSortArray = currentCardsSortArray.Reverse().ToArray();
            for (int i = 0; j > i; i++) //sorts cards by name
            {
                currentCardsSortTransform.transform.Find(currentCardsSortArray[i].name).SetSiblingIndex(i);
            }

            deckManager.filterCurrent = "currentCBA";
            CurrrentFilterSelection(deckManager.filterCurrent);
        }

    }

    public void SortChildrenByLevelCurrent()
    {
        IDictionary<GameObject, int> cardsAndLevels = new Dictionary<GameObject, int>();
        currentCardsSortTransform = currentCardsSort.transform;
        currentCardsSortArray = new GameObject[currentCardsSortTransform.childCount];

        int j = currentCardsSortTransform.childCount;

        for (int i = 0; j > i; i++) //saving cards into an array
        {
            currentCardsSortArray[i] = currentCardsSortTransform.GetChild(i).gameObject;
        }

        currentCardsSortArray = currentCardsSortArray.OrderBy(go => go.name).ToArray(); //orders cards in the array by name

        for (int i = 0; j > i; i++) //saving cards into a dictionary with levels
        {
            cardsAndLevels.Add(currentCardsSortArray[i], int.Parse(currentCardsSortTransform.Find(currentCardsSortArray[i].name).gameObject.transform.Find("Level").GetComponent<TMP_Text>().text.Replace("Level ", "")));
        }

        if (currentNameLevelBool) //123
        {
            var sortedDict = from entry in cardsAndLevels orderby entry.Value ascending select entry;
            var backToDictionary = sortedDict.ToDictionary(r => r.Key, r => r.Value);
            currentCardsSortArrayLevelObject = backToDictionary.Keys.ToArray();

            deckManager.filterCurrent = "current123";
            CurrrentFilterSelection(deckManager.filterCurrent);
        }
        else if (!currentNameLevelBool) //321
        {
            var sortedDict = from entry in cardsAndLevels orderby entry.Value descending select entry;
            var backToDictionary = sortedDict.ToDictionary(r => r.Key, r => r.Value);
            currentCardsSortArrayLevelObject = backToDictionary.Keys.ToArray();

            deckManager.filterCurrent = "current321"; //todo
            CurrrentFilterSelection(deckManager.filterCurrent);
        }

        for (int i = 0; j > i; i++) //sorts card by level
        {
            currentCardsSortTransform.transform.Find(currentCardsSortArrayLevelObject[i].name).SetSiblingIndex(i);
        }

    }

    public void SortCurrentClear()
    {
        currentABCselected.SetActive(false);
        currentCBAselected.SetActive(false);
        current123selected.SetActive(false);
        current321selected.SetActive(false);
        currentCardsSortPopup.SetActive(false);
        deckManager.filterCurrent = "";
    }
    public void SortAvailableClear()
    {
        availableABCselected.SetActive(false);
        availableCBAselected.SetActive(false);
        available123selected.SetActive(false);
        available321selected.SetActive(false);
        availableCardsSortPopup.SetActive(false);
        deckManager.filterAvailable = "";
    }

    public void SortChildrenByNameAvailable()
    {
        Debug.Log("cards sorted");
        availableCardsSortTransform = availableCardsSort.transform;
        int j = availableCardsSortTransform.childCount;
        availableCardsSortArrayAll = new GameObject[j];
        cardImages = new Image[j];

        int k = 0; //unmatched array counter
        int l = 0; //matched array counter
        for (int i = 0; j > i; i++) //saving cards into an array
        {
            availableCardsSortArrayAll[i] = availableCardsSortTransform.GetChild(i).gameObject;
            cardImages[i] = availableCardsSortArrayAll[i].transform.Find("Frame").GetComponentInChildren<Image>();

            if (cardImages[i].color.ToString().Contains("0.502"))
            {
                Array.Resize(ref availableCardsSortArrayUnmatched, availableCardsSortArrayUnmatched.Length + 1);
                availableCardsSortArrayUnmatched[k] = availableCardsSortArrayAll[i];
                k++;

            }
            else if (cardImages[i].color.ToString() != null)
            {
                Array.Resize(ref availableCardsSortArrayMatched, availableCardsSortArrayMatched.Length + 1);
                availableCardsSortArrayMatched[l] = availableCardsSortArrayAll[i];
                l++;
            }

        }

        if (availableNameBool) //abc
        {
            availableCardsSortArrayMatched = availableCardsSortArrayMatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
            availableCardsSortArrayUnmatched = availableCardsSortArrayUnmatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
            int m = 0;
            for (int i = 0; availableCardsSortArrayMatched.Length > i; i++) //sorts cards by name
            {
                availableCardsSortTransform.transform.Find(availableCardsSortArrayMatched[i].name).SetSiblingIndex(i);
                m++;
            }
            for (int i = 0; availableCardsSortArrayUnmatched.Length > i; i++) //sorts cards by name
            {
                availableCardsSortTransform.transform.Find(availableCardsSortArrayUnmatched[i].name).SetSiblingIndex(m);
                m++;
            }

            deckManager.filterAvailable = "availableABC";
            AvailableFilterSelection(deckManager.filterAvailable);
        }
        else if (!availableNameBool) //cba
        {
            availableCardsSortArrayMatched = availableCardsSortArrayMatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
            availableCardsSortArrayUnmatched = availableCardsSortArrayUnmatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
            availableCardsSortArrayMatched = availableCardsSortArrayMatched.Reverse().ToArray();
            availableCardsSortArrayUnmatched = availableCardsSortArrayUnmatched.Reverse().ToArray();
            int m = 0;
            for (int i = 0; availableCardsSortArrayMatched.Length > i; i++) //sorts cards by name
            {
                availableCardsSortTransform.transform.Find(availableCardsSortArrayMatched[i].name).SetSiblingIndex(i);
                m++;
            }
            for (int i = 0; availableCardsSortArrayUnmatched.Length > i; i++) //sorts cards by name
            {
                availableCardsSortTransform.transform.Find(availableCardsSortArrayUnmatched[i].name).SetSiblingIndex(m);
                m++;
            }

            deckManager.filterAvailable = "availableCBA";
            AvailableFilterSelection(deckManager.filterAvailable);
        }
        Array.Resize(ref availableCardsSortArrayUnmatched, 0);
        Array.Resize(ref availableCardsSortArrayMatched, 0);

        deckManager.LiveSearch();
        HideUnmatchedCardsInvoke();
    }

    public void SortChildrenByLevelAvailable()
    {
        IDictionary<GameObject, int> cardsAndLevelsMatched = new Dictionary<GameObject, int>();
        IDictionary<GameObject, int> cardsAndLevelsUnmatched = new Dictionary<GameObject, int>();
        availableCardsSortTransform = availableCardsSort.transform;
        int j = availableCardsSortTransform.childCount;
        availableCardsSortArrayAll = new GameObject[j];
        cardImages = new Image[j];

        int k = 0; //unmatched array counter
        int l = 0; //matched array counter
        for (int i = 0; j > i; i++) //saving cards into an array
        {
            availableCardsSortArrayAll[i] = availableCardsSortTransform.GetChild(i).gameObject;
            cardImages[i] = availableCardsSortArrayAll[i].transform.Find("Frame").GetComponentInChildren<Image>();

            if (cardImages[i].color.ToString().Contains("0.502"))
            {
                Array.Resize(ref availableCardsSortArrayUnmatched, availableCardsSortArrayUnmatched.Length + 1);
                availableCardsSortArrayUnmatched[k] = availableCardsSortArrayAll[i];
                k++;

            }
            else if (cardImages[i].color.ToString() != null)
            {
                Array.Resize(ref availableCardsSortArrayMatched, availableCardsSortArrayMatched.Length + 1);
                availableCardsSortArrayMatched[l] = availableCardsSortArrayAll[i];
                l++;
            }

        }

        availableCardsSortArrayMatched = availableCardsSortArrayMatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
        availableCardsSortArrayUnmatched = availableCardsSortArrayUnmatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name


        for (int i = 0; availableCardsSortArrayMatched.Length > i; i++) //saving cards into a dictionary with levels
        {
            cardsAndLevelsMatched.Add(availableCardsSortArrayMatched[i], int.Parse(availableCardsSortTransform.Find(availableCardsSortArrayMatched[i].name).gameObject.transform.Find("Level").GetComponent<TMP_Text>().text.Replace("Level ", "")));
        }
        for (int i = 0; availableCardsSortArrayUnmatched.Length > i; i++) //saving cards into a dictionary with levels
        {
            cardsAndLevelsUnmatched.Add(availableCardsSortArrayUnmatched[i], int.Parse(availableCardsSortTransform.Find(availableCardsSortArrayUnmatched[i].name).gameObject.transform.Find("Level").GetComponent<TMP_Text>().text.Replace("Level ", "")));
        }

        if (availableNameLevelBool) //123
        {
            var sortedDictMatched = from entry in cardsAndLevelsMatched orderby entry.Value ascending select entry;
            var backToDictionaryMatched = sortedDictMatched.ToDictionary(r => r.Key, r => r.Value);
            availableMatchedSortObject = backToDictionaryMatched.Keys.ToArray();

            var sortedDictUnmatched = from entry in cardsAndLevelsUnmatched orderby entry.Value ascending select entry;
            var backToDictionaryUnmatched = sortedDictUnmatched.ToDictionary(r => r.Key, r => r.Value);
            availableUnmatchedSortObject = backToDictionaryUnmatched.Keys.ToArray();

            deckManager.filterAvailable = "available123";
            AvailableFilterSelection(deckManager.filterAvailable);
        }
        else if (!availableNameLevelBool) //321
        {
            var sortedDictMatched = from entry in cardsAndLevelsMatched orderby entry.Value descending select entry;
            var backToDictionaryMatched = sortedDictMatched.ToDictionary(r => r.Key, r => r.Value);
            availableMatchedSortObject = backToDictionaryMatched.Keys.ToArray();

            var sortedDictUnmatched = from entry in cardsAndLevelsUnmatched orderby entry.Value descending select entry;
            var backToDictionaryUnmatched = sortedDictUnmatched.ToDictionary(r => r.Key, r => r.Value);
            availableUnmatchedSortObject = backToDictionaryUnmatched.Keys.ToArray();

            deckManager.filterAvailable = "available321";
            AvailableFilterSelection(deckManager.filterAvailable);
        }

        int m = 0;
        for (int i = 0; availableMatchedSortObject.Length > i; i++) //sorts card by level
        {
            availableCardsSortTransform.transform.Find(availableMatchedSortObject[i].name).SetSiblingIndex(i);
            m++;
        }
        for (int i = 0; availableUnmatchedSortObject.Length > i; i++) //sorts card by level
        {
            availableCardsSortTransform.transform.Find(availableUnmatchedSortObject[i].name).SetSiblingIndex(m);
            m++;
        }

        Array.Resize(ref availableCardsSortArrayUnmatched, 0);
        Array.Resize(ref availableCardsSortArrayMatched, 0);

        deckManager.LiveSearch();
        HideUnmatchedCardsInvoke();
    }
    public void SortChildrenAvailableDefault()
    {
        Debug.Log("cards sorted");
        availableCardsSortTransform = availableCardsSort.transform;
        int j = availableCardsSortTransform.childCount;
        availableCardsSortArrayAll = new GameObject[j];
        cardImages = new Image[j];

        int k = 0; //unmatched array counter
        int l = 0; //matched array counter
        for (int i = 0; j > i; i++) //saving cards into an array
        {
            availableCardsSortArrayAll[i] = availableCardsSortTransform.GetChild(i).gameObject;
            cardImages[i] = availableCardsSortArrayAll[i].transform.Find("Frame").GetComponentInChildren<Image>();

            if (cardImages[i].color.ToString().Contains("0.502"))
            {
                Array.Resize(ref availableCardsSortArrayUnmatched, availableCardsSortArrayUnmatched.Length + 1);
                availableCardsSortArrayUnmatched[k] = availableCardsSortArrayAll[i];
                k++;

            }
            else if (cardImages[i].color.ToString() != null)
            {
                Array.Resize(ref availableCardsSortArrayMatched, availableCardsSortArrayMatched.Length + 1);
                availableCardsSortArrayMatched[l] = availableCardsSortArrayAll[i];
                l++;
            }

        }

        availableCardsSortArrayMatched = availableCardsSortArrayMatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
        availableCardsSortArrayUnmatched = availableCardsSortArrayUnmatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
        int m = 0;
        for (int i = 0; availableCardsSortArrayMatched.Length > i; i++) //sorts cards by name
        {
            availableCardsSortTransform.transform.Find(availableCardsSortArrayMatched[i].name).SetSiblingIndex(i);
            m++;
        }
        for (int i = 0; availableCardsSortArrayUnmatched.Length > i; i++) //sorts cards by name
        {
            availableCardsSortTransform.transform.Find(availableCardsSortArrayUnmatched[i].name).SetSiblingIndex(m);
            m++;
        }

        Array.Resize(ref availableCardsSortArrayUnmatched, 0);
        Array.Resize(ref availableCardsSortArrayMatched, 0);

        deckManager.LiveSearch();
        HideUnmatchedCardsInvoke();
    }


    public void HideUnmatchedCardsButton() //todo: do something with playerprefs and a public void start after this is working...
    {
        if (!hideUnmatchedButton.activeSelf) //todo: make it so that when things are reinstantiated, if this is on, set object active to false
        {
            hideUnmatchedButton.SetActive(true);
            deckManager.hideAvailableCards = 1;

            availableCardsSortTransform = availableCardsSort.transform;
            int j = availableCardsSortTransform.childCount;
            availableCardsSortArrayAll = new GameObject[j];
            cardImages = new Image[j];

            int k = 0; //unmatched array counter
            for (int i = 0; j > i; i++) //saving cards into an array
            {
                availableCardsSortArrayAll[i] = availableCardsSortTransform.GetChild(i).gameObject;
                cardImages[i] = availableCardsSortArrayAll[i].transform.Find("Frame").GetComponentInChildren<Image>();

                if (cardImages[i].color.ToString().Contains("0.502"))
                {
                    Array.Resize(ref availableCardsHideArrayUnmatched, availableCardsHideArrayUnmatched.Length + 1);
                    availableCardsHideArrayUnmatched[k] = availableCardsSortArrayAll[i];
                    availableCardsHideArrayUnmatched[k].SetActive(false);
                    k++;

                }
            }
        }
        else
        {
            hideUnmatchedButton.SetActive(false);
            deckManager.hideAvailableCards = 0;

            availableCardsSortTransform = availableCardsSort.transform;
            int j = availableCardsSortTransform.childCount;
            availableCardsSortArrayAll = new GameObject[j];
            cardImages = new Image[j];

            int k = 0; //unmatched array counter
            for (int i = 0; j > i; i++) //saving cards into an array
            {
                availableCardsSortArrayAll[i] = availableCardsSortTransform.GetChild(i).gameObject;
                cardImages[i] = availableCardsSortArrayAll[i].transform.Find("Frame").GetComponentInChildren<Image>();

                if (cardImages[i].color.ToString().Contains("0.502"))
                {
                    Array.Resize(ref availableCardsHideArrayUnmatched, availableCardsHideArrayUnmatched.Length + 1);
                    availableCardsHideArrayUnmatched[k] = availableCardsSortArrayAll[i];
                    availableCardsHideArrayUnmatched[k].SetActive(true);
                    k++;

                }
            }
        }

        deckManager.LiveSearch();
    }
    public void HideUnmatchedGameboardsButton()
    {
        if (!hideUnmatchedButtonGameboard.activeSelf)
        {
            hideUnmatchedButtonGameboard.SetActive(true);
            deckManager.hideAvailableGameboards = 1; 

            gameboardSortTransform = gameboardSort.transform;
            int j = gameboardSortTransform.childCount;
            gameboardSortArrayAll = new GameObject[j];
            gameboardImages = new Image[j];

            int k = 0; //unmatched array counter
            for (int i = 0; j > i; i++) //saving cards into an array
            {
                gameboardSortArrayAll[i] = gameboardSortTransform.GetChild(i).gameObject;
                gameboardImages[i] = gameboardSortArrayAll[i].transform.GetComponentInChildren<Image>();

                Debug.Log(gameboardImages[i].color.ToString());

                if (gameboardImages[i].color.ToString().Contains("0.502"))
                {
                    Array.Resize(ref gameboardsHideArrayUnmatched, gameboardsHideArrayUnmatched.Length + 1);
                    gameboardsHideArrayUnmatched[k] = gameboardSortArrayAll[i];
                    Debug.Log(gameboardsHideArrayUnmatched[k]);
                    gameboardsHideArrayUnmatched[k].SetActive(false);
                    k++;

                }
            }
        }
        else
        {
            hideUnmatchedButtonGameboard.SetActive(false);
            deckManager.hideAvailableGameboards = 0;

            gameboardSortTransform = gameboardSort.transform;
            int j = gameboardSortTransform.childCount;
            gameboardSortArrayAll = new GameObject[j];
            gameboardImages = new Image[j];

            int k = 0; //unmatched array counter
            for (int i = 0; j > i; i++) //saving cards into an array
            {
                gameboardSortArrayAll[i] = gameboardSortTransform.GetChild(i).gameObject;
                gameboardImages[i] = gameboardSortArrayAll[i].transform.GetComponentInChildren<Image>();

                if (gameboardImages[i].color.ToString().Contains("0.502"))
                {
                    Array.Resize(ref gameboardsHideArrayUnmatched, gameboardsHideArrayUnmatched.Length + 1);
                    gameboardsHideArrayUnmatched[k] = gameboardSortArrayAll[i];
                    gameboardsHideArrayUnmatched[k].SetActive(true);
                    k++;

                }
            }
        }

        deckManager.LiveSearchGameboards();
    }
    public void HideUnmatchedCardsInvoke()
    {
        if (hideUnmatchedButton.activeSelf)
        {
            deckManager.hideAvailableCards = 1;
            availableCardsSortTransform = availableCardsSort.transform;
            int j = availableCardsSortTransform.childCount;
            availableCardsSortArrayAll = new GameObject[j];
            cardImages = new Image[j];

            int k = 0; //unmatched array counter
            for (int i = 0; j > i; i++) //saving cards into an array
            {
                availableCardsSortArrayAll[i] = availableCardsSortTransform.GetChild(i).gameObject;
                cardImages[i] = availableCardsSortArrayAll[i].transform.Find("Frame").GetComponentInChildren<Image>();

                if (cardImages[i].color.ToString().Contains("0.502"))
                {
                    Array.Resize(ref availableCardsHideArrayUnmatched, availableCardsHideArrayUnmatched.Length + 1);
                    availableCardsHideArrayUnmatched[k] = availableCardsSortArrayAll[i];
                    availableCardsHideArrayUnmatched[k].SetActive(false);
                    k++;

                }
            }
        }
    }
    public void HideUnmatchedGameboardsInvoke()
    {
        if (hideUnmatchedButtonGameboard.activeSelf)
        {
            deckManager.hideAvailableGameboards = 1;
            gameboardSortTransform = gameboardSort.transform;
            int j = gameboardSortTransform.childCount;
            gameboardSortArrayAll = new GameObject[j];
            gameboardImages = new Image[j];

            int k = 0; //unmatched array counter
            for (int i = 0; j > i; i++) //saving cards into an array
            {
                gameboardSortArrayAll[i] = gameboardSortTransform.GetChild(i).gameObject;
                gameboardImages[i] = gameboardSortArrayAll[i].transform.GetComponentInChildren<Image>();

                if (gameboardImages[i].color.ToString().Contains("0.502"))
                {
                    Array.Resize(ref gameboardsHideArrayUnmatched, gameboardsHideArrayUnmatched.Length + 1);
                    gameboardsHideArrayUnmatched[k] = gameboardSortArrayAll[i];
                    gameboardsHideArrayUnmatched[k].SetActive(false);
                    k++;

                }
            }
        }
    }
    public void HideUnmatchedCardsSaveDeck()
    {
        if (hideUnmatchedButton.activeSelf)
        {
            availableCardsSortTransform = availableCardsSort.transform;
            int j = availableCardsSortTransform.childCount;
            availableCardsSortArrayAll = new GameObject[j];
            cardImages = new Image[j];

            for (int i = 0; j > i; i++) //saving cards into an array
            {
                availableCardsSortArrayAll[i] = availableCardsSortTransform.GetChild(i).gameObject;
                availableCardsSortArrayAll[i].SetActive(true);
                Debug.Log(availableCardsSortTransform.GetChild(i).gameObject);
            }
        }
    }
    public void CurrentABCbutton()
    {
        currentNameBool = true;
        SortChildrenByNameCurrent();
        currentCardsSortPopup.SetActive(false);
    }
    public void CurrentZYXbutton()
    {
        currentNameBool = false;
        SortChildrenByNameCurrent();
        currentCardsSortPopup.SetActive(false);
    }
    public void Current123button()
    {
        currentNameLevelBool = true;
        SortChildrenByLevelCurrent();
        currentCardsSortPopup.SetActive(false);
    }
    public void Current321button()
    {
        currentNameLevelBool = false;
        SortChildrenByLevelCurrent();
        currentCardsSortPopup.SetActive(false);
    }

    public void AvailableABCbutton()
    {
        availableNameBool = true;
        SortChildrenByNameAvailable();
        availableCardsSortPopup.SetActive(false);
    }
    public void AvailableZYXbutton()
    {
        availableNameBool = false;
        SortChildrenByNameAvailable();
        availableCardsSortPopup.SetActive(false);
    }
    public void Available123button()
    {
        availableNameLevelBool = true;
        SortChildrenByLevelAvailable();
        availableCardsSortPopup.SetActive(false);
    }
    public void Available321button()
    {
        availableNameLevelBool = false;
        SortChildrenByLevelAvailable();
        availableCardsSortPopup.SetActive(false);
    }


    public void CurrrentFilterSelection(string filter)
    {
        if (filter == "currentABC")
        {
            currentABCselected.SetActive(true);
            currentCBAselected.SetActive(false);
            current123selected.SetActive(false);
            current321selected.SetActive(false);
        }
        else if (filter == "currentCBA")
        {
            currentABCselected.SetActive(false);
            currentCBAselected.SetActive(true);
            current123selected.SetActive(false);
            current321selected.SetActive(false);
        }
        else if (filter == "current123")
        {
            currentABCselected.SetActive(false);
            currentCBAselected.SetActive(false);
            current123selected.SetActive(true);
            current321selected.SetActive(false);
        }
        else if (filter == "current321")
        {
            currentABCselected.SetActive(false);
            currentCBAselected.SetActive(false);
            current123selected.SetActive(false);
            current321selected.SetActive(true);
        }
    }

    public void AvailableFilterSelection(string filter)
    {
        if (filter == "availableABC")
        {
            availableABCselected.SetActive(true);
            availableCBAselected.SetActive(false);
            available123selected.SetActive(false);
            available321selected.SetActive(false);
        }
        else if (filter == "availableCBA")
        {
            availableABCselected.SetActive(false);
            availableCBAselected.SetActive(true);
            available123selected.SetActive(false);
            available321selected.SetActive(false);
        }
        else if (filter == "available123")
        {
            availableABCselected.SetActive(false);
            availableCBAselected.SetActive(false);
            available123selected.SetActive(true);
            available321selected.SetActive(false);
        }
        else if (filter == "available321")
        {
            availableABCselected.SetActive(false);
            availableCBAselected.SetActive(false);
            available123selected.SetActive(false);
            available321selected.SetActive(true);
        }
    }

    //---------------- Gameboard Sorting ------------------
    public void SortChildrenByNameGameboards()
    {
        gameboardSortTransform = gameboardSort.transform;
        int j = gameboardSortTransform.childCount;
        gameboardSortArrayAll = new GameObject[j];
        gameboardImages = new Image[j];

        int k = 0; //unmatched array counter
        int l = 0; //matched array counter
        for (int i = 0; j > i; i++) //saving gameboards into an array
        {
            gameboardSortArrayAll[i] = gameboardSortTransform.GetChild(i).gameObject;
            gameboardImages[i] = gameboardSortArrayAll[i].transform.GetComponentInChildren<Image>();

            if (gameboardImages[i].color.ToString().Contains("0.502"))
            {
                Array.Resize(ref gameboardSortArrayUnmatched, gameboardSortArrayUnmatched.Length + 1);
                gameboardSortArrayUnmatched[k] = gameboardSortArrayAll[i];
                Debug.Log(gameboardSortArrayUnmatched[k]);
                k++;

            }
            else if (gameboardImages[i].color.ToString() != null)
            {
                Array.Resize(ref gameboardSortArrayMatched, gameboardSortArrayMatched.Length + 1);
                gameboardSortArrayMatched[l] = gameboardSortArrayAll[i];
                l++;
            }

        }

        if (gameboardsNameBool) //abc
        {
            gameboardSortArrayMatched = gameboardSortArrayMatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
            gameboardSortArrayUnmatched = gameboardSortArrayUnmatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
            int m = 0;
            for (int i = 0; gameboardSortArrayMatched.Length > i; i++) //sorts cards by name
            {
                gameboardSortTransform.transform.Find(gameboardSortArrayMatched[i].name).SetSiblingIndex(i);
                m++;
            }
            for (int i = 0; gameboardSortArrayUnmatched.Length > i; i++) //sorts cards by name
            {
                gameboardSortTransform.transform.Find(gameboardSortArrayUnmatched[i].name).SetSiblingIndex(m);
                m++;
            }

            if (deckManager.filterGameboard != "gameboardsABC")
            {
                deckManager.filterGameboard = "gameboardsABC";
                GameboardFilterSelection(deckManager.filterGameboard);
            }
        }
        else if (!gameboardsNameBool) //cba
        {

            gameboardSortArrayMatched = gameboardSortArrayMatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
            gameboardSortArrayUnmatched = gameboardSortArrayUnmatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
            gameboardSortArrayMatched = gameboardSortArrayMatched.Reverse().ToArray();
            gameboardSortArrayUnmatched = gameboardSortArrayUnmatched.Reverse().ToArray();

            int m = 0;
            for (int i = 0; gameboardSortArrayMatched.Length > i; i++) //sorts cards by name
            {
                gameboardSortTransform.transform.Find(gameboardSortArrayMatched[i].name).SetSiblingIndex(i);
                m++;
            }
            for (int i = 0; gameboardSortArrayUnmatched.Length > i; i++) //sorts cards by name
            {
                gameboardSortTransform.transform.Find(gameboardSortArrayUnmatched[i].name).SetSiblingIndex(m);
                m++;
            }

            if (deckManager.filterGameboard != "gameboardsCBA")
            {
                deckManager.filterGameboard = "gameboardsCBA";
                GameboardFilterSelection(deckManager.filterGameboard);
            }
        }

        Array.Resize(ref gameboardSortArrayUnmatched, 0);
        Array.Resize(ref gameboardSortArrayMatched, 0);

        deckManager.LiveSearch();
        HideUnmatchedCardsInvoke(); //this also covers gameboards
    }

    public void SortChildrenByNameGameboardsDefault() //for default sorting, might be useful to do this for cards...
    {
        gameboardSortTransform = gameboardSort.transform;
        int j = gameboardSortTransform.childCount;
        gameboardSortArrayAll = new GameObject[j];
        gameboardImages = new Image[j];
        gameboardText = new TMP_Text[j];

        int k = 0; //unmatched array counter
        int l = 0; //matched array counter
        for (int i = 0; j > i; i++) //saving gameboards into an array
        {
            gameboardSortArrayAll[i] = gameboardSortTransform.GetChild(i).gameObject;
            gameboardImages[i] = gameboardSortArrayAll[i].transform.GetComponentInChildren<Image>();
            gameboardText[i] = gameboardSortArrayAll[i].transform.GetComponentInChildren<TMP_Text>();
            Debug.Log(gameboardText[i].text);

            if (gameboardImages[i].color.ToString().Contains("0.502"))
            {
                Array.Resize(ref gameboardSortArrayUnmatched, gameboardSortArrayUnmatched.Length + 1);
                gameboardSortArrayUnmatched[k] = gameboardSortArrayAll[i];
                k++;

            }
            else if (gameboardImages[i].color.ToString() != null)
            {
                Array.Resize(ref gameboardSortArrayMatched, gameboardSortArrayMatched.Length + 1);
                gameboardSortArrayMatched[l] = gameboardSortArrayAll[i];
                l++;
            }

        }
        gameboardSortArrayMatched = gameboardSortArrayMatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
        gameboardSortArrayUnmatched = gameboardSortArrayUnmatched.OrderBy(go => go.name).ToArray(); //orders cards in the array by name
        int m = 0;
        for (int i = 0; gameboardSortArrayMatched.Length > i; i++) //sorts cards by name
        {
            gameboardSortTransform.transform.Find(gameboardSortArrayMatched[i].name).SetSiblingIndex(i);
            m++;
        }
        for (int i = 0; gameboardSortArrayUnmatched.Length > i; i++) //sorts cards by name
        {
            gameboardSortTransform.transform.Find(gameboardSortArrayUnmatched[i].name).SetSiblingIndex(m);
            m++;
        }

        Array.Resize(ref gameboardSortArrayUnmatched, 0);
        Array.Resize(ref gameboardSortArrayMatched, 0);
    }

    public void GameboardsABCbutton()
    {
        gameboardsNameBool = true;
        SortChildrenByNameGameboards();
        gameboardSortPopup.SetActive(false);
    }
    public void GameboardsZYXbutton()
    {
        gameboardsNameBool = false;
        SortChildrenByNameGameboards();
        gameboardSortPopup.SetActive(false);
    }
    public void SortGameboardsClear()
    {
        gameboardABCselected.SetActive(false);
        gameboardCBAselected.SetActive(false);
        gameboardSortPopup.SetActive(false);
        deckManager.filterGameboard = "";
    }

    public void GameboardFilterSelection(string filter)
    {
        if (filter == "gameboardsABC")
        {
            gameboardABCselected.SetActive(true);
            gameboardCBAselected.SetActive(false);
        }
        else if (filter == "gameboardsCBA")
        {
            gameboardABCselected.SetActive(false);
            gameboardCBAselected.SetActive(true);
        }
    }

}