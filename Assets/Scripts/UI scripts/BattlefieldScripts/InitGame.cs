using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    private void Awake() 
    {
        GameManager gameManager = GameManager.AwakeManager();
    }
}
