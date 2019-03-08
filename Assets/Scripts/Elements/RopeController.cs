using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RopeController : MonoBehaviour
{
    protected Transform t;
    protected CircleCollider2D cc;
    protected Rigidbody2D rb;
    protected GameObject obj;

    [SerializeField] protected GameObject HookNotUsed;
    [SerializeField] protected GameObject HookUsed;
    [SerializeField] protected GameObject Target;

    public enum RopeMode
    {
        NotUsed,
        Used,
        Target
    }

    private RopeMode mode;

    public RopeMode Mode
    {
        get
        {
            return mode;
        }

        set
        {
            if (mode != value)
            {
                switch(value)
                {
                    case RopeMode.NotUsed:
                        HookNotUsed.SetActive(true);
                        HookUsed.SetActive(false);
                        Target.SetActive(false);
                        break;
                    case RopeMode.Target:
                        HookNotUsed.SetActive(true);
                        HookUsed.SetActive(false);
                        Target.SetActive(true);
                        break;
                    case RopeMode.Used:
                        HookNotUsed.SetActive(false);
                        HookUsed.SetActive(true);
                        Target.SetActive(false);
                        break;
                    default:
                        Debug.LogWarning(obj.name + "is change unknowed mode");
                        break;

                }

                mode = value;
            }
        }
    }

    private void Awake()
    {

        t = GetComponent<Transform>();
        cc = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        obj = gameObject;
    }

    float Distance(Vector2 vector, out Rigidbody2D data, out RopeController rope)
    {
        data = rb;
        rope = this;
        return Vector2.Distance(rb.position, vector);
    }

    private void OnEnable()
    {
        GameController.DistanceToPoint += Distance;
    }

    private void OnDisable()
    {
        GameController.DistanceToPoint -= Distance;
    }
}
