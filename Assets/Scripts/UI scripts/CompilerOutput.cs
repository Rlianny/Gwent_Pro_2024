using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CompilerOutput : MonoBehaviour
{
    public static CompilerOutput compilerOutput;
    public VerticalLayoutGroup Shower;
    public GameObject MessagePrefab;
    void Start()
    {
        compilerOutput = this; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Report(string message)
    {
        var newMessage = Instantiate(MessagePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newMessage.transform.SetParent(Shower.transform);
        newMessage.GetComponent<TextMeshProUGUI>().text = message;
    }
}
