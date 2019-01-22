using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class BoardManager: MonoBehaviour
{
	public GameObject Frame;
	public GameObject O;
	public GameObject[] Lines;
	public int Rows;
	public int Columns;
	public int StartRows;
	public int StartColumns;
	public int OpenCells;
	
	private GameObject _line;
	private Transform _board;
	private GameManager _gameScript; 
	private List<GridStatus> _grid = new List<GridStatus>();

	PlayerClick.Factory _playerClickFactory;


	[Inject]
	public void Construct(GameManager gameScript,PlayerClick.Factory enemyFactory)
	{
		_gameScript = gameScript;
		_playerClickFactory = enemyFactory;
	}
	

	private void Start ()
	{
		BuildGrid();
		BuildBoard();
	}

	private void BuildGrid()
	{
		_grid.Clear();
		for (int x = 1; x <= Columns; x++)
		{
			for (int y = 1; y <= Rows; y++)
			{
				_grid.Add(new GridStatus(){isEmpty = "hide",isX = x,isY = y});
			}
		}
	}

	private void Update()
	{
		if (_gameScript.IsNewRound())
		{
			OpenNewCells();
			_gameScript.ClearRound();
			
		}
	}

	private void BuildBoard()
	{
		
		_board = new GameObject("Board").transform;

		for (int x = 5; x <= StartColumns + 4 ; x++)
		{
			for (int y = 5; y <= StartRows + 4; y++)
			{
				_grid.First(d => d.isX ==  x & d.isY == y).isEmpty = "empty";
				_playerClickFactory.Create(x,y);
			}
		}
	}

	public void BotPlays()
	{
		var freeGrid = _grid.FindAll(s => s.isEmpty == "empty");
		if (freeGrid.Count != 0)
		{
			int randomFreeCells = Random.Range(0, freeGrid.Count);
			var freeCells = freeGrid[randomFreeCells];
			_grid.First(d => d.isX == freeCells.isX & d.isY == freeCells.isY).isEmpty = "empty";
			Instantiate(O, new Vector2(freeCells.isX,freeCells.isY),Quaternion.identity);
			V3(freeCells.isX,freeCells.isY,"O");
		}
		else
		{
			GameEnd();
		}
	}

	private void GameEnd()
	{
		var endGridX = _grid.FindAll(s => s.isEmpty == "X");
		for (int i=0;  i < endGridX.Count; i++)
		{
			_gameScript.AddPoints("X1");
		
		}
		var endGridO = _grid.FindAll(s => s.isEmpty == "O");
		for (int i=0;  i < endGridO.Count; i++)
		{
			_gameScript.AddPoints("O1");
		}
		_gameScript.EndGame();
	}

	private void OpenNewCells()
	{
		for (int i = 1; i <= OpenCells; i++)
		{
			var openGrid = _grid.FindAll(s => s.isEmpty == "hide");
			if(openGrid.Count != 0)
			{
				int randomCell = Random.Range(0, openGrid.Count);
				var openCell = openGrid[randomCell];
				_grid.First(d => d.isX == openCell.isX & d.isY == openCell.isY).isEmpty = "empty";
				_playerClickFactory.Create(openCell.isX,openCell.isY);
			}
		}
	}
	
	private bool CheckCell(string status, int curX, int curY)
	{
		if (_grid.Contains(new GridStatus {isEmpty = status, isX = curX, isY = curY})) return true;
		return false;
	}

	private void PointedCell(string status,int curX, int curY, int lineStatus)
	{
		_grid.First(d => d.isX ==  curX & d.isY == curY).isEmpty = status + "pointed";
		_gameScript.AddPoints(status);
		
		switch (lineStatus)
		{
			//horizontal line
			case 1:
				 _line = Lines[0];
				break;
				
			//vertical line
			case 2:
				 _line = Lines[1];
				break;
			// left-right line
			case 3:
				_line = Lines[2];
				break;
			// right-left line
			case 4:
				_line = Lines[3];
				break;
		}
		Instantiate(_line,new Vector2(curX, curY),Quaternion.identity);
	}
	
	#region Check cell in the grid for win points
	
	public void V3(int currentX, int currentY, string currentStatus)
	{
		if (currentStatus != null)
		{
			// Edit one grid 
			_grid.First(d => d.isX == currentX & d.isY == currentY).isEmpty = currentStatus;
			_gameScript.AddRound();
		}
		
		if (CheckCell(currentStatus, currentX - 1, currentY + 1))
		{
			if (CheckCell(currentStatus, currentX - 2, currentY + 2))
			{
				PointedCell(currentStatus, currentX, currentY, 3);
				PointedCell(currentStatus, currentX - 1, currentY + 1, 3);
				PointedCell(currentStatus, currentX - 2, currentY + 2, 3);
				return;
			}
			if (CheckCell(currentStatus, currentX + 1, currentY - 1))
			{
				PointedCell(currentStatus, currentX, currentY, 3);
				PointedCell(currentStatus, currentX - 1, currentY + 1, 3);
				PointedCell(currentStatus, currentX + 1, currentY - 1, 3);
				return;
			}
		}
		
		if (CheckCell(currentStatus, currentX, currentY + 1))
		{
			if (CheckCell(currentStatus, currentX, currentY + 2))
			{
				PointedCell(currentStatus, currentX, currentY, 2);
				PointedCell(currentStatus, currentX, currentY + 1, 2);
				PointedCell(currentStatus, currentX, currentY + 2, 2);
				return;
			}
			if (CheckCell(currentStatus, currentX, currentY - 1))
			{
				PointedCell(currentStatus, currentX, currentY, 2);
				PointedCell(currentStatus, currentX, currentY + 1, 2);
				PointedCell(currentStatus, currentX, currentY - 1, 2);
				return;
			}
		}
		
		if (CheckCell(currentStatus, currentX + 1, currentY + 1))
		{
			if (CheckCell(currentStatus, currentX + 2, currentY + 2))
			{
				PointedCell(currentStatus, currentX, currentY, 4);
				PointedCell(currentStatus, currentX + 1, currentY + 1, 4);
				PointedCell(currentStatus, currentX + 2, currentY + 2, 4);
				return;
			}
			if (CheckCell(currentStatus, currentX - 1, currentY - 1))
			{
				PointedCell(currentStatus, currentX, currentY, 4);
				PointedCell(currentStatus, currentX + 1, currentY + 1, 4);
				PointedCell(currentStatus, currentX - 1, currentY - 1, 4);
				return;
			}
		}
		
		if (CheckCell(currentStatus, currentX + 1, currentY))
		{
			if (CheckCell(currentStatus, currentX + 2, currentY))
			{
				PointedCell(currentStatus, currentX, currentY, 1);
				PointedCell(currentStatus, currentX + 1, currentY, 1);
				PointedCell(currentStatus, currentX + 2, currentY, 1);
				return;
			}
			if (CheckCell(currentStatus, currentX - 1, currentY))
			{
				PointedCell(currentStatus, currentX, currentY, 1);
				PointedCell(currentStatus, currentX + 1, currentY, 1);
				PointedCell(currentStatus, currentX - 1, currentY, 1);
				return;
			}
		}
		
		if (CheckCell(currentStatus, currentX + 1, currentY - 1))
		{
			if (CheckCell(currentStatus, currentX + 2, currentY - 2))
			{
				PointedCell(currentStatus, currentX, currentY, 3);
				PointedCell(currentStatus, currentX + 1, currentY - 1, 3);
				PointedCell(currentStatus, currentX + 2, currentY - 2, 3);
				return;
			}
			if (CheckCell(currentStatus, currentX - 1, currentY + 1))
			{
				PointedCell(currentStatus, currentX, currentY, 3);
				PointedCell(currentStatus, currentX + 1, currentY - 1, 3);
				PointedCell(currentStatus, currentX - 1, currentY + 1, 3);
				return;
			}
		}
	
		if (CheckCell(currentStatus, currentX, currentY - 1))
		{
			if (CheckCell(currentStatus, currentX, currentY - 2))
			{
				PointedCell(currentStatus, currentX, currentY, 2);
				PointedCell(currentStatus, currentX, currentY - 1, 2);
				PointedCell(currentStatus, currentX, currentY - 2, 2);
				return;
			}
			if (CheckCell(currentStatus, currentX, currentY + 1))
			{
				PointedCell(currentStatus, currentX, currentY, 2);
				PointedCell(currentStatus, currentX, currentY - 1, 2);
				PointedCell(currentStatus, currentX, currentY + 1, 2);
				return;
			}
			
		}
		
		if (CheckCell(currentStatus, currentX - 1, currentY - 1))
		{
			if (CheckCell(currentStatus, currentX - 2, currentY - 2))
			{
				PointedCell(currentStatus, currentX, currentY, 4);
				PointedCell(currentStatus, currentX - 1, currentY - 1, 4);
				PointedCell(currentStatus, currentX - 2, currentY - 2, 4);
				return;
			}
			if (CheckCell(currentStatus, currentX + 1, currentY + 1))
			{
				PointedCell(currentStatus, currentX, currentY, 4);
				PointedCell(currentStatus, currentX - 1, currentY - 1, 4);
				PointedCell(currentStatus, currentX + 1, currentY + 1, 4);
				return;
			}
		}
		
		if (CheckCell(currentStatus, currentX - 1, currentY))
		{
			if (CheckCell(currentStatus, currentX - 2, currentY))
			{
				PointedCell(currentStatus, currentX, currentY, 1);
				PointedCell(currentStatus, currentX - 1, currentY, 1);
				PointedCell(currentStatus, currentX - 2, currentY, 1);
				return;
			}
			if (CheckCell(currentStatus, currentX + 1, currentY))
			{
				PointedCell(currentStatus, currentX, currentY, 1);
				PointedCell(currentStatus, currentX - 1, currentY, 1);
				PointedCell(currentStatus, currentX + 1, currentY, 1);
				return;
			}
		}
		
	}
	
	#endregion	
	
}
