using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GamemodeBase> Gamemodes;
    public List<GamemodeBase> randomGamemodes = new List<GamemodeBase>();

    public List<TextMeshProUGUI> texts;

    public int multiplyer = 1;
    public int[] scroes = new int[2];
    public int currendGameIndex = 0;

    // Use this for initialization
    void Start()
    {
        RandomOrder();
        multiplyer = 1;
        PlayAnim();
        FirstGame();

        Debug.Log(randomGamemodes.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayAnim()
    {
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
}
