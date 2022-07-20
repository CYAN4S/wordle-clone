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
    public Action Pressed;

    public Action<char, string> Change;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        // Get buttons
        _enter = _root.Q<Button>("enter");
        _erase = _root.Q<Button>("erase");
        _keys = Enumerable
            .Range('a', 26)
            .Select(i => _root.Q<Button>($"{(char) i}"))
            .ToList();

        // Add events to buttons
        _enter.clickable.clicked += () => Entered?.Invoke();
        _enter.clickable.clicked += () => Pressed?.Invoke();

        _erase.clickable.clicked += () => Erased?.Invoke();
        _erase.clickable.clicked += () => Pressed?.Invoke();

        _keys.ForEach(key =>
            {
                key.clickable.clicked += () => Typed?.Invoke(key.name[0]);
                key.clickable.clicked += () => Pressed?.Invoke();
            }
        );

        Change += OnChange;
    }

    private void OnChange(char c, string className)
    {
        _keys[c - 'a'].RemoveFromClassList("hit");
        _keys[c - 'a'].RemoveFromClassList("ball");
        _keys[c - 'a'].RemoveFromClassList("miss");

        _keys[c - 'a'].AddToClassList(className);
    }
}