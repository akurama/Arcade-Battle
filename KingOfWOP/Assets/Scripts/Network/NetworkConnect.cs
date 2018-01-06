using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkConnect : MonoBehaviour 
{

	private string playerName;
	public static NetworkConnect instance;

	public GameObject mainCanvas;

	//PlayerCount
	public GameObject playerCountText;
	public bool refreshPlayerCountText;

	//CreateRoomStuff
	public GameObject roomCreateCanvas;
	public GameObject roomNameText;

	//RoomCanvas
	public GameObject roomCanvas;

	void Awake()
	{
		SetNickname();
		MakeSingleton();		
	}

	// Use this for initialization
	void Start () 
	{
		InitUI();
		Debug.Log("Connecting to server.......");
		PhotonNetwork.ConnectUsingSettings("0.0.1");	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnDestroy()
	{
		SceneManager.LoadScene("MainMenu");
	}

	//PhotonCallbacks
	private void OnConnectedToMaster()
	{
		Debug.Log("Connection succeded");
		PhotonNetwork.playerName = playerName;
		StartCoroutine("RefreshPlayerCount");
		PhotonNetwork.JoinLobby(TypedLobby.Default);
	}

	private void OnJoinedLobby()
	{
		Debug.Log("Joined Lobby.....");
	}

	private void OnJoinedRoom()
	{
		Debug.Log("Joined Room");
		OpenRoomCanvas();
	}

	private void OnLeftRoom()
	{
		Debug.Log("Room Left");
		CloseRoomCanvas();
	}

	private void OnDisconnectedFromPhoton()
	{
		instance = null;
		Destroy(this.gameObject);
	}
	//END Callbacks

	public void CreateRoom()
	{
		if(!PhotonNetwork.inRoom)
		{
			string roomName = roomNameText.GetComponent<TMP_InputField>().text;

			RoomOptions roomOptions = new RoomOptions();
			roomOptions.MaxPlayers = 2;

			TypedLobby typedLobby = new TypedLobby();
			typedLobby.Type = LobbyType.Default;

			PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
		}
	}

	public void JoinRoom()
	{
		string roomName = "";
		PhotonNetwork.JoinRoom(roomName);
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public void JoinRandomRoom()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public void OpenCreateRoomCanvas()
	{
		roomCreateCanvas.SetActive(true);
		mainCanvas.SetActive(false);
		roomCanvas.SetActive(false);
	}

	public void CloseCreateRoomCanvas()
	{
		mainCanvas.SetActive(true);
		roomCreateCanvas.SetActive(false);
		roomCanvas.SetActive(false);
	}

	public void OpenRoomCanvas()
	{
		roomCreateCanvas.SetActive(false);
		mainCanvas.SetActive(false);
		roomCanvas.SetActive(true);
	}

	public void CloseRoomCanvas()
	{
		roomCreateCanvas.SetActive(false);
		mainCanvas.SetActive(true);
		roomCanvas.SetActive(false);
	}

	public void Disconect()
	{
		PhotonNetwork.Disconnect();
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

	IEnumerator RefreshPlayerCount()
	{
		int playerCount = PhotonNetwork.countOfPlayersOnMaster;

		if(refreshPlayerCountText)
		{
			playerCountText.GetComponent<TextMeshProUGUI>().text = "Players Online: " + playerCount;
		}

		yield return new WaitForSeconds(5);
		StartCoroutine("RefreshPlayerCount");	
	}

	void InitUI()
	{
		mainCanvas.SetActive(true);
		roomCreateCanvas.SetActive(false);
		roomCanvas.SetActive(false);
	}

}