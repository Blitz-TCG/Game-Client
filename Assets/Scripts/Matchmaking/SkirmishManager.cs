using Photon.Pun;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SkirmishManager : MonoBehaviourPunCallbacks
{
    public static SkirmishManager instance;
    public CursorManager cursorSkirmish;

    public bool IsSelected { get; private set; } = false;
    [SerializeField] private GameObject skirmishObject;
    public GameObject m_MyGameObject;
    public GameObject notesGameObject;

    [SerializeField] public GameObject deckNotes;
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
    [SerializeField] public GameObject selectedDeck1;
    [SerializeField] public GameObject selectedDeck2;
    [SerializeField] public GameObject selectedDeck3;
    [SerializeField] public GameObject selectedDeck4;
    [SerializeField] public GameObject selectedDeck5;
    [SerializeField] private GameObject playButtonOn;
    [SerializeField] private GameObject playButtonOff;

    public static int generalsIndex = 0;
    public static int generalsIndexStatic;
    [SerializeField] private Image deckSelected;
    [SerializeField] private Image deckHighlighted;
    [SerializeField] private Image deckOriginal;
    [SerializeField] private Button[] buttons;

    [SerializeField] private Button deckBuilderButton;
    //[SerializeField] private Button playButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Image[] images;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject loadingInitial;
    [SerializeField] private Button button;
    [SerializeField] private Sprite[] pfpImages;
    [SerializeField] private Image profileImage;

    public int deckId;
    public int deckCountSkirmish;

    private void Awake()
    {
        Debug.Log("Awake called ");
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }

        //if (GameBoardManager.isCompleted)
        //{
        //    GameBoardManager.isCompleted = false;
        //    PhotonNetwork.LeaveRoom();
        //    PhotonNetwork.Disconnect();
        //}

        //if (PhotonNetwork.IsConnected)
        //{
        //    Debug.Log("connected");
        //    if (PhotonNetwork.InRoom)
        //    {
        //        Debug.Log("in room " + PhotonNetwork.InRoom);
        //        PhotonNetwork.LeaveRoom();
        //    }
        //    Debug.Log(" already connected ");
        //    PhotonNetwork.Disconnect();
        //    Debug.Log("PhotonNetwork.IsConnected " + PhotonNetwork.IsConnected);
        //    //gameObject.SetActive(false);
        //}

        //if (GameBoardManager.connectUsing)
        //{
        //    gameObject.SetActive(false);
        //}

        //if (GameInitializer.isStarted)
        //{
        //    Debug.Log(PhotonNetwork.InRoom + " photon room ");
        //    if (!PhotonNetwork.InRoom)
        //    {
        //        Debug.Log("!PhotonNetwork.InRoom");
        //        SceneManager.LoadScene(3);
        //    }
        //    else
        //    {
        //        Debug.Log("else");
        //        if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
        //        {
        //            Debug.Log("PhotonNetwork.CurrentRoom.PlayerCount != 2");
        //            PhotonNetwork.Disconnect();
        //            SceneManager.LoadScene(3);
        //        }
        //    }
        //    GameInitializer.isStarted = false;
        //}
        
    }

    private void Start()
    {
        Debug.Log(ErgoQuery.instance.deckGeneralStore[0] + " deck general store");
        if (ErgoQuery.instance.deckGeneralStore[0] == 0)
        {
            deckCountSkirmish = 0;
            Debug.Log(deckCountSkirmish + " deckCountSkirmish ");
        }
        else if (ErgoQuery.instance.deckGeneralStore[4] > 0)
        {
            deckCountSkirmish = 5;
            Debug.Log(deckCountSkirmish + " deckCountSkirmish ");
        }
        else if (ErgoQuery.instance.deckGeneralStore[3] > 0)
        {
            deckCountSkirmish = 4;
            Debug.Log(deckCountSkirmish + " deckCountSkirmish ");
        }
        else if (ErgoQuery.instance.deckGeneralStore[2] > 0)
        {
            deckCountSkirmish = 3;
            Debug.Log(deckCountSkirmish + " deckCountSkirmish ");
        }
        else if (ErgoQuery.instance.deckGeneralStore[1] > 0)
        {
            deckCountSkirmish = 2;
            Debug.Log(deckCountSkirmish + " deckCountSkirmish ");
        }
        else if (ErgoQuery.instance.deckGeneralStore[0] > 0)
        {
            deckCountSkirmish = 1;
            Debug.Log(deckCountSkirmish + " deckCountSkirmish ");
        }

        if (deckCountSkirmish > 0)
        {
            Debug.Log(deckCountSkirmish + " deckCountSkirmish ");
            for (int i = 0; i < deckCountSkirmish; i++)
            {

                images[i].enabled = true;
            }
        }
        for (int i = deckCountSkirmish; i < deckCountSkirmish; i++)
        {
            images[i].enabled = false;
        }

        //Debug.Log(PhotonNetwork.InRoom + " photon room ");
        //if (!PhotonNetwork.InRoom)
        //{
        //    Debug.Log("!PhotonNetwork.InRoom");
        //    SceneManager.LoadScene(3);
        //}
        //else
        //{
        //    Debug.Log("else");
        //    if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
        //    {
        //        Debug.Log("PhotonNetwork.CurrentRoom.PlayerCount != 2");
        //        PhotonNetwork.Disconnect();
        //        SceneManager.LoadScene(3);
        //    }
        //}
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && loadingPanel.activeSelf)
        {
            Debug.Log("Input.GetMouseButtonDown(0) && loadingPanel.activeSelf");
            ObjectSelection();
        }
        if (Input.GetMouseButtonDown(0) && loadingInitial.activeSelf)
        {
            Debug.Log("Input.GetMouseButtonDown(0) && loadingInitial.activeSelf");
            ObjectSelection();
        }

        if (Input.GetMouseButtonDown(0) && skirmishObject.activeSelf && !loadingPanel.activeSelf && !loadingInitial.activeSelf) //this controls deck selection and hovering UI/mechanics, as well as DeckID
        {
            Debug.Log("Input.GetMouseButtonDown(0) && skirmishObject.activeSelf && !loadingPanel.activeSelf && !loadingInitial.activeSelf");
            RaycastHit hit;
            //Send a ray from the camera to the mouseposition
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Create a raycast from the Camera and output anything it hits
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Physics.Raycast(ray, out hit)");
                //Check the hit GameObject has a Collider
                if (hit.collider != null)
                {
                    Debug.Log("hit.collider != null");
                    //Click a GameObject to return that GameObject your mouse pointer hit
                    m_MyGameObject = hit.collider.gameObject;
                    Debug.Log("m_MyGameObject " + m_MyGameObject);
                    //Set this GameObject you clicked as the currently selected in the EventSystem
                    if (IsSelected == true)
                    {
                        Debug.Log("IsSelected " + IsSelected);
                        if (m_MyGameObject.ToString() == "Deck Builder (UnityEngine.GameObject)" || m_MyGameObject.ToString() == "Play Parent (UnityEngine.GameObject)" ||
                            m_MyGameObject.ToString() == "Back (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Builder (UnityEngine.GameObject)\" || m_MyGameObject.ToString() == \"Play Parent (UnityEngine.GameObject)\" ||\r\n                            m_MyGameObject.ToString() == \"Back (UnityEngine.GameObject)\"");
                            ObjectSelection();
                            Debug.Log("hello1");
                        }
                    }
                    else
                    {
                        Debug.Log("else");
                        EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                    }
                    //Selects the game object we found
                    Debug.Log(m_MyGameObject.ToString());
                    Debug.Log(IsSelected);
                    //outputs the gameobjects name to the log
                    if ((IsSelected == false) && m_MyGameObject.GetComponent<Image>().IsActive())
                    {
                        Debug.Log("(IsSelected == false) && m_MyGameObject.GetComponent<Image>().IsActive()");
                        if (m_MyGameObject.ToString() == "Deck Image 1 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 1 (UnityEngine.GameObject)\"");
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(1);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 2 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 2 (UnityEngine.GameObject)\"");
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(2);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 3 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 3 (UnityEngine.GameObject)\"");
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(3);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 4 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 4 (UnityEngine.GameObject)\"");
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(4);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 5 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 5 (UnityEngine.GameObject)\"");
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
                        Debug.Log("IsSelected == true && m_MyGameObject.ToString().Contains(\"Image\") &&\r\n                        !m_MyGameObject.ToString().Contains((deckId).ToString()) && m_MyGameObject.GetComponent<Image>().IsActive()");
                        if (m_MyGameObject.ToString() == "Deck Image 1 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 1 (UnityEngine.GameObject)\"");
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(1);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 2 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 2 (UnityEngine.GameObject)\"");
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(2);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 3 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 3 (UnityEngine.GameObject)\"");
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(3);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 4 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 4 (UnityEngine.GameObject)\"");
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(4);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                        else if (m_MyGameObject.ToString() == "Deck Image 5 (UnityEngine.GameObject)")
                        {
                            Debug.Log("m_MyGameObject.ToString() == \"Deck Image 5 (UnityEngine.GameObject)\"");
                            IsSelected = true;
                            EventSystem.current.SetSelectedGameObject(m_MyGameObject);
                            OnDeckClick(5);
                            Debug.Log(IsSelected);
                            Debug.Log(deckId);
                        }
                    }
                    else if (IsSelected == true && m_MyGameObject.ToString() != "Deck Builder (UnityEngine.GameObject)"
                        && m_MyGameObject.ToString() != "Play Parent (UnityEngine.GameObject)"
                        && m_MyGameObject.ToString() != "Back (UnityEngine.GameObject)")
                    {
                        Debug.Log("IsSelected == true && m_MyGameObject.ToString() != \"Deck Builder (UnityEngine.GameObject)\"\r\n                        && m_MyGameObject.ToString() != \"Play Parent (UnityEngine.GameObject)\"\r\n                        && m_MyGameObject.ToString() != \"Back (UnityEngine.GameObject)\"");
                        //if (m_MyGameObject.ToString() != "Deck Notes Parent (UnityEngine.GameObject)")
                        //{
                        Debug.Log(deckId);
                        IsSelected = false;
                        playButtonOn.SetActive(false);
                        playButtonOff.SetActive(true);
                        //playButton.interactable = false;
                        //StoreNotes(deckNotesTitleSelected.text, deckNotesBodySelected.text, deckId); //when they click new deck while editing notes

                        SpriteState spriteState = new SpriteState();
                        spriteState.highlightedSprite = deckHighlighted.sprite; // Set the highlighted sprite
                        spriteState.pressedSprite = deckSelected.sprite;
                        spriteState.selectedSprite = deckSelected.sprite;
                        buttons[deckId - 1].spriteState = spriteState;

                        deckId = -1;
                        EventSystem.current.SetSelectedGameObject(null);

                        if (!IsMouseOverDeck())
                        {
                            Debug.Log(deckId);
                            profileImage.sprite = pfpImages[5];
                            if (m_MyGameObject.ToString() != "Deck Builder (UnityEngine.GameObject)" && m_MyGameObject.ToString() != "Deck Notes Parent (UnityEngine.GameObject)")
                            {
                                deckNotes.SetActive(false);
                            }
                        }

                        if (!m_MyGameObject.GetComponent<Image>().IsActive()) //for when you click a deck and then click that same deck again or an empty deck
                        {
                            Debug.Log("!m_MyGameObject.GetComponent<Image>().IsActive()");
                            profileImage.sprite = pfpImages[5];
                            deckNotes.SetActive(false);
                        }

                        for (int i = 0; i < images.Length; i++) //this resets the deck images when they shouldn't be selected - specifically from dragging notes fields which would be a bug
                        {
                            Debug.Log("int i = 0; i < images.Length; i++");
                            if (images[i].sprite != deckOriginal.sprite)
                            {
                                Debug.Log("images[i].sprite != deckOriginal.sprite");
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
                        //}
                    }
                }
            }
            else if (!Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider);
                IsSelected = false;
                playButtonOn.SetActive(false);
                playButtonOff.SetActive(true);
                //playButton.interactable = false;
                deckId = -1;
                EventSystem.current.SetSelectedGameObject(null);
                deckNotes.SetActive(false);
                profileImage.sprite = pfpImages[5];
                Debug.Log(IsSelected);
                Debug.Log(deckId);

                for (int i = 0; i < images.Length; i++) //this resets the deck images when they shouldn't be selected - specifically from dragging notes fields which would be a bug
                {
                    if (images[i].sprite != deckOriginal.sprite)
                    {
                        Debug.Log("images[i].sprite != deckOriginal.sprite");
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
            Debug.Log("Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)");
            if (deckId > 0)
            {
                Debug.Log("deckId " + deckId);
                int imageArrayPosition = deckId - 1;
                if (images[imageArrayPosition].sprite != deckSelected.sprite)
                {
                    Debug.Log("images[imageArrayPosition].sprite != deckSelected.sprite");
                    images[imageArrayPosition].sprite = deckSelected.sprite;
                }
            }
        }
    }

    public void Back()
    {
        Debug.Log("Back");
        GameManager.instance.ChangeScene(1);
        PhotonNetwork.Disconnect();
    }

    public void DeckBuilder()
    {
        GameManager.instance.ChangeScene(2);
        PhotonNetwork.Disconnect();
    }

    public void OnDeckClick(int id) //make this the same as deck builder enentually
    {
        Debug.Log("OnDeckClick " + id);
        generalsIndex = ErgoQuery.instance.deckGeneralStore[id - 1];
        profileImage.sprite = pfpImages[generalsIndex - 1];
        deckNotes.SetActive(true);

        if (IsSelected == true)
        {
            Debug.Log("IsSelected " + IsSelected);
            deckId = id;
            deckNotesTitleSelected.text = ErgoQuery.instance.deckTitleStore[id - 1];
            deckNotesBodySelected.text = ErgoQuery.instance.deckBodyStore[id - 1];
            int[] cardCount = ErgoQuery.instance.cardIdCurrentStore[id - 1];

            if (cardCount.Length >= 10)
            {
                Debug.Log(cardCount.Length);
                playButtonOn.SetActive(true);
                playButtonOff.SetActive(false);
                //playButton.interactable = true;
            }
            else if (cardCount.Length < 10)
            {
                playButtonOn.SetActive(false);
                playButtonOff.SetActive(true);
                //playButton.interactable = false;
            }

        }

        generalsIndexStatic = generalsIndex; //to assist with resetting the hovering previews

        for (int i = 0; i < images.Length; i++) //this resets the deck images when they shouldn't be selected - specifically from dragging notes fields which would be a bug
        {
            if (i != (id - 1))
            {
                Debug.Log("i != (id - 1)");
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
        Debug.Log(deckId);
    }

    public bool IsMouseOverDeck() //to fix when a user tries to drag notes and lands over a deck
    {
        Debug.Log("IsMouseOverDeck");
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void OnHoverDeckEnter(int id)
    {
        Debug.Log("OnHoverDeckEnter " + id);
        int hoverDeckGeneral = ErgoQuery.instance.deckGeneralStore[id - 1];
        int hoverDeckId = ErgoQuery.instance.deckIdStore[id - 1];

        if (deckId >= -1)
        {
            Debug.Log("deckId >= -1");
            profileImage.sprite = pfpImages[hoverDeckGeneral - 1];

            if (hoverDeckId == 1)
            {
                Debug.Log("hoverDeckId == 1");
                ShowDeckNotes1();
                deckNotesTitle1.text = ErgoQuery.instance.deckTitleStore[id - 1];
                deckNotesBody1.text = ErgoQuery.instance.deckBodyStore[id - 1];
                deckNotes.SetActive(true);

                RightMiddleClickEnterReset(hoverDeckId);
            }
            else if (hoverDeckId == 2)
            {
                Debug.Log("hoverDeckId == 2");
                ShowDeckNotes2();
                deckNotesTitle2.text = ErgoQuery.instance.deckTitleStore[id - 1];
                deckNotesBody2.text = ErgoQuery.instance.deckBodyStore[id - 1];
                deckNotes.SetActive(true);

                RightMiddleClickEnterReset(hoverDeckId);
            }
            else if (hoverDeckId == 3)
            {
                Debug.Log("hoverDeckId == 3");
                ShowDeckNotes3();
                deckNotesTitle3.text = ErgoQuery.instance.deckTitleStore[id - 1];
                deckNotesBody3.text = ErgoQuery.instance.deckBodyStore[id - 1];
                deckNotes.SetActive(true);

                RightMiddleClickEnterReset(hoverDeckId);
            }
            else if (hoverDeckId == 4)
            {
                Debug.Log("hoverDeckId == 4");
                ShowDeckNotes4();
                deckNotesTitle4.text = ErgoQuery.instance.deckTitleStore[id - 1];
                deckNotesBody4.text = ErgoQuery.instance.deckBodyStore[id - 1];
                deckNotes.SetActive(true);

                RightMiddleClickEnterReset(hoverDeckId);
            }
            else if (hoverDeckId == 5)
            {
                Debug.Log("hoverDeckId == 5");
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
        Debug.Log("RightMiddleClickEnterReset " + hoverDeckId);
        if (deckId > 0 && deckId == hoverDeckId)
        {
            Debug.Log("deckId > 0 && deckId == hoverDeckId");
            SpriteState spriteState = new SpriteState();
            spriteState.highlightedSprite = deckSelected.sprite;
            spriteState.pressedSprite = deckSelected.sprite;
            spriteState.selectedSprite = deckSelected.sprite;
            buttons[hoverDeckId - 1].spriteState = spriteState;
        }
    }

    public void OnHoverDeckExit(int id)
    {
        Debug.Log("OnHoverDeckExit " + id);
        if (IsSelected)
        {
            Debug.Log(IsSelected + " IsSelected");
            deckNotesTitleSelected.text = ErgoQuery.instance.deckTitleStore[deckId - 1];
            deckNotesBodySelected.text = ErgoQuery.instance.deckBodyStore[deckId - 1];
        }

        if (deckId < 1)
        {
            Debug.Log("deckId < 1");
            profileImage.sprite = pfpImages[5];
            deckNotes.SetActive(false);
        }
        else if (deckId >= 1)
        {
            Debug.Log("deckId >= 1");
            profileImage.sprite = pfpImages[generalsIndexStatic - 1];

            if (deckId == 1)
            {
                Debug.Log("deckId == 1");
                ShowDeckNotes1();
                deckNotes.SetActive(true);
            }
            else if (deckId == 2)
            {
                Debug.Log("deckId == 2");
                ShowDeckNotes2();
                deckNotes.SetActive(true);
            }
            else if (deckId == 3)
            {
                Debug.Log("deckId == 3");
                ShowDeckNotes3();
                deckNotes.SetActive(true);
            }
            else if (deckId == 4)
            {
                Debug.Log("deckId == 4");
                ShowDeckNotes4();
                deckNotes.SetActive(true);
            }
            else if (deckId == 5)
            {
                Debug.Log("deckId == 5");
                ShowDeckNotes5();
                deckNotes.SetActive(true);
            }
        }
    }
    public void ShowDeckNotes1() // When user select blue general
    {
        Debug.Log("ShowDeckNotes1");
        deckNotesParent1.SetActive(true);
        deckNotesParent2.SetActive(false);
        deckNotesParent3.SetActive(false);
        deckNotesParent4.SetActive(false);
        deckNotesParent5.SetActive(false);
    }
    public void ShowDeckNotes2() // When user select blue general
    {
        Debug.Log("ShowDeckNotes2");
        deckNotesParent1.SetActive(false);
        deckNotesParent2.SetActive(true);
        deckNotesParent3.SetActive(false);
        deckNotesParent4.SetActive(false);
        deckNotesParent5.SetActive(false);
    }
    public void ShowDeckNotes3() // When user select blue general
    {
        Debug.Log("ShowDeckNotes3");
        deckNotesParent1.SetActive(false);
        deckNotesParent2.SetActive(false);
        deckNotesParent3.SetActive(true);
        deckNotesParent4.SetActive(false);
        deckNotesParent5.SetActive(false);
    }
    public void ShowDeckNotes4() // When user select blue general
    {
        Debug.Log("ShowDeckNotes4");
        deckNotesParent1.SetActive(false);
        deckNotesParent2.SetActive(false);
        deckNotesParent3.SetActive(false);
        deckNotesParent4.SetActive(true);
        deckNotesParent5.SetActive(false);
    }
    public void ShowDeckNotes5() // When user select blue general
    {
        Debug.Log("ShowDeckNotes5");
        deckNotesParent1.SetActive(false);
        deckNotesParent2.SetActive(false);
        deckNotesParent3.SetActive(false);
        deckNotesParent4.SetActive(false);
        deckNotesParent5.SetActive(true);
    }
    public void OnClickCancel()
    {
        Debug.Log("OnClickCancel");
        cursorSkirmish.CursorNormal();
        IsSelected = true;
        OnDeckClick(deckId);
    }

    public void ObjectSelection()
    {
        Debug.Log("ObjectSelection");
        if (deckId == 1)
        {
            Debug.Log("deckId == 1");
            EventSystem.current.SetSelectedGameObject(selectedDeck1);
        }
        else if (deckId == 2)
        {
            Debug.Log("deckId == 2");
            EventSystem.current.SetSelectedGameObject(selectedDeck2);
        }
        else if (deckId == 3)
        {
            Debug.Log("deckId == 3");
            EventSystem.current.SetSelectedGameObject(selectedDeck3);
        }
        else if (deckId == 4)
        {
            Debug.Log("deckId == 4");
            EventSystem.current.SetSelectedGameObject(selectedDeck4);
        }
        else if (deckId == 5)
        {
            Debug.Log("deckId == 5");
            EventSystem.current.SetSelectedGameObject(selectedDeck5);
        }
    }
}
