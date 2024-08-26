using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadBattlefield : MonoBehaviour
{
    public TextMeshProUGUI LoadingMessage;
    void Start()
    {
        StartCoroutine(StartLoading());
    }

    IEnumerator StartLoading()
    {
        yield return new WaitForSeconds(3);
        
        AsyncOperation load = SceneManager.LoadSceneAsync("Battlefield");
        load.allowSceneActivation = false;

        while(load.isDone == false)
        {
            if(load.progress >= 0.9f)
            {
                LoadingMessage.text = "Presiona una tecla para continuar";

                if(Input.anyKey)
                {
                    load.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
