using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeBase : MonoBehaviour
{
    public GameManager gameManager;
    public int[] gameScroes = new int[2];
    public int gameToTotal;
    public float time = 60f;

    public void InitStart()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public virtual void CheckIfEnd()
    {

    }

    public virtual void GameEnd()
    {

    }
}
