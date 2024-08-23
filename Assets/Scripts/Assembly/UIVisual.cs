using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
// using UnityEditor.ShortcutManagement;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIVisual : MonoBehaviour, IObserver
{
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
        //GameManager.gameManager.AddObserver(this);
    }

    private void OnDisable()
    {
        GameManager.gameManager.RemoveObserver(this);
    }

    void Start()
    {
        GameManager.gameManager.AddObserver(this);

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
    public void OnNotify(GameEventReport eventReport)
    {
        Debug.Log($"UIVisual ha sido notificado para {eventReport.GameEvent.ToString()}");

        switch (eventReport.GameEvent)
        {
            case GameEvents.Start:
                StartCoroutine(StartGame(eventReport));
                return;

            case GameEvents.Summon:
                StartCoroutine(Summon(eventReport));
                return;

            case GameEvents.PassTurn:
                StartCoroutine(PassTurn(eventReport));
                return;

            case GameEvents.FinishRound:
                StartCoroutine(FinishRound());
                return;

            case GameEvents.StartRound:
                StartCoroutine(StartRound(eventReport));
                return;

            case GameEvents.FinishGame:
                StartCoroutine(FinishGame());
                return;

            case GameEvents.DrawCard:
                DrawCard();
                return;

            case GameEvents.Invoke:
                InvokeUI(eventReport);
                return;

            case GameEvents.DecoyEventStart:
                DecoyEventInit(eventReport);
                return;

            case GameEvents.DecoyEventFinish:
                DecoyEventEnd(eventReport);
                return;

            case GameEvents.DecoyEventAbort:
                AbortingDecoyEvent(eventReport);
                return;
        }
    }

    private void DrawCard()
    {
        if (GameManager.Player1.PlayerHand.PlayerHand.Count <= 10)
            Player1HandScript.DrawCardUI(GameManager.Player1.PlayerHand.PlayerHand[GameManager.Player1.PlayerHand.PlayerHand.Count - 1]);

        if (GameManager.Player1.PlayerHand.PlayerHand.Count <= 10)
            Player1HandScript.DrawCardUI(GameManager.Player1.PlayerHand.PlayerHand[GameManager.Player1.PlayerHand.PlayerHand.Count - 2]);

        if (GameManager.Player2.PlayerHand.PlayerHand.Count <= 10)
            Player2HandScript.DrawCardUI(GameManager.Player2.PlayerHand.PlayerHand[GameManager.Player2.PlayerHand.PlayerHand.Count - 1]);

        if (GameManager.Player2.PlayerHand.PlayerHand.Count <= 10)
            Player2HandScript.DrawCardUI(GameManager.Player2.PlayerHand.PlayerHand[GameManager.Player2.PlayerHand.PlayerHand.Count - 2]);
    }

    public void AbortingDecoyEvent(GameEventReport eventReport)
    {
        UpdateScores();
        UpdateBattlefieldUI();

        if (eventReport.RivalPlayer.HasPassed == false)
            StartCoroutine(ShowMessage($"Turno de {eventReport.RivalPlayer.PlayerName}"));

    }

    public void DecoyEventEnd(GameEventReport eventReport)
    {
        UpdateScores();
        UpdateBattlefieldUI();

        if (GameManager.Player1.IsActive == true && GameManager.Player2.HasPassed == false)
            InternalDecoyEventEnd(eventReport.Card, Player1HandScript, GameManager.Player2);

        if (GameManager.Player2.IsActive == true && GameManager.Player1.HasPassed == false)
            InternalDecoyEventEnd(eventReport.Card, Player2HandScript, GameManager.Player1);
    }

    private void InternalDecoyEventEnd(Card card, UIHand hand, Player nextPlayer)
    {
        var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newCard.transform.SetParent(hand.Hand.GetComponent<HorizontalLayoutGroup>().transform);
        newCard.GetComponent<UICard>().PrintCard(card);
        for (int i = 0; i < DecoyEventCardsShower.transform.childCount; i++)
        {
            GameObject cardToClear = DecoyEventCardsShower.transform.GetChild(i).gameObject;
            Destroy(cardToClear);
        }
        SmogForDecoyEvent.gameObject.SetActive(false);
        StartCoroutine(ShowMessage($"Turno de {nextPlayer.PlayerName}"));

    }
    public void DecoyEventInit(GameEventReport eventReport)
    {
        List<UnityCard> Row = eventReport.ActivePlayer.Battlefield.GetRowFromBattlefield(eventReport.RowOfCard);
        List<SilverUnityCard> CardsInDecoyRow = new();

        if (Row.Count == 1) GameManager.gameManager.AbortDecoyEvent();
        else
        {
            foreach (UnityCard unityCard in Row)
            {
                if (unityCard is SilverUnityCard silverUnityCard)
                    CardsInDecoyRow.Add(silverUnityCard);
            }

            if (CardsInDecoyRow.Count > 0)
            {
                foreach (SilverUnityCard silverUnityCard in CardsInDecoyRow)
                {
                    Debug.Log(CardsInDecoyRow.Count);
                    var newCard = Instantiate(CardToPickPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    newCard.transform.SetParent(DecoyEventCardsShower.transform);
                    UICard ui = newCard.GetComponent<UICard>();
                    ui.PrintCard(silverUnityCard);
                }

            }

            else
            {
                GameManager.gameManager.AbortDecoyEvent();
                return;
            }

            SmogForDecoyEvent.gameObject.SetActive(true);

        }
    }

    private void InvokeUI(GameEventReport eventReport)
    {
        UpdateScores();
        UpdateBattlefieldUI();

        if(!eventReport.RivalPlayer.HasPassed)
        {
            StartCoroutine(ShowMessage($"Turno de {eventReport.RivalPlayer.PlayerName}"));
        }

        if (eventReport.Card is WeatherCard weatherCard)
        {
            switch (weatherCard.Row)
            {
                case RowTypes.Melee:
                    InternalInvokeUI(eventReport.Card, WeatherM);
                    ShowWeatherInRow(WeatherEffect2M);
                    ShowWeatherInRow(WeatherEffect1M);
                    return;

                case RowTypes.Ranged:
                    InternalInvokeUI(eventReport.Card, WeatherR);
                    ShowWeatherInRow(WeatherEffect2R);
                    ShowWeatherInRow(WeatherEffect1R);
                    return;

                case RowTypes.Siege:
                    InternalInvokeUI(eventReport.Card, WeatherS);
                    ShowWeatherInRow(WeatherEffect2S);
                    ShowWeatherInRow(WeatherEffect1S);
                    return;
            }
        }

        else if (GameManager.Player1.IsActive == true)
        {
            if (eventReport.Card is IncreaseCard increaseCard)
            {
                switch (increaseCard.Row)
                {
                    case RowTypes.Melee:
                        InternalInvokeUI(eventReport.Card, IncreaseM1); return;

                    case RowTypes.Ranged:
                        InternalInvokeUI(eventReport.Card, IncreaseR1); return;

                    case RowTypes.Siege:
                        InternalInvokeUI(eventReport.Card, IncreaseS1);
                        return;
                }
            }
        }

        else if (GameManager.Player2.IsActive == true)
        {
            if (eventReport.Card is IncreaseCard increaseCard)
            {
                switch (increaseCard.Row)
                {
                    case RowTypes.Melee:
                        InternalInvokeUI(eventReport.Card, IncreaseM2); return;

                    case RowTypes.Ranged:
                        InternalInvokeUI(eventReport.Card, IncreaseR2); return;

                    case RowTypes.Siege:
                        InternalInvokeUI(eventReport.Card, IncreaseS2);
                        return;
                }
            }
        }
    }

    private void InternalInvokeUI(Card card, HorizontalLayoutGroup place)
    {
        var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newCard.transform.SetParent(place.transform);
        newCard.transform.position = place.transform.position;
        newCard.GetComponent<UICard>().PrintCard(card);
    }

    private void ShowWeatherInRow(Image weatherImage)
    {
        weatherImage.gameObject.SetActive(true);
        weatherImage.raycastTarget = false;
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

    private void ClearRowUI(HorizontalLayoutGroup toClear)
    {
        for (int i = 0; i < toClear.transform.childCount; i++)
        {
            GameObject card = toClear.transform.GetChild(i).gameObject;

            if (toClear.transform.parent.parent.name == "Player1")
                SendToCemetery(card, Player1Cemetery);

            if (toClear.transform.parent.parent.name == "Player2")
                SendToCemetery(card, Player2Cemetery);
        }
    }

    private void SendToCemetery(GameObject card, Image cemeteryImage)
    {
        LeanTween.move(card, cemeteryImage.transform, 1f)
                .setOnComplete(() => Destroy(card));
        ShowImage(cemeteryImage);
    }

    private void ClearSlot(HorizontalLayoutGroup toClear)
    {
        for (int i = 0; i < toClear.transform.childCount; i++)
        {
            GameObject card = toClear.transform.GetChild(i).gameObject;

            if (toClear.transform.parent.parent.name == "Player1")
                SendToCemetery(card, Player1Cemetery);

            if (toClear.transform.parent.parent.name == "Player2")
                SendToCemetery(card, Player2Cemetery);

            if (toClear.transform.parent.parent.name == "BattlefieldBoard")
                Destroy(card);
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

    private IEnumerator StartGame(GameEventReport eventReport)
    {
        UpdateScores();

        yield return new WaitForSeconds(2f);

        StartCoroutine(ShowMessage($"{eventReport.ActivePlayer.PlayerName} empieza..."));
    }

    private IEnumerator Summon(GameEventReport eventReport)
    {
        UpdateScores();
        UpdateBattlefieldUI();

        if (eventReport.Card != null)
        {

            if (eventReport.ActivePlayer.IsActive == true && eventReport.RivalPlayer.HasPassed == false)
            {
                StartCoroutine(ShowMessage($"Turno de {eventReport.RivalPlayer.PlayerName}"));
            }

            if (eventReport.Card.Name == "Lluvia de Plumbuses")
            {
                ShowWeatherInRow(WeatherEffect2M);
                ShowWeatherInRow(WeatherEffect1M);
            }

            if (eventReport.Card.Name == "Tormenta interdimensional")
            {
                ShowWeatherInRow(WeatherEffect2S);
                ShowWeatherInRow(WeatherEffect1S);
            }

            if (eventReport.Card.Name == "Granizo de portales")
            {
                ShowWeatherInRow(WeatherEffect2R);
                ShowWeatherInRow(WeatherEffect1R);
            }

            if (eventReport.Card.Type == CardTypes.Carta_de_Despeje || eventReport.Card.EffectNumber == 12)
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

            if (eventReport.Card.EffectNumber == 7)
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

            if (eventReport.Card.EffectNumber == 16)
            {
                Player1HandScript.LeaderSkillIsEnable = false;
                Player2HandScript.LeaderSkillIsEnable = false;

            }

            if (eventReport.Card.EffectNumber == 15)
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

    IEnumerator PassTurn(GameEventReport eventReport)
    {
        if (eventReport.ActivePlayer.HasPassed == true && eventReport.RivalPlayer.HasPassed == false)
        {
            StartCoroutine(ShowMessage($"{eventReport.ActivePlayer} ha pasado"));

            yield return new WaitForSeconds(2f);

            StartCoroutine(ShowMessage($"Turno de {eventReport.RivalPlayer.PlayerName}"));
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

    IEnumerator StartRound(GameEventReport eventReport)
    {
        ShowVictories();

        if (eventReport.RivalPlayer.HasPassed == false)
        {
            yield return new WaitForSeconds(4f);
            StartCoroutine(ShowMessage($"Turno de {eventReport.ActivePlayer.PlayerName}"));
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
        UpdateBattlefieldRowUI(RowS1, GameManager.Player1, 2);
        UpdateBattlefieldRowUI(RowR1, GameManager.Player1, 1);
        UpdateBattlefieldRowUI(RowM1, GameManager.Player1, 0);
        UpdateBattlefieldRowUI(RowM2, GameManager.Player2, 0);
        UpdateBattlefieldRowUI(RowR2, GameManager.Player2, 1);
        UpdateBattlefieldRowUI(RowS2, GameManager.Player2, 2);

        Debug.Log("El campo ha sido actualizado");
    }

    private void UpdateBattlefieldRowUI(HorizontalLayoutGroup rowToClear, Player playerBeingUpdated, int RowCorrespondency)
    {
        for (int i = 0; i < rowToClear.transform.childCount; i++)
        {
            Card card = rowToClear.transform.GetChild(i).GetComponent<UICard>().MotherCard;
            if (card is SilverUnityCard silverUnityCard)
            {
                if (!playerBeingUpdated.Battlefield.GetRowFromBattlefield(Tools.RowForIndex[RowCorrespondency]).Contains(silverUnityCard))
                {
                    GameObject cardToDestroy = rowToClear.transform.GetChild(i).gameObject;
                    SendToCemetery(cardToDestroy, Player2Cemetery);
                    Debug.Log(card.Name + "ha sido enviado al cementerio");
                }

                else
                    rowToClear.transform.GetChild(i).GetComponent<UICard>().CardPower.text = silverUnityCard.ActualPower.ToString();
            }
        }
    }
}

