using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;


public class ScaleCardOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static UnityEngine.Vector3 vector = new UnityEngine.Vector3(1.2f, 1.2f, 1.2f); // representa el tamaño al que escalaremos la carta
    public static UnityEngine.Vector3 OriginalScale; // vector para guardar la escala original de la carta

    public void OnPointerEnter(PointerEventData eventData) // se ejecuta una vez el puntero está encima de una carta
    {
        OriginalScale = transform.localScale; // se guarda la escala original como la escala actual
        transform.localScale = vector;  // se establece la nueva escala guardada en el vector
    }

    public void OnPointerExit(PointerEventData eventData) // se llama una vez que el puntero es retirado de la carta
    {
        transform.localScale = OriginalScale; // se establece la escala como la escala original de la carta
    }

}
