using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInMainMenu : MonoBehaviour
{
    enum TypePlayerPhase
    {
        idle,

        left1,
        left2,
        right1,
        right2,
        middle
    }

    TypePlayerPhase currentPlayerPhase;
    [SerializeField] GameObject [] playerPhase;

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

    DistanceJoint2D dj;
    LineRenderer lr;
    Rigidbody2D rb;
    TrailRenderer tr;

    [SerializeField] Rigidbody2D connectingBody;

    Vector2 startPosition;
    bool init;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dj = GetComponent<DistanceJoint2D>();
        lr = GetComponent<LineRenderer>();
        tr = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        startPosition = rb.position;
        rb.position = startPosition;
        rb.velocity = Vector3.down * 10;
        init = true;
    }

    private void OnEnable()
    {
        if (!init)
        {
            rb.position = startPosition;
            rb.velocity = Vector3.down * 10;
        }
    }

    private void FixedUpdate()
    {
        lr.SetPosition(0, rb.position);
        lr.SetPosition(1, connectingBody.position);
        dj.connectedBody = connectingBody;

        rb.rotation = 90.0f + 
            Mathf.Atan2(rb.position.y - connectingBody.position.y, 
            rb.position.x - connectingBody.position.x) / Mathf.PI * 180;
        //Debug.Log(rb.rotation + " " + Mathf.Atan2(dj.reactionForce.y, dj.reactionForce.x) / Mathf.PI * 180 + " " + dj.reactionForce.y + " " + dj.reactionForce.x);
        //Debug.DrawRay(rb.position, dj.reactionForce);

        if (Mathf.Abs(rb.velocity.x) < 0.1f && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            CurrentPlayerPhase = TypePlayerPhase.middle;
        }
        else if (rb.position.x - connectingBody.position.x > 0 && rb.velocity.x > 0)
        {
            CurrentPlayerPhase = TypePlayerPhase.right1;
        }
        else if (rb.position.x - connectingBody.position.x > 0 && rb.velocity.x < 0)
        {
            CurrentPlayerPhase = TypePlayerPhase.right2;
        }
        else if (rb.position.x - connectingBody.position.x < 0 && rb.velocity.x > 0)
        {
            CurrentPlayerPhase = TypePlayerPhase.left2;
        }
        else if (rb.position.x - connectingBody.position.x < 0 && rb.velocity.x < 0)
        {
            CurrentPlayerPhase = TypePlayerPhase.left1;
        }
    }
}
