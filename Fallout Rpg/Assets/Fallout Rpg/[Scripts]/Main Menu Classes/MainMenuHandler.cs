using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour {

	private bool showHome;
	private bool showLoadGame;
	private bool showNewGame;
	private bool keyPressed;
	public Texture2D splashImage;
	
	public Texture2D logo;
	public Texture2D mainMenuBgImage;

	// Use this for initialization
	void Start () {
		showHome = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown)
		{
			keyPressed = true;
		}
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (showHome) keyPressed = false;
			else
			{
				showHome = true;
				showNewGame = false;
				showLoadGame = false;
			}
		}
	}

	void OnGUI()
	{
		if (keyPressed)
		{
			GUI.DrawTexture(
				new Rect((float)(Screen.width / 2 - mainMenuBgImage.width / 2),
			         (float)(Screen.height / 2 - mainMenuBgImage.height / 2),
			         mainMenuBgImage.width,
			         mainMenuBgImage.height)
				, mainMenuBgImage);
			
			GUI.DrawTexture(
				new Rect((float)(Screen.width / 2 - logo.width / 2),
			         (float)(Screen.height / 2 - logo.height / 2 - 140),
			         logo.width,
			         logo.height)
				, logo);
			
			if (showHome) ShowHome();
			else if (showNewGame)
			{
				ShowNewGame();
				ShowBack();
			}
			else if (showLoadGame)
			{
				ShowLoadGame();
				ShowBack();
			}
			
		}
		else
		{
			GUI.DrawTexture(
				new Rect((float) (Screen.width/2 - splashImage.width/2),
			         (float) (Screen.height/2 - splashImage.height/2),
			         splashImage.width,
			         splashImage.height)
				, splashImage);
			
			GUI.Box(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 40, 800, 80), "<size=64>Fallout Rpg</size>");
			GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 55, 200, 50), "Press any key to continue");
		}
		
		
		
	}

	/// <summary>
	/// Shows the previous window.
	/// </summary>
	void ShowBack()
	{
		if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height - 35, 80, 30), "Back"))
		{
			showHome = true;
			showNewGame = false;
			showLoadGame = false;
		}
	}

	/// <summary>
	/// Shows the home.
	/// </summary>
	void ShowHome()
	{
		if (GUI.Button(new Rect(Screen.width / 2 - 110, Screen.height / 2 - 90, 220, 50), "New Game"))
		{
			showHome = false;
			showNewGame = true;
			showLoadGame = false;
		}
		
		if (GUI.Button(new Rect(Screen.width / 2 - 110, Screen.height / 2 - 35, 220, 50), "Load Game"))
		{
			showHome = false;
			showNewGame = false;
			showLoadGame = true;
		}
		
		if (GUI.Button(new Rect(Screen.width / 2 - 110, Screen.height / 2 + 20, 220, 50), "Exit Game"))
		{
			Application.Quit();
		}
	}
	
	/// <summary>
	/// Implement over-writing new games.
	/// </summary>
	void ShowNewGame()
	{
		string Slot1Info = GetInfo(0);
		if (GUI.Button(new Rect(Screen.width / 2 - 420 + 145, Screen.height / 2 - 90, 140, 100), Slot1Info))
		{
			Application.LoadLevel("character_generation");
		}
		
		string Slot2Info = GetInfo(1);
		if (GUI.Button(new Rect(Screen.width / 2 - 420 + 290, Screen.height / 2 - 90, 140, 100), Slot2Info))
		{
			Application.LoadLevel("character_generation");
		}
		
		string Slot3Info = GetInfo(2);
		if (GUI.Button(new Rect(Screen.width / 2 - 420 + 435, Screen.height / 2 - 90, 140, 100), Slot3Info))
		{
			Application.LoadLevel("character_generation");
		}
		
		string Slot4Info = GetInfo(3);
		if (GUI.Button(new Rect(Screen.width / 2 - 420 + 580, Screen.height / 2 - 90, 140, 100), Slot4Info))
		{
			Application.LoadLevel("character_generation");
		}
	}
	
	/// <summary>
	/// Shows Load Game Options.
	/// </summary>
	void ShowLoadGame()
	{
		
		string Slot1Info = GetInfo(0);
		if (GUI.Button(new Rect(Screen.width / 2 - 420 + 145, Screen.height / 2 - 90, 140, 100), Slot1Info))
		{

		}
		
		string Slot2Info = GetInfo(1);
		if (GUI.Button(new Rect(Screen.width / 2 - 420 + 290, Screen.height / 2 - 90, 140, 100), Slot2Info))
		{

		}
		
		string Slot3Info = GetInfo(2);
		if (GUI.Button(new Rect(Screen.width / 2 - 420 + 435, Screen.height / 2 - 90, 140, 100), Slot3Info))
		{

		}
		
		string Slot4Info = GetInfo(3);
		if (GUI.Button(new Rect(Screen.width / 2 - 420 + 580, Screen.height / 2 - 90, 140, 100), Slot4Info))
		{
		
		}
	}
	/// <summary>
	/// Gets the informations about the file.
	/// </summary>
	/// <returns>The info about the save.</returns>
	/// <param name="slotNumber">Slot number.</param>
	string GetInfo(int slotNumber)
	{
		string SaveLoadSlot = slotNumber.ToString() + "_";
		string info = "";
		info = "Empty \n Slot";
		return info;
	}

	/// <summary>
	/// Exits the game.
	/// </summary>
	void ExitGame()
	{
		Application.Quit();
	}
}

