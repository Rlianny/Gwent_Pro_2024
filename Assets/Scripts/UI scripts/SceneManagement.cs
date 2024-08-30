using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadBackMenu() // main menu to back
    {
        SceneManager.LoadScene("MainMenuToBack");
    }

    public void LoadSwapToCards() // battlefield
    {
        if (GameData.IsReady == true)
            SceneManager.LoadScene("SwapTwoCards");

        else
            SceneManager.LoadScene("SetPlayerName");
    }

    public void LoadCompiler() // compiler
    {
        //CharacterManager.Init();
        SceneManager.LoadScene("Compiler");
    }

    public void LoadPlayerDeck()
    {
        SceneManager.LoadScene("PlayerDeck");
    }

    public void LoadSetPlayerName()
    {
        //CharacterManager.Init();
        GwentCompiler.Compile(GwentCompiler.GetFileContent(PathContainer.TestFilePath));
        SceneManager.LoadScene("SetPlayerName");
    }

    public void LoadCardShower()
    {
        SceneManager.LoadScene("CardShower");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
