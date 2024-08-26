using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.TextCore.Text;

public class CompilerOutput : MonoBehaviour
{
    public static CompilerOutput compilerOutput;
    public VerticalLayoutGroup Shower;
    public GameObject MessagePrefab;
    //public HorizontalLayoutGroup CardShower;
    public GameObject NewCardsShowerPrefab;
    public GameObject CardPrefab;

    void Start()
    {
        compilerOutput = this;
    }

    public void Report(string message)
    {
        var newMessage = Instantiate(MessagePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newMessage.transform.SetParent(Shower.transform);
        newMessage.GetComponent<TextMeshProUGUI>().text = message;
    }

    async public void ShowNewCards(List<Card> cards)
    {
        var shower = Instantiate(NewCardsShowerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        shower.transform.SetParent(Shower.transform.parent.transform);
        shower.transform.position = Shower.transform.parent.transform.position;
        foreach (Card card in cards)
        {
            var newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(shower.transform.GetChild(0).GetComponent<HorizontalLayoutGroup>().transform);
            UICard ui = newCard.GetComponent<UICard>();
            ui.PrintCard(card);
        }
        await Task.Delay(6000);

        shower.SetActive(false);
    }

}
