using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptions : MonoBehaviour 
{
	public PhotonPlayer thisPlayer;
	public Canvas canvas;

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		canvas = GameObject.Find("RoomCanvas").GetComponent<Canvas>();
	}


	public void KickPlayer()
	{
		if(PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.CloseConnection(thisPlayer);
		}
	}

	public void OpenPlayerOptions()
	{
		if(PhotonNetwork.isMasterClient)
		{
			
		}
	}
}
