using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneToSetPlayerName : MonoBehaviour
{
    public void Start()
    {
        string pathTxt = PathContainer.CardDataBaseDirectoryPath;
        string pathSerialized = PathContainer.SerializedFilesDirectoryPath;
        CardsCollection cardsCollection = new CardsCollection(CardsCreator.GetCardInfoList(pathTxt), CardsCreator.LoadAll(pathSerialized));
    }
    public void ChangeScene()
    {
        CharacterManager.Init();
        GwentCompiler.Compile(GwentCompiler.GetFileContent(PathContainer.TestFilePath));
        SceneManager.LoadScene("SetPlayerName");
    }
}
