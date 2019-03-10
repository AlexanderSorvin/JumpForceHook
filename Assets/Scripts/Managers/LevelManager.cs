using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public HaskeyIntController Level;
    public delegate void CheckGameFinishPositionHandler(Vector2 pos);

    public static CheckGameFinishPositionHandler CheckGameFinishPosition;

    private void Awake()
    {
        Level = new HaskeyIntController("Level", 1);
        GameManager.ChangeNewMode += ChangeNewMode;
    }

    private void ChangeNewMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Finish:
                Level.Set(Level + 1);
                break;
            default:
                break;
        }
    }
}
