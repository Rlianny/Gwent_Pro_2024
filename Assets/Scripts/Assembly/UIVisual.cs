using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
// using UnityEditor.ShortcutManagement;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class UIVisual : MonoBehaviour, IObserver
{
    [SerializeField] Subject gameManager;

    public GameObject CardPrefab;

    // Player1
    public TextMeshProUGUI Player1Name;
    public TextMeshProUGUI Player1Faction;
    public UnityEngine.UI.Image Player1FirstVictory;
    public UnityEngine.UI.Image Player1SecondVictory;
    public TextMeshProUGUI Player1DeckCount;
    public UnityEngine.UI.Image Player1Cemetery;
    public TextMeshProUGUI Player1MScore;
    public TextMeshProUGUI Player1RScore;
    public TextMeshProUGUI Player1SScore;
    public TextMeshProUGUI Player1TotalScore;
    public HorizontalLayoutGroup Player1LeaderPlace;
    [SerializeField] UIHand Player1HandScript;

    // Player2
    public TextMeshProUGUI Player2Name;
    public TextMeshProUGUI Player2Faction;
    public UnityEngine.UI.Image Player2FirstVictory;
    public UnityEngine.UI.Image Player2SecondVictory;
    public TextMeshProUGUI Player2DeckCount;
    public UnityEngine.UI.Image Player2Cemetery;
    public TextMeshProUGUI Player2MScore;
    public TextMeshProUGUI Player2RScore;
    public TextMeshProUGUI Player2SScore;
    public TextMeshProUGUI Player2TotalScore;
    public HorizontalLayoutGroup Player2LeaderPlace;
    [SerializeField] UIHand Player2HandScript;

    public Image WeatherEffect2S;
    public Image WeatherEffect2R;
    public Image WeatherEffect2M;
    public Image WeatherEffect1M;
    public Image WeatherEffect1R;
    public Image WeatherEffect1S;
    public Image Information;
    public TextMeshProUGUI MessageInformation;

    // Battlefiel Objects

    public HorizontalLayoutGroup RowS1;
    public HorizontalLayoutGroup RowR1;
    public HorizontalLayoutGroup RowM1;
    public HorizontalLayoutGroup RowM2;
    public HorizontalLayoutGroup RowR2;
    public HorizontalLayoutGroup RowS2;
    public HorizontalLayoutGroup IncreaseS1;
    public HorizontalLayoutGroup IncreaseR1;
    public HorizontalLayoutGroup IncreaseM1;
    public HorizontalLayoutGroup IncreaseM2;
    public HorizontalLayoutGroup IncreaseR2;
    public HorizontalLayoutGroup IncreaseS2;
    public HorizontalLayoutGroup WeatherM;
    public HorizontalLayoutGroup WeatherR;
    public HorizontalLayoutGroup WeatherS;

    // Otros elementos de la UI

    public Image SmogForDecoyEvent;
    public HorizontalLayoutGroup DecoyEventCardsShower;
    public GameObject CardToPickPrefab;

    private void OnEnable()
    {
        gameManager.AddObserver(this);
    }

    private void OnDisable()
    {
        gameManager.RemoveObserver(this);
    }

    void Start()
    {
        Information.gameObject.SetActive(false);
        SmogForDecoyEvent.gameObject.SetActive(false);

        HideImage(Player1FirstVictory);
        HideImage(Player1SecondVictory);
        HideImage(Player2FirstVictory);
        HideImage(Player2SecondVictory);
        HideImage(Player1Cemetery);
        HideImage(Player2Cemetery);

        WeatherEffect1M.gameObject.SetActive(false);
        WeatherEffect2M.gameObject.SetActive(false);
        WeatherEffect1R.gameObject.SetActive(false);
        WeatherEffect2R.gameObject.SetActive(false);
        WeatherEffect1S.gameObject.SetActive(false);
        WeatherEffect2S.gameObject.SetActive(false);

        Player1MScore.text = " ";
        Player1RScore.text = " ";
        Player1SScore.text = " ";
        Player2MScore.text = " ";
        Player2RScore.text = " ";
        Player2SScore.text = " ";

        Player1TotalScore.text = "0";
        Player2TotalScore.text = "0";

        Player1Name.text = GameManager.Player1.PlayerName;
        Player1Faction.text = GameManager.Player1.PlayerFaction;

        var newCard1 = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newCard1.transform.SetParent(Player1LeaderPlace.transform);
        UICard ui = newCard1.GetComponent<UICard>();
        ui.PrintCard(GameManager.Player1.PlayerLeader);


        Player2Name.text = GameManager.Player2.PlayerName;
        Player2Faction.text = GameManager.Player2.PlayerFaction;

        var newCard2 = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newCard2.transform.SetParent(Player2LeaderPlace.transform);
        UICard uICard = newCard2.GetComponent<UICard>();
        uICard.PrintCard(GameManager.Player2.PlayerLeader);
    }

    void Update()
    {
        Player1DeckCount.text = "  " + GameManager.Player1.PlayerHand.GameDeck.Count.ToString();
        Player2DeckCount.text = "  " + GameManager.Player2.PlayerHand.GameDeck.Count.ToString();
    }
    public void OnNotify(System.Enum action, Card card)
    {
        Debug.Log($"UIVisual ha sido notificado para {action.ToString()}");

        switch (action)
        {
            case GameEvents.Start:
                StartCoroutine(StartGame());
                return;

            case GameEvents.Summon:
                StartCoroutine(Summon(card));
                return;

            case GameEvents.PassTurn:
                StartCoroutine(PassTurn());
                return;

            case GameEvents.FinishRound:
                StartCoroutine(FinishRound());
                return;

            case GameEvents.StartRound:
                StartCoroutine(StartRound());
                return;

            case GameEvents.FinishGame:
                StartCoroutine(FinishGame());
                return;

            case GameEvents.DrawCard:
                if (GameManager.Player1.PlayerHand.PlayerHand.Count <= 10)
                    Player1HandScript.DrawCardUI(GameManager.Player1.PlayerHand.PlayerHand[GameManager.Player1.PlayerHand.PlayerHand.Count - 1]);

                if (GameManager.Player1.PlayerHand.PlayerHand.Count <= 10)
                    Player1HandScript.DrawCardUI(GameManager.Player1.PlayerHand.PlayerHand[GameManager.Player1.PlayerHand.PlayerHand.Count - 2]);

                if (GameManager.Player2.PlayerHand.PlayerHand.Count <= 10)
                    Player2HandScript.DrawCardUI(GameManager.Player2.PlayerHand.PlayerHand[GameManager.Player2.PlayerHand.PlayerHand.Count - 1]);

                if (GameManager.Player2.PlayerHand.PlayerHand.Count <= 10)
                    Player2HandScript.DrawCardUI(GameManager.Player2.PlayerHand.PlayerHand[GameManager.Player2.PlayerHand.PlayerHand.Count - 2]);
                return;

            case GameEvents.Invoke:
                InvokeUI(card);
                return;

            case GameEvents.DecoyEventStart:
                DecoyEventInit();
                return;

            case GameEvents.DecoyEventFinish:
                DecoyEventEnd(card);
                return;

            case GameEvents.DecoyEventAbort:
                AbortingDecoyEvent();
                return;
        }
    }

    public void AbortingDecoyEvent()
    {
        UpdateScores();
        UpdateBattlefieldUI();

        if (GameManager.Player1.IsActive == true && GameManager.Player2.HasPassed == false)
        {
            StartCoroutine(ShowMessage($"Turno de {GameManager.Player2.PlayerName}"));
        }

        else if (GameManager.Player2.IsActive == true && GameManager.Player1.HasPassed == false)
        {
            StartCoroutine(ShowMessage($"Turno de {GameManager.Player1.PlayerName}"));
        }
    }
    public void DecoyEventEnd(Card card)
    {
        UpdateScores();
        UpdateBattlefieldUI();

        if (GameManager.Player1.IsActive == true && GameManager.Player2.HasPassed == false)
        {
            var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(Player1HandScript.Hand.GetComponent<HorizontalLayoutGroup>().transform);
            UICard ui = newCard.GetComponent<UICard>();
            ui.PrintCard(card);
            ClearRowUI(DecoyEventCardsShower);
            SmogForDecoyEvent.gameObject.SetActive(false);
            StartCoroutine(ShowMessage($"Turno de {GameManager.Player2.PlayerName}"));
        }

        if (GameManager.Player2.IsActive == true && GameManager.Player1.HasPassed == false)
        {
            var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(Player2HandScript.Hand.GetComponent<HorizontalLayoutGroup>().transform);
            UICard ui = newCard.GetComponent<UICard>();
            ui.PrintCard(card);
            ClearRowUI(DecoyEventCardsShower);
            SmogForDecoyEvent.gameObject.SetActive(false);
            StartCoroutine(ShowMessage($"Turno de {GameManager.Player1.PlayerName}"));
        }
    }
    public void DecoyEventInit()
    {
        List<SilverUnityCard> CardsInDecoyRow = new();

        if (GameManager.Player1.IsActive == true)
        {
            List<UnityCard> Row = null;

            foreach (List<UnityCard> cardList in GameManager.Player1.Battlefield.Battlefield)
            {
                foreach (UnityCard unityCard in cardList)
                {
                    if (unityCard.Type == "Señuelo")
                        Row = cardList;
                }
            }

            if (Row != null)
            {
                if (Row.Count == 1)
                {
                    GameManager.gameManager.AbortDecoyEvent();
                }

                else
                {
                    foreach (UnityCard unityCard in Row)
                    {
                        if (unityCard is SilverUnityCard silverUnityCard)
                            CardsInDecoyRow.Add(silverUnityCard);
                    }

                    foreach (SilverUnityCard silverUnityCard in CardsInDecoyRow)
                    {
                        var newCard = Instantiate(CardToPickPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        newCard.transform.SetParent(DecoyEventCardsShower.transform);
                        UICard ui = newCard.GetComponent<UICard>();
                        ui.PrintCard(silverUnityCard);
                    }

                    SmogForDecoyEvent.gameObject.SetActive(true);

                }
            }

            else
            {
                GameManager.gameManager.AbortDecoyEvent();
            }
        }

        else
        {
            List<UnityCard> Row = null;

            foreach (List<UnityCard> cardList in GameManager.Player2.Battlefield.Battlefield)
            {
                foreach (UnityCard unityCard in cardList)
                {
                    if (unityCard.Type == "Señuelo")
                        Row = cardList;
                }
            }

            if (Row != null)
            {
                if (Row.Count == 1)
                {
                    GameManager.gameManager.AbortDecoyEvent();
                }

                else
                {
                    foreach (UnityCard unityCard in Row)
                    {
                        if (unityCard is SilverUnityCard silverUnityCard)
                            CardsInDecoyRow.Add(silverUnityCard);
                    }

                    foreach (SilverUnityCard silverUnityCard in CardsInDecoyRow)
                    {
                        var newCard = Instantiate(CardToPickPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        newCard.transform.SetParent(DecoyEventCardsShower.transform);
                        UICard ui = newCard.GetComponent<UICard>();
                        ui.PrintCard(silverUnityCard);
                    }

                    SmogForDecoyEvent.gameObject.SetActive(true);

                }
            }

            else
            {
                GameManager.gameManager.AbortDecoyEvent();
            }
        }
    }

    private void InvokeUI(Card card)
    {
        UpdateScores();
        UpdateBattlefieldUI();

        if (GameManager.Player1.IsActive == true && GameManager.Player2.HasPassed == false)
            StartCoroutine(ShowMessage($"Turno de {GameManager.Player2.PlayerName}"));

        if (GameManager.Player2.IsActive == true && GameManager.Player1.HasPassed == false)
            StartCoroutine(ShowMessage($"Turno de {GameManager.Player1.PlayerName}"));

        if (card is WeatherCard weatherCard)
        {
            switch (weatherCard.Row)
            {
                case "M":
                    var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    newCard.transform.SetParent(WeatherM.transform);
                    newCard.transform.position = WeatherM.transform.position;
                    UICard ui = newCard.GetComponent<UICard>();
                    ui.PrintCard(card);

                    WeatherEffect2M.gameObject.SetActive(true);
                    WeatherEffect2M.raycastTarget = false;
                    WeatherEffect1M.gameObject.SetActive(true);
                    WeatherEffect1M.raycastTarget = false;
                    return;

                case "R":
                    newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    newCard.transform.SetParent(WeatherR.transform);
                    newCard.transform.position = WeatherR.transform.position;
                    ui = newCard.GetComponent<UICard>();
                    ui.PrintCard(card);

                    WeatherEffect2R.gameObject.SetActive(true);
                    WeatherEffect2R.raycastTarget = false;
                    WeatherEffect1R.gameObject.SetActive(true);
                    WeatherEffect1R.raycastTarget = false;
                    return;

                case "S":
                    newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    newCard.transform.SetParent(WeatherS.transform);
                    newCard.transform.position = WeatherS.transform.position;
                    ui = newCard.GetComponent<UICard>();
                    ui.PrintCard(card);

                    WeatherEffect2S.gameObject.SetActive(true);
                    WeatherEffect2S.raycastTarget = false;
                    WeatherEffect1S.gameObject.SetActive(true);
                    WeatherEffect1S.raycastTarget = false;
                    return;
            }
        }

        else if (GameManager.Player1.IsActive == true)
        {
            if (card is IncreaseCard increaseCard)
            {
                switch (increaseCard.Row)
                {
                    case "M":
                        var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        newCard.transform.SetParent(IncreaseM1.transform);
                        newCard.transform.position = IncreaseM1.transform.position;
                        UICard ui = newCard.GetComponent<UICard>();
                        ui.PrintCard(card);
                        return;

                    case "R":
                        newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        newCard.transform.SetParent(IncreaseR1.transform);
                        newCard.transform.position = IncreaseR1.transform.position;
                        ui = newCard.GetComponent<UICard>();
                        ui.PrintCard(card);
                        return;

                    case "S":
                        newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        newCard.transform.SetParent(IncreaseS1.transform);
                        newCard.transform.position = IncreaseS1.transform.position;
                        ui = newCard.GetComponent<UICard>();
                        ui.PrintCard(card);
                        return;
                }
            }
        }

        else if (GameManager.Player2.IsActive == true)
        {
            if (card is IncreaseCard increaseCard)
            {
                switch (increaseCard.Row)
                {
                    case "M":
                        var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        newCard.transform.SetParent(IncreaseM2.transform);
                        newCard.transform.position = IncreaseM2.transform.position;
                        UICard ui = newCard.GetComponent<UICard>();
                        ui.PrintCard(card);
                        return;

                    case "R":
                        newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        newCard.transform.SetParent(IncreaseR2.transform);
                        newCard.transform.position = IncreaseR2.transform.position;
                        ui = newCard.GetComponent<UICard>();
                        ui.PrintCard(card);
                        return;

                    case "S":
                        newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        newCard.transform.SetParent(IncreaseS2.transform);
                        newCard.transform.position = IncreaseS2.transform.position;
                        ui = newCard.GetComponent<UICard>();
                        ui.PrintCard(card);
                        return;
                }
            }
        }
    }

    private void UpdateScores()
    {
        Player1MScore.text = GameManager.Player1.Battlefield.MeleeRowScore.ToString();
        Player1RScore.text = GameManager.Player1.Battlefield.RangedRowScore.ToString();
        Player1SScore.text = GameManager.Player1.Battlefield.SigeeRowScore.ToString();
        Player2MScore.text = GameManager.Player2.Battlefield.MeleeRowScore.ToString();
        Player2RScore.text = GameManager.Player2.Battlefield.RangedRowScore.ToString();
        Player2SScore.text = GameManager.Player2.Battlefield.SigeeRowScore.ToString();

        Player1TotalScore.text = GameManager.Player1.Battlefield.TotalScore.ToString();
        Player2TotalScore.text = GameManager.Player2.Battlefield.TotalScore.ToString();

        Debug.Log("Los puntos han sido actualizados");
    }

    IEnumerator ShowMessage(string message)
    {
        Debug.Log("Se mostrará el mensaje: " + message);
        MessageInformation.text = message;
        Information.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        Information.gameObject.SetActive(false);
        Debug.Log("Se ocultará el mensaje: " + message);
    }

    private void ShowFinalMessage(string message)
    {
        Debug.Log("Se mostrará el mensaje: " + message);
        MessageInformation.text = message;
        Information.gameObject.SetActive(true);
    }

    private void ShowImage(UnityEngine.UI.Image image)
    {
        // Mostrar la imagen
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    private void HideImage(UnityEngine.UI.Image image)
    {
        // Ocultar la imagen
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    private async void ClearBattlefieldUI()
    {

        await Task.Delay(3000);

        ClearRowUI(RowM1);
        ClearRowUI(RowM2);
        ClearRowUI(RowR1);
        ClearRowUI(RowR2);
        ClearRowUI(RowS2);
        ClearRowUI(RowS1);
        ClearRowUI(RowR2);

        ClearSlot(WeatherM);
        ClearSlot(WeatherR);
        ClearSlot(WeatherS);

        ClearSlot(IncreaseM1);
        ClearSlot(IncreaseR1);
        ClearSlot(IncreaseS1);
        ClearSlot(IncreaseM2);
        ClearSlot(IncreaseR2);
        ClearSlot(IncreaseS2);

        WeatherEffect1M.gameObject.SetActive(false);
        WeatherEffect2M.gameObject.SetActive(false);
        WeatherEffect1R.gameObject.SetActive(false);
        WeatherEffect2R.gameObject.SetActive(false);
        WeatherEffect1S.gameObject.SetActive(false);
        WeatherEffect2S.gameObject.SetActive(false);
    }

    IEnumerator DrawOnNewRound()
    {
        yield return new WaitForSeconds(0.1f);

        Player1HandScript.DrawCardUI(GameManager.Player1.PlayerHand.PlayerHand[GameManager.Player1.PlayerHand.PlayerHand.Count - 1]);
        Player2HandScript.DrawCardUI(GameManager.Player2.PlayerHand.PlayerHand[GameManager.Player2.PlayerHand.PlayerHand.Count - 1]);
    }

    private void ClearRowUI(HorizontalLayoutGroup toClear)
    {
        for (int i = 0; i < toClear.transform.childCount; i++)
        {
            GameObject card = toClear.transform.GetChild(i).gameObject;

            if (toClear.transform.parent.parent.name == "Player1")
            {
                LeanTween.move(card, Player1Cemetery.transform, 1f)
                .setOnComplete(() => Destroy(card));
                ShowImage(Player1Cemetery);
            }

            if (toClear.transform.parent.parent.name == "Player2")
            {
                LeanTween.move(card, Player2Cemetery.transform, 1f)
                .setOnComplete(() => Destroy(card));
                ShowImage(Player2Cemetery);
            }
        }
    }

    private void ClearSlot(HorizontalLayoutGroup toClear)
    {
        for (int i = 0; i < toClear.transform.childCount; i++)
        {
            GameObject card = toClear.transform.GetChild(i).gameObject;

            if (toClear.transform.parent.parent.name == "Player1")
            {
                LeanTween.move(card, Player1Cemetery.transform, 1f)
                .setOnComplete(() => Destroy(card));
                ShowImage(Player1Cemetery);
            }

            if (toClear.transform.parent.parent.name == "Player2")
            {
                LeanTween.move(card, Player2Cemetery.transform, 1f)
                .setOnComplete(() => Destroy(card));
                ShowImage(Player2Cemetery);
            }

            if (toClear.transform.parent.parent.name == "BattlefieldBoard")
            {
                Destroy(card);
            }
        }
    }

    private void ShowVictories()
    {
        if (GameManager.Player1.GamesWon < 2 && GameManager.Player1.GamesWon > 0)
            ShowImage(Player1FirstVictory);

        if (GameManager.Player1.GamesWon == 2)
            ShowImage(Player1SecondVictory);

        if (GameManager.Player2.GamesWon < 2 && GameManager.Player2.GamesWon > 0)
            ShowImage(Player2FirstVictory);

        if (GameManager.Player2.GamesWon == 2)
            ShowImage(Player2SecondVictory);
    }

    private IEnumerator StartGame()
    {
        UpdateScores();

        yield return new WaitForSeconds(2f);

        if (GameManager.Player1.IsActive == true)
        {
            StartCoroutine(ShowMessage($"{GameManager.Player1.PlayerName} empieza..."));
        }

        if (GameManager.Player2.IsActive == true)
        {
            StartCoroutine(ShowMessage($"{GameManager.Player2.PlayerName} empieza..."));
        }
    }

    private IEnumerator Summon(Card card)
    {
        UpdateScores();
        UpdateBattlefieldUI();

        if (card != null)
        {
            if (GameManager.Player1.IsActive == true && GameManager.Player2.HasPassed == false)
            {
                StartCoroutine(ShowMessage($"Turno de {GameManager.Player2.PlayerName}"));
            }

            else if (GameManager.Player2.IsActive == true && GameManager.Player1.HasPassed == false)
            {
                StartCoroutine(ShowMessage($"Turno de {GameManager.Player1.PlayerName}"));
            }



            if (card.Name == "Lluvia de Plumbuses")
            {
                WeatherEffect2M.gameObject.SetActive(true);
                WeatherEffect2M.raycastTarget = false;
                WeatherEffect1M.gameObject.SetActive(true);
                WeatherEffect1M.raycastTarget = false;
            }

            if (card.Name == "Tormenta interdimensional")
            {
                WeatherEffect2S.gameObject.SetActive(true);
                WeatherEffect2S.raycastTarget = false;
                WeatherEffect1S.gameObject.SetActive(true);
                WeatherEffect1S.raycastTarget = false;
            }

            if (card.Name == "Granizo de portales")
            {
                WeatherEffect2R.gameObject.SetActive(true);
                WeatherEffect2R.raycastTarget = false;
                WeatherEffect1R.gameObject.SetActive(true);
                WeatherEffect1R.raycastTarget = false;
            }

            if (card.Type == "Carta de Despeje" || card.EffectNumber == 12)
            {
                WeatherEffect1M.gameObject.SetActive(false);
                WeatherEffect2M.gameObject.SetActive(false);
                WeatherEffect1R.gameObject.SetActive(false);
                WeatherEffect2R.gameObject.SetActive(false);
                WeatherEffect1S.gameObject.SetActive(false);
                WeatherEffect2S.gameObject.SetActive(false);
                ClearSlot(WeatherR);
                ClearSlot(WeatherS);
                ClearSlot(WeatherM);
            }

            if (card.EffectNumber == 7)
            {
                if (GameManager.Player1.IsActive == true)
                {
                    Player1HandScript.DrawCardUI(GameManager.Player1.PlayerHand.PlayerHand[GameManager.Player1.PlayerHand.PlayerHand.Count - 1]);
                }

                else if (GameManager.Player2.IsActive == true)
                {
                    Player2HandScript.DrawCardUI(GameManager.Player2.PlayerHand.PlayerHand[GameManager.Player2.PlayerHand.PlayerHand.Count - 1]);
                }
            }

            if (card.EffectNumber == 16)
            {
                Player1HandScript.LeaderSkillIsEnable = false;
                Player2HandScript.LeaderSkillIsEnable = false;

            }

            if (card.EffectNumber == 15)
            {
                if (GameManager.Player1.IsActive == true)
                {
                    Player1HandScript.LeaderSkillIsEnable = false;
                }

                else if (GameManager.Player2.IsActive == true)
                {
                    Player2HandScript.LeaderSkillIsEnable = false;
                }
            }
        }

        else
            yield return null;
    }

    IEnumerator PassTurn()
    {
        if (GameManager.Player1.HasPassed == true && GameManager.Player2.HasPassed == false)
        {
            StartCoroutine(ShowMessage($"{GameManager.Player1.PlayerName} ha pasado"));

            yield return new WaitForSeconds(2f);

            StartCoroutine(ShowMessage($"Turno de {GameManager.Player2.PlayerName}"));
        }

        else if (GameManager.Player2.HasPassed == true && GameManager.Player1.HasPassed == false)
        {
            StartCoroutine(ShowMessage($"{GameManager.Player2.PlayerName} ha pasado turno"));

            yield return new WaitForSeconds(2f);

            StartCoroutine(ShowMessage($"Turno de {GameManager.Player1.PlayerName}"));
        }
    }

    IEnumerator FinishRound()
    {
        StartCoroutine(ShowMessage("La ronda ha terminado"));

        ClearBattlefieldUI();

        if (GameManager.Player1.Battlefield.TotalScore > GameManager.Player2.Battlefield.TotalScore)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(ShowMessage($"{GameManager.Player1.PlayerName} ha ganado la ronda"));
        }

        else if (GameManager.Player2.Battlefield.TotalScore > GameManager.Player1.Battlefield.TotalScore)
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(ShowMessage($"{GameManager.Player2.PlayerName} ha ganado la ronda"));
        }

        else
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(ShowMessage($"Ha habido un empate"));
        }

    }

    IEnumerator StartRound()
    {
        ShowVictories();

        if (GameManager.Player1.IsActive == true && GameManager.Player2.HasPassed == false)
        {
            yield return new WaitForSeconds(4f);
            StartCoroutine(ShowMessage($"Turno de {GameManager.Player1.PlayerName}"));
        }

        else if (GameManager.Player2.IsActive == true && GameManager.Player1.HasPassed == false)
        {
            yield return new WaitForSeconds(4f);
            StartCoroutine(ShowMessage($"Turno de {GameManager.Player2.PlayerName}"));
        }

        UpdateScores();
        UpdateBattlefieldUI();
    }

    IEnumerator FinishGame()
    {
        if (GameManager.Player1.GamesWon > GameManager.Player2.GamesWon)
        {
            yield return new WaitForSeconds(5f);
            ShowFinalMessage($"{GameManager.Player1.PlayerName} ha ganado la partida");
        }

        else if (GameManager.Player2.GamesWon > GameManager.Player1.GamesWon)
        {
            yield return new WaitForSeconds(5f);
            ShowFinalMessage($"{GameManager.Player2.PlayerName} ha ganado la partida");
        }

        else
        {
            yield return new WaitForSeconds(5f);
            ShowFinalMessage($"Ambos jugadores han ganado la partida");
        }

        yield return new WaitForSeconds(26f);
        SceneManager.LoadScene("MainMenutoBack");
    }

    private void UpdateBattlefieldUI()
    {
        for (int i = 0; i < RowS1.transform.childCount; i++)
        {
            Card card = RowS1.transform.GetChild(i).GetComponent<UICard>().MotherCard;
            if (card is SilverUnityCard silverUnityCard)
            {
                if (!GameManager.Player1.Battlefield.Battlefield[2].Contains(silverUnityCard))
                {
                    GameObject cardToDestroy = RowS1.transform.GetChild(i).gameObject;
                    LeanTween.move(cardToDestroy, Player1Cemetery.transform, 1f)
                    .setOnComplete(() => Destroy(cardToDestroy));
                    ShowImage(Player1Cemetery);

                    Debug.Log(card.Name + "ha sido enviado al cementerio");
                }

                else
                    RowS1.transform.GetChild(i).GetComponent<UICard>().CardPower.text = silverUnityCard.ActualPower.ToString();

            }
        }

        for (int i = 0; i < RowR1.transform.childCount; i++)
        {
            Card card = RowR1.transform.GetChild(i).GetComponent<UICard>().MotherCard;
            if (card is SilverUnityCard silverUnityCard)
            {
                if (!GameManager.Player1.Battlefield.Battlefield[1].Contains(silverUnityCard))
                {
                    GameObject cardToDestroy = RowR1.transform.GetChild(i).gameObject;
                    LeanTween.move(cardToDestroy, Player1Cemetery.transform, 1f)
                    .setOnComplete(() => Destroy(cardToDestroy));
                    ShowImage(Player1Cemetery);

                    Debug.Log(card.Name + "ha sido enviado al cementerio");
                }

                else
                    RowR1.transform.GetChild(i).GetComponent<UICard>().CardPower.text = silverUnityCard.ActualPower.ToString();
            }
        }

        for (int i = 0; i < RowM1.transform.childCount; i++)
        {
            Card card = RowM1.transform.GetChild(i).GetComponent<UICard>().MotherCard;
            if (card is SilverUnityCard silverUnityCard)
            {
                if (!GameManager.Player1.Battlefield.Battlefield[0].Contains(silverUnityCard))
                {
                    GameObject cardToDestroy = RowM1.transform.GetChild(i).gameObject;
                    LeanTween.move(cardToDestroy, Player1Cemetery.transform, 1f)
                    .setOnComplete(() => Destroy(cardToDestroy));
                    ShowImage(Player1Cemetery);

                    Debug.Log(card.Name + "ha sido enviado al cementerio");
                }

                else
                    RowM1.transform.GetChild(i).GetComponent<UICard>().CardPower.text = silverUnityCard.ActualPower.ToString();
            }
        }

        for (int i = 0; i < RowS2.transform.childCount; i++)
        {
            Card card = RowS2.transform.GetChild(i).GetComponent<UICard>().MotherCard;
            if (card is SilverUnityCard silverUnityCard)
            {
                if (!GameManager.Player2.Battlefield.Battlefield[2].Contains(silverUnityCard))
                {
                    GameObject cardToDestroy = RowS2.transform.GetChild(i).gameObject;
                    LeanTween.move(cardToDestroy, Player2Cemetery.transform, 1f)
                    .setOnComplete(() => Destroy(cardToDestroy));
                    ShowImage(Player2Cemetery);

                    Debug.Log(card.Name + "ha sido enviado al cementerio");
                }

                else
                    RowS2.transform.GetChild(i).GetComponent<UICard>().CardPower.text = silverUnityCard.ActualPower.ToString();
            }
        }

        for (int i = 0; i < RowR2.transform.childCount; i++)
        {
            Card card = RowR2.transform.GetChild(i).GetComponent<UICard>().MotherCard;
            if (card is SilverUnityCard silverUnityCard)
            {
                if (!GameManager.Player2.Battlefield.Battlefield[1].Contains(silverUnityCard))
                {
                    GameObject cardToDestroy = RowR2.transform.GetChild(i).gameObject;
                    LeanTween.move(cardToDestroy, Player2Cemetery.transform, 1f)
                    .setOnComplete(() => Destroy(cardToDestroy));
                    ShowImage(Player2Cemetery);

                    Debug.Log(card.Name + "ha sido enviado al cementerio");
                }

                else
                    RowR2.transform.GetChild(i).GetComponent<UICard>().CardPower.text = silverUnityCard.ActualPower.ToString();
            }
        }

        for (int i = 0; i < RowM2.transform.childCount; i++)
        {
            Card card = RowM2.transform.GetChild(i).GetComponent<UICard>().MotherCard;
            if (card is SilverUnityCard silverUnityCard)
            {
                if (!GameManager.Player2.Battlefield.Battlefield[0].Contains(silverUnityCard))
                {
                    GameObject cardToDestroy = RowM2.transform.GetChild(i).gameObject;
                    LeanTween.move(cardToDestroy, Player2Cemetery.transform, 1f)
                    .setOnComplete(() => Destroy(cardToDestroy));
                    ShowImage(Player2Cemetery);

                    Debug.Log(card.Name + "ha sido enviado al cementerio");
                }

                else
                    RowM2.transform.GetChild(i).GetComponent<UICard>().CardPower.text = silverUnityCard.ActualPower.ToString();
            }
        }

        Debug.Log("El campo ha sido actualizado");
    }
}

