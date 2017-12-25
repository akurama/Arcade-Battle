using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongOutBounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PongBallController>().ResetBall(this.gameObject);
    }
}
