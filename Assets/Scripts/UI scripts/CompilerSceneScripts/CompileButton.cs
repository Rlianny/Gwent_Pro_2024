using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompileButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    public VerticalLayoutGroup verticalLayoutGroup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Compile()
    {
        for (int i = verticalLayoutGroup.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(verticalLayoutGroup.transform.GetChild(i).gameObject);
        }
        if (text.text.Length >= 2)
            GwentCompiler.Compile(text.text[..(text.text.Length - 2)]);
    }
}
