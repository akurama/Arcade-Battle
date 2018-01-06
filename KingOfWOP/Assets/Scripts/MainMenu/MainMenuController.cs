using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour 
{

	//All menu GameObjects
	public GameObject MainMenu;
	public GameObject OnlineMenu;
	public GameObject SettingsMenu;
	public GameObject LocalMenu;

	public EventSystem eventSystem;

	//Selected object
	private GameObject lastSelected;
	public Color selectedColor;

	[Header("OnlineLobby")]
	public GameObject playerNameInputField;

	// Use this for initialization
	void Start () 
	{
		MainMenu.SetActive(true);
		OnlineMenu.SetActive(false);
		SettingsMenu.SetActive(false);
		LocalMenu.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		HighlightSelectedText();
	}

	public void ChangeToOnlineMenu()
	{
		MainMenu.SetActive(false);
		LocalMenu.SetActive(false);
		OnlineMenu.SetActive(true);
		SettingsMenu.SetActive(false);

		playerNameInputField.GetComponent<TMP_InputField>().text = PlayerPrefs.GetString("PlayerName");
	}

	public void ChangeToSettingsMenu()
	{
		MainMenu.SetActive(false);
		LocalMenu.SetActive(false);
		OnlineMenu.SetActive(false);
		SettingsMenu.SetActive(true);

	}

	public void ChangeToMainMenu()
	{
		MainMenu.SetActive(true);
		LocalMenu.SetActive(false);
		OnlineMenu.SetActive(false);
		SettingsMenu.SetActive(false);
	}

	public void ChangeToLocalMenu()
	{
		MainMenu.SetActive(true);
		LocalMenu.SetActive(false);
		OnlineMenu.SetActive(false);
		SettingsMenu.SetActive(false);
	}

	void HighlightSelectedText()
	{
		if(lastSelected == null)
		{
			lastSelected = eventSystem.currentSelectedGameObject;
			lastSelected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedColor;
		}
		else
		{
			if(lastSelected != eventSystem.currentSelectedGameObject)
			{
				lastSelected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
				lastSelected = eventSystem.currentSelectedGameObject;
				lastSelected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedColor;
			}
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void GoOnline()
	{
		PlayerPrefs.SetString("PlayerName", playerNameInputField.GetComponent<TMP_InputField>().text);
		SceneManager.LoadScene("OnlineLobby");
	}
}
