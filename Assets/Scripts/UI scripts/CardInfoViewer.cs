using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cardInfoPrefab; // Prefab del CardInfo asignado en el inspector
    private GameObject cardInfoInstance; // Instancia de CardInfo que será creada

    public void OnPointerEnter(PointerEventData eventData)
    {

        cardInfoInstance = Instantiate(cardInfoPrefab, new Vector3(0, 0, 0), Quaternion.identity);      // se instancia una nueva imagen para mostar los datos de la carta a partir de u prefab

        UICard uicard = gameObject.GetComponent<UICard>();      //se accede al script UICard 
        Card card = uicard.MotherCard;      // se accede a la carta que contiene los datos de la carta

        GameObject battlefield = GameObject.Find("BattlefieldBoard");       // se accede al campo de batalla
        if (battlefield != null)
        {
            // establece Battlefield como el padre de CardInfo

            cardInfoInstance.transform.SetParent(battlefield.transform);
            cardInfoInstance.transform.localPosition = new Vector3(799.745f, -8.0395f, 0f);
        }
        else
        {
            throw new ArgumentException("No se encontró el objeto Battlefield en la escena");
        }

        // Se llenan los campos de la información de la carta

        Transform cardNameText = cardInfoInstance.transform.Find("CardName");
        if (cardNameText != null)
        {
            TextMeshProUGUI textComponent = cardNameText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "Nombre: " + card.Name;
            }
        }

        Transform cardTypeText = cardInfoInstance.transform.Find("CardType");
        if (cardTypeText != null)
        {
            TextMeshProUGUI textComponent = cardTypeText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "Tipo: " + card.Type;
            }
        }

        Transform cardRowText = cardInfoInstance.transform.Find("CardRow");
        if (cardRowText != null)
        {
            TextMeshProUGUI textComponent = cardRowText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                if (card is UnityCard unityCard)
                    textComponent.text = "Fila: " + unityCard.RowString;

                else
                    textComponent.text = "";
            }
        }

        Transform cardPowerText = cardInfoInstance.transform.Find("CardPower");
        if (cardPowerText != null)
        {
            TextMeshProUGUI textComponent = cardPowerText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                if (card is UnityCard unityCard)
                    textComponent.text = "Poder: " + unityCard.Power.ToString();

                else
                    textComponent.text = "";
            }
        }

        Transform cardEffectText = cardInfoInstance.transform.Find("CardEffect");
        if (cardEffectText != null)
        {
            TextMeshProUGUI textComponent = cardEffectText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "Efecto: " + card.EffectDescription;
            }
        }

        Transform cardDescriptionText = cardInfoInstance.transform.Find("CardDescription");
        if (cardDescriptionText != null)
        {
            TextMeshProUGUI textComponent = cardDescriptionText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "Descripción: " + card.CharacterDescription;
            }
        }

        Transform cardQuoteText = cardInfoInstance.transform.Find("CardQuote");
        if (cardQuoteText != null)
        {
            TextMeshProUGUI textComponent = cardQuoteText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = card.Quote;
            }
        }
    }

    public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        // destruye la instancia de CardInfo una vez el puntero no está encima de la carta
        
        if (cardInfoInstance != null)
        {
            Destroy(cardInfoInstance);
        }
    }
}
