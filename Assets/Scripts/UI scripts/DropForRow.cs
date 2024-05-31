using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropForRow : MonoBehaviour, IDropHandler
{
    public RowTypes CardType; // El tipo de carta que esta fila acepta.
    public HorizontalLayoutGroup Row;       // HorizontalLayoutGroup que guarda las cartas
    public GameObject CardBeingDropped;     // carta siendo soltada sobre la fila
    public static bool WasDroped = false;
    public AudioClip Pop;
    public AudioSource UIAudible;

    public void OnDrop(PointerEventData eventData)
    {
        CardBeingDropped = DragItem.CardBeingDragged;       // se establece como la carta siendo arrastrada

        DragItem drag = CardBeingDropped.GetComponent<DragItem>();      // se accede al script de la carta que permite el arrastre

        if (drag.OriginalParent.transform.parent.name == this.transform.parent.parent.name)
        {
            UICard uicard = CardBeingDropped.GetComponent<UICard>();        // se accede al script UICard 
            Card card = uicard.MotherCard;      // se accede a la carta que contiene los datos de la carta

            Debug.Log($"La carta {card.Name} fue arrastrada a la fila {CardType}");

            if (card is UnityCard unityCard && unityCard.Row.Contains(CardType) && Row.transform.childCount < 8)        // se verifica si la fila de la carta coincide con la fila en la que esta siendo soltada
            {
                CardBeingDropped.GetComponent<CanvasGroup>().blocksRaycasts = true;
                CardBeingDropped.transform.SetParent(Row.transform);        // la carta se añade al HorizontalLayoutGroup
                CardBeingDropped.transform.position = Row.transform.position;
                drag.enabled = false;       // se desactiva el script
                WasDroped = true;

                GameManager.gameManager.PlayACard(card, CardType);
                UIAudible.PlayOneShot(Pop);
            }
        }

    }

}