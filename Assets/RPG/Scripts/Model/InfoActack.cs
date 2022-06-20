using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAge.Model
{
    [System.Serializable] 
    public class InfoActack
    {
        public string NameEnemy;
        public int Heal;
        public int Damage;
        public int AttackSpeed;//milisecend
        public float RangleActack;
        public GameObject effectActack;
        public TYPE_ACTACK typeActack;
    }

    public enum TYPE_ACTACK
    {
        RANGLE,
        MAGIC,
        MEELE
    }
}
