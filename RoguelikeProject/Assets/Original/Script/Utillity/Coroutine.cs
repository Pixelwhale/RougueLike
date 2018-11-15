using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace utility
{
    public class Coroutine
    {
        //渡された処理を指定時間後に実行
        public static IEnumerator DelayMethod(float waitTime, System.Action action)
        {
            yield return new WaitForSeconds(waitTime);
            action();
        }
    }
}