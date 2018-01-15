using UnityEngine;

public class PongBallController : MonoBehaviour
{
    public Vector3 movementDirection;
    public float speed = 1f;
    private float initSpeed;
    public float maxSpeed = 5f;
    [Range(0, 1)] public float speedMultiplyer;
    ContactPoint[] points;
    private Vector3 beginDir;
    public GamemodePong gamemodePong;

    [Header("Network")]
    public PhotonView photonView;
    [SerializeField] Vector3 targetPosition;


    // Use this for initialization
    void Start()
    {
        //Setup Values
        beginDir = movementDirection;
        gamemodePong = GameObject.Find("Pong").GetComponent<GamemodePong>();
        initSpeed = speed;

        SetUpDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.connected)
        {
            Movement();
        }
        else
        {
            Movement();
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //Position
            stream.SendNext(movementDirection);
        }
        else
        {
            //Position
            movementDirection = (Vector3) stream.ReceiveNext();
        }
    }

    bool RandomStart()
    {
        int result = Random.Range(0, 2);
        if (result == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Movement()
    {
        movementDirection = movementDirection.normalized;
        transform.position += movementDirection * speed * Time.deltaTime;
        
        if(speed <= maxSpeed)
        {
            speed += Time.deltaTime * speedMultiplyer;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (collision.gameObject.tag == "Player")
                {
                    movementDirection.x *= -1f;
                    movementDirection.y = hitVelo(transform.position, collision.transform.position, collision.collider.bounds.size.y);
                }
                else if (collision.gameObject.tag == "Bounds")
                {
                    movementDirection.y *= -1f;
                }
            }
        }
    }

    float hitVelo(Vector3 ballPos, Vector3 coliderPos, float colliderSize)
    {
        return (ballPos.y - coliderPos.y) / colliderSize;
    }

    public void ResetBall(GameObject site)
    {
        if(site.name == "LeftOut")
        {
            gamemodePong.gameScroes[1]++;
            gamemodePong.CheckIfEnd();
        }
        else if(site.name == "RightOut")
        {
            gamemodePong.gameScroes[0]++;
            gamemodePong.CheckIfEnd();
        }

        this.transform.position = Vector3.zero;
        
        SetUpDirection();

        speed = initSpeed;
    }

    void SetUpDirection()
    {
        if(RandomStart())
        {
            movementDirection.x = -1;
            movementDirection.y = 0;
        }
        else
        {
            movementDirection.x = 1;
            movementDirection.y = 0;
        }
    }

    void OnGUI()
    {
        string text = "Movement x: " + movementDirection.x + " Movement y: " + movementDirection.y;

        GUIStyle style = new GUIStyle();

        style.fontSize = 30;

        GUI.Label(new Rect(10, 10, 500, 100), text, style);
    }
}