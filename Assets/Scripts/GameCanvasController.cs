using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] protected Text currentLevel;
    [SerializeField] protected Text nextLevel;
    [SerializeField] protected Scrollbar scrollbarLevel;

    float startPosition;
    float finishPosition;

    private void OnEnable()
    {
        LevelManager.CheckGameFinishPosition += CheckGameFinishPosition;
        GameController.PlayerPosition += CheckPlayerPosition;
        GameManager.ChangeNewMode += ChangeMode;

        currentLevel.text = LevelManager.Instance.Level.ToString();
        nextLevel.text = (LevelManager.Instance.Level + 1).ToString();
    }

    void ChangeMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.GamePlay:
                startPosition = 0.0f;
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        LevelManager.CheckGameFinishPosition -= CheckGameFinishPosition;
        GameController.PlayerPosition -= CheckPlayerPosition;
        GameManager.ChangeNewMode -= ChangeMode;
    }

    private void CheckGameFinishPosition(Vector2 pos)
    {
        finishPosition = pos.x;
    }

    private void CheckPlayerPosition(Rigidbody2D player)
    {
        if (startPosition == 0.0f)
        {
            scrollbarLevel.size = 0.0f;
            startPosition = player.position.x;
        }
        else
        {
            scrollbarLevel.size = 
                Mathf.Clamp01((player.position.x - startPosition)/(finishPosition - startPosition));
        }
    }
}
