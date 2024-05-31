using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerActions : MonoBehaviour
{
    public void Pass()
    {
        GameManager.gameManager.PassTurn();
    }

    public void UseLeaderSkill()
    {
        GameManager.gameManager.UseLeaderSkill();
    }
}

