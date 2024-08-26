using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowBigCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cardBigPrefab;
    private GameObject cardBigInstance;
    public void OnPointerEnter(PointerEventData eventData)
    {
        UICard uicard = gameObject.GetComponent<UICard>();      // se accede al script UICard 
        Card card = uicard.MotherCard;

        cardBigInstance = Instantiate(cardBigPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        cardBigInstance.transform.SetParent(transform.root.transform.GetChild(1).transform);
        UICard ui = cardBigInstance.GetComponent<UICard>();
        ui.PrintCard(card);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardBigInstance != null)
        {
            Destroy(cardBigInstance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
