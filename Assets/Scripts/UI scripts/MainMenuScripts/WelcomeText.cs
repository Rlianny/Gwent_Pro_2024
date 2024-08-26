using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeText : MonoBehaviour
{
    public TextMeshProUGUI WelcomeMessage;
    public string TextString = "¡Bienvenidos, intrépidos viajeros interdimensionales, a Gwent-pro! \n En este juego, la realidad se retuerce como un portal de bolsillo y los límites entre dimensiones se desvanecen. ¿Estás listo para enfrentarte a criaturas cósmicas, enemigos misterioros y giros temporales? ¡Entonces prepárate para una experiencia alucinante! \n La trama te llevará desde la Tierra hasta un planeta desconocido para dirigir junto al líder del equipo que escogerás a continuación, una épica batalla interdimensional. ¿Por qué? Porque sí. Y porque Rick lo dijo. \n Te esperan diálogos sarcásticos, referencias a la cultura pop y chistes que desafían las leyes de la física. \n \n Requisitos para jugar: \n - Mente abierta: Deja tus expectativas en la puerta. Aquí, las reglas son flexibles y la lógica es opcional. \n - Amor por lo absurdo: Si alguna vez te preguntaste qué pasaría si un pepinillo gobernara el mundo, este es tu juego. \n - Ganas de reír: Porque, sinceramente, ¿quién no necesita más risas en su vida? \n \n Así que, aventureros, ajusten sus cinturones multiversales y prepárense para una montaña rusa de locura. ¡Nos vemos en la batalla!";
    public float typingSpeed = 0.01f;

    private void Start() 
    {
        StartCoroutine(DisplayWelcomeMessage(TextString));
    }

    private IEnumerator DisplayWelcomeMessage(string TextString)
    {
        // se establece el mensaje como vacío
        WelcomeMessage.text = "";

        // se muestra cada letra una a la vez
        foreach(char letter in TextString.ToCharArray())
        {
            WelcomeMessage.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
   
}
