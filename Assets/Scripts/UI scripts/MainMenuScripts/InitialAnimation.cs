using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialAnimation : MonoBehaviour
{
    public GameObject Panel;
    private float timer = 0f;

    void Start()
    {
        string pathTxt = PathContainer.CardDataBaseDirectoryPath;
        string pathSerialized = PathContainer.SerializedFilesDirectoryPath;
        CardsCollection cardsCollection = new CardsCollection(CardsCreator.GetCardInfoList(pathTxt), CardsCreator.LoadAll(pathSerialized));
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 12f)
        {
            Panel.SetActive(false);
        }
    }
}
