using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GamemodePong : GamemodeBase
{
    [Header("Network-Pong")]
    public PhotonView _photonView;
    public int spawnedPlayers = 0;

    public GameObject localPlayer;
    public bool doOnce = false;
    public GameObject ball;

    // Use this for initialization
    void Start()
    {
        if(PhotonNetwork.connected)
        {
            photonView = GetComponent<PhotonView>();
            _photonView = GetComponent<PhotonView>();
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
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (doOnce == false && spawnedPlayers == PhotonNetwork.playerList.Length)
                {
                    InitBall();
                }
            }
        }

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

        doOnce = false;
        photonView.RPC("CreatePlayers", PhotonTargets.All);
    }

    public void InitBall()
    {
        PhotonNetwork.Instantiate(Path.Combine("Pong", "Ball"), new Vector3(0, 0, 0), Quaternion.identity, 0);
        doOnce = true;
    }

    [PunRPC]
    void CleanUp()
    {
        PhotonNetwork.Destroy(localPlayer);
        localPlayer = null;
        doOnce = false;

        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(ball);
            ball = null;
        }
    }

    [PunRPC]
    void CreatePlayers()
    {
        Vector3 SpawnPosition = new Vector3();
        if(PhotonNetwork.isMasterClient)
        {
            SpawnPosition.x = -10f;
            SpawnPosition.y = 0f;
            SpawnPosition.z = 0f;

            localPlayer = PhotonNetwork.Instantiate(Path.Combine("Pong", "Player"), SpawnPosition, Quaternion.identity, 0);
            spawnedPlayers++;
        }
        else
        {
            SpawnPosition.x = 10f;
            SpawnPosition.y = 0f;
            SpawnPosition.z = 0f;

            localPlayer = PhotonNetwork.Instantiate(Path.Combine("Pong", "Player"), SpawnPosition, Quaternion.identity, 0);
            Debug.Log(this._photonView);
            GetComponent<PhotonView>().RPC("AddValueInt", PhotonTargets.MasterClient, spawnedPlayers, 1);
        }
    }

    [PunRPC]
    void AddValueInt(int value, int amout)
    {
        //value += amout;
    }
}
