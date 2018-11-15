using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace utility
{
    //GameObjectに関するラップ関数
    public class WrapGameObject
    {
        //引数のGameObjectの子オブジェクトを全て取得する
        public static List<GameObject> GetChilds(GameObject parent)
        {
            List<GameObject> result = new List<GameObject>();

            foreach (Transform child in parent.transform)
            {
                result.Add(child.gameObject);
            }

            return result;
        }
    }
}
