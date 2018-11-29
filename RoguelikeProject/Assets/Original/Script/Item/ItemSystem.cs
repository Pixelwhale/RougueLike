using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour
{
    public int index;
    public System.Action click;

	void Start ()
    {
        click = null;
	}

    //クリックされたときの処理
    public void Click()
    {
        if (click != null)
        {
            click.Invoke();
        }
    }
}
