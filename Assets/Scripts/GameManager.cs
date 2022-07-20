using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Keyboard _keyboard;
    private Board _board;
    private Alert _alert;

    private int _attempt = 0;

    private string _current = "";
    private string _answer;

    private bool _isInGame = true;

    private Dictionary<char, Judge> _status = new();

    private void Awake()
    {
        _keyboard = GetComponent<Keyboard>();
        _board = GetComponent<Board>();
        _alert = GetComponent<Alert>();

        _keyboard.Entered += OnEntered;
        _keyboard.Erased += OnErased;
        _keyboard.Typed += OnTyped;
        Prepare();
    }

    private void OnTyped(char c)
    {
        if (!_isInGame)
        {
            SceneManager.LoadScene(0);
            return;
        }

        if (_current.Length >= 5) return;

        _board.Set(_attempt, _current.Length, c);
        _current += c;
    }

    private void OnErased()
    {
        if (!_isInGame)
        {
            SceneManager.LoadScene(0);
            return;
        }

        if (_current.Length == 0) return;

        _current = _current.Remove(_current.Length - 1);
        _board.Erase(_attempt, _current.Length);
    }

    private void OnEntered()
    {
        if (!_isInGame)
        {
            SceneManager.LoadScene(0);
            return;
        }

        if (_current.Length != 5)
        {
            _alert.Show(Alert.Short);
            return;
        }

        if (Const.Words.Contains(_current))
        {
            var judges = Check(_answer, _current);

            for (var index = 0; index < judges.Count; index++)
            {
                var className = judges[index] switch
                {
                    Judge.Hit => "hit",
                    Judge.Ball => "ball",
                    _ => "miss"
                };

                _board.Change(_attempt, index, className);

                if (_status.ContainsKey(_current[index]))
                {
                    if (_status[_current[index]] <= judges[index]) continue;

                    _status[_current[index]] = judges[index];
                    _keyboard.Change(_current[index], className);
                }
                else
                {
                    _status.Add(_current[index], judges[index]);
                    _keyboard.Change(_current[index], className);
                }
            }

            if (judges.All(j => j == Judge.Hit))
            {
                _alert.Show(Alert.Score(_attempt));
                _isInGame = false;
                return;
            }

            _attempt += 1;
            _current = "";

            if (_attempt == 6)
            {
                _alert.Show(Alert.Answer(_answer));
                _isInGame = false;
            }
            else
            {
                _alert.Show("");
            }
        }
        else
        {
            _alert.Show(Alert.NoWord);
        }
    }


    private void Prepare()
    {
        _answer = Const.Words[new System.Random().Next(0, Const.Words.Length)];
    }

    public enum Judge
    {
        Hit,
        Ball,
        Miss
    }

    private static List<Judge> Check(string answer, string input)
    {
        var tmp = new Dictionary<char, int>();
        var x = new List<Tuple<int, char>>();
        var result = new List<Judge> {Judge.Hit, Judge.Hit, Judge.Hit, Judge.Hit, Judge.Hit};

        for (var i = 0; i < 5; i++)
        {
            if (answer[i] == input[i])
                continue;

            if (tmp.ContainsKey(answer[i]))
                tmp[answer[i]] += 1;
            else
                tmp.Add(answer[i], 1);

            x.Add(new Tuple<int, char>(i, input[i]));
        }

        foreach (var tuple in x)
        {
            if (tmp.ContainsKey(tuple.Item2))
            {
                result[tuple.Item1] = Judge.Ball;
                tmp[tuple.Item2] -= 1;

                if (tmp[tuple.Item2] == 0)
                    tmp.Remove(tuple.Item2);
            }
            else
                result[tuple.Item1] = Judge.Miss;
        }

        return result;
    }
}