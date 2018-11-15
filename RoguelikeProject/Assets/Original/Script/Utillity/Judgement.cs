using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace utility
{
    public class Judgement
    {
        //レイヤーとの比較
        public static bool IsCompareLayer(string layerName, GameObject gameObject)
        {
            return LayerMask.LayerToName(gameObject.layer) == layerName;
        }
        public static bool IsCompareLayer(GameObject gameObject, string layerName)
        {
            return IsCompareLayer(layerName, gameObject);
        }
        public static bool IsCompareLayers(GameObject gameObject, params string[] layerNames)
        {
            foreach (var layerName in layerNames)
            {
                if (IsCompareLayer(gameObject, layerName)) return true;
            }

            return false;
        }

        //タグとの比較
        public static bool IsCompareTag(string tagName, GameObject gameObject)
        {
            return tagName == gameObject.tag;
        }
        public static bool IsCompareTag(GameObject gameObject, string tagName)
        {
            return IsCompareTag(tagName, gameObject);
        }
        public static bool IsCompareTags(GameObject gameObject, params string[] tagNames)
        {
            foreach (var tagName in tagNames)
            {
                if (IsCompareTag(gameObject, tagName)) return true;
            }

            return false;
        }

        //引数のTransformの子オブジェクトのタグに同じものがあるかどうか(1つでもあったらtrue)
        public static bool IsCompareTagChild(string tagName, Transform parent)
        {
            foreach (Transform child in parent)
            {
                //1つでも同じタグのものがあったらtrue
                if (IsCompareTag(tagName, child.gameObject)) return true;
            }

            //1つも同じものがなかったのでfalse
            return false;
        }
    }
}
