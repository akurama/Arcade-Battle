using UnityEngine;

public class PongBallController : MonoBehaviour
{
    public Vector3 movementDirection;
    public float speed = 1f;
    ContactPoint[] points;
    private Vector3 beginDir;
    public GamemodePong gamemodePong;


    // Use this for initialization
    void Start()
    {
        beginDir = movementDirection;
        gamemodePong = GameObject.Find("Pong").GetComponent<GamemodePong>();

        if (RandomStart())
        {
            movementDirection = -movementDirection;
        }
        else
        {
            movementDirection = movementDirection;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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
        transform.position += movementDirection * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Left")
        {
            movementDirection.x = 1f;
            movementDirection.y = hitVelo(transform.position, collision.transform.position, collision.collider.bounds.size.y);
        }
        else if (collision.gameObject.name == "Right")
        {
            movementDirection.x = -1f;
            movementDirection.y = hitVelo(transform.position, collision.transform.position, collision.collider.bounds.size.y);
        }
        else if (collision.gameObject.name == "Bounds")
        {
            movementDirection.y *= -1f;
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
        if(RandomStart())
        {
            movementDirection = -movementDirection;
        }
        else
        {
            movementDirection = movementDirection;
        }
    }
}
