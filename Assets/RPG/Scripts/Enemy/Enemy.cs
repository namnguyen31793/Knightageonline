using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAge
{
    public class Enemy : MonoBehaviour
    {
        public float _speed;
        public float _rangleActack;
        public ENEMY_TYPE_ACTACK _typeActack = ENEMY_TYPE_ACTACK.MELEE;
        private Transform target;

        /// <summary>
        /// Action move
        /// </summary>
        public virtual void Move(float deltaTime){
            if(target == null)
                return;
            if(_typeActack == ENEMY_TYPE_ACTACK.MELEE){
                MoveMelee(deltaTime);
            }else if(_typeActack == ENEMY_TYPE_ACTACK.RANGLE){
                MoveRangle(deltaTime);
            }
        }

        private void MoveMelee(float deltaTime){
            float step = _speed * deltaTime;
            this.transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }

        private void MoveRangle(float deltaTime){
            float step = _speed * deltaTime;
            this.transform.position = Vector2.MoveTowards(transform.position, target.position, step);
            //check distance actack
        }

        /// <summary>
        /// Add Target Actack
        /// </summary>
        public virtual void AddTarget(Transform targetActack){
            if(target == null)
                return;
            this.target = targetActack;
        }

        /// <summary>
        /// Add Target Actack
        /// </summary>
        public virtual void ClearTarget(){
            this.target = null;
        }

    }

    public enum ENEMY_TYPE_ACTACK
    {
        MELEE = 0,
        RANGLE = 1,
    }
}
