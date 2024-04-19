using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialAnimation : MonoBehaviour
{
    public GameObject Panel;
    private float timer = 0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 13f)
        {
            Panel.SetActive(false);
        }
    }
}
