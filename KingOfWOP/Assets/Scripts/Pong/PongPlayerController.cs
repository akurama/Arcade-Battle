using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayerController : MonoBehaviour
{
    public PlayerNumber player;
    public float speed = 1f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float v;

        if (player == PlayerNumber.Player1)
        {
            v = Input.GetAxis("VerticalPlayer1");
        }
        else
        {
            v = Input.GetAxis("VerticalPlayer2");
        }

        this.GetComponent<Rigidbody>().velocity = new Vector3(0, v * speed, 0);

    }
}

public enum PlayerNumber
{
    Player1,
    Player2
}