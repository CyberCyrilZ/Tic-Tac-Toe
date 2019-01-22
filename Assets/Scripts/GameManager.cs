using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour {
	
	public GameObject RestartButton;
	
	[NonSerialized]
	public bool Turn;

	private Text _xPointsText;
	private Text _oPointsText;
	private Text _winText;
	private BoardManager _boardScript; 
	private int _xPoints;
	private int _oPoints;
	private int _round;
	private bool _end;

	[Inject]
	public void Construct(BoardManager boardScript)
	{
		_boardScript = boardScript;
	}

	private void Start ()
	{
		Turn = true;
		_round = 1;
		_xPointsText = GameObject.Find("XPointsText").GetComponent<Text>();
		_oPointsText = GameObject.Find("OPointsText").GetComponent<Text>();
		_winText = GameObject.Find("WinText").GetComponent<Text>();
		_winText.text = "";
		RestartButton = GameObject.Find("ButtonRestart");
		RestartButton.SetActive(false);
	}

	public void Update ()
	{
		if (_end)
		{
			RestartButton.gameObject.SetActive(true);
			_end = false;
		}
		if (Input.GetKey("q"))
		{
			EndGame();
		}
	}
	
	public void AddPoints(string xo)
	{
		if (xo == "X")
		{
			_xPoints = _xPoints + 5;
			_xPointsText.text = "Your points: " + _xPoints;
		}
			
		if (xo == "O")
		{
			_oPoints = _oPoints + 5;
			_oPointsText.text = "Enemy points: " + _oPoints;
		}
		if (xo == "X1")
		{
			_xPoints++;
			_xPointsText.text = "Your points: " + _xPoints;
		}
			
		if (xo == "O1")
		{
			_oPoints++;
			_oPointsText.text = "Enemy points: " + _oPoints;
		}
	}

	public void EndGame()
	{
		if (_xPoints > _oPoints)
		{
			_winText.text = "You win!";
		
		}
		else if (_oPoints > _xPoints )
		{
			_winText.text = "You lose!";
			
		}
		else 
		{
			_winText.text = "Draw!";
		}
		_end = true;
	}
	
	public void AddRound()
	{
		_round++;
		
	}

	public bool IsNewRound()
	{
		return _round > 6;
	}

	public void ClearRound()
	{
		_round = 1;
	}

	public void PlayerTurn(bool botTurn)
	{
		if (botTurn == false)
		{
			Turn = false;
		}
		else
		{
			_boardScript.BotPlays();
			Turn = true;
		}
	}

}
