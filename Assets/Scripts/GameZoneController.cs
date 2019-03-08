using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZoneController : MonoBehaviour
{
    Transform t;

    [SerializeField] protected ZoneController[] StartZones;
    [SerializeField] protected ZoneController[] GameZones;
    [SerializeField] protected ZoneController FinishZone;

    [SerializeField] protected AnimationCurve LevelBlocks;

    enum TypeBlock
    {
        start,
        game,
        finish
    }

    class ElementInfo
    {
        public ElementInfo(ZoneController element, TypeBlock type, int number)
        {
            t = element.GetComponent<Transform>();
            zc = element.GetComponent<ZoneController>();
            obj = element.gameObject;
            this.type = type;
            this.number = number;
        }

        public readonly GameObject obj;
        public readonly TypeBlock type;
        public readonly int number;
        public readonly Transform t;
        public readonly ZoneController zc;
    }

    Queue<ElementInfo>[] BankGameZones;
    ElementInfo[] BankStartZones;
    ElementInfo BankFinishZone;

    Queue<ElementInfo> CurrentZones;

    private void OnEnable()
    {
        CreateNewLevel(LevelManager.Instance.Level);
        GameController.PlayerPosition += SleepSomeGameZone;
    }

    private void OnDisable()
    {
        GameController.PlayerPosition -= SleepSomeGameZone;
        if (GameManager.Instance.Mode == GameMode.FinishMenu)
        {
            ClearLevel();
        }
    }

    private void Awake()
    {
        t = GetComponent<Transform>(); 
        BankGameZones = new Queue<ElementInfo>[GameZones.Length];
        BankStartZones = new ElementInfo[StartZones.Length];
        BankFinishZone = new ElementInfo(Instantiate(FinishZone, t), TypeBlock.finish, 0);

        CurrentZones = new Queue<ElementInfo>();

        for (int i = 0; i < GameZones.Length; i++)
        {
            BankGameZones[i] = new Queue<ElementInfo>();
        }
    }

    void CreateNewLevel(int Level)
    {
        if (CurrentZones.Count > 0) return;

        int coutBlocks = Mathf.RoundToInt(LevelBlocks.Evaluate(Level));
        Vector3 offset = t.position;

        CurrentZones.Enqueue(CreateStartBlock(ref offset));

        for (int i = 0; i < coutBlocks; i++)
        {
            CurrentZones.Enqueue(CreateGameBlock(ref offset));
        }

        BankFinishZone.t.position = offset;
        BankFinishZone.obj.SetActive(true);
    }

    ElementInfo CreateStartBlock(ref Vector3 offset)
    {
        ElementInfo result;
        int num = Random.Range(0, StartZones.Length);

        if (BankStartZones[num] != null)
        {
            result = BankStartZones[num];
        }
        else
        {
            result = new ElementInfo(Instantiate(StartZones[num], t), TypeBlock.start, num);
            BankStartZones[num] = result;
        }

        result.t.position = offset;
        result.obj.SetActive(true);
        offset.x += result.zc.sizeX;
        return result;
    }

    ElementInfo CreateGameBlock(ref Vector3 offset)
    {
        ElementInfo result;
        int num = Random.Range(0, GameZones.Length);

        if (BankGameZones[num].Count > 0)
        {
            result = BankGameZones[num].Dequeue();
        }
        else
        {
            result = new ElementInfo(Instantiate(GameZones[num], t), TypeBlock.game, num);
        }

        result.t.position = offset;
        result.obj.SetActive(true);
        offset.x += result.zc.sizeX;
        return result;
    }

    void ClearLevel()
    {
        int cout = CurrentZones.Count;
        for (int i = 0; i < cout; i++)
        {
            ElementInfo element = CurrentZones.Dequeue();
            element.obj.SetActive(false);
            if (element.type == TypeBlock.game)
            {
                BankGameZones[element.number].Enqueue(element);
            }
        }
    }

    void SleepSomeGameZone(Rigidbody2D player)
    {
        int it = 0;
        ElementInfo prevElement = null;
        foreach (ElementInfo element in CurrentZones)
        {
            if (element.type == TypeBlock.start)
            {
                element.obj.SetActive(true);
            }
            else if (element.t.position.x + element.zc.sizeX > player.position.x && it == 0)
            {
                if (prevElement != null)
                    prevElement.obj.SetActive(true);
                element.obj.SetActive(true);
                it++;
            }
            else if (it > 0 && it < 2)
            {
                element.obj.SetActive(true);
                it++;
            }
            else
            {
                element.obj.SetActive(false);
                prevElement = element;
            }
        }
    }
}

