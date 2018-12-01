using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownText : MonoBehaviour
{
    [SerializeField]
    private float limitTime = 5f;

    private float currentTime;

    private Text text;

    private string defaultText;

    private System.Action timeupAction;

    public System.Action TimeUpAction
    {
        set { timeupAction = value; }
        get { return timeupAction; }
    }

    private void Awake()
    {
        timeupAction = null;
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0.0f, currentTime);

        WriteText(currentTime);

        InvokeAction();
    }

    private void Initialize()
    {
        text = GetComponent<Text>();
        defaultText = text.text;
        currentTime = limitTime;
    }

    private void WriteText(float time)
    {
        int intTime = (int)time;
        text.text = defaultText + intTime;
    }

    private void InvokeAction()
    {
        if (timeupAction == null) return;

        if (currentTime <= 0.0f)
        {
            timeupAction.Invoke();
            Completed.GameManager.Restart();
        }
    }
}
