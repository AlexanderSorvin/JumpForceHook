using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinish : MonoBehaviour
{
    Transform t;

    private void Start()
    {
        GameManager.ChangeNewMode += ChangeMode;
        t = GetComponent<Transform>();
    }

    void ChangeMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.GamePlay:
                GameController.PlayerPosition += FinishCheckEvent;
                break;
            case GameMode.Finish:
            case GameMode.GameOver:
                GameController.PlayerPosition -= FinishCheckEvent;
                break;
            default:
                break;
        }
    }


    void FinishCheckEvent(Rigidbody2D player)
    {
        if (t.position.x < player.position.x)
        {
            GameManager.Instance.Mode = GameMode.Finish;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(transform.position.x, - 500.0f, 0.0f), new Vector3(transform.position.x, 500.0f, 0.0f));

    }
}
