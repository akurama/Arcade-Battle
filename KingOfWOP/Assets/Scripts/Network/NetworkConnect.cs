using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class NetworkConnect : Photon.MonoBehaviour 
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
	private List<GameObject> roomEntryList;
	public GameObject roomEntry;
	public GameObject lobbyPanel;

	//PlayerCanvas
	public GameObject playerCanvas;
	private List <GameObject> playerEntryList;
	public GameObject playerEntry;
	public GameObject roomPanel;
	public int readyPlayers;
	public GameObject GoButton;
	public bool playerIsReady = false;

	void Awake()
	{
		SetNickname();
		MakeSingleton();		
	}

	// Use this for initialization
	void Start () 
	{
		roomEntryList = new List<GameObject>();
		playerEntryList = new List<GameObject>();

		InitUI();

		Debug.Log("Connecting to server.......");
		PhotonNetwork.ConnectUsingSettings("0.0.1");

	    PhotonNetwork.sendRate = 50;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!PhotonNetwork.isMasterClient)
			return;

		if(AllPlayersReady())
		{
			//Show Ready Button
			if(GoButton != null)
				GoButton.SetActive(true);
		}
		else
		{
			//Disappear ReadyButton
			if(GoButton != null)
				GoButton.SetActive(false);
		}
	}

	void OnDestroy()
	{
		#if !UNITY_EDITOR
		SceneManager.LoadScene("MainMenu");
		#endif
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
		ShowRooms();
	}

	private void OnJoinedRoom()
	{
		Debug.Log("Joined Room");
		OpenRoomCanvas();
		
		photonView.RPC("ShowPlayersInRoom", PhotonTargets.All);
	}

	private void OnLeftRoom()
	{
		Debug.Log("Room Left");
		CloseRoomCanvas();
	}

	private void OnPlayerJoined()
	{
		ShowPlayersInRoom();
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

			if(roomName == "")
			{
				string store = PhotonNetwork.playerName;
				roomName = store + "s Room";
			}

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
		GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
		roomName = clickedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
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
		GoButton.SetActive(false);		
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
			if(playerCountText != null)
				playerCountText.GetComponent<TextMeshProUGUI>().text = "Players Online: " + playerCount;
		}

		yield return new WaitForSeconds(5);
		StartCoroutine("RefreshPlayerCount");	
	}

	public void ShowRooms()
	{
		StartCoroutine("ClearRoomList");

		roomEntryList = new List<GameObject>();
		var roomList = PhotonNetwork.GetRoomList();

		foreach (var room in roomList)
		{
			if(room.PlayerCount < 2)
			{
				GameObject currentEntry = Instantiate(roomEntry, lobbyPanel.transform);
				roomEntryList.Add(currentEntry);
				currentEntry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
			}
		}
	}

	[PunRPC]
	public void ShowPlayersInRoom()
	{
		StartCoroutine("ClearPlayerList");
		
		playerEntryList = new List<GameObject>();
		var players = PhotonNetwork.playerList;
		
		foreach (var player in players)
		{
			GameObject currentEntry = Instantiate(playerEntry, roomPanel.transform);
			currentEntry.GetComponent<PlayerOptions>().thisPlayer = player;
			playerEntryList.Add(currentEntry);
			currentEntry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.NickName;
		}
	}

	public void PressReady()
	{

	}

	IEnumerator ClearRoomList()
	{
		foreach (var room in roomEntryList)
		{
			Destroy(room);
		}
		yield return new WaitForSeconds(0.1f);
	}

	IEnumerator ClearPlayerList()
	{
		foreach (var player in playerEntryList)
		{
			Destroy(player);
		}
		yield return new WaitForSeconds(0.1f);
	}

	void InitUI()
	{
		mainCanvas.SetActive(true);
		roomCreateCanvas.SetActive(false);
		roomCanvas.SetActive(false);
	}

	bool AllPlayersReady()
	{
		if(readyPlayers == PhotonNetwork.playerList.Length)
		{
			return true;
		}
		
		return false;
	}

	public void ReadyButtonPressed()
	{
		photonView.RPC("LoadGameScene", PhotonTargets.All);
	}

	public void OnReadyPressed()
	{
		if(!PhotonNetwork.player.IsLocal)
			return;

		playerIsReady = !playerIsReady;
		photonView.RPC("PlayerPressedReady", PhotonTargets.All, playerIsReady);
	}

	[PunRPC]
	void LoadGameScene()
	{
		SceneManager.LoadScene("GameSceneOnline");
	}

	[PunRPC]
	void PlayerPressedReady(bool isReady)
	{
		if(isReady)
		{
			readyPlayers++;
		}
		else
		{
			readyPlayers--;
		}
	}
}