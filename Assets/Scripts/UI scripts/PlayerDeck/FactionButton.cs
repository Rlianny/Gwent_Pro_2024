using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FactionButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void Create()
    {
        PlayerDeckManager.playerDeckManager.GenerateDeck(text.text);
    }
}
