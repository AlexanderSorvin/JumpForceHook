using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum GameMode
{
    idle,

    MainMenu,
    FinishMenu,
    GamePlay,
    Finish,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Делегат обработки изменения текущего режима игры
    /// </summary>
    /// <param name="mode">Новый режим игры</param>
    public delegate void ChangeNewModeHandler(GameMode mode);
    public static ChangeNewModeHandler ChangeNewMode;

    private GameMode mode;

    public GameMode Mode
    {
        get
        {
            return mode;
        }

        set
        {
            StartCoroutine(ChangingNewMode(value));
        }
    }

    private IEnumerator ChangingNewMode(GameMode value)
    {
        yield return new WaitForFixedUpdate();
        if (mode != value)
        {
            mode = value;
            ChangeNewMode?.Invoke(mode);
            Debug.Log("New mode is " + mode);
        }
        else
        {
            Debug.LogWarning("Current mode request to change " + mode);
        }
    }

    private void Start()
    {
        Mode = GameMode.MainMenu;
    }
}
