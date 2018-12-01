using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeText : MonoBehaviour
{
    private RectTransform canvasRectTransform;

    private LifeTextData data;

    private Text myText;

    //現在の経過時間
    private float currentTime;

    //初期位置
    private Vector2 initPosition;

    private System.Action textUpdate;

    [SerializeField]
    private GameObject lifeOwner;

    private RectTransform recttransform;

	void Start ()
    {
        myText = GetComponent<Text>();
        canvasRectTransform = lifeOwner.transform.Find("Canvas").GetComponent<RectTransform>();

        currentTime = 0f;
        initPosition = transform.position;

        textUpdate = null;

        data = GameObject.Find("GameDataManager").GetComponent<LifeTextData>();
        recttransform = GetComponent<RectTransform>();
        myText.fontSize = data.fontSize;
	}
	
	void Update ()
    {
        TextUpdate();
	}

    public void CallHealText(int healvalue)
    {
        if (healvalue < 0) return;
        WriteText(healvalue, data.healColor);
    }

    public void CallEatText(int eatvalue)
    {
        if (eatvalue < 0) return;
        WriteText(eatvalue, data.eatColor);
    }

    public void CallDamageText(int damageValue)
    {
        if (damageValue < 0) return;
        WriteText(damageValue, data.damageColor);
    }

    private void WriteText(int value, Color color)
    {
        color.a = 1.0f;
        myText.color = color;
        myText.text = value.ToString();
        currentTime = 0f;

        //出現位置を設定する
        SetAppearPosition();

        textUpdate = null;
        textUpdate += AlphaUpdate;
        textUpdate += MoveUpdate;
    }

    private void AlphaUpdate()
    {
        currentTime += Time.deltaTime;

        float timeRate = currentTime / data.moveTime;
        timeRate = Mathf.Clamp01(timeRate);

        Color color = myText.color;
        color.a = 1.0f - timeRate;
        myText.color = color;
    }

    private void MoveUpdate()
    {
        float timeRate = currentTime / data.moveTime;
        recttransform.position = Vector2.Lerp(initPosition, initPosition + data.moveVelocity, timeRate);
        //recttransform.position = initPosition;
    }

    private void TextUpdate()
    {
        if (textUpdate != null)
        {
            textUpdate.Invoke();
            FuncRemove();
            return;
        }
    }

    private void FuncRemove()
    {
        float timeRate = currentTime / data.moveTime;
        if (timeRate >= 1.0f)
        {
            myText.text = "";
            textUpdate = null;
        }
    }

    private void SetAppearPosition()
    {
        Vector2 appearPosition = utility.WrapMathf.ScreenPosition(lifeOwner.transform.position + (Vector3)data.appearShift, canvasRectTransform);
        //Vector2 appearPosition = utility.WrapMathf.ScreenPosition(lifeOwner.transform.position, canvasRectTransform);

        initPosition = appearPosition;
        recttransform.position = initPosition;
    }
}
