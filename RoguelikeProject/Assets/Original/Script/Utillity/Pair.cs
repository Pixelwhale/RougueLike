using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace utility
{
    //struct同士のペア
    public struct Pair_Struct<STRUCT1, STRUCT2>
        where STRUCT1 : struct
        where STRUCT2 : struct
    {
        public STRUCT1 first;
        public STRUCT2 second;

        public Pair_Struct(STRUCT1 first, STRUCT2 second)
        {
            this.first = first;
            this.second = second;
        }
    }

    //class同士のペア
    public struct Pair_Class<CLASS1, CLASS2>
        where CLASS1 : class
        where CLASS2 : class
    {
        public CLASS1 first;
        public CLASS2 second;

        public Pair_Class(CLASS1 first, CLASS2 second)
        {
            this.first = first;
            this.second = second;
        }
    }

    //firstにclass、secondにstructのペア
    public struct Pair_Mix<CLASS, STRUCT>
        where CLASS : class
        where STRUCT : struct
    {
        public CLASS first;
        public STRUCT second;

        public Pair_Mix(CLASS first, STRUCT second)
        {
            this.first = first;
            this.second = second;
        }
    }
}