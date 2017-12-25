using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodePong : GamemodeBase
{
    // Use this for initialization
    void Start()
    {
        InitStart();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
    }

    public override void CheckIfEnd()
    {
        if(time <= 0f)
        {
            GameEnd();
        }
    }

    public override void GameEnd()
    {
        for(int i = 0; i < gameManager.scroes.Length; i++)
        {
            gameManager.scroes[i] = (gameScroes[i] * gameToTotal) * gameManager.multiplyer;
        }
    }
}
