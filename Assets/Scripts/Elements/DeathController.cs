using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
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
                GameController.PlayerPosition += DeathCheckEvent;
                break;
            case GameMode.Finish:
            case GameMode.GameOver:
                GameController.PlayerPosition -= DeathCheckEvent;
                break;
            default:
                break;
        }
    }


    void DeathCheckEvent(Rigidbody2D player)
    {
        if (t.position.y > player.position.y)
        {
            GameManager.Instance.Mode = GameMode.GameOver;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-500.0f, transform.position.y, 0.0f), new Vector3(500.0f, transform.position.y, 0.0f));

    }
}
