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

    // Use this for initialization
    void Start()
    {
        RandomOrder();
        multiplyer = 1;
        PlayAnim();
        FirstGame();
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
        randomGamemodes[0].gameObject.SetActive(true);
    }

    void RandomOrder()
    {
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
        
    }
}
