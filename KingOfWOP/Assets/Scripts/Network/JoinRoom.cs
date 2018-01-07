using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoom : MonoBehaviour 
{
	public void OnJoinPressed()
	{
		GameObject.Find("NetworkManager").GetComponent<NetworkConnect>().JoinRoom();
	}	
}
