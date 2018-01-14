using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GamemodeBase> Gamemodes;
    public List<GamemodeBase> randomGamemodes = new List<GamemodeBase>();
    public List<TextMeshProUGUI> texts;
    public GameObject AnimationObject;
    public AnimationClip initialAnim;

    public int multiplyer = 1;
    public int[] scroes = new int[2];
    public int currendGameIndex = 0;

    //Network Stuff
    [Header("Network")]
    public int playersInScene;
    public bool doOnce = false;
    public GameObject connectingText;
    public PhotonView photonView;

    // Use this for initialization
    void Start()
    {
        if(PhotonNetwork.connected)
        {
            photonView = GetComponent<PhotonView>();

            photonView.RPC("PlayerJoined", PhotonTargets.All);
        }
        else
        {
            RandomOrder();
            multiplyer = 1;
            PlayAnim();
            FirstGame();

            Debug.Log(randomGamemodes.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.connected)
        {
            CheckIfAllConnected();
        }
    }

    void PlayAnim()
    {
        AnimationObject.SetActive(true);
        
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = randomGamemodes[i].gameObject.name;
        }
    }

    void FirstGame()
    {
        currendGameIndex = 0;
        randomGamemodes[currendGameIndex].gameObject.SetActive(true);
    }

    void RandomOrder()
    {
        if(!PhotonNetwork.connected)
            Debug.Log("Local Multiplayer");

        List<GamemodeBase> store = Gamemodes;

        int i = store.Count;

        for (int j = 0; j < i; j++)
        {
            int index = Random.Range(0, store.Count);

            randomGamemodes.Add(store[index]);
            store.Remove(store[index]);
        }
    }

    public void NextGame()
    {
        if(currendGameIndex + 1 < randomGamemodes.Count)
        {
            randomGamemodes[currendGameIndex].gameObject.active = false;
            currendGameIndex++;
            randomGamemodes[currendGameIndex].gameObject.active = true;
        }
        else
        {
            ShowResults();
        }
    }

    public void ShowResults()
    {
        Debug.Log("Hey zeige mal die results");
    }

    void CheckIfAllConnected()
    {
        if(!PhotonNetwork.isMasterClient)
            return;

        if(PhotonNetwork.playerList.Length == playersInScene && doOnce == false)
        {
            InitSetup();
            doOnce = true;
        }
    }

    void InitSetup()
    {
        photonView.RPC("DeactivateLoadingAnim", PhotonTargets.All);
        CreateIndex();
        StartCoroutine(WaitTillAnimationEnd(initialAnim.length));
    }

    void CreateIndex()
    {
        Debug.Log("Call CreateIndex()");

        List<int> indexList = new List<int>();
        
        CreateIndexList(indexList);

        List<int> randomIndexList = new List<int>();

        int i = indexList.Count;

        for (int j = 0; j < i; j++)
        {
            int index = Random.Range(0, indexList.Count);

            randomIndexList.Add(indexList[index]);
            indexList.Remove(indexList[index]);
        }

        photonView.RPC("SyncRandomModes", PhotonTargets.Others, randomIndexList[0], randomIndexList[1], randomIndexList[2], randomIndexList[3], randomIndexList[4], randomIndexList[5]);

        for(int j = 0; j < randomIndexList.Count; j++)
        {
            randomGamemodes.Add(Gamemodes[randomIndexList[j]]);
        }

        photonView.RPC("PlayAnimSync", PhotonTargets.All);
    }

    void CreateIndexList(List<int> indexList)
    {
        for (int i = 0; i < Gamemodes.Count; i++)
        {
            indexList.Add(i);
        }
    }

    IEnumerator WaitTillAnimationEnd(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);
        photonView.RPC("StartNextGame", PhotonTargets.All);
    }

#region PunRPCs

    [PunRPC]
    void PlayerJoined()
    {
        playersInScene++;
    }

    [PunRPC]
    void DeactivateLoadingAnim()
    {
        connectingText.GetComponent<TextMeshProUGUI>().text = "All Connected";
    }

    [PunRPC]
    void SyncRandomModes(int indexModeOne, int indexModeTwo, int indexModeThree, int indexModeFour, int indexModeFive, int indexModeSix)
    {
        randomGamemodes.Add(Gamemodes[indexModeOne]);
        randomGamemodes.Add(Gamemodes[indexModeTwo]);
        randomGamemodes.Add(Gamemodes[indexModeThree]);
        randomGamemodes.Add(Gamemodes[indexModeFour]);
        randomGamemodes.Add(Gamemodes[indexModeFive]);
        randomGamemodes.Add(Gamemodes[indexModeSix]);

        currendGameIndex = 0;
    }

    [PunRPC]
    void PlayAnimSync()
    {
        PlayAnim();
    }

    [PunRPC]
    void StartNextGame()
    {
        AnimationObject.SetActive(false);
        randomGamemodes[currendGameIndex].gameObject.SetActive(true);
    }

#endregion

}


