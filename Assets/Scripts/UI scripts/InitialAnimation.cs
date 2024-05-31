using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialAnimation : MonoBehaviour
{
    public GameObject Panel;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 12f)
        {
            Panel.SetActive(false);
        }
    }
}
