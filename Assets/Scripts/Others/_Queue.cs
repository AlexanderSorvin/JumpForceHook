using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Queue<T> : Queue<T>
{
    public T Last { get; private set; }

    public new void Enqueue(T item)
    {
        Last = item;
        base.Enqueue(item);
    }
}
