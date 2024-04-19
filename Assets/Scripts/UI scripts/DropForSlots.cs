using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropForSlots : MonoBehaviour, IDropHandler
{
    public string CardType; // El tipo de carta que esta fila acepta.
    public string RowCorrespondency;        // la fila a la que afecta la carta
    public HorizontalLayoutGroup Slot;      // el slot que contiene este script
    public GameObject CardBeingDropped;     // la carta siendo soltada
    public static bool WasDroped = false;
    public AudioClip Pop;
    public AudioSource UIAudible;

    public void OnDrop(PointerEventData eventData)
    {
        CardBeingDropped = DragItem.CardBeingDragged;       // se establece a partir de la carta siendo arrastrada 

        UICard uicard = CardBeingDropped.GetComponent<UICard>();        //  se accede al script UICard 
        Card card = uicard.MotherCard;      // se accede a la carta que contiene los datos de la carta

        Debug.Log($"La carta {card.Name} fue arrastrada a la casilla {CardType}");

        switch (CardType)
        {
            case "Carta de Aumento":
                if (Slot.transform.childCount < 1 && card is IncreaseCard increaseCard && increaseCard.Row == RowCorrespondency)        // si el slot está vacío y el tipo de carta coincide con la carta aceptada por el slot
                {
                    CardBeingDropped.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    CardBeingDropped.transform.SetParent(Slot.transform);
                    CardBeingDropped.transform.position = Slot.transform.position;
                    DragItem drag = CardBeingDropped.GetComponent<DragItem>(); // se accede al script de la carta que permite el arrastre
                    drag.enabled = false; // se desactiva el script
                    WasDroped = true;

                    GameManager.gameManager.PlayACard(card, RowCorrespondency);
                    UIAudible.PlayOneShot(Pop);
                }
                return;

            case "Carta de Clima":
                if (card is WeatherCard weatherCard && weatherCard.Row == RowCorrespondency && Slot.transform.childCount < 1)       // si el slot está vacío y el tipo de carta coincide con la carta aceptada por el slot
                {
                    CardBeingDropped.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    CardBeingDropped.transform.SetParent(Slot.transform);
                    CardBeingDropped.transform.position = Slot.transform.position;
                    DragItem drag = CardBeingDropped.GetComponent<DragItem>(); // se accede al script de la carta que permite el arrastre
                    drag.enabled = false; // se desactiva el script
                    WasDroped = true;

                    GameManager.gameManager.PlayACard(card, RowCorrespondency);
                    UIAudible.PlayOneShot(Pop);
                }
                return;

            default:
                return;

        }
    }

}