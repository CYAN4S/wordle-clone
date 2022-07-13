using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    private VisualElement _root;
    private List<List<Label>> _blocks;

    public Action<int, int, char> Set;
    public Action<int, int> Erase;
    public Action<int, int, string> Change;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _blocks = Enumerable
            .Range(0, 6)
            .Select(i => Enumerable
                .Range(0, 5)
                .Select(j => _root.Q<Label>($"board-{i}-{j}"))
                .ToList()
            )
            .ToList();

        Set += OnSet;
        Erase += OnErase;
        Change += OnChange;
    }


    private void OnSet(int r, int c, char x)
    {
        _blocks[r][c].text = x.ToString().ToUpper();
    }

    private void OnErase(int r, int c)
    {
        _blocks[r][c].text = "";
    }

    private void OnChange(int r, int c, string s)
    {
        _blocks[r][c].AddToClassList(s);
    }
}