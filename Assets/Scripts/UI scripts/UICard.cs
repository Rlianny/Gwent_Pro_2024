using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UICard : MonoBehaviour
{
    public Card MotherCard;     // carta que contiene las propiedades de la carta
    public Image CardImage;
    public TextMeshProUGUI CardPower;

    void Start()
    {

    }

    void Update()
    {

    }

    public void PrintCard(Card card)
    {
        MotherCard = card;

        CardImage.sprite = Resources.Load<Sprite>(MotherCard.Name);

        if (MotherCard is UnityCard unityCard)
            CardPower.text = unityCard.Power.ToString();

        else
            CardPower.text = " ";
    }

    public void UpdateValue(Card card)
    {
        
    }
}
