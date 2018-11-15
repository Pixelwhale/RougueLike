using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace utility
{
    [System.Serializable]
    public struct Range
    {
        public float min, max;

        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        // min <= value <= max だったらtrue
        public static bool IsRangeOfInt(int value, int min, int max)
        {
            if (value < min) return false;
            if (value > max) return false;

            return true;
        }

        // min <= value <= max だったらtrue
        public static bool IsRangeOfFloat(float value, float min, float max)
        {
            if (value < min) return false;
            if (value > max) return false;

            return true;
        }
        public static bool IsRangeOfFloat(float value, utility.Range range)
        {
            return IsRangeOfFloat(value, range.min, range.max);
        }
    }
}