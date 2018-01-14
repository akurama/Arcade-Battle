using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayerController : MonoBehaviour
{
    public PlayerNumber player;
    public float speed = 1f;

    [Header("Network")]
    private Vector3 targetPosition;
    public PhotonView photonView;

    // Use this for initialization
    void Start()
    {
        if(PhotonNetwork.connected)
        {
            photonView = GetComponent<PhotonView>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.connected)
        {
            if(photonView.isMine)
            {
                Movement();
            }
            else
            {
                SmoothMovement();
            }
        }
        else
        {
            Movement();
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            //Positon
            stream.SendNext(transform.position);
        }
        else
        {
            //Position
            targetPosition = (Vector3) stream.ReceiveNext();
        }
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

    void SmoothMovement()
    {

    }
}

public enum PlayerNumber
{
    Player1,
    Player2
}