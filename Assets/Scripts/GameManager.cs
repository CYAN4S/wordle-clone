using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Keyboard _keyboard;
    private Board _board;

    private int _attempt = 0;
    
    private string _current = "";
    private string _answer;

    private void Awake()
    {
        _keyboard = GetComponent<Keyboard>();
        _board = GetComponent<Board>();

        _keyboard.Entered += OnEntered;
        _keyboard.Erased += OnErased;
        _keyboard.Typed += OnTyped;
        
        Prepare();
    }

    private void OnTyped(char c)
    {
        if (_current.Length >= 5) return;

        _board.Set(_attempt, _current.Length, c);
        _current += c;
        
        Debug.Log(_current);
    }

    private void OnErased()
    {
        if (_current.Length == 0) return;

        _current = _current.Remove(_current.Length - 1);
        _board.Erase(_attempt, _current.Length);

        Debug.Log(_current);
    }

    private void OnEntered()
    {
        if (_current.Length != 5) return;

        if (Const.Words.Contains(_current))
        {
            Debug.Log("Valid");

            var bs = Check();
            for (var index = 0; index < bs.Count; index++)
            {
                var b = bs[index];
                _board.Change(_attempt, index, b ? "hit" : "miss");
            }

            _attempt += 1;
            _current = "";
        }
        else
        {
            Debug.Log("Not in word list");
        }

    }
    
    
    private void Prepare()
    {
        _answer = Const.Words[new System.Random().Next(0, Const.Words.Length)];
    }

    private List<bool> Check()
    {
        return Enumerable.Range(0, 5).Select((i) => _answer[i] == _current[i]).ToList();
    }
    
    
}
