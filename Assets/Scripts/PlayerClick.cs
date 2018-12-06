using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerClick : MonoBehaviour
{
    
    public GameObject X;
    
    private int _xInd;
    private int _yInd;
    private Transform _tf;
    
    private void Start()
    {
        _tf = GetComponent<Transform>();
        _xInd = (int) _tf.position.x;
        _yInd = (int) _tf.position.y;
    }
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) & GameManager.Instance.Turn)
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
        BoardManager.Instance.V3(_xInd, _yInd, "X");
        GameManager.Instance.PlayerTurn(true);
    }
}
