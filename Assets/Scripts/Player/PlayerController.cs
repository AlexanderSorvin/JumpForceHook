using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    GameObject obj;
    Transform t;
    Rigidbody2D rb;
    BoxCollider2D bc;
    DistanceJoint2D dj;
    LineRenderer lr;
    TrailRenderer tr;
    Animation an;
    [SerializeField] protected GameObject [] playerPhase;
    [SerializeField] protected Transform playerImage;



    [SerializeField] protected float MaximumSpeed;

    enum TypePlayerPhase
    {
        idle,

        ball,
        left1,
        left2,
        right1,
        right2,
        middle,
        finish
    }

    TypePlayerPhase currentPlayerPhase;

    TypePlayerPhase CurrentPlayerPhase
    {
        get
        {
            return currentPlayerPhase;
        }

        set
        {
            if (value != currentPlayerPhase && value != 0)
            {
                if (currentPlayerPhase != TypePlayerPhase.idle)
                    playerPhase[(int)currentPlayerPhase - 1].SetActive(false);
                playerPhase[(int)value - 1].SetActive(true);
                currentPlayerPhase = value;
            }
        }
    }

    RopeController NearestRope;
    Rigidbody2D rbNearestRope;

    [SerializeField] float distanceReduction = 0.1f;

    Vector2 LevelStartPosition;

    [SerializeField] protected GameObject startEffectObj;
    [SerializeField] protected GameObject finishParticleObj;
    Transform startEffect;
    Animation startAn;
    Transform finishEffect;
    ParticleSystem finishParticle;

    [SerializeField] protected float finishTime = 3.0f;

    private void Awake()
    {
        obj = gameObject;
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        dj = GetComponent<DistanceJoint2D>();
        lr = GetComponent<LineRenderer>();
        tr = GetComponent<TrailRenderer>();
        an = GetComponent<Animation>();

        dj.enabled = false;
    }

    private void OnEnable()
    {
        GameController.TapOffGameEvent += TapOffGameEvent;
        GameManager.ChangeNewMode += ChangeNewMode;
        GameController.CheckNearestEvent += CheckNearestEvent;
        LevelStartPosition = rb.position;
    }

    private void OnDisable()
    {
        GameController.TapOnGameEvent -= TapOnGameEvent;
        GameController.TapOffGameEvent -= TapOffGameEvent;
        GameManager.ChangeNewMode -= ChangeNewMode;
        GameController.CheckNearestEvent -= CheckNearestEvent;
        TapOffGameEvent();
    }

    private void CheckNearestEvent()
    {
        float MaxDistance = 5.0f;

        if (rbNearestRope != null)
        {
            float dis = Vector2.Distance(rb.position, rbNearestRope.position);
            if (dis > MaxDistance)
            {
                NearestRope.Mode = RopeController.RopeMode.NotUsed;
                NearestRope = null;
                rbNearestRope = null;
            }
            else
            {
                MaxDistance = dis;
            }
        }

        if (GameController.DistanceToPoint != null)
            foreach (GameController.DistanceToPointHandler element in GameController.DistanceToPoint.GetInvocationList())
            {
                float dis = element.Invoke(rb.position, out Rigidbody2D Element, out RopeController rope);
                
                if (MaxDistance > dis)
                {
                    if (NearestRope != null)
                        NearestRope.Mode = RopeController.RopeMode.NotUsed;
                    rbNearestRope = Element;
                    NearestRope = rope;
                    MaxDistance = dis;
                    NearestRope.Mode = RopeController.RopeMode.Target;
                }
            }

        CurrentPlayerPhase = TypePlayerPhase.ball;
    }

    private void ChangeNewMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.GamePlay:
                if (startEffect == null)
                {
                    GameObject eventobj = Instantiate(startEffectObj, t.parent);
                    startEffect = eventobj.GetComponent<Transform>();
                    startAn = eventobj.GetComponent<Animation>();
                }
                t.localScale = Vector3.zero;
                startEffect.position = t.position;
                StartCoroutine(StartEffectController());
                break;
            case GameMode.GameOver:
                GameController.TapOnGameEvent -= TapOnGameEvent;
                rb.position = LevelStartPosition;
                rb.velocity = Vector2.zero;
                tr.Clear();
                tr.enabled = false;
                GameManager.Instance.Mode = GameMode.GamePlay;
                break;
            case GameMode.Finish:
                GameController.TapOnGameEvent -= TapOnGameEvent;
                dj.connectedBody = null;
                dj.enabled = false;
                tr.Clear();
                tr.enabled = false;
                if (finishEffect == null)
                {
                    GameObject eventobj = Instantiate(finishParticleObj, t.parent);
                    finishEffect = eventobj.GetComponent<Transform>();
                    finishParticle = eventobj.GetComponent<ParticleSystem>();
                }
                StartCoroutine(FinishEffectController());
                break;
            default:
                break;
        }
    }

    IEnumerator FinishEffectController()
    {
        finishEffect.position = rb.position;
        finishEffect.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * 180 / Mathf.PI + 90.0f);
        rb.rotation = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * 180 / Mathf.PI - 90;
        finishParticle.Play();
        rb.gravityScale = 0;
        rb.velocity = rb.velocity.normalized;
        CurrentPlayerPhase = TypePlayerPhase.finish;

        GameController.TapOnGameEvent -= TapOnGameEvent;
        GameController.TapOffGameEvent -= TapOffGameEvent;
        GameManager.ChangeNewMode -= ChangeNewMode;
        GameController.CheckNearestEvent -= CheckNearestEvent;

        yield return new WaitForSeconds(finishTime);

        rb.gravityScale = 1;
        finishParticle.Stop();
        rb.position = LevelStartPosition;
        rb.velocity = Vector2.zero;

        yield return new WaitForFixedUpdate();

        rb.simulated = false;
        GameManager.Instance.Mode = GameMode.FinishMenu;
    }

    IEnumerator StartEffectController()
    {

        rb.simulated = false;
        startAn.Play("AnimationWithOutPlayer");

        yield return new WaitUntil(() => !startAn.isPlaying);

        t.localScale = Vector3.one;
        an.Play("PlayerIsComeBack");
        startAn.Play("AnimationWithPlayer");

        yield return new WaitUntil(() => !an.isPlaying);

        tr.enabled = true;
        rb.simulated = true;
        GameController.TapOnGameEvent += TapOnGameEvent;

    }

    void TapOffGameEvent()
    {
        dj.enabled = false;
        if (LineRendererUpdateICoroutine != null) StopCoroutine(LineRendererUpdateICoroutine);
        lr.enabled = false;
        if (NearestRope != null) NearestRope.Mode = RopeController.RopeMode.Target;
    }

    void TapOnGameEvent()
    {
        if (rbNearestRope != null)
        {
            dj.enabled = true;
            lr.enabled = true;
            dj.autoConfigureDistance = true;
            dj.connectedBody = rbNearestRope;
            lr.SetPosition(0, rb.position);
            lr.SetPosition(1, rbNearestRope.position);
            if (LineRendererUpdateICoroutine != null) StopCoroutine(LineRendererUpdateICoroutine);
            LineRendererUpdateICoroutine = StartCoroutine(LineRendererUpdate(rbNearestRope));
            NearestRope.Mode = RopeController.RopeMode.Used;
        }

    }

    Coroutine LineRendererUpdateICoroutine;

    IEnumerator LineRendererUpdate(Rigidbody2D rigidbody)
    {
        yield return new WaitForFixedUpdate();
        dj.autoConfigureDistance = false;
        dj.distance -= distanceReduction;
        while (true)
        {
            rb.rotation = 90.0f + Mathf.Atan2(rb.position.y - rigidbody.position.y, rb.position.x - rigidbody.position.x) / Mathf.PI * 180;
            //Debug.Log(rb.rotation + " " + Mathf.Atan2(dj.reactionForce.y, dj.reactionForce.x) / Mathf.PI * 180 + " " + dj.reactionForce.y + " " + dj.reactionForce.x);
            //Debug.DrawRay(rb.position, dj.reactionForce);

            if (Mathf.Abs(rb.velocity.x) < 0.1f && Mathf.Abs(rb.velocity.y) < 0.1f)
            {
                CurrentPlayerPhase = TypePlayerPhase.middle;
            }
            else if (rb.position.x - rigidbody.position.x > 0 && rb.velocity.x > 0)
            {
                CurrentPlayerPhase = TypePlayerPhase.right1;
            }
            else if (rb.position.x - rigidbody.position.x > 0 && rb.velocity.x < 0)
            {
                CurrentPlayerPhase = TypePlayerPhase.right2;
            }
            else if (rb.position.x - rigidbody.position.x < 0 && rb.velocity.x > 0)
            {
                CurrentPlayerPhase = TypePlayerPhase.left2;
            }
            else if (rb.position.x - rigidbody.position.x < 0 && rb.velocity.x < 0)
            {
                CurrentPlayerPhase = TypePlayerPhase.left1;
            }

            lr.SetPosition(0, rb.position);
            lr.SetPosition(1, rigidbody.position);

            yield return new WaitForFixedUpdate();
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -MaximumSpeed, MaximumSpeed));
        GameController.PlayerPosition?.Invoke(rb);
    }
}
