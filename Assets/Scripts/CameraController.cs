﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float V = 0.1f;
    Rigidbody2D rb;
    //[SerializeField] protected float positionY;
    Vector2 startPos;
    float delta;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.ChangeNewMode += ChangeNewMode;
        //GameController.PlayerPosition += CameraMovetoPlayer;
    }

    private void Start()
    {
        startPos = rb.position;
    }

    private void ChangeNewMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.GamePlay:
                rb.position = startPos;
                GameController.PlayerPosition += CameraMove;
                break;
            case GameMode.GameOver:
                GameController.PlayerPosition -= CameraMove;
                break;
            case GameMode.Finish:
                GameController.PlayerPosition -= CameraMove;
                delta = 0.0f;
                break;
            default:
                break;
        }
    }

    //void CameraMovetoPlayer(Rigidbody2D player)
    //{
    //    startPos = new Vector2(player.position.x, positionY);
    //    GameController.PlayerPosition -= CameraMovetoPlayer;
    //}

    void CameraMove(Rigidbody2D player)
    {
        if (delta == 0.0f) delta = rb.position.x - player.position.x;
        rb.position += Vector2.right * V * (player.position.x + delta - rb.position.x);
    }
}