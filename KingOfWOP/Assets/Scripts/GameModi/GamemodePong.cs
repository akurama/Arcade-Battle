﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodePong : GamemodeBase
{
    // Use this for initialization
    void Start()
    {
        if(PhotonNetwork.connected)
        {
            photonView = GetComponent<PhotonView>();
            InitStartNetwork();
        }
        else
        {
            InitStart();
        }
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

        gameManager.NextGame();
    }

    public override void Refresh()
    {
        base.Refresh();
        for(int i = 0; i < gameScroes.Length; i++)
        {
            gameScroes[i] = 0;
        }
    }

    public void InitStartNetwork()
    {
        if(!PhotonNetwork.isMasterClient)
            return;

        
    }

    [PunRPC]
    void CreatePlayers()
    {
        Vector3 SpawnPosition = new Vector3();
        if(PhotonNetwork.isMasterClient)
        {

        }
        else
        {
            
        }
    }
}
