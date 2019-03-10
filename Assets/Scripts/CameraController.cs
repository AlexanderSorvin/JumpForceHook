using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float V = 0.1f;
    Rigidbody2D rb;
    Camera Camera;
    //[SerializeField] protected float positionY;
    Vector2 startPos;
    float delta;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Camera = GetComponent<Camera>();
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
                Camera.orthographicSize = 5.0f;
                rb.position = startPos;
                GameController.PlayerPosition += CameraMove;
                break;
            case GameMode.GameOver:
                GameController.PlayerPosition -= CameraMove;
                break;
            case GameMode.Finish:
                GameController.PlayerPosition -= CameraMove;
                GameController.PlayerPosition += CameraFinishMove;
                delta = 0.0f;
                break;
            default:
                break;
        }
    }

    void CameraMove(Rigidbody2D player)
    {
        if (delta == 0.0f) delta = rb.position.x - player.position.x;
        rb.position += Vector2.right * V * (player.position.x + delta - rb.position.x);
    }

    void CameraFinishMove(Rigidbody2D player)
    {
        StartCoroutine(FinishAnimation(player));
        GameController.PlayerPosition -= CameraFinishMove;
    }

    IEnumerator FinishAnimation(Rigidbody2D player)
    {
        float time = 0;

        while (time < 1.0f)
        {
            Camera.orthographicSize = Mathf.Lerp(5f, 2.5f, time);
            rb.position = new Vector2(
                Mathf.Lerp(rb.position.x, player.position.x, time),
                Mathf.Lerp(rb.position.y, player.position.y, time)
                );

            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;
        }
    }
}
