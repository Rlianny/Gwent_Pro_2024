using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UICard : MonoBehaviour
{
    public Card MotherCard;     // carta que contiene las propiedades de la carta
    public TextMeshProUGUI CardPower;
    public TextMeshProUGUI CardName;
    public Image Character;
    public HorizontalLayoutGroup CardInfo;
    public GameObject ItemPrefab;

    void Start()
    {

    }

    void Update()
    {

    }

    public void PrintCard(Card card)
    {
        MotherCard = card;

        CardName.text = card.Name;

        string character = CharacterManager.Query(MotherCard.Name  + " " + MotherCard.CharacterDescription);
        Debug.Log(character.ToUpper());

        Character.sprite = Resources.Load<Sprite>(character);

        var newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newItem.transform.SetParent(CardInfo.transform);
        Image ui = newItem.GetComponent<Image>();

        switch (card.Type)
        {
            case CardTypes.Líder:

                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Leader");
                break;

            case CardTypes.Unidad_Héroe:

                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Hero");
                break;

            case CardTypes.Unidad_de_Plata:
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Silver");
                break;

            case CardTypes.Carta_de_Aumento:
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Special");
                newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newItem.transform.SetParent(CardInfo.transform);
                ui = newItem.GetComponent<Image>();
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Increase");
                break;

            case CardTypes.Carta_de_Clima:
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Special");
                newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newItem.transform.SetParent(CardInfo.transform);
                ui = newItem.GetComponent<Image>();
                switch (card.Name)
                {
                    case "Lluvia de Plumbuses":
                        ui.sprite = Resources.Load<Sprite>("CardTemplateImages/WeatherRain");
                        break;

                    case "Granizo de portales":
                        ui.sprite = Resources.Load<Sprite>("CardTemplateImages/WeatherFrost");
                        break;

                    case "Tormenta interdimensional":
                        ui.sprite = Resources.Load<Sprite>("CardTemplateImages/WeatherStorm");
                        break;
                }
                break;

            case CardTypes.Carta_de_Despeje:
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Special");
                newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newItem.transform.SetParent(CardInfo.transform);
                ui = newItem.GetComponent<Image>();
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Clearance");
                break;

            case CardTypes.Señuelo:
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Special");
                newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newItem.transform.SetParent(CardInfo.transform);
                ui = newItem.GetComponent<Image>();
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Decoy");
                break;
        }

        if (MotherCard is UnityCard unityCard)
        {
            CardPower.text = unityCard.Power.ToString();

            if (unityCard.Row.Contains(RowTypes.Melee))
            {
                newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newItem.transform.SetParent(CardInfo.transform);
                ui = newItem.GetComponent<Image>();
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Melee");
            }

            if (unityCard.Row.Contains(RowTypes.Ranged))
            {
                newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newItem.transform.SetParent(CardInfo.transform);
                ui = newItem.GetComponent<Image>();
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Ranged");
            }

            if (unityCard.Row.Contains(RowTypes.Siege))
            {
                newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newItem.transform.SetParent(CardInfo.transform);
                ui = newItem.GetComponent<Image>();
                ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Siege");
            }
        }

        else
            CardPower.text = " ";

        if (MotherCard is IncreaseCard increaseCard)
        {
            switch (increaseCard.Row)
            {
                case RowTypes.Melee:
                    newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    newItem.transform.SetParent(CardInfo.transform);
                    ui = newItem.GetComponent<Image>();
                    ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Melee");
                    break;

                case RowTypes.Ranged:
                    newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    newItem.transform.SetParent(CardInfo.transform);
                    ui = newItem.GetComponent<Image>();
                    ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Ranged");
                    break;

                case RowTypes.Siege:
                    newItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    newItem.transform.SetParent(CardInfo.transform);
                    ui = newItem.GetComponent<Image>();
                    ui.sprite = Resources.Load<Sprite>("CardTemplateImages/Siege");
                    break;
            }
        }

    }

    public void UpdateValue(Card card)
    {

    }
}
