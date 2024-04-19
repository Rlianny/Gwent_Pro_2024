using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetValidName : MonoBehaviour
{
    public InputField InputField;
    public Text InputOrder;
    public Text PlayerName;
    public Button OkButton;

    void Start()
    {
        OkButton.interactable = false;

        if(GameData.Player1Name == null)
        InputOrder.text = "Introduzca el nombre del primer jugador";

        else
        InputOrder.text = "Introduzca el nombre del segundo jugador";
    }

    void Update()
    {
        if (PlayerName.text.Length > 4)     // si el nombre del jugador tiene más de cuatro letras
        {
            OkButton.interactable = true;       // activa el botón aceptar
        }

        else
        OkButton.interactable = false;
    }

    public void SaveName()
    {
        if(GameData.Player1 == null)
        GameData.Player1Name = PlayerName.text;

        else
        GameData.Player2Name = PlayerName.text;

        SceneManager.LoadScene("CardCreator");
    }
}
