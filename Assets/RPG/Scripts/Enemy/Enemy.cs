using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAge
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _rangleActack;
        [SerializeField]
        private ENEMY_TYPE_ACTACK _typeActack = ENEMY_TYPE_ACTACK.MELEE;
        [SerializeField]
        private ENEMY_STATUS _status = ENEMY_STATUS.IDLE;
        private Transform target;
        [SerializeField]
        private int CampId;

        private Vector3 _startPos;

        public void Init(int CampId) {
            this.CampId = CampId;
        }

        /// <summary>
        /// Add time by Frame
        /// </summary>
        public void CheckTime(float deltaTime){
            if(_status == ENEMY_STATUS.IDLE)
                MoveIdle(deltaTime);
            if (_status == ENEMY_STATUS.MOVE)
                Move(deltaTime);
            if (_status == ENEMY_STATUS.ACTACK)
                Actack(deltaTime);
            if (_status == ENEMY_STATUS.ACTACK)
                Stun(deltaTime);
        }

        public virtual void MoveIdle(float deltaTime)
        {

        }

        /// <summary>
        /// Action Move Enemy
        /// </summary>
        public virtual void Move(float deltaTime)
        {
            if (target == null)
                return;
            float step = _speed * deltaTime;
            this.transform.position = Vector2.MoveTowards(transform.position, target.position, step);
            //check distance actack or idle
            //--------------
        }

        /// <summary>
        /// Action Enemy Actack
        /// </summary>
        public virtual void Actack(float deltaTime)
        {
            if (target == null)
                return;
            //check time next turn actack
        }

        /// <summary>
        /// Action Enemy Actack
        /// </summary>
        public virtual void Stun(float deltaTime)
        {
            if (target == null)
                return;
            //check time Change Status, if target far clear and move to start pos
        }

        /// <summary>
        /// Add Target enemy forward
        /// </summary>
        public virtual void AddTarget(Transform targetActack){
            if(target == null)
                return;
            this.target = targetActack;
        }

        public virtual void ClearTarget(){
            this.target = null;
        }


        public virtual void Dispose() {
            target = null;
        }

    }

    public enum ENEMY_TYPE_ACTACK
    {
        MELEE = 0,
        RANGLE = 1,
    }

    public enum ENEMY_STATUS
    {
        IDLE = 0,
        MOVE = 1,
        ACTACK = 2,
        STUN = 3,
    }
}
