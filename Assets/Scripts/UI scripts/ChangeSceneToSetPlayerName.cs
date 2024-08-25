using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneToSetPlayerName : MonoBehaviour
{
    public void Start()
    {
        string pathTxt = "CardsCollection";
        string pathSerialized = "Assets/CardsCollection/Serialized";
        CardsCollection cardsCollection = new CardsCollection(CardsCreator.GetCardInfoList(pathTxt), CardsCreator.LoadAll(pathSerialized));
    }
    public void ChangeScene()
    {
        CharacterManager.Init();
        GwentCompiler.Compile();
        SceneManager.LoadScene("SetPlayerName");
    }
}
