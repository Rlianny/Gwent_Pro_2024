using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwappingCards : MonoBehaviour
{
    public GameObject CardToSwapPrefab;
    public HorizontalLayoutGroup Player1Hand;
    public HorizontalLayoutGroup Player2Hand;
    public TextMeshProUGUI Player1Text;
    public TextMeshProUGUI Player2Text;
    public Button Player1OkButton;
    public Button Player2OkButton;
    public bool ReadyPlayer1 = false;
    public bool ReadyPlayer2 = false;
    public static int Player1Swaps;
    public static int Player2Swaps;

    void Start()
    {
        Player1Text.text = "Mano de " + GameData.Player1.PlayerName;
        Player2Text.text = "Mano de " + GameData.Player2.PlayerName;


        foreach (Card card in GameData.Player1.PlayerHand.PlayerHand)
        {
            var newCard = Instantiate(CardToSwapPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(Player1Hand.transform);
            UICard ui = newCard.GetComponent<UICard>();
            ui.PrintCard(card);
        }

        foreach (Card card in GameData.Player2.PlayerHand.PlayerHand)
        {
            var newCard = Instantiate(CardToSwapPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(Player2Hand.transform);
            UICard ui = newCard.GetComponent<UICard>();
            ui.PrintCard(card);
        }

    }

    void Update()
    {
        if(ReadyPlayer1 == true && ReadyPlayer2 == true)
        {
            SceneManager.LoadScene("Battlefield");
        }
    }

    public void ReadyButtonForPlayer1()
    {
        ReadyPlayer1 = true;
    }

    public void ReadyButtonForPlayer2()
    {
        ReadyPlayer2 = true;
    }
}
