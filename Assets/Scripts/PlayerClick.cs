using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerClick : MonoBehaviour
{
    
    public GameObject X;
    private Vector2 _framePostion;
    private int _xInd;
    private int _yInd;
    private Transform _tf;

    public GameManager _gameScript;
    public BoardManager _boardScript;

    [Inject]
    public void Construct(GameManager gameScript, BoardManager boardScript, int xInd, int yInd)
    {
        _gameScript = gameScript;
        _boardScript = boardScript;
        _xInd = xInd;
        _yInd = yInd;
    }

    private void CreateCell(int x, int y)
    {
        _framePostion = new Vector2(x,y);
    }
    
    private void Start()
    {   
        _tf = GetComponent<Transform>();
        _tf.position =  new Vector2(_xInd,_yInd) ;
    }
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) & _gameScript.Turn)
        {
            PlayerX(); 
        }
    }

    private void PlayerX()
    {
        if (CompareTag("X"))
        {
            return;
        }
        Instantiate(X, _tf.position, _tf.rotation);
        _boardScript.V3(_xInd, _yInd, "X");
        _gameScript.PlayerTurn(true);
    }
    
    public class Factory : PlaceholderFactory<int,int,PlayerClick>
    {
    }
}
