using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace utility
{
    //Mathfをラップした関数を保持する
    public class WrapMathf
    {
        public static bool IsZero(Vector2 v2)
        {
            return v2 == Vector2.zero;
        }

        public static bool IsZero(Vector3 v3)
        {
            return v3 == Vector3.zero;
        }

        //Vector3のXZ成分からVector2を生成する
        public static Vector2 CreateVector2_XZ(Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        //Vector3の成分全ての絶対値をとる
        public static Vector3 FabsV3(Vector3 v)
        {
            v.x = Mathf.Abs(v.x);
            v.y = Mathf.Abs(v.y);
            v.z = Mathf.Abs(v.z);

            return v;
        }

        //valueを矩形の範囲にclamp
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            return value;
        }

        //valueを3D空間にclamp
        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            value.z = Mathf.Clamp(value.z, min.z, max.z);

            return value;
        }

        //2つの値を交換する
        public static void SwapF(ref float f1, ref float f2)
        {
            float temp = f1;
            f1 = f2;
            f2 = temp;
        }

        public static Vector2 ScreenPosition(Vector3 worldPosition, RectTransform canvasRectTransform)
        {
            Vector2 screenPosition = Camera.main.WorldToViewportPoint(worldPosition);

            float resultX = screenPosition.x * canvasRectTransform.sizeDelta.x;
            float resultY = screenPosition.y * canvasRectTransform.sizeDelta.y;

            return new Vector2(resultX, resultY);
        }
    }
}
