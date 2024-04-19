using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector3 OriginalPosition;
    public Transform OriginalParent;
    public static GameObject CardBeingDragged;

    public void OnBeginDrag(PointerEventData eventData)     // al iniciar el arrastre
    {
        OriginalPosition = transform.position;      // guardamos la posicion original
        OriginalParent = transform.parent;      // guardamos el padre original en la jerarquia de la escena

        transform.SetParent(transform.root);        // establecemos como padre en la jerarquia el canvas que contiene a toda la escena
        transform.SetAsLastSibling();       // ubicamos el objeto en la ultima posicion dentro de la escena, asi evitamos que sea mostrado debajo de otro objeto

        CardBeingDragged = gameObject;

        GetComponent<CanvasGroup>().blocksRaycasts = false;     // bloqueamos el Raycast del objeto siendo arrastrado para que el puntero detecte que objetos estan debajo de el
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // si la carta no es soltada sobre un objeto que pueda recibirla restablecemos sus propiedades iniciales

        if (CardBeingDragged.GetComponent<UICard>().MotherCard.Type == "Carta de Despeje")
        {
            GameManager.gameManager.PlayACard(CardBeingDragged.GetComponent<UICard>().MotherCard, "R");
            Destroy(CardBeingDragged.gameObject);
            CardBeingDragged = null;
        }

        else
        {
            CardBeingDragged = null;

            transform.position = OriginalPosition;
            transform.SetParent(OriginalParent);

            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }


    }

}
