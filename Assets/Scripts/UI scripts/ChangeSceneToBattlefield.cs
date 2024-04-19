using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneToBattlefield : MonoBehaviour
{
    public void ChangeScene()
    {
        if(GameData.IsReady == true)
        SceneManager.LoadScene("SwapTwoCards");

        else
        SceneManager.LoadScene("SetPlayerName");
    }
}
