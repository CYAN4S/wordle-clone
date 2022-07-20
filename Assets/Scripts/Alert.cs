using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Alert : MonoBehaviour
{
    private Label _label;

    private void Awake()
    {
        _label = GetComponent<UIDocument>().rootVisualElement.Q<Label>("alert");
    }

    public static string Answer(string answer) => $"The answer was {answer.ToUpper()}";

    public static string Score(int attempt) => attempt switch
    {
        0 => "Lucky!",
        1 => "Awesome!",
        2 => "Great!",
        3 => "So Good!",
        4 => "Good!",
        _ => "That was close!"
    };

    public const string NoWord = "Not in word list.";
    public const string Short = "Too short.";

    public void Show(string msg)
    {
        _label.text = msg;
    }
}