using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{

    public static DeckManager instance;
    public CursorManager cursorManager;
    public bool IsSelected { get; private set; } = false;
    public GameObject m_MyGameObject;
    public GameObject notesGameObject;
    public CardSorting cardSorting;
    public GameObject availableCardsSortSearch;
    public Transform availableCardsSortTransformSearch;
    public GameObject[] availableCardsSortArrayAllSearch;
    public GameObject[] availableCardsHideArrayUnmatchedSearch;
    public int hideAvailableCards;
    public int hideAvailableGameboards;
    public string filterCurrent;
    public string filterAvailable;
    public string filterGameboard;

    private float firstLeftClickTime;
    private float timeBetweenLeftClick = 0.25f;
    private bool isTimeCheckAllowed = true;
    public bool isDragCheckAllowed = true;
    private bool doubleClick = false;
    private int leftClickNum = 0;
    public static Transform doubleClickParent;
    public static int doubleClickCardclassIndex;
    public static string doubleClickTokenId;
    //private Image[] clickImages;

    public int deckCount;

    [Header("Buttons")]
    [SerializeField] private Button addDeck;
    [SerializeField] private Button editDeck;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button deleteButton;

    [Header("Game objects")]
    [SerializeField] private GameObject addDeckObject;
    [SerializeField] private GameObject addDeckButtonEnabled;
    [SerializeField] private GameObject addDeckButtonDisabled;
    [SerializeField] public GameObject editDeckObject;
    [SerializeField] private GameObject editDeckButtonEnabled;
    [SerializeField] private GameObject editDeckButtonDisabled;
    [SerializeField] private GameObject saveDeckButtonEnabled;
    [SerializeField] private GameObject saveDeckButtonDisabled;
    [SerializeField] private GameObject deleteDeckButtonEnabled;
    [SerializeField] private GameObject deleteDeckButtonDisabled;

    [SerializeField] public GameObject selectedDeck1;
    [SerializeField] public GameObject selectedDeck2;
    [SerializeField] public GameObject selectedDeck3;
    [SerializeField] public GameObject selectedDeck4;
    [SerializeField] public GameObject selectedDeck5;

    [SerializeField] public GameObject loadingObject;
    [SerializeField] public GameObject availableListOfCard;
    [SerializeField] public GameObject currentListOfCard;
    [SerializeField] public GameObject search;
    [SerializeField] public GameObject popUpPanel;
    [SerializeField] public GameObject toolTip;
    [SerializeField] public GameObject loading;
    [SerializeField] public GameObject deleteDeckPopup;
    [SerializeField] public GameObject deleteDeckDontAsk;

    [SerializeField] public GameObject generalSelectLarge;
    [SerializeField] public GameObject generalSelectSmall;
    [SerializeField] public GameObject deckNotes;
    [SerializeField] public GameObject deckNotesGraphic;
    [SerializeField] public GameObject deckNotesParent1;
    [SerializeField] public GameObject deckNotesParent2;
    [SerializeField] public GameObject deckNotesParent3;
    [SerializeField] public GameObject deckNotesParent4;
    [SerializeField] public GameObject deckNotesParent5;
    [SerializeField] public TMP_InputField deckNotesTitle1;
    [SerializeField] public TMP_InputField deckNotesBody1;
    [SerializeField] public TMP_InputField deckNotesTitle2;
    [SerializeField] public TMP_InputField deckNotesBody2;
    [SerializeField] public TMP_InputField deckNotesTitle3;
    [SerializeField] public TMP_InputField deckNotesBody3;
    [SerializeField] public TMP_InputField deckNotesTitle4;
    [SerializeField] public TMP_InputField deckNotesBody4;
    [SerializeField] public TMP_InputField deckNotesTitle5;
    [SerializeField] public TMP_InputField deckNotesBody5;
    [SerializeField] public TMP_Text deckNotesTitleSelected;
    [SerializeField] public TMP_Text deckNotesBodySelected;

    [SerializeField] private GameObject popupCardObject;
    [SerializeField] private GameObject popupButtonOff;
    [SerializeField] private GameObject popupButtonOn;


    [Header("images")]
    [SerializeField] private Image deckProfile;
    [SerializeField] private Image margoDeckPreview;
    [SerializeField] private Image miosDeckPreview;
    [SerializeField] private Image nassetariDeckPreview;
    [SerializeField] private Image voidDeckPreview;
    [SerializeField] private Image tootDeckPreview;
    [SerializeField] private Image unknownDeckPreview;
    [SerializeField] private Image deckSelected;
    [SerializeField] private Image deckHighlighted;
    [SerializeField] private Image deckOriginal;
    [SerializeField] private Image[] images;
    [SerializeField] private Card bigCardPrefab;

    [Header("static variable")]
    public static bool isAdd = false;
    public static bool isEdit = false;
    public static bool isMatch = false;
    public static bool hasToken = false;
    public static int generalsIndex = 0;
    public static int generalsIndexStatic; //enables hovering updates
    public static List<Card> availableCards = new List<Card>();
    public static List<Card> currentCards = new List<Card>();
    public static int onDragCardCount;
    public bool hitCardLimit = false;

    [Header("Cards")]
    [SerializeField] private GameObject cardButtonUIon;
    [SerializeField] private GameObject cardButtonUIoff;
    public int cardIndex;
    private int nextCardCount;

    public Card cardPrefabs;
    private List<CardDetails> cardDetails;
    public GameObject cardHideParent;

    private int deckId = -1;
    private string searchText;
    private Card cardInstance;

    private List<CardDetails> matchedCardListForAvailable = new List<CardDetails>();
    private List<CardDetails> unmatchedCardListForAvailable = new List<CardDetails>();

    private Color enableColor = new Vector4(1f, 1f, 1f, 1f);
    public Color disableColor = new Vector4(200f / 255f, 200f / 255f, 200f / 255f, 128f / 255f);

    public AudioSource doubleClickCard;
    public AudioSource doubleClickCardError;
    public AudioSource singleClickCard;

    private int countOfLevelOne = 0;
    private int[] tempAvailCard;
    private int[] tempCurrCard;

    [Header("Gameboards")]
    [SerializeField] private GameObject gameboardButtonUIon;
    [SerializeField] private GameObject gameboardButtonUIoff;
    [SerializeField] public GameObject gameboardUI;
    [SerializeField] private string searchTextGameboard;
    [SerializeField] public GameObject searchGameboard;
    public Transform gameboardSortTransformSearch;
    public GameObject gameboardSortSearch;
    public GameObject[] gameboardSortArrayAllSearch;
    public GameObject gameboardHideParent;

    public GameObject selectedGameboardLoad;
    public Transform selectedGameboardTransformLoad;
    public Image selectedGameboardImageLoad;
    public TMP_Text selectedGameboardNameLoad;
    public Image[] selectedGameboardImagesLoad;
    public GameObject[] selectedGameboardArrayLoad;

    [Header("Generals")]
    [SerializeField] private GameObject generalButtonUIon;
    [SerializeField] private GameObject generalButtonUIoff;

    [Header("Metrics")]
    [SerializeField] public int masqueradesGamesPlayed;
    [SerializeField] public Image masquerdesGameboard;
    [SerializeField] public TMP_Text masquerdesGameboardName;
    [SerializeField] public int theOldKingdomGamesPlayed;
    [SerializeField] public Image theOldKingdomGameboard;
    [SerializeField] public TMP_Text theOldKingdomGameboardName;
    [SerializeField] public int fairytalesGamesPlayed;
    [SerializeField] public Image fairytalesGameboard;
    [SerializeField] public TMP_Text fairytalesGameboardName;
    [SerializeField] public int darkMatterGamesPlayed;
    [SerializeField] public Image darkMatterGameboard;
    [SerializeField] public TMP_Text darkMatterGameboardName;
    [SerializeField] public int tinkerersGamesPlayed;
    [SerializeField] public Image tinkerersGameboard;
    [SerializeField] public TMP_Text tinkerersGameboardName;

    private void Awake()
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
    private void Start()
    {
        cardDetails = CardDataBase.instance.cardDetails;

        DeckCount();

        DisplayDeck();
        ClearCards();
        DestroyCardList();

        StartCoroutine(RetrieveMetricsDeckBuilder());
    }

    public void DeckCount()
    {
        if (ErgoQuery.instance.deckGeneralStore[0] == 0)
        {
            deckCount = 0;
        }
        else if (ErgoQuery.instance.deckGeneralStore[4] > 0)
        {
            deckCount = 5;
            Debug.Log(ErgoQuery.instance.deckGeneralStore[4]);
        }
        else if (ErgoQuery.instance.deckGeneralStore[3] > 0)
        {
            deckCount = 4;
            Debug.Log(ErgoQuery.instance.deckGeneralStore[4]);
        }
        else if (ErgoQuery.instance.deckGeneralStore[2] > 0)
        {
            deckCount = 3;
        }
        else if (ErgoQuery.instance.deckGeneralStore[1] > 0)
        {
            deckCount = 2;
        }
        else if (ErgoQuery.instance.deckGeneralStore[0] > 0)
        {
            deckCount = 1;
        }
    }

    private void Update()
    {
        if (isEdit && !deleteDeckButtonEnabled.activeSelf)
        {
            deleteDeckButtonEnabled.SetActive(true);
            deleteDeckButtonDisabled.SetActive(false);
        }

        if (currentListOfCard.transform.childCount < 10 && saveDeckButtonEnabled.activeSelf)
        {
            saveDeckButtonEnabled.SetActive(false);
            saveDeckButtonDisabled.SetActive(true);
        }
        else if (currentListOfCard.transform.childCount >= 10 && !saveDeckButtonEnabled.activeSelf)
        {
            saveDeckButtonEnabled.SetActive(true);
            saveDeckButtonDisabled.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && popUpPanel.activeSelf)
        {
            cursorManager.AudioClickButtonStandard();
            OnBackFromPopUp();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && popUpPanel.activeSelf)
        {
            NextCardLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && popUpPanel.activeSelf)
        {
            NextCardRight();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && generalSelectLarge.activeSelf)
        {
            cursorManager.AudioClickButtonStandard();
            generalSelectLarge.SetActive(false);
            generalSelectSmall.SetActive(false);
            deckProfile.enabled = true;
            deckNotesGraphic.SetActive(true);
        }

        if (Draggable.dragEnd)
        {
            //DragEndInstance(); //test to-do
            if (hitCardLimit == true)
            {
                hitCardLimit = false;
                ActiveTooltip("You cannot have more than 25 cards");
                Invoke("RemoveToolTip", 3f);
            }
            else if (generalsIndex != Draggable.cardclassIndex && Draggable.cardclassIndex != 0)
            {
                ActiveTooltip("You cannot use this card with this General");
                Invoke("RemoveToolTip", 3f);
            }
            else if (!ErgoQuery.instance.tokenIDs.Contains(Draggable.tokenId) && Draggable.tokenId != "" && !popUpPanel.activeSelf)
            {
                ActiveTooltip("You do not own this card");
                Invoke("RemoveToolTip", 3f);
            }
            Draggable.dragEnd = false;
        }

        if (Input.GetMouseButtonDown(0) && loadingObject.activeSelf)
        {
            ObjectSelection();
        }

        if (Input.GetMouseButtonDown(0) && addDeckObject.activeSelf && !loadingObject.activeSelf) //this controls deck selection and hovering UI/mechanics, as well as DeckID
        {
            RaycastHit hit;
            //Send a ray from the camera to the mouseposition
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Create a raycast from the Camera and output anything it hits
            if (Physics.Raycast(ray, out hit))
            {
                //Check the hit GameObject has a Collider
                if (hit.collider != null)
                {
                    //Click a GameObject to return that GameObject your mouse pointer hit
                    m_MyGameObject = hit.collider.gameObject;
                    //Set this GameObject you clicked as the currently selected in the EventSystem
                    if (IsSelected == true)
                    {
                        if (m_MyGameObject.ToString() == "New Deck (UnityEngine.GameObject)" || m_MyGameObject.ToString() == "Edit Deck (UnityEngine.GameObject)" ||
                            m_MyGameObject.ToString() == "Back (UnityEngine.GameObject)")
                        {
                            ObjectSelection();
                            Debug.Log("hello1");
                        }
                    }
                    else if (!m_MyGameObject.ToString().Contains("General"))
                    {
                        EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                    }
                    //Selects the game object we found
                    Debug.Log(m_MyGameObject.ToString());
                    Debug.Log(IsSelected);
                    //outputs the gameobjects name to the log
                    if ((IsSelected == false) && m_MyGameObject.GetComponent<Image>().IsActive())
                    {
                        if (m_MyGameObject.ToString() == "Deck Image 1 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(1);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 2 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(2);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 3 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(3);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 4 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(4);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 5 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(5);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                    }
                    else if (IsSelected == true && m_MyGameObject.ToString().Contains("Image") &&
                        !m_MyGameObject.ToString().Contains((deckId).ToString()) && m_MyGameObject.GetComponent<Image>().IsActive())
                    {
                        if (m_MyGameObject.ToString() == "Deck Image 1 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(1);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 2 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(2);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 3 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(3);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 4 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(4);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 5 (UnityEngine.GameObject)")
                        {
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(5);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                    }
                    else if (IsSelected == true && m_MyGameObject.ToString() != "Edit Deck (UnityEngine.GameObject)"
                        && m_MyGameObject.ToString() != "New Deck (UnityEngine.GameObject)"
                        && m_MyGameObject.ToString() != "Back (UnityEngine.GameObject)")
                    {
                        if (m_MyGameObject.ToString() != "Deck Notes Parent (UnityEngine.GameObject)")
                        {
                            Debug.Log(deckId);
                            IsSelected = false;
                            StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId); //when they click new deck while editing notes

                            SpriteState spriteState = new SpriteState();
                            spriteState.highlightedSprite = deckHighlighted.sprite; // Set the highlighted sprite
                            spriteState.pressedSprite = deckSelected.sprite;
                            spriteState.selectedSprite = deckSelected.sprite;
                            buttons[deckId - 1].spriteState = spriteState;

                            deckId = -1;
                            editDeckButtonEnabled.SetActive(false);
                            editDeckButtonDisabled.SetActive(true);
                            EventSystem.current.SetSelectedGameObject(null);

                            if (!IsMouseOverDeck())
                            {
                                Debug.Log(deckId);
                                deckProfile.sprite = unknownDeckPreview.sprite;
                                if (m_MyGameObject.ToString() != "New Deck (UnityEngine.GameObject)" && m_MyGameObject.ToString() != "Deck Notes Parent (UnityEngine.GameObject)")
                                {
                                    deckNotes.SetActive(false);
                                }
                            }

                            if (!m_MyGameObject.GetComponent<Image>().IsActive()) //for when you click a deck and then click that same deck again or an empty deck
                            {
                                deckProfile.sprite = unknownDeckPreview.sprite;
                                deckNotes.SetActive(false);
                            }

                            for (int i = 0; i < images.Length; i++) //this resets the deck images when they shouldn't be selected - specifically from dragging notes fields which would be a bug
                            {
                                if (images[i].sprite != deckOriginal.sprite)
                                {
                                    images[i].sprite = deckOriginal.sprite;
                                    Sprite newSprite1 = deckSelected.sprite;
                                    Sprite newSprite2 = deckHighlighted.sprite;
                                    SpriteState st = new SpriteState();
                                    st.highlightedSprite = newSprite2;
                                    st.pressedSprite = newSprite1;
                                    st.selectedSprite = newSprite1;
                                    buttons[i].spriteState = st;
                                }
                            }
                        }
                    }
                }
            }
            else if (!Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider);
                IsSelected = false;
                deckId = -1;
                editDeckButtonEnabled.SetActive(false);
                editDeckButtonDisabled.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                deckNotes.SetActive(false);
                deckProfile.sprite = unknownDeckPreview.sprite;
                Debug.Log(IsSelected);
                Debug.Log(deckId);

                for (int i = 0; i < images.Length; i++) //this resets the deck images when they shouldn't be selected - specifically from dragging notes fields which would be a bug
                {
                    if (images[i].sprite != deckOriginal.sprite)
                    {
                        images[i].sprite = deckOriginal.sprite;
                        Sprite newSprite1 = deckSelected.sprite;
                        Sprite newSprite2 = deckHighlighted.sprite;
                        SpriteState st = new SpriteState();
                        st.highlightedSprite = newSprite2;
                        st.pressedSprite = newSprite1;
                        st.selectedSprite = newSprite1;
                        buttons[i].spriteState = st;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (deckId > 0)
            {
                int imageArrayPosition = deckId - 1;
                if (images[imageArrayPosition].sprite != deckSelected.sprite)
                {
                    images[imageArrayPosition].sprite = deckSelected.sprite;
                }
            }
        }
    }

    public bool IsMouseOverDeck() //to fix when a user tries to drag notes and lands over a deck
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void ObjectSelection()
    {
        if (deckId == 1)
        {
            EventSystem.current.SetSelectedGameObject(selectedDeck1);
        }
        else if (deckId == 2)
        {
            EventSystem.current.SetSelectedGameObject(selectedDeck2);
        }
        else if (deckId == 3)
        {
            EventSystem.current.SetSelectedGameObject(selectedDeck3);
        }
        else if (deckId == 4)
        {
            EventSystem.current.SetSelectedGameObject(selectedDeck4);
        }
        else if (deckId == 5)
        {
            EventSystem.current.SetSelectedGameObject(selectedDeck5);
        }
    }

    public void DeckNotesTyping()
    {
        if (deckId == 1)
        {
            deckNotesTitleSelected.text = deckNotesTitle1.text;
            deckNotesBodySelected.text = deckNotesBody1.text;
        }
        else if (deckId == 2)
        {
            deckNotesTitleSelected.text = deckNotesTitle2.text;
            deckNotesBodySelected.text = deckNotesBody2.text;
        }
        else if (deckId == 3)
        {
            deckNotesTitleSelected.text = deckNotesTitle3.text;
            deckNotesBodySelected.text = deckNotesBody3.text;
        }
        else if (deckId == 4)
        {
            deckNotesTitleSelected.text = deckNotesTitle4.text;
            deckNotesBodySelected.text = deckNotesBody4.text;
        }
        else if (deckId == 5)
        {
            deckNotesTitleSelected.text = deckNotesTitle5.text;
            deckNotesBodySelected.text = deckNotesBody5.text;
        }
    }

    public void DeckNotesSelect()
    {
        int imageArrayPosition = deckId - 1;
        if (images[imageArrayPosition].sprite != deckSelected.sprite)
        {
            images[imageArrayPosition].sprite = deckSelected.sprite;

            Sprite newSprite = deckSelected.sprite;
            SpriteState st = new SpriteState();
            st.highlightedSprite = newSprite;
            st.pressedSprite = newSprite;
            st.selectedSprite = newSprite;
            buttons[imageArrayPosition].spriteState = st;
        }
    }
    public void DeckNotesDeselect()
    {
        int imageArrayPosition = deckId - 1;
        RaycastHit hitNotes;
        Ray rayNotes = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayNotes, out hitNotes))
        {
            if (hitNotes.collider != null)
            {
                notesGameObject = hitNotes.collider.gameObject;

                if (notesGameObject.ToString() != "Deck Notes Parent (UnityEngine.GameObject)") //this is if we click any othe rgame object
                {
                    images[imageArrayPosition].sprite = deckOriginal.sprite;

                    Sprite newSprite1 = deckSelected.sprite;
                    Sprite newSprite2 = deckHighlighted.sprite;
                    SpriteState st = new SpriteState();
                    st.highlightedSprite = newSprite2;
                    st.pressedSprite = newSprite1;
                    st.selectedSprite = newSprite1;
                    buttons[imageArrayPosition].spriteState = st;

                    if (m_MyGameObject.ToString() == "New Deck (UnityEngine.GameObject)" || m_MyGameObject.ToString() == "Edit Deck (UnityEngine.GameObject)")
                    {
                        StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId);
                    }
                }
                else if (notesGameObject.ToString() == "Deck Notes Parent (UnityEngine.GameObject)")
                {
                    StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId);
                }
            }
            else //this one will never happen unless the collider is not on a game object
            {
                images[imageArrayPosition].sprite = deckOriginal.sprite;

                Sprite newSprite1 = deckSelected.sprite;
                Sprite newSprite2 = deckHighlighted.sprite;
                SpriteState st = new SpriteState();
                st.highlightedSprite = newSprite2;
                st.pressedSprite = newSprite1;
                st.selectedSprite = newSprite1;
                buttons[imageArrayPosition].spriteState = st;

                StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId);
            }
        }
        else
        {
            images[imageArrayPosition].sprite = deckOriginal.sprite;

            Sprite newSprite1 = deckSelected.sprite;
            Sprite newSprite2 = deckHighlighted.sprite;
            SpriteState st = new SpriteState();
            st.highlightedSprite = newSprite2;
            st.pressedSprite = newSprite1;
            st.selectedSprite = newSprite1;
            buttons[imageArrayPosition].spriteState = st;

            StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId);
        }
    }
    IEnumerator DetectDoubleLeftClick(GameObject cardClicked, Card card, string doubleClickCardType)
    {
        int currentCount = currentListOfCard.transform.childCount;
        //clickImages = card.GetComponentsInChildren<Image>();
        isTimeCheckAllowed = false;
        while (Time.time < firstLeftClickTime + timeBetweenLeftClick)
        {
            if (leftClickNum == 2)
            {
                if ((generalsIndex == (int)card.cardClass || (int)card.cardClass == 0)
                    && (ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId) || card.ergoTokenId == "") && currentCount <= 25)
                {
                    if (doubleClickCardType == "Available" && currentCount < 25)
                    {
/*                        if (clickImages != null) //unsure what this used to do, doesn't seem needed anymore - leave for now just in case
                        {
                            foreach (Image thisImage in clickImages)

                            {
                                if ((thisImage.ToString() == "Image (UnityEngine.UI.Image)" || thisImage.ToString() == "Frame (UnityEngine.UI.Image)")
                                    && !thisImage.color.ToString().Contains("0.502"))
                                {
                                    thisImage.color = new Vector4(0f, 0f, 0f, 0f);
                                }
                            }
                        }*/

                        Debug.Log("Double Click Available to Current Move");
                        doubleClickCard.Play();
                        cardClicked.transform.SetParent(currentListOfCard.transform);
                        CurrentSelectedLoad();
                    }
                    else if (doubleClickCardType == "Available" && currentCount >= 25)
                    {
                        Debug.Log("25 Double Click Available to Current Move");
                        doubleClickCardError.Play();
                        ActiveToolTipDoubleClick("You cannot have more than 25 cards");
                        Invoke("RemoveToolTipDoubleClick", 2f);
                    }
                    else if (doubleClickCardType == "Current")
                    {
                        Debug.Log("Double Click Current to Available Move");
                        doubleClickCard.Play();
                        cardClicked.transform.SetParent(availableListOfCard.transform);
                        AvailableSelectedLoad();
                    }
                }
                else if (generalsIndex != (int)card.cardClass)
                {
                    ActiveToolTipDoubleClick("You cannot use this card with this General");
                    doubleClickCardError.Play();
                    Invoke("RemoveToolTipDoubleClick", 2f);
                }
                else if (!ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId))
                {
                    ActiveToolTipDoubleClick("You do not own this card.");
                    doubleClickCardError.Play();
                    Invoke("RemoveToolTipDoubleClick", 2f);
                }
                else if (currentCount >= 25 && doubleClickCardType == "Available") //just in case something else fails above
                {
                    ActiveToolTipDoubleClick("You cannot have more than 25 cards");
                    doubleClickCardError.Play();
                    Invoke("RemoveToolTipDoubleClick", 2f);
                }

                doubleClick = true;
                break;
            }
            yield return null;
        }
        if (doubleClick == false)
        {
            Debug.Log("single click");
            singleClickCard.Play();
            popUpPanel.SetActive(true);

            if (card.transform.parent.parent.parent.name.Split(" ")[0] == "Available")
            {
                if (generalsIndex == (int)card.cardClass || (int)card.cardClass == 0) //is it the right general or f2p
                {

                    if (ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId) || card.ergoTokenId == "") //do i own the token or is it f2p
                    {
                        popupButtonOff.SetActive(false);
                        popupButtonOn.SetActive(true);
                        popupButtonOn.transform.GetComponentInChildren<TMP_Text>().text = "Add";

                        if (ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId) && card.ergoTokenId != "") //this sets the right amounts
                        {
                            int amountIndex = ErgoQuery.instance.tokenIDs.FindIndex(a => a.Contains(card.ergoTokenId));
                            long tokenAmounts = ErgoQuery.instance.tokenAmounts[amountIndex];
                            card.ergoTokenAmount = tokenAmounts;
                        }
                        else if (card.ergoTokenId == "") //if it's a f2p card
                        {
                            card.ergoTokenAmount = 1;
                        }
                    }
                    else //if it's the right general and not a f2p card and I don't own the token
                    {

                        popupButtonOff.SetActive(true);
                        popupButtonOn.SetActive(false);
                        popupButtonOff.transform.GetComponentInChildren<TMP_Text>().text = "Add";

                        card.ergoTokenAmount = 0;
                    }
                }
                else //its the wrong general and not a f2p card
                {
                    popupButtonOff.SetActive(true);
                    popupButtonOn.SetActive(false);
                    popupButtonOff.transform.GetComponentInChildren<TMP_Text>().text = "Add";

                    if (ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId) && card.ergoTokenId != "") //if I own the card but it's the wrong general
                    {
                        int amountIndex = ErgoQuery.instance.tokenIDs.FindIndex(a => a.Contains(card.ergoTokenId));
                        long tokenAmounts = ErgoQuery.instance.tokenAmounts[amountIndex];
                        card.ergoTokenAmount = tokenAmounts;
                    }
                    else if (card.ergoTokenId == "") //if it's a f2p card
                    {
                        card.ergoTokenAmount = 1;
                    }
                    else if (!ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId)) //if I don't own the card
                    {
                        card.ergoTokenAmount = 0;
                    }
                }
            }
            else if (card.transform.parent.parent.parent.name.Split(" ")[0] == "Current")
            {
                popupButtonOff.SetActive(false);
                popupButtonOn.SetActive(true);
                popupButtonOn.transform.GetComponentsInChildren<Button>()[0].GetComponentInChildren<TMP_Text>().text = "Remove";

                if (ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId) && card.ergoTokenId != "") //I own the card and it's a blockchain card
                {
                    int amountIndex = ErgoQuery.instance.tokenIDs.FindIndex(a => a.Contains(card.ergoTokenId));
                    long tokenAmounts = ErgoQuery.instance.tokenAmounts[amountIndex];
                    card.ergoTokenAmount = tokenAmounts;
                }
                else if (card.ergoTokenId == "") //it's a f2p card
                {
                    card.ergoTokenAmount = 1;
                }
            }

            popupCardObject = cardClicked;
            cardInstance = Instantiate<Card>(bigCardPrefab, popUpPanel.transform);
            cardInstance.SetProperties(card.id, card.ergoTokenId, card.ergoTokenAmount, card.cardName, card.cardDescription, card.attack, card.HP, card.gold, card.XP, card.fieldLimit, card.clan, card.levelRequired, card.image.sprite, card.frame.sprite, card.cardClass, card.ability
                //, card.requirements, card.abilityLevel
                );
        }

        leftClickNum = 0;
        doubleClick = false;
        isTimeCheckAllowed = true;
        isDragCheckAllowed = true;
    }

    public void OnClickSingleCard(GameObject cardClicked, Card selectedCard, string doubleClickCardType) // see FindDeckManager script. Responsible for,
                                                                                                         // when user click any card either current or available, Popup panel open
    {
        isDragCheckAllowed = false;
        leftClickNum += 1;
        if (leftClickNum == 1 && isTimeCheckAllowed)
        {
            firstLeftClickTime = Time.time;
            StartCoroutine(DetectDoubleLeftClick(cardClicked, selectedCard, doubleClickCardType));
        }
    }

    public void CardClickAddButtonAudio()
    {
        doubleClickCard.Play();
    }

    public void AddOrRemoveCard() // If popuped card is matched, Add or Remove card on respective card
    {
        int currentCount = currentListOfCard.transform.childCount;
        Card card = popUpPanel.transform.GetComponentInChildren<Card>();
        popupCardObject.SetActive(true); //for when things are filtered and someone is going left / right through cards

        if (popupCardObject.transform.parent.name.Contains("Available")) //comes from the click enumerator
        {
            if ((int)card.cardClass == generalsIndex || (int)card.cardClass == 0)
            {
                //Card removeCard = availableCards.Find(singleCard => singleCard.cardName == card.cardName);
                if (currentCount <= 24)
                {
                    popupCardObject.transform.SetParent(currentListOfCard.transform);
                    CurrentSelectedLoad();
                }
                else
                {
                    ActiveTooltip("You cannot have more than 25 cards");
                    doubleClickCardError.Play();
                    Invoke("RemoveToolTip", 2f);
                }
            }
        }
        else if (popupCardObject.transform.parent.name.Contains("Current")) //comes from the click enumerator
        {
            popupCardObject.transform.SetParent(availableListOfCard.transform);
            AvailableSelectedLoad();
        }

        Destroy(popUpPanel.transform.GetComponentInChildren<Card>().gameObject);
        cursorManager.CursorNormal();
        popUpPanel.SetActive(false);

        GameObject ghostCard = GameObject.FindWithTag("Card"); //this is to fix the bug with them dragging the card at the same time as clicking a card
        if (ghostCard.transform.parent == null)
        {
            Destroy(ghostCard);
        }
    }

    public void OnBackFromPopUp() // When press back button from popup panel
    {
        Destroy(popUpPanel.transform.GetComponentInChildren<Card>().gameObject);
        cursorManager.CursorNormal();
        popUpPanel.SetActive(false);


        GameObject ghostCard = GameObject.FindWithTag("Card");
        if (ghostCard.transform.parent == null)
        {
            Destroy(ghostCard);
        }
    }

    public void NextCardLeft() // go to next gameobject in card popup
    {
        Destroy(popUpPanel.transform.GetComponentInChildren<Card>().gameObject);
        string nextCardType = popupCardObject.transform.parent.parent.parent.name.Split(" ")[0];
        if (nextCardType == "Available")
        {
            nextCardCount = availableListOfCard.transform.childCount;
        }
        else if (nextCardType == "Current")
        {
            nextCardCount = currentListOfCard.transform.childCount;
        }

        if (cardIndex == 0)
        {
            cardIndex = nextCardCount - 1;
            GameObject nextCard = popupCardObject.transform.parent.GetChild(cardIndex).gameObject;
            Card nextCardPrefap = popupCardObject.transform.parent.GetChild(cardIndex).gameObject.GetComponent<Card>();
            NextCard(nextCard, nextCardPrefap);
        }
        else
        {
            cardIndex--;
            GameObject nextCard = popupCardObject.transform.parent.GetChild(cardIndex).gameObject;
            Card nextCardPrefap = popupCardObject.transform.parent.GetChild(cardIndex).gameObject.GetComponent<Card>();
            NextCard(nextCard, nextCardPrefap);
        }
    }

    public void NextCardRight() // go to next gameobject in card popup
    {
        Destroy(popUpPanel.transform.GetComponentInChildren<Card>().gameObject);
        string nextCardType = popupCardObject.transform.parent.parent.parent.name.Split(" ")[0];
        if (nextCardType == "Available")
        {
            nextCardCount = availableListOfCard.transform.childCount;
        }
        else if (nextCardType == "Current")
        {
            nextCardCount = currentListOfCard.transform.childCount;
        }

        if (cardIndex + 1 == nextCardCount)
        {
            cardIndex = 0;
            GameObject nextCard = popupCardObject.transform.parent.GetChild(cardIndex).gameObject;
            Card nextCardPrefap = popupCardObject.transform.parent.GetChild(cardIndex).gameObject.GetComponent<Card>();
            NextCard(nextCard, nextCardPrefap);
        }
        else
        {
            cardIndex++;
            GameObject nextCard = popupCardObject.transform.parent.GetChild(cardIndex).gameObject;
            Card nextCardPrefap = popupCardObject.transform.parent.GetChild(cardIndex).gameObject.GetComponent<Card>();
            NextCard(nextCard, nextCardPrefap);
        }
    }

    public void NextCard(GameObject cardClicked, Card card)
    {
        string nextCardType = popupCardObject.transform.parent.parent.parent.name.Split(" ")[0];

        int currentCount = currentListOfCard.transform.childCount;
        //clickImages = card.GetComponentsInChildren<Image>();
        Debug.Log("next card");
        singleClickCard.Play();

        if (card.transform.parent.parent.parent.name.Split(" ")[0] == "Available")
        {
            if (generalsIndex == (int)card.cardClass || (int)card.cardClass == 0) //is it the right general or f2p
            {

                if (ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId) || card.ergoTokenId == "") //do i own the token or is it f2p
                {
                    popupButtonOff.SetActive(false);
                    popupButtonOn.SetActive(true);
                    popupButtonOn.transform.GetComponentInChildren<TMP_Text>().text = "Add";

                    if (ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId) && card.ergoTokenId != "") //this sets the right amounts
                    {
                        int amountIndex = ErgoQuery.instance.tokenIDs.FindIndex(a => a.Contains(card.ergoTokenId));
                        long tokenAmounts = ErgoQuery.instance.tokenAmounts[amountIndex];
                        card.ergoTokenAmount = tokenAmounts;
                    }
                    else if (card.ergoTokenId == "") //if it's a f2p card
                    {
                        card.ergoTokenAmount = 1;
                    }
                }
                else //if it's the right general and not a f2p card and I don't own the token
                {
                    popupButtonOff.SetActive(true);
                    popupButtonOn.SetActive(false);
                    popupButtonOff.transform.GetComponentInChildren<TMP_Text>().text = "Add";

                    card.ergoTokenAmount = 0;
                }
            }
            else //its the wrong general and not a f2p card
            {
                popupButtonOff.SetActive(true);
                popupButtonOn.SetActive(false);
                popupButtonOff.transform.GetComponentInChildren<TMP_Text>().text = "Add";

                if (ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId) && card.ergoTokenId != "") //if I own the card but it's the wrong general
                {
                    int amountIndex = ErgoQuery.instance.tokenIDs.FindIndex(a => a.Contains(card.ergoTokenId));
                    long tokenAmounts = ErgoQuery.instance.tokenAmounts[amountIndex];
                    card.ergoTokenAmount = tokenAmounts;
                }
                else if (card.ergoTokenId == "") //if it's a f2p card
                {
                    card.ergoTokenAmount = 1;
                }
                else if (!ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId)) //if I don't own the card
                {
                    card.ergoTokenAmount = 0;
                }
            }
        }
        else if (card.transform.parent.parent.parent.name.Split(" ")[0] == "Current")
        {
            popupButtonOff.SetActive(false);
            popupButtonOn.SetActive(true);
            popupButtonOn.transform.GetComponentsInChildren<Button>()[0].GetComponentInChildren<TMP_Text>().text = "Remove";

            if (ErgoQuery.instance.tokenIDs.Contains(card.ergoTokenId) && card.ergoTokenId != "") //I own the card and it's a blockchain card
            {
                int amountIndex = ErgoQuery.instance.tokenIDs.FindIndex(a => a.Contains(card.ergoTokenId));
                long tokenAmounts = ErgoQuery.instance.tokenAmounts[amountIndex];
                card.ergoTokenAmount = tokenAmounts;
            }
            else if (card.ergoTokenId == "") //it's a f2p card
            {
                card.ergoTokenAmount = 1;
            }
        }

        popupCardObject = cardClicked;
        cardInstance = Instantiate<Card>(bigCardPrefab, popUpPanel.transform);
        cardInstance.SetProperties(card.id, card.ergoTokenId, card.ergoTokenAmount, card.cardName, card.cardDescription, card.attack, card.HP, card.gold, card.XP, card.fieldLimit, card.clan, card.levelRequired, card.image.sprite, card.frame.sprite, card.cardClass, card.ability
            //, card.requirements, card.abilityLevel
            );
    }


    public void OnClickAddDeck() // click on add deck button
    {
        Debug.Log(deckId);
        if (deckId >= 1)
        {
            StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId);

            int imageArrayPosition = deckId - 1;
            if (images[imageArrayPosition].sprite == deckSelected.sprite)
            {
                images[imageArrayPosition].sprite = deckOriginal.sprite;
            }
        }

        deckId = -1;
        IsSelected = false;
        Debug.Log(deckId);
        EventSystem.current.SetSelectedGameObject(m_MyGameObject);
        generalSelectLarge.SetActive(true);
        generalSelectSmall.SetActive(true);
        deckNotes.SetActive(false);
        deckProfile.sprite = unknownDeckPreview.sprite;

        isAdd = true;
        deleteDeckButtonEnabled.SetActive(false);
        deleteDeckButtonDisabled.SetActive(true);
        saveDeckButtonEnabled.SetActive(false);
        saveDeckButtonDisabled.SetActive(true);
        editDeckButtonEnabled.SetActive(false);
        editDeckButtonDisabled.SetActive(true);

        isEdit = false;
    }

    public void OnClickMargoDeck(int index) // When user select red general
    {
        editDeckObject.SetActive(true);
        /*        deckNotes.SetActive(false);
                deckNotesGraphic.SetActive(false);*/
        generalSelectLarge.SetActive(false);
        generalSelectSmall.SetActive(false);
        addDeckObject.SetActive(false);
        generalsIndex = index;
        Init();
    }
    public void OnClickMiosDeck(int index) // When user select green general
    {
        editDeckObject.SetActive(true);
        /*        deckNotes.SetActive(false);
                deckNotesGraphic.SetActive(false);*/
        generalSelectLarge.SetActive(false);
        generalSelectSmall.SetActive(false);
        addDeckObject.SetActive(false);
        generalsIndex = index;
        Init();
    }
    public void OnClickNassetari(int index) // When user select blue general
    {
        editDeckObject.SetActive(true);
        /*        deckNotes.SetActive(false);
                deckNotesGraphic.SetActive(false);*/
        generalSelectLarge.SetActive(false);
        generalSelectSmall.SetActive(false);
        addDeckObject.SetActive(false);
        generalsIndex = index;
        Init();
    }

    public void OnClickVoidDeck(int index) // When user select blue general
    {
        editDeckObject.SetActive(true);
        /*        deckNotes.SetActive(false);
                deckNotesGraphic.SetActive(false);*/
        generalSelectLarge.SetActive(false);
        generalSelectSmall.SetActive(false);
        addDeckObject.SetActive(false);
        generalsIndex = index;
        Init();
    }

    public void OnClickTootDeck(int index) // When user select blue general
    {
        editDeckObject.SetActive(true);
        /*        deckNotes.SetActive(false);
                deckNotesGraphic.SetActive(false);*/
        generalSelectLarge.SetActive(false);
        generalSelectSmall.SetActive(false);
        addDeckObject.SetActive(false);
        generalsIndex = index;
        Init();
    }

    public void ShowDeckNotes1() // When user select blue general
    {
        deckNotesParent1.SetActive(true);
        deckNotesParent2.SetActive(false);
        deckNotesParent3.SetActive(false);
        deckNotesParent4.SetActive(false);
        deckNotesParent5.SetActive(false);
    }
    public void ShowDeckNotes2() // When user select blue general
    {
        deckNotesParent1.SetActive(false);
        deckNotesParent2.SetActive(true);
        deckNotesParent3.SetActive(false);
        deckNotesParent4.SetActive(false);
        deckNotesParent5.SetActive(false);
    }
    public void ShowDeckNotes3() // When user select blue general
    {
        deckNotesParent1.SetActive(false);
        deckNotesParent2.SetActive(false);
        deckNotesParent3.SetActive(true);
        deckNotesParent4.SetActive(false);
        deckNotesParent5.SetActive(false);
    }
    public void ShowDeckNotes4() // When user select blue general
    {
        deckNotesParent1.SetActive(false);
        deckNotesParent2.SetActive(false);
        deckNotesParent3.SetActive(false);
        deckNotesParent4.SetActive(true);
        deckNotesParent5.SetActive(false);
    }
    public void ShowDeckNotes5() // When user select blue general
    {
        deckNotesParent1.SetActive(false);
        deckNotesParent2.SetActive(false);
        deckNotesParent3.SetActive(false);
        deckNotesParent4.SetActive(false);
        deckNotesParent5.SetActive(true);
    }

    public void OnHoverDeckEnter(int id)
    {
        int hoverDeckGeneral = ErgoQuery.instance.deckGeneralStore[id - 1];
        int hoverDeckId = ErgoQuery.instance.deckIdStore[id - 1];

        Debug.Log(hoverDeckId);


        if (IsSelected)
        {
            StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId);
        }

        if (deckId >= -1)
        {
            if (deckId == -1)
            {
                SpriteState spriteState = new SpriteState();
                spriteState.highlightedSprite = deckHighlighted.sprite; // Set the highlighted sprite
                spriteState.pressedSprite = deckSelected.sprite;
                spriteState.selectedSprite = deckSelected.sprite;
                buttons[hoverDeckId - 1].spriteState = spriteState;
            }

            if (hoverDeckGeneral == 1)
            {
                deckProfile.sprite = margoDeckPreview.sprite;

            }
            else if (hoverDeckGeneral == 2)
            {
                deckProfile.sprite = miosDeckPreview.sprite;
            }
            else if (hoverDeckGeneral == 3)
            {
                deckProfile.sprite = nassetariDeckPreview.sprite;
            }
            else if (hoverDeckGeneral == 4)
            {
                deckProfile.sprite = voidDeckPreview.sprite;
            }
            else if (hoverDeckGeneral == 5)
            {
                deckProfile.sprite = tootDeckPreview.sprite;
            }

            if (hoverDeckId == 1)
            {
                ShowDeckNotes1();
                deckNotesTitle1.text = ErgoQuery.instance.deckTitleStore[id - 1];
                deckNotesBody1.text = ErgoQuery.instance.deckBodyStore[id - 1];
                deckNotes.SetActive(true);

                RightMiddleClickEnterReset(hoverDeckId);
            }
            else if (hoverDeckId == 2)
            {
                ShowDeckNotes2();
                deckNotesTitle2.text = ErgoQuery.instance.deckTitleStore[id - 1];
                deckNotesBody2.text = ErgoQuery.instance.deckBodyStore[id - 1];
                deckNotes.SetActive(true);

                RightMiddleClickEnterReset(hoverDeckId);
            }
            else if (hoverDeckId == 3)
            {
                ShowDeckNotes3();
                deckNotesTitle3.text = ErgoQuery.instance.deckTitleStore[id - 1];
                deckNotesBody3.text = ErgoQuery.instance.deckBodyStore[id - 1];
                deckNotes.SetActive(true);

                RightMiddleClickEnterReset(hoverDeckId);
            }
            else if (hoverDeckId == 4)
            {
                ShowDeckNotes4();
                deckNotesTitle4.text = ErgoQuery.instance.deckTitleStore[id - 1];
                deckNotesBody4.text = ErgoQuery.instance.deckBodyStore[id - 1];
                deckNotes.SetActive(true);

                RightMiddleClickEnterReset(hoverDeckId);
            }
            else if (hoverDeckId == 5)
            {
                ShowDeckNotes5();
                deckNotesTitle5.text = ErgoQuery.instance.deckTitleStore[id - 1];
                deckNotesBody5.text = ErgoQuery.instance.deckBodyStore[id - 1];
                deckNotes.SetActive(true);

                RightMiddleClickEnterReset(hoverDeckId);
            }
        }
    }

    public void RightMiddleClickEnterReset(int hoverDeckId)
    {
        if (deckId > 0 && deckId == hoverDeckId)
        {
            SpriteState spriteState = new SpriteState();
            spriteState.highlightedSprite = deckSelected.sprite;
            spriteState.pressedSprite = deckSelected.sprite;
            spriteState.selectedSprite = deckSelected.sprite;
            buttons[hoverDeckId - 1].spriteState = spriteState;
        }
    }
    public void OnHoverDeckExit(int id)
    {
        if (IsSelected)
        {
            deckNotesTitleSelected.text = ErgoQuery.instance.deckTitleStore[deckId - 1];
            deckNotesBodySelected.text = ErgoQuery.instance.deckBodyStore[deckId - 1];
        }

        if (deckId < 1)
        {
            deckProfile.sprite = unknownDeckPreview.sprite;
            deckNotes.SetActive(false);
        }
        else if (deckId >= 1)
        {
            if (generalsIndexStatic == 1)
            {
                deckProfile.sprite = margoDeckPreview.sprite;
            }
            else if (generalsIndexStatic == 2)
            {
                deckProfile.sprite = miosDeckPreview.sprite;
            }
            else if (generalsIndexStatic == 3)
            {
                deckProfile.sprite = nassetariDeckPreview.sprite;
            }
            else if (generalsIndexStatic == 4)
            {
                deckProfile.sprite = voidDeckPreview.sprite;
            }
            else if (generalsIndexStatic == 5)
            {
                deckProfile.sprite = tootDeckPreview.sprite;
            }

            if (deckId == 1)
            {
                ShowDeckNotes1();
                deckNotes.SetActive(true);
            }
            else if (deckId == 2)
            {
                ShowDeckNotes2();
                deckNotes.SetActive(true);
            }
            else if (deckId == 3)
            {
                ShowDeckNotes3();
                deckNotes.SetActive(true);
            }
            else if (deckId == 4)
            {
                ShowDeckNotes4();
                deckNotes.SetActive(true);
            }
            else if (deckId == 5)
            {
                ShowDeckNotes5();
                deckNotes.SetActive(true);
            }
        }
    }

    public void OnHoverNewDeckEnter(int id)
    {
        if (id == 1)
        {
            deckProfile.sprite = margoDeckPreview.sprite;
        }
        else if (id == 2)
        {
            deckProfile.sprite = miosDeckPreview.sprite;
        }
        else if (id == 3)
        {
            deckProfile.sprite = nassetariDeckPreview.sprite;
        }
        else if (id == 4)
        {
            deckProfile.sprite = voidDeckPreview.sprite;
        }
        else if (id == 5)
        {
            deckProfile.sprite = tootDeckPreview.sprite;
        }
        else
        {
            deckProfile.sprite = unknownDeckPreview.sprite;
        }
    }
    public void OnHoverNewDeckExit()
    {
        deckProfile.sprite = unknownDeckPreview.sprite;
    }
    public void OnDeckClick(int id) // Responsible for, When user click available deck
    {
        generalSelectLarge.SetActive(false);
        generalSelectSmall.SetActive(false);
        deckNotes.SetActive(true);
        deckNotesGraphic.SetActive(true);

        generalsIndex = ErgoQuery.instance.deckGeneralStore[id - 1];

        if (IsSelected == true)
        {
            deckId = id;
            deckNotesTitleSelected.text = ErgoQuery.instance.deckTitleStore[id - 1];
            deckNotesBodySelected.text = ErgoQuery.instance.deckBodyStore[id - 1];
        }
        editDeckButtonEnabled.SetActive(true);
        editDeckButtonDisabled.SetActive(false);

        generalsIndexStatic = generalsIndex; //to assist with resetting the hovering previews
        deckProfile.enabled = true;

        if (generalsIndex == 1)
        {
            deckProfile.sprite = margoDeckPreview.sprite;
        }
        else if (generalsIndex == 2)
        {
            deckProfile.sprite = miosDeckPreview.sprite;
        }
        else if (generalsIndex == 3)
        {
            deckProfile.sprite = nassetariDeckPreview.sprite;
        }
        else if (generalsIndex == 4)
        {
            deckProfile.sprite = voidDeckPreview.sprite;
        }
        else if (generalsIndex == 5)
        {
            deckProfile.sprite = tootDeckPreview.sprite;
        }

        isAdd = false;

        for (int i = 0; i < images.Length; i++) //this resets the deck images when they shouldn't be selected - specifically from dragging notes fields which would be a bug
        {
            if (i != (id - 1))
            {
                images[i].sprite = deckOriginal.sprite;
                Sprite newSprite1 = deckSelected.sprite;
                Sprite newSprite2 = deckHighlighted.sprite;
                SpriteState st = new SpriteState();
                st.highlightedSprite = newSprite2;
                st.pressedSprite = newSprite1;
                st.selectedSprite = newSprite1;
                buttons[i].spriteState = st;
            }
        }
    }
    public void Panel()
    {
        StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId);
        /*deckNotesGraphic.SetActive(false);
        deckNotes.SetActive(false);*/
        loading.SetActive(true);
        Invoke("OnClickEditDeck", 0.5f);
    }

    public void OnClickEditDeck() // User click edit deck button, all current and available card instantiate
    {
        cursorManager.CursorNormal();
        Debug.Log(deckId);
        isEdit = true;
        int availableCardLength = ErgoQuery.instance.cardIdAvailableStore[deckId - 1].Length;
        int currentCardLength = ErgoQuery.instance.cardIdCurrentStore[deckId - 1].Length;

        tempAvailCard = new int[ErgoQuery.instance.cardIdAvailableStore[deckId - 1].Length];
        tempCurrCard = new int[ErgoQuery.instance.cardIdCurrentStore[deckId - 1].Length];

        editDeckButtonEnabled.SetActive(false);
        editDeckButtonDisabled.SetActive(true);
        currentCards.Clear();
        availableCards.Clear();

        DestroyCardList();

        for (int i = 0; i < availableCardLength; i++)
        {
            tempAvailCard[i] = ErgoQuery.instance.cardIdAvailableStore[deckId - 1][i]; ; // adds all the available card IDs to the temperary array
        }

        for (int i = 0; i < currentCardLength; i++)
        {
            tempCurrCard[i] = ErgoQuery.instance.cardIdCurrentStore[deckId - 1][i];
        }

        //creats new lists from the arrays created previously
        List<int> tempAvailList = tempAvailCard.ToList(); //this has all the cards ID in it from the saved file
        List<int> tempCurrList = tempCurrCard.ToList();

        for (int i = 0; i < tempAvailList.Count; i++)
        {
            if (cardDetails.Where(card => card.id == tempAvailList[i] && (int)card.cardClass == 0).Count() == 1) //accounting for free to play cards
            {
                Card cardInstance = Instantiate<Card>(cardPrefabs, availableListOfCard.transform);
                cardInstance.SetProperties(cardDetails[tempAvailList[i] - 1].id, cardDetails[tempAvailList[i] - 1].ergoTokenId, cardDetails[tempAvailList[i] - 1].ergoTokenAmount, cardDetails[tempAvailList[i] - 1].cardName, cardDetails[tempAvailList[i] - 1].cardDescription, cardDetails[tempAvailList[i] - 1].attack, cardDetails[tempAvailList[i] - 1].HP, cardDetails[tempAvailList[i] - 1].gold, cardDetails[tempAvailList[i] - 1].XP, cardDetails[tempAvailList[i] - 1].fieldLimit, cardDetails[tempAvailList[i] - 1].clan, cardDetails[tempAvailList[i] - 1].levelRequired, cardDetails[tempAvailList[i] - 1].cardImage, cardDetails[tempAvailList[i] - 1].cardFrame, cardDetails[tempAvailList[i] - 1].cardClass, cardDetails[tempAvailList[i] - 1].ability
                    //, cardDetails[tempAvailList[i] - 1].requirements, cardDetails[tempAvailList[i] - 1].abilityLevel
                    );
                cardInstance.name = cardDetails[tempAvailList[i] - 1].cardName;
                availableCards.Add(cardInstance);
            }
            else if (cardDetails.Where(card => card.id == tempAvailList[i] && (int)card.cardClass != generalsIndex).Count() == 1)
            {
                Card cardInstance = Instantiate<Card>(cardPrefabs, availableListOfCard.transform);
                cardInstance.SetProperties(cardDetails[tempAvailList[i] - 1].id, cardDetails[tempAvailList[i] - 1].ergoTokenId, cardDetails[tempAvailList[i] - 1].ergoTokenAmount, cardDetails[tempAvailList[i] - 1].cardName, cardDetails[tempAvailList[i] - 1].cardDescription, cardDetails[tempAvailList[i] - 1].attack, cardDetails[tempAvailList[i] - 1].HP, cardDetails[tempAvailList[i] - 1].gold, cardDetails[tempAvailList[i] - 1].XP, cardDetails[tempAvailList[i] - 1].fieldLimit, cardDetails[tempAvailList[i] - 1].clan, cardDetails[tempAvailList[i] - 1].levelRequired, cardDetails[tempAvailList[i] - 1].cardImage, cardDetails[tempAvailList[i] - 1].cardFrame, cardDetails[tempAvailList[i] - 1].cardClass, cardDetails[tempAvailList[i] - 1].ability
                    //, cardDetails[tempAvailList[i] - 1].requirements, cardDetails[tempAvailList[i] - 1].abilityLevel
                    );
                cardInstance.name = cardDetails[tempAvailList[i] - 1].cardName;
                cardInstance.frame.GetComponent<Image>().color = disableColor;
                cardInstance.image.GetComponent<Image>().color = disableColor;
                availableCards.Add(cardInstance);
            }
            else if (cardDetails[tempAvailList[i] - 1].ergoTokenId != "" && !ErgoQuery.instance.tokenIDs.Contains(cardDetails[tempAvailList[i] - 1].ergoTokenId))
            {
                Card cardInstance = Instantiate<Card>(cardPrefabs, availableListOfCard.transform);
                cardInstance.SetProperties(cardDetails[tempAvailList[i] - 1].id, cardDetails[tempAvailList[i] - 1].ergoTokenId, cardDetails[tempAvailList[i] - 1].ergoTokenAmount, cardDetails[tempAvailList[i] - 1].cardName, cardDetails[tempAvailList[i] - 1].cardDescription, cardDetails[tempAvailList[i] - 1].attack, cardDetails[tempAvailList[i] - 1].HP, cardDetails[tempAvailList[i] - 1].gold, cardDetails[tempAvailList[i] - 1].XP, cardDetails[tempAvailList[i] - 1].fieldLimit, cardDetails[tempAvailList[i] - 1].clan, cardDetails[tempAvailList[i] - 1].levelRequired, cardDetails[tempAvailList[i] - 1].cardImage, cardDetails[tempAvailList[i] - 1].cardFrame, cardDetails[tempAvailList[i] - 1].cardClass, cardDetails[tempAvailList[i] - 1].ability
                    //, cardDetails[tempAvailList[i] - 1].requirements, cardDetails[tempAvailList[i] - 1].abilityLevel
                    );
                cardInstance.name = cardDetails[tempAvailList[i] - 1].cardName;
                cardInstance.frame.GetComponent<Image>().color = disableColor;
                cardInstance.image.GetComponent<Image>().color = disableColor;
                availableCards.Add(cardInstance);
            }
            else if (cardDetails.Where(card => card.id == tempAvailList[i] && (int)card.cardClass == generalsIndex).Count() == 1)
            {
                Card cardInstance = Instantiate<Card>(cardPrefabs, availableListOfCard.transform);
                cardInstance.SetProperties(cardDetails[tempAvailList[i] - 1].id, cardDetails[tempAvailList[i] - 1].ergoTokenId, cardDetails[tempAvailList[i] - 1].ergoTokenAmount, cardDetails[tempAvailList[i] - 1].cardName, cardDetails[tempAvailList[i] - 1].cardDescription, cardDetails[tempAvailList[i] - 1].attack, cardDetails[tempAvailList[i] - 1].HP, cardDetails[tempAvailList[i] - 1].gold, cardDetails[tempAvailList[i] - 1].XP, cardDetails[tempAvailList[i] - 1].fieldLimit, cardDetails[tempAvailList[i] - 1].clan, cardDetails[tempAvailList[i] - 1].levelRequired, cardDetails[tempAvailList[i] - 1].cardImage, cardDetails[tempAvailList[i] - 1].cardFrame, cardDetails[tempAvailList[i] - 1].cardClass, cardDetails[tempAvailList[i] - 1].ability
                    //, cardDetails[tempAvailList[i] - 1].requirements, cardDetails[tempAvailList[i] - 1].abilityLevel
                    );
                cardInstance.name = cardDetails[tempAvailList[i] - 1].cardName;
                availableCards.Add(cardInstance);
            }

        }

        for (int i = 0; i < tempCurrList.Count; i++)
        {
            if (cardDetails.Where(card => card.id == tempCurrList[i]).Count() == 1)
            {
                if (cardDetails[tempCurrList[i] - 1].ergoTokenId != "" && ErgoQuery.instance.tokenIDs.Contains(cardDetails[tempCurrList[i] - 1].ergoTokenId))
                {
                    Card cardInstance = Instantiate<Card>(cardPrefabs, currentListOfCard.transform);
                    cardInstance.SetProperties(cardDetails[tempCurrList[i] - 1].id, cardDetails[tempCurrList[i] - 1].ergoTokenId, cardDetails[tempCurrList[i] - 1].ergoTokenAmount, cardDetails[tempCurrList[i] - 1].cardName, cardDetails[tempCurrList[i] - 1].cardDescription, cardDetails[tempCurrList[i] - 1].attack, cardDetails[tempCurrList[i] - 1].HP, cardDetails[tempCurrList[i] - 1].gold, cardDetails[tempCurrList[i] - 1].XP, cardDetails[tempCurrList[i] - 1].fieldLimit, cardDetails[tempCurrList[i] - 1].clan, cardDetails[tempCurrList[i] - 1].levelRequired, cardDetails[tempCurrList[i] - 1].cardImage, cardDetails[tempCurrList[i] - 1].cardFrame, cardDetails[tempCurrList[i] - 1].cardClass, cardDetails[tempCurrList[i] - 1].ability
                        //, cardDetails[tempCurrList[i] - 1].requirements, cardDetails[tempCurrList[i] - 1].abilityLevel
                        );
                    cardInstance.name = cardDetails[tempCurrList[i] - 1].cardName;
                    currentCards.Add(cardInstance);
                }
                else if (cardDetails[tempCurrList[i] - 1].ergoTokenId != "" && !ErgoQuery.instance.tokenIDs.Contains(cardDetails[tempCurrList[i] - 1].ergoTokenId)) //this removes the card from their current list if they no longer own the token
                {
                    Card cardInstance = Instantiate<Card>(cardPrefabs, availableListOfCard.transform);
                    cardInstance.SetProperties(cardDetails[tempCurrList[i] - 1].id, cardDetails[tempCurrList[i] - 1].ergoTokenId, cardDetails[tempCurrList[i] - 1].ergoTokenAmount, cardDetails[tempCurrList[i] - 1].cardName, cardDetails[tempCurrList[i] - 1].cardDescription, cardDetails[tempCurrList[i] - 1].attack, cardDetails[tempCurrList[i] - 1].HP, cardDetails[tempCurrList[i] - 1].gold, cardDetails[tempCurrList[i] - 1].XP, cardDetails[tempCurrList[i] - 1].fieldLimit, cardDetails[tempCurrList[i] - 1].clan, cardDetails[tempCurrList[i] - 1].levelRequired, cardDetails[tempCurrList[i] - 1].cardImage, cardDetails[tempCurrList[i] - 1].cardFrame, cardDetails[tempCurrList[i] - 1].cardClass, cardDetails[tempCurrList[i] - 1].ability
                        //, cardDetails[tempCurrList[i] - 1].requirements, cardDetails[tempCurrList[i] - 1].abilityLevel
                        );
                    cardInstance.name = cardDetails[tempCurrList[i] - 1].cardName;
                    cardInstance.frame.GetComponent<Image>().color = disableColor;
                    cardInstance.image.GetComponent<Image>().color = disableColor;
                    availableCards.Add(cardInstance);
                }
                else if (cardDetails[tempCurrList[i] - 1].ergoTokenId == "")
                {
                    Card cardInstance = Instantiate<Card>(cardPrefabs, currentListOfCard.transform);
                    cardInstance.SetProperties(cardDetails[tempCurrList[i] - 1].id, cardDetails[tempCurrList[i] - 1].ergoTokenId, cardDetails[tempCurrList[i] - 1].ergoTokenAmount, cardDetails[tempCurrList[i] - 1].cardName, cardDetails[tempCurrList[i] - 1].cardDescription, cardDetails[tempCurrList[i] - 1].attack, cardDetails[tempCurrList[i] - 1].HP, cardDetails[tempCurrList[i] - 1].gold, cardDetails[tempCurrList[i] - 1].XP, cardDetails[tempCurrList[i] - 1].fieldLimit, cardDetails[tempCurrList[i] - 1].clan, cardDetails[tempCurrList[i] - 1].levelRequired, cardDetails[tempCurrList[i] - 1].cardImage, cardDetails[tempCurrList[i] - 1].cardFrame, cardDetails[tempCurrList[i] - 1].cardClass, cardDetails[tempCurrList[i] - 1].ability
                        //, cardDetails[tempCurrList[i] - 1].requirements, cardDetails[tempCurrList[i] - 1].abilityLevel
                        );
                    cardInstance.name = cardDetails[tempCurrList[i] - 1].cardName;
                    currentCards.Add(cardInstance);
                }
            }
        }

        if (ErgoQuery.instance.hideCardsAvailableStore[deckId - 1] == 1)
        {
            cardSorting.hideUnmatchedButton.SetActive(true);
            cardSorting.HideUnmatchedCardsInvoke();
        }

        if (ErgoQuery.instance.hideGameboardsStore[deckId - 1] == 1)
        {
            cardSorting.hideUnmatchedButtonGameboard.SetActive(true);
            cardSorting.HideUnmatchedGameboardsInvoke();
        }

        CurrentFilterCheck(ErgoQuery.instance.filterCardsCurrentStore[deckId - 1]);
        AvailableFilterCheck(ErgoQuery.instance.filterCardsAvailableStore[deckId - 1]);
        GameboardFilterCheck(ErgoQuery.instance.filterGameboardsStore[deckId - 1]);
        GameboardSelectedLoad(ErgoQuery.instance.gameboardCurrentStore[deckId - 1]);
        AvailableSelectedLoad();
        CurrentSelectedLoad();

        addDeckObject.SetActive(false);
        editDeckObject.SetActive(true);
        search.GetComponent<TMP_InputField>().text = "";
        loading.SetActive(false);
    }


    public void SaveDeck() // Save the deck
    {
        currentCards.Clear(); //mdb
        availableCards.Clear();
        search.GetComponent<TMP_InputField>().text = "";
        searchGameboard.GetComponent<TMP_InputField>().text = "";
        cardSorting.HideUnmatchedCardsSaveDeck();

        if (currentListOfCard.GetComponentsInChildren<Card>().Length > 0)
        {
            for (int i = 0; i < currentListOfCard.GetComponentsInChildren<Card>().Length; i++)
            {
                currentCards.Add(currentListOfCard.GetComponentsInChildren<Card>()[i]);
            }
        }

        if (availableListOfCard.GetComponentsInChildren<Card>().Length > 0)
        {
            for (int i = 0; i < availableListOfCard.GetComponentsInChildren<Card>().Length; i++)
            {
                availableCards.Add(availableListOfCard.GetComponentsInChildren<Card>()[i]);
            }
        }

        for (int i = 0; i < currentListOfCard.transform.childCount; i++)
        {
            int levelText = (int)currentCards[i].levelRequired;
            //int levelInt = int.Parse(levelText.Replace("Starter", "1").Replace("Lower", "2").Replace("Middle", "3").Replace("Upper", "4"));
            if (levelText == 0)
            {
                countOfLevelOne++;
            }
        }

        if (countOfLevelOne < 2)
        {
            ActiveTooltip("You need at least two Starter cards");
            Invoke("RemoveToolTip", 2f);
        }
        else
        {
            int[] currentCardindex = new int[currentListOfCard.transform.childCount];
            int[] availabeCardindex = new int[availableListOfCard.transform.childCount];

            for (int i = 0; i < currentListOfCard.transform.childCount; i++)
            {
                currentCardindex[i] = currentCards[i].id;
            }
            for (int i = 0; i < availableListOfCard.transform.childCount; i++)
            {
                availabeCardindex[i] = availableCards[i].id;
            }


            if (isAdd)
            {
                DeckCount();
                SaveCurrentData(currentCardindex, filterCurrent, selectedGameboardNameLoad.text, "generalPfpCurrentPlaceholder", deckCount + 1, generalsIndex);
                SaveAvailableDataAdd(availabeCardindex, hideAvailableCards, filterAvailable, hideAvailableGameboards, filterGameboard, 0, "generalPfpAvailablePlaceholder");
            }
            else if (isEdit)
            {
                SaveCurrentData(currentCardindex, filterCurrent, selectedGameboardNameLoad.text, "generalPfpCurrentPlaceholder", deckId, generalsIndex);
                SaveAvailableData(availabeCardindex, hideAvailableCards, filterAvailable, hideAvailableGameboards, filterGameboard, 0, "generalPfpAvailablePlaceholder");
            }

            editDeckObject.SetActive(false);
            addDeckObject.SetActive(true);
            deckNotes.SetActive(false);
            deckNotesGraphic.SetActive(true);
            isAdd = false;
            isEdit = false;
            deckProfile.enabled = false;
            DisplayDeck();
            cursorManager.CursorNormal();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        countOfLevelOne = 0;
    }

    public void SaveCurrentData(int[] currentCardindex, string filterCurrent, string selectedGameboardNameLoad, string generalPfpCurrentPlaceholder, int deckId, int generalsIndex)
    {
        ErgoQuery.instance.cardIdCurrentStore[deckId - 1] = currentCardindex;
        ErgoQuery.instance.deckGeneralStore[deckId - 1] = generalsIndex;
        ErgoQuery.instance.deckIdStore[deckId - 1] = deckId;
        ErgoQuery.instance.filterCardsCurrentStore[deckId - 1] = filterCurrent;
        ErgoQuery.instance.gameboardCurrentStore[deckId - 1] = selectedGameboardNameLoad;
        ErgoQuery.instance.generalPfpCurrentStore[deckId - 1] = generalPfpCurrentPlaceholder;
    }

    public void SaveAvailableData(int[] availabeCardindex, int hideAvailableCards, string filterAvailable, int hideAvailableGameboards, string filterGameboard, int hideGeneralPfpStore, string filterGeneralPfp)
    {
        ErgoQuery.instance.cardIdAvailableStore[deckId - 1] = availabeCardindex;
        ErgoQuery.instance.hideCardsAvailableStore[deckId - 1] = hideAvailableCards;
        ErgoQuery.instance.filterCardsAvailableStore[deckId - 1] = filterAvailable;
        ErgoQuery.instance.hideGameboardsStore[deckId - 1] = hideAvailableGameboards;
        ErgoQuery.instance.filterGameboardsStore[deckId - 1] = filterGameboard;
        ErgoQuery.instance.hideGeneralPfpStore[deckId - 1] = hideGeneralPfpStore;
        ErgoQuery.instance.filterGeneralPfpStore[deckId - 1] = filterGeneralPfp;
    }
    public void SaveAvailableDataAdd(int[] availabeCardindex, int hideAvailableCards, string filterAvailable, int hideAvailableGameboards, string filterGameboard, int hideGeneralPfpStore, string filterGeneralPfp)
    {
        ErgoQuery.instance.cardIdAvailableStore[deckCount] = availabeCardindex;
        ErgoQuery.instance.hideCardsAvailableStore[deckCount] = hideAvailableCards;
        ErgoQuery.instance.filterCardsAvailableStore[deckCount] = filterAvailable;
        ErgoQuery.instance.hideGameboardsStore[deckCount] = hideAvailableGameboards;
        ErgoQuery.instance.filterGameboardsStore[deckCount] = filterGameboard;
        ErgoQuery.instance.hideGeneralPfpStore[deckCount] = hideGeneralPfpStore;
        ErgoQuery.instance.filterGeneralPfpStore[deckCount] = filterGeneralPfp;
    }

    public void SaveCurrentDataDelete(int[] currentCardindex, string filterCurrent, string selectedGameboardNameLoad, string generalPfpCurrentPlaceholder, int deckIdMove, int generalsIndex, string deckTitle, string deckBody, int arrayMovement)
    {
        Debug.Log(arrayMovement);
        Debug.Log(deckId - 1 + arrayMovement);

        ErgoQuery.instance.cardIdCurrentStore[(deckId - 1 + arrayMovement)] = currentCardindex;
        ErgoQuery.instance.deckGeneralStore[(deckId - 1 + arrayMovement)] = generalsIndex;
        ErgoQuery.instance.deckIdStore[(deckId - 1 + arrayMovement)] = deckIdMove - 1;
        ErgoQuery.instance.filterCardsCurrentStore[(deckId - 1 + arrayMovement)] = filterCurrent;
        ErgoQuery.instance.gameboardCurrentStore[(deckId - 1 + arrayMovement)] = selectedGameboardNameLoad;
        ErgoQuery.instance.generalPfpCurrentStore[(deckId - 1 + arrayMovement)] = generalPfpCurrentPlaceholder;
        ErgoQuery.instance.deckTitleStore[(deckId - 1 + arrayMovement)] = deckTitle;
        ErgoQuery.instance.deckBodyStore[(deckId - 1 + arrayMovement)] = deckBody;
    }

    public void SaveAvailableDataDelete(int[] availabeCardindex, int hideAvailableCards, string filterAvailable, int hideAvailableGameboards, string filterGameboard, int hideGeneralPfpStore, string filterGeneralPfp, int arrayMovement)
    {
        ErgoQuery.instance.cardIdAvailableStore[(deckId - 1 + arrayMovement)] = availabeCardindex;
        ErgoQuery.instance.hideCardsAvailableStore[(deckId - 1 + arrayMovement)] = hideAvailableCards;
        ErgoQuery.instance.filterCardsAvailableStore[(deckId - 1 + arrayMovement)] = filterAvailable;
        ErgoQuery.instance.hideGameboardsStore[(deckId - 1 + arrayMovement)] = hideAvailableGameboards;
        ErgoQuery.instance.filterGameboardsStore[(deckId - 1 + arrayMovement)] = filterGameboard;
        ErgoQuery.instance.hideGeneralPfpStore[(deckId - 1 + arrayMovement)] = hideGeneralPfpStore;
        ErgoQuery.instance.filterGeneralPfpStore[(deckId - 1 + arrayMovement)] = filterGeneralPfp;
    }

    public void StoreNotes(string deckTitle, string deckBody, int deckId)
    {
        if (deckId >= 0)
        {
            ErgoQuery.instance.deckTitleStore[deckId - 1] = deckTitle;
            ErgoQuery.instance.deckBodyStore[deckId - 1] = deckBody;
        }
    }

    public void DeleteDeck()
    {
        if (!deleteDeckPopup.activeSelf && PlayerPrefs.GetString("DisableDeleteDeckWarning") == "F")
        {
            deleteDeckPopup.SetActive(true);
        }
        else if (PlayerPrefs.GetString("DisableDeleteDeckWarning") == "T")
        {
            DeleteDeckConfirm();
        }
        else
        {
            deleteDeckPopup.SetActive(true);
        }
    }

    public void DeleteDeckBack()
    {
        if (deleteDeckPopup.activeSelf)
        {
            deleteDeckPopup.SetActive(false);
            cursorManager.CursorNormal();
        }
    }

    public void DeleteDeckDontAsk()
    {
        if (!deleteDeckDontAsk.activeSelf)
        {
            deleteDeckDontAsk.SetActive(true);
            PlayerPrefs.SetString("DisableDeleteDeckWarning", "T");
            PlayerPrefs.Save();
        }
        else if (deleteDeckDontAsk.activeSelf)
        {
            deleteDeckDontAsk.SetActive(false);
            PlayerPrefs.SetString("DisableDeleteDeckWarning", "F");
            PlayerPrefs.Save();
        }
    }

    public void DeleteDeckConfirm()
    {
        availableCards.Clear();
        currentCards.Clear();
        int[] arrayEmpty = new int[] { };
        int arrayPos = deckId - 1;
        int deckDifference = deckCount - deckId;
        int arrayMovement = 0;

        if (deckCount == deckId)
        {
            SaveCurrentDataDelete(arrayEmpty, "", "Purple Moon", "generalPfpCurrentPlaceholder", deckCount + 1, 0, "", "", arrayMovement);
            SaveAvailableDataDelete(arrayEmpty, 0, "", 0, "", 0, "generalPfpAvailablePlaceholder", arrayMovement);
        }
        else if (deckCount != deckId)
        {
            for (int n = 1; n <= deckDifference; n++)
            {
                Debug.Log(arrayPos + n);


                SaveCurrentDataDelete(ErgoQuery.instance.cardIdCurrentStore[arrayPos + n], ErgoQuery.instance.filterCardsCurrentStore[arrayPos + n], ErgoQuery.instance.gameboardCurrentStore[arrayPos + n], ErgoQuery.instance.generalPfpCurrentStore[arrayPos + n], ErgoQuery.instance.deckIdStore[arrayPos + n],
                    ErgoQuery.instance.deckGeneralStore[arrayPos + n], ErgoQuery.instance.deckTitleStore[arrayPos + n], ErgoQuery.instance.deckBodyStore[arrayPos + n], arrayMovement);

                SaveAvailableDataDelete(ErgoQuery.instance.cardIdAvailableStore[arrayPos + n], ErgoQuery.instance.hideCardsAvailableStore[arrayPos + n], ErgoQuery.instance.filterCardsAvailableStore[arrayPos + n], ErgoQuery.instance.hideGameboardsStore[arrayPos + n],
                    ErgoQuery.instance.filterGameboardsStore[arrayPos + n], ErgoQuery.instance.hideGeneralPfpStore[arrayPos + n], ErgoQuery.instance.filterGeneralPfpStore[arrayPos + n], arrayMovement);

                if (n == deckDifference)
                {
                    Debug.Log("hello");
                    arrayMovement++;
                    SaveCurrentDataDelete(arrayEmpty, "", "Purple Moon", "generalPfpCurrentPlaceholder", deckCount + 1, 0, "", "", arrayMovement);
                    SaveAvailableDataDelete(arrayEmpty, 0, "", 0, "", 0, "generalPfpAvailablePlaceholder", arrayMovement);
                }

                arrayMovement++;
            }
        }

        isEdit = false;
        isAdd = false;
        cursorManager.CursorNormal();
        deleteDeckPopup.SetActive(false);
        editDeckObject.SetActive(false);
        addDeckObject.SetActive(true);
        deckNotes.SetActive(false);
        deckNotesGraphic.SetActive(true);
        search.GetComponent<TMP_InputField>().text = "";
        availableCards.Clear();
        currentCards.Clear();
        DisplayDeck();
        cursorManager.CursorNormal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickBack()
    {
        cursorManager.CursorNormal();
        if (deckId >= 1)
        {
            StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId);
        }
        StartCoroutine(PushDeckDataToFirebase());
    }

    public IEnumerator PushDeckDataToFirebase()
    {
        if (FirebaseManager.instance.user != null)
        {
            FirebaseUser user = FirebaseManager.instance.user;

            for (int i = 1; i <= 5; i++)
            {
                if (i <= deckCount)
                {
                    FirebaseManager.DeckCurrent currentDataSave = new FirebaseManager.DeckCurrent(string.Join(",", ErgoQuery.instance.cardIdCurrentStore[i - 1]), ErgoQuery.instance.filterCardsCurrentStore[i - 1], ErgoQuery.instance.gameboardCurrentStore[i - 1], ErgoQuery.instance.generalPfpCurrentStore[i - 1],
                        ErgoQuery.instance.deckTitleStore[i - 1], ErgoQuery.instance.deckBodyStore[i - 1], ErgoQuery.instance.deckIdStore[i - 1], ErgoQuery.instance.deckGeneralStore[i - 1]);

                    string jsonStringCurrent = Newtonsoft.Json.JsonConvert.SerializeObject(currentDataSave);

                    FirebaseManager.DeckAvailable availableDataSave = new FirebaseManager.DeckAvailable(string.Join(",", ErgoQuery.instance.cardIdAvailableStore[i - 1]), ErgoQuery.instance.hideCardsAvailableStore[i - 1], ErgoQuery.instance.filterCardsAvailableStore[i - 1], ErgoQuery.instance.hideGameboardsStore[i - 1],
                        ErgoQuery.instance.filterGameboardsStore[i - 1], ErgoQuery.instance.hideGeneralPfpStore[i - 1], ErgoQuery.instance.filterGeneralPfpStore[i - 1]);

                    string jsonStringAvailable = Newtonsoft.Json.JsonConvert.SerializeObject(availableDataSave);

                    var createCurrentDeck = FirebaseDatabase.DefaultInstance.RootReference.Child("decks").Child(user.UserId).Child("CurrentCards" + i).SetRawJsonValueAsync(jsonStringCurrent);
                    yield return new WaitUntil(predicate: () => createCurrentDeck.IsCompleted);
                    if (createCurrentDeck.IsFaulted)
                    {
                        Debug.LogError("Current deck push failure " + i); ;
                    }
                    else if (createCurrentDeck.IsCompleted)
                    {
                        Debug.Log("Current deck push success " + i);
                    }

                    var createAvailableDeck = FirebaseDatabase.DefaultInstance.RootReference.Child("decks").Child(user.UserId).Child("AvailableCards" + i).SetRawJsonValueAsync(jsonStringAvailable);
                    yield return new WaitUntil(predicate: () => createAvailableDeck.IsCompleted);
                    if (createAvailableDeck.IsFaulted)
                    {
                        Debug.LogError("Available deck push failure " + i); ;
                    }
                    else if (createAvailableDeck.IsCompleted)
                    {
                        Debug.Log("Available deck push success " + i);
                    }
                }
                else if (i > deckCount)
                {
                    FirebaseManager.DeckCurrent deckCurrent = new FirebaseManager.DeckCurrent("", "", "Purple Moon", "generalPfpCurrentPlaceholder", "", "", i, 0);
                    string jsonStringCurrent = Newtonsoft.Json.JsonConvert.SerializeObject(deckCurrent);

                    FirebaseManager.DeckAvailable deckAvailable = new FirebaseManager.DeckAvailable("", 0, "", 0, "", 0, "");
                    string jsonStringAvailable = Newtonsoft.Json.JsonConvert.SerializeObject(deckAvailable);

                    var createCurrentDeck = FirebaseDatabase.DefaultInstance.RootReference.Child("decks").Child(user.UserId).Child("CurrentCards" + i).SetRawJsonValueAsync(jsonStringCurrent);
                    yield return new WaitUntil(predicate: () => createCurrentDeck.IsCompleted);
                    if (createCurrentDeck.IsFaulted)
                    {
                        Debug.LogError("Current deck push failure " + i); ;
                    }
                    else if (createCurrentDeck.IsCompleted)
                    {
                        Debug.Log("Current deck push success " + i);
                    }

                    var createAvailableDeck = FirebaseDatabase.DefaultInstance.RootReference.Child("decks").Child(user.UserId).Child("AvailableCards" + i).SetRawJsonValueAsync(jsonStringAvailable);
                    yield return new WaitUntil(predicate: () => createAvailableDeck.IsCompleted);
                    if (createAvailableDeck.IsFaulted)
                    {
                        Debug.LogError("Available deck push failure " + i); ;
                    }
                    else if (createAvailableDeck.IsCompleted)
                    {
                        Debug.Log("Available deck push success " + i);
                    }
                }
            }

        }
        else
        {
            Debug.Log("Unable to find Firebase.instance.user");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync(1);
    }

    public void OnEditBack() // Back from edit deck screen to add deck builder
    {
        cursorManager.AudioClickButtonStandard();
        editDeckObject.SetActive(false);
        addDeckObject.SetActive(true);
        deckNotes.SetActive(false);
        deckNotesGraphic.SetActive(true);
        isEdit = false;
        isAdd = false;
        deckProfile.enabled = false;
        search.GetComponent<TMP_InputField>().text = "";
        availableCards.Clear();
        currentCards.Clear();
        cursorManager.CursorNormal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Init() // Initilize when user click on new deck
    {
        cursorManager.CursorNormal();
        matchedCardListForAvailable.Clear();
        unmatchedCardListForAvailable.Clear();
        availableCards.Clear();
        currentCards.Clear();

        for (int i = 0; i < cardDetails.Count; i++)
        {

            if (cardDetails[i].ergoTokenId == "")
            {
                if ((int)cardDetails[i].cardClass == generalsIndex || (int)cardDetails[i].cardClass == 0) //checks that the card matches the General of the deck
                {
                    matchedCardListForAvailable.Add(cardDetails[i]);
                }
                else if ((int)cardDetails[i].cardClass != generalsIndex && (int)cardDetails[i].cardClass > 0) //if it doesn't, then it's unavailable
                {
                    unmatchedCardListForAvailable.Add(cardDetails[i]);
                }
            }
            if (cardDetails[i].ergoTokenId != "")
            {
                if (ErgoQuery.instance.tokenIDs.Contains(cardDetails[i].ergoTokenId) && (int)cardDetails[i].cardClass == generalsIndex) //checks that the card matches the General of the deck
                {
                    matchedCardListForAvailable.Add(cardDetails[i]);
                }
                else if (!ErgoQuery.instance.tokenIDs.Contains(cardDetails[i].ergoTokenId) || (int)cardDetails[i].cardClass != generalsIndex) //if it doesn't, then it's unavailable
                {
                    unmatchedCardListForAvailable.Add(cardDetails[i]);
                }
            }
        }

        for (int i = 0; i < matchedCardListForAvailable.Count; i++)
        {
            Card cardInstance = Instantiate<Card>(cardPrefabs, availableListOfCard.transform);
            cardInstance.SetProperties(matchedCardListForAvailable[i].id, matchedCardListForAvailable[i].ergoTokenId, matchedCardListForAvailable[i].ergoTokenAmount, matchedCardListForAvailable[i].cardName, matchedCardListForAvailable[i].cardDescription, matchedCardListForAvailable[i].attack, matchedCardListForAvailable[i].HP, matchedCardListForAvailable[i].gold, matchedCardListForAvailable[i].XP, matchedCardListForAvailable[i].fieldLimit, matchedCardListForAvailable[i].clan, matchedCardListForAvailable[i].levelRequired, matchedCardListForAvailable[i].cardImage, matchedCardListForAvailable[i].cardFrame, matchedCardListForAvailable[i].cardClass, matchedCardListForAvailable[i].ability
                //, matchedCardListForAvailable[i].requirements, matchedCardListForAvailable[i].abilityLevel
                );
            cardInstance.name = matchedCardListForAvailable[i].cardName;
            availableCards.Add(cardInstance);
        }

        for (int i = 0; i < unmatchedCardListForAvailable.Count; i++)
        {
            Card cardInstance = Instantiate<Card>(cardPrefabs, availableListOfCard.transform);
            cardInstance.SetProperties(unmatchedCardListForAvailable[i].id, unmatchedCardListForAvailable[i].ergoTokenId, unmatchedCardListForAvailable[i].ergoTokenAmount, unmatchedCardListForAvailable[i].cardName, unmatchedCardListForAvailable[i].cardDescription, unmatchedCardListForAvailable[i].attack, unmatchedCardListForAvailable[i].HP, unmatchedCardListForAvailable[i].gold, unmatchedCardListForAvailable[i].XP, unmatchedCardListForAvailable[i].fieldLimit, unmatchedCardListForAvailable[i].clan, unmatchedCardListForAvailable[i].levelRequired, unmatchedCardListForAvailable[i].cardImage, unmatchedCardListForAvailable[i].cardFrame, unmatchedCardListForAvailable[i].cardClass, unmatchedCardListForAvailable[i].ability
                //, unmatchedCardListForAvailable[i].requirements, unmatchedCardListForAvailable[i].abilityLevel
                );
            cardInstance.name = unmatchedCardListForAvailable[i].cardName;
            cardInstance.frame.GetComponent<Image>().color = disableColor;
            cardInstance.image.GetComponent<Image>().color = disableColor;
            availableCards.Add(cardInstance);
        }

        AvailableSelectedLoad();
        search.GetComponent<TMP_InputField>().text = "";
    }


    public void LiveSearch() // Live search, user can search any position in card name
    {
        searchText = search.GetComponent<TMP_InputField>().text.ToLower();

        availableCardsSortTransformSearch = availableCardsSortSearch.transform;
        int j = availableCardsSortTransformSearch.childCount;
        availableCardsSortArrayAllSearch = new GameObject[j];

        for (int i = 0; j > i; i++) //saving cards into an array
        {
            availableCardsSortArrayAllSearch[i] = availableCardsSortTransformSearch.GetChild(i).gameObject;

            if (!availableCardsSortArrayAllSearch[i].name.ToString().ToLower().Contains(searchText))
            {
                availableCardsSortArrayAllSearch[i].SetActive(false);

            }
            else if (availableCardsSortArrayAllSearch[i].name.ToString().ToLower().Contains(searchText))
            {
                availableCardsSortArrayAllSearch[i].SetActive(true);
            }
        }

        cardSorting.HideUnmatchedCardsInvoke();
    }

    public void LiveSearchGameboards()
    {
        searchTextGameboard = searchGameboard.GetComponent<TMP_InputField>().text.ToLower();

        gameboardSortTransformSearch = gameboardSortSearch.transform;
        int j = gameboardSortTransformSearch.childCount;
        gameboardSortArrayAllSearch = new GameObject[j];

        for (int i = 0; j > i; i++) //saving cards into an array
        {
            gameboardSortArrayAllSearch[i] = gameboardSortTransformSearch.GetChild(i).gameObject;

            if (!gameboardSortArrayAllSearch[i].name.ToString().ToLower().Contains(searchTextGameboard))
            {
                gameboardSortArrayAllSearch[i].SetActive(false);

            }
            else if (gameboardSortArrayAllSearch[i].name.ToString().ToLower().Contains(searchTextGameboard))
            {
                gameboardSortArrayAllSearch[i].SetActive(true);
            }
        }

        cardSorting.HideUnmatchedGameboardsInvoke();
    }

    public void RemoveToolTip() // remove tooltip after some time 
    {
        toolTip.SetActive(false);
    }

    public void ActiveTooltip(string message)
    {
        toolTip.SetActive(true);
        Transform toolTipText = toolTip.transform.Find("Info TMP");
        toolTipText.GetComponent<TMP_Text>().text = message;
    }

    private void DestroyCardList() // Destroy the cards in available and current card
    {
        foreach (Transform child in currentListOfCard.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in availableListOfCard.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void DisplayDeck() // Display deck in deck builder screen
    {

        if (deckCount == 5)
        {
            addDeckButtonEnabled.SetActive(false);
            addDeckButtonDisabled.SetActive(true);
        }
        else
        {
            addDeckButtonEnabled.SetActive(true);
            addDeckButtonDisabled.SetActive(false);
        }

        if (deckCount > 0)
        {
            for (int i = 0; i < deckCount; i++)
            {

                images[i].enabled = true;
            }
        }
        for (int i = deckCount; i < deckCount; i++)
        {
            images[i].enabled = false;
        }
    }
    private void ClearCards() // Clear the list of cards (List<Card>)
    {
        availableCards.Clear();
        currentCards.Clear();
        unmatchedCardListForAvailable.Clear();
        matchedCardListForAvailable.Clear();
    }
    public void RemoveToolTipDoubleClick() // Remove tooltip some specific time
    {
        toolTip.SetActive(false);
    }

    public void ActiveToolTipDoubleClick(string message)
    {
        toolTip.SetActive(true);
        Transform toolTipText = toolTip.transform.Find("Info TMP");
        toolTipText.GetComponent<TMP_Text>().text = message;
    }

    public void CurrentFilterCheck(string filter)
    {
        if (filter == "currentABC")
        {
            filterCurrent = filter;
            cardSorting.currentNameBool = true;
            cardSorting.currentABCselected.SetActive(true);
        }
        else if (filter == "currentCBA")
        {
            filterCurrent = filter;
            cardSorting.currentNameBool = false;
            cardSorting.currentCBAselected.SetActive(true);
        }
        else if (filter == "current123")
        {
            filterCurrent = filter;
            cardSorting.currentNameLevelBool = true;
            cardSorting.current123selected.SetActive(true);
        }
        else if (filter == "current321")
        {
            filterCurrent = filter;
            cardSorting.currentNameLevelBool = false;
            cardSorting.current321selected.SetActive(true);
        }
    }
    public void AvailableFilterCheck(string filter)
    {
        if (filter == "availableABC")
        {
            filterAvailable = filter;
            cardSorting.availableNameBool = true;
            cardSorting.availableABCselected.SetActive(true);
            Debug.Log(filter);
        }
        else if (filter == "availableCBA")
        {
            filterAvailable = filter;
            cardSorting.availableNameBool = false;
            cardSorting.availableCBAselected.SetActive(true);
            Debug.Log(filter);
        }
        else if (filter == "available123")
        {
            filterAvailable = filter;
            cardSorting.availableNameLevelBool = true;
            cardSorting.available123selected.SetActive(true);
            Debug.Log(filter);
        }
        else if (filter == "available321")
        {
            filterAvailable = filter;
            cardSorting.availableNameLevelBool = false;
            cardSorting.available321selected.SetActive(true);
            Debug.Log(filter);
        }
    }

    public void GameboardFilterCheck(string filter)
    {
        if (filter == "gameboardsABC")
        {
            filterGameboard = filter;
            cardSorting.gameboardsNameBool = true;
            cardSorting.gameboardABCselected.SetActive(true);
            cardSorting.SortChildrenByNameGameboards();
        }
        else if (filter == "gameboardsCBA")
        {
            filterGameboard = filter;
            cardSorting.gameboardsNameBool = false;
            cardSorting.gameboardCBAselected.SetActive(true);
            cardSorting.SortChildrenByNameGameboards();
        }
    }

    public void GameboardSelectedLoad(string gameBoard)
    {
        if (gameBoard != null)
        {
            selectedGameboardNameLoad.text = gameBoard;
        }

        selectedGameboardTransformLoad = selectedGameboardLoad.transform;
        int j = selectedGameboardTransformLoad.childCount;
        selectedGameboardArrayLoad = new GameObject[j];
        selectedGameboardImagesLoad = new Image[j];

        for (int i = 0; j > i; i++)
        {
            selectedGameboardArrayLoad[i] = selectedGameboardTransformLoad.GetChild(i).gameObject;
            selectedGameboardImagesLoad[i] = selectedGameboardArrayLoad[i].transform.GetComponentInChildren<Image>();

            if (selectedGameboardArrayLoad[i].name == gameBoard)
            {
                selectedGameboardImageLoad.sprite = selectedGameboardImagesLoad[i].sprite;
            }
        }
    }

    public void CurrentSelectedLoad()
    {
        if (filterCurrent == "currentABC" || filterCurrent == "currentCBA")
        {
            cardSorting.SortChildrenByNameCurrent();
        }
        else if (filterCurrent == "current123" || filterCurrent == "current321")
        {
            cardSorting.SortChildrenByLevelCurrent();
        }
    }
    public void AvailableSelectedLoad()
    {
        if (filterAvailable == "availableABC" || filterAvailable == "availableCBA")
        {
            cardSorting.SortChildrenByNameAvailable();
        }
        else if (filterAvailable == "available123" || filterAvailable == "available321")
        {
            cardSorting.SortChildrenByLevelAvailable();
        }
        else if (filterAvailable == "")
        {
            cardSorting.SortChildrenAvailableDefault();
        }
    }
    public void uiSelectCards()
    {
        cursorManager.CursorNormal();
        availableListOfCard.SetActive(true);
        currentListOfCard.SetActive(true);
        cardButtonUIon.SetActive(true);
        cardButtonUIoff.SetActive(false);
        gameboardButtonUIon.SetActive(false);
        gameboardButtonUIoff.SetActive(true);
        generalButtonUIon.SetActive(false);
        generalButtonUIoff.SetActive(true);

        cardSorting.gameboardSortPopup.SetActive(false);
        cardHideParent.SetActive(true);
        gameboardHideParent.SetActive(false);
        gameboardUI.SetActive(false);
        cardSorting.currentCardsUI.SetActive(true);

    }
    public void uiSelectGameboards()
    {
        if (filterGameboard == "") //sorts the gameboards to put unmatched last, matched first, and default is abc order
        {
            cardSorting.SortChildrenByNameGameboardsDefault();
        }

        availableListOfCard.SetActive(false);
        currentListOfCard.SetActive(false);

        cursorManager.CursorNormal();
        cardHideParent.SetActive(false);
        gameboardHideParent.SetActive(true);
        cardButtonUIon.SetActive(false);
        cardButtonUIoff.SetActive(true);
        gameboardButtonUIon.SetActive(true);
        gameboardButtonUIoff.SetActive(false);
        generalButtonUIon.SetActive(false);
        generalButtonUIoff.SetActive(true);

        gameboardUI.SetActive(true);
        cardSorting.currentCardsUI.SetActive(false);
        cardSorting.currentCardsSortPopup.SetActive(false);
        cardSorting.availableCardsSortPopup.SetActive(false);
    }

    public void DetermineGameboardUnlocks()
    {
        if (masqueradesGamesPlayed >= 10)
        {
            masquerdesGameboard.color = new Vector4(1f, 1f, 1f, 1f);
            masquerdesGameboardName.color = new Vector4(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
        if (theOldKingdomGamesPlayed >= 10)
        {
            theOldKingdomGameboard.color = new Vector4(1f, 1f, 1f, 1f);
            theOldKingdomGameboardName.color = new Vector4(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
        if (fairytalesGamesPlayed >= 10)
        {
            fairytalesGameboard.color = new Vector4(1f, 1f, 1f, 1f);
            fairytalesGameboardName.color = new Vector4(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
        if (darkMatterGamesPlayed >= 10)
        {
            darkMatterGameboard.color = new Vector4(1f, 1f, 1f, 1f);
            darkMatterGameboardName.color = new Vector4(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
        if (tinkerersGamesPlayed >= 10)
        {
            tinkerersGameboard.color = new Vector4(1f, 1f, 1f, 1f);
            tinkerersGameboardName.color = new Vector4(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
    }


    public void uiSelectGenerals()
    {
        availableListOfCard.SetActive(false);
        currentListOfCard.SetActive(false);

        cursorManager.CursorNormal();
        cardButtonUIon.SetActive(false);
        gameboardButtonUIon.SetActive(false);
        generalButtonUIon.SetActive(true);
    }

    public IEnumerator RetrieveMetricsDeckBuilder()
    {
        Debug.Log(FirebaseManager.instance.user.UserId);
        var metricsLoad = FirebaseDatabase.DefaultInstance.GetReference("open").Child(FirebaseManager.instance.user.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => metricsLoad.IsCompleted);
        if (metricsLoad.IsFaulted)
        {
            Debug.Log("Unable to load metrics data.");
        }
        else if (metricsLoad.IsCompleted && metricsLoad.Result.Value != null)
        {
            Debug.Log("Loaded metrics data successfully.");

            DataSnapshot dataSnapshot = metricsLoad.Result;
            /*            var fullJSON = JsonConvert.DeserializeObject(dataSnapshot.GetRawJsonValue());
                        PlayerMetrics metricsData = JsonUtility.FromJson<PlayerMetrics>(fullJSON.ToString());*/
            string jsonString = dataSnapshot.GetRawJsonValue();
            PlayerMetrics metricsData = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerMetrics>(jsonString);

            masqueradesGamesPlayed = metricsData.margoDeckWinOpen + metricsData.margoDeckLossOpen;
            theOldKingdomGamesPlayed = metricsData.miosDeckWinOpen + metricsData.miosDeckLossOpen;
            fairytalesGamesPlayed = metricsData.nasseDeckWinOpen + metricsData.nasseDeckLossOpen;
            darkMatterGamesPlayed = metricsData.voidDeckWinOpen + metricsData.voidDeckLossOpen;
            tinkerersGamesPlayed = metricsData.tootDeckWinOpen + metricsData.tootDeckLossOpen;

            DetermineGameboardUnlocks();
        }
        else if (metricsLoad.IsCompleted && metricsLoad.Result.Value == null)
        {
            Debug.Log("No metrics yet generated for this user: " + FirebaseManager.instance.user.UserId);
            masqueradesGamesPlayed = 0;
            theOldKingdomGamesPlayed = 0;
            fairytalesGamesPlayed = 0;
            darkMatterGamesPlayed = 0;
            tinkerersGamesPlayed = 0;
        }
    }

    public class PlayerMetrics
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

        public PlayerMetrics(int xpOpen, int mmrOpen, int totalTimePlayedOpen, int totalTurnsTakenOpen, int margoDeckWinOpen, int margoDeckLossOpen,
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
}

