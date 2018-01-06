using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkConnect : MonoBehaviour 
{

	private string playerName;
	public static NetworkConnect instance;

	void Awake()
	{
		SetNickname();
		MakeSingleton();
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	private void OnConnectedToMaster()
	{
		
	}

	private void SetNickname()
	{
		playerName = PlayerPrefs.GetString("PlayerName") + "#" + Random.Range(1000, 9999);
	}

	private void MakeSingleton()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else if(instance != this)
		{
			Destroy(this);
		}
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene("OnlineLobby");
	}

}
