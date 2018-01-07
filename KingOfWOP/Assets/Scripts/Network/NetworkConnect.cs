using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
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
	private List<GameObject> roomEntryList;
	public GameObject roomEntry;
	public GameObject lobbyPanel;

	//PlayerCanvas
	public GameObject playerCanvas;
	private List <GameObject> playerEntryList;
	public GameObject playerEntry;
	public GameObject roomPanel;

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
	}
	
	// Update is called once per frame
	void Update () 
	{

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
		ShowPlayersInRoom();
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

	public void ShowRooms()
	{
		StartCoroutine("ClearRoomList");

		roomEntryList = new List<GameObject>();
		var roomList = PhotonNetwork.GetRoomList();

		foreach (var room in roomList)
		{
			GameObject currentEntry = Instantiate(roomEntry, lobbyPanel.transform);
			roomEntryList.Add(currentEntry);
			currentEntry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
		}
	}

	public void ShowPlayersInRoom()
	{
		StartCoroutine("ClearPlayerList");
		
		playerEntryList = new List<GameObject>();
		var players = PhotonNetwork.playerList;
		
		foreach (var player in players)
		{
			GameObject currentEntry = Instantiate(playerEntry, roomPanel.transform);
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
}