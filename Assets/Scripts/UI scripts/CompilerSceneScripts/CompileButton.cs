using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompileButton : MonoBehaviour
{
    public TextMeshProUGUI text;

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
        GwentCompiler.Compile(text.text[..(text.text.Length - 2)]);
    }
}
