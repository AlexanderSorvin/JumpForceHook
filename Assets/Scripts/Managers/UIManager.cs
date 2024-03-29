﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] protected GameObject MainMenu;
    [SerializeField] protected GameObject GameZone;
    [SerializeField] protected GameObject GameCanvas;

    private void Start()
    {
        GameManager.ChangeNewMode += ChangeNewMode;
    }

    private void ChangeNewMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.MainMenu:
            case GameMode.FinishMenu:
                GameZone.SetActive(false);
                GameCanvas.SetActive(false);
                MainMenu.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        GameZone.SetActive(true);
        GameCanvas.SetActive(true);
        MainMenu.SetActive(false);
        GameManager.Instance.Mode = GameMode.GamePlay;
    }
}
