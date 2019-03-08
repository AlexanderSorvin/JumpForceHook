using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController : Singleton<GameManager>
{

    public delegate void TapOnGameEventHandler();
    public static TapOnGameEventHandler TapOnGameEvent, TapOffGameEvent;
    public static TapOnGameEventHandler CheckNearestEvent;

    public delegate float DistanceToPointHandler(Vector2 vector, out Rigidbody2D rb, out RopeController rope);
    public static DistanceToPointHandler DistanceToPoint;

    public delegate void PlayerPositionHandler(Rigidbody2D vector);
    public static PlayerPositionHandler PlayerPosition;

    void ChangeMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.GamePlay:
                StartCoroutine(GameCheckEvent());
                break;
            default:
                break;
        }
    }

    protected bool CheckNearestRope()
    {
        CheckNearestEvent?.Invoke();

#if UNITY_EDITOR
        return Input.GetMouseButtonDown(0);
#else
        return Input.touchCount > 0;
#endif
    }

    protected IEnumerator GameCheckEvent()
    {
        while (GameManager.Instance.Mode == GameMode.GamePlay)
        {
            yield return new WaitUntil(CheckNearestRope);

            TapOnGameEvent?.Invoke();

#if UNITY_EDITOR
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
#else
            yield return new WaitUntil(() => Input.touchCount == 0);
#endif

            TapOffGameEvent?.Invoke();
        }
    }

    private void Start()
    {
        GameManager.ChangeNewMode += ChangeMode;
    }

}
