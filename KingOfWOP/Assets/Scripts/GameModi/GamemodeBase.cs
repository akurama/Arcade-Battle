using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeBase : MonoBehaviour
{
    public GameManager gameManager;
    public int[] gameScroes = new int[2];
    public int gameToTotal;
    public float time = 60f;
    private float storeTime = 0f;

    [SerializeField] private bool debug;

    public void InitStart()
    {
        if(debug)
            time /= 6;

        if(gameManager == null)
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        storeTime = time;
    }

    public virtual void Refresh()
    {
        if(storeTime > 0f)
            time = storeTime;
    }

    public virtual void CheckIfEnd()
    {

    }

    public virtual void GameEnd()
    {

    }

    void OnEnable()
    {
        Refresh();        
    }
}
