using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Keyboard : MonoBehaviour
{
    private Button _enterButton;

    private VisualElement _root;
    private Button _enter, _erase;
    private List<Button> _keys;

    public Action Entered;
    public Action Erased;
    public Action<char> Typed;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _enter = _root.Q<Button>("enter");
        _erase = _root.Q<Button>("erase");
        _keys = Enumerable
            .Range('a', 26)
            .Select(c => _root.Q<Button>($"{(char) c}"))
            .ToList();

        _enter.clickable.clicked += () => { Entered?.Invoke(); };
        _erase.clickable.clicked += () => { Erased?.Invoke(); };

        foreach (var key in _keys)
        {
            key.clickable.clicked += () => { Typed?.Invoke(key.name[0]); };
        }
    }
}