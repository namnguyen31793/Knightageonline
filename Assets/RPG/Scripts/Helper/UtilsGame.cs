using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAge.Helper
{
    public class UtilsGame
    {
        public static int GetTimeNbf(){
            return (int)(DateTime.UtcNow.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
        }
    }
}
