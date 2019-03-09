using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(ChangeLevelInText());
    }

    IEnumerator ChangeLevelInText()
    {
        yield return new WaitUntil(() => LevelManager.Instance != null);

        text.text = "Level " + ((int)LevelManager.Instance.Level).ToString();
    }
}
