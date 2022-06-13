using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KnightAge.Model;

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
        
        [SerializeField]
        InfoActack info;
        public Animator animator;
        private Transform target;

        [SerializeField]
        private int _campId ;
        public int CamId { get { return _campId; } }
        [SerializeField]
        private int _enemyId;
        public int EnemyId { get { return _enemyId; } }

        private Vector3 _startPos;

        public void Init(int CampId, int EnemyId) {
            this._campId = CampId;
            this._enemyId = EnemyId;
            this._startPos = this.transform.position;
        }

        /// <summary>
        /// Add time by Frame
        /// </summary>
        public void CheckTime(float deltaTime){
            if(_status == ENEMY_STATUS.IDLE)
                Idle(deltaTime);
            if (_status == ENEMY_STATUS.MOVE_SPAWN)
                MoveSpawn(deltaTime);
            if (_status == ENEMY_STATUS.CHASE)
                Chase(deltaTime);
            if (_status == ENEMY_STATUS.ACTACK)
                Actack(deltaTime);
            if (_status == ENEMY_STATUS.STUN)
                Stun(deltaTime);
        }

        public virtual void Idle(float deltaTime)
        {
            float step = _speed * deltaTime;
            // this.transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }

        
        /// <summary>
        /// Action Move to spaw after play far
        /// </summary>
        public virtual void MoveSpawn(float deltaTime)
        {
            this.transform.position = Vector2.MoveTowards(transform.position, this._startPos, _speed * deltaTime);
            if(Vector3.Distance(this.transform.position, this._startPos) < 1){
                _status = ENEMY_STATUS.IDLE;
                this._startPos = this.transform.position;
                return;
            }
        }
        /// <summary>
        /// Action Move chase play 
        /// </summary>
        public virtual void Chase(float deltaTime)
        {
            if (target == null)
                return;
            float step = _speed * deltaTime;
            if(Vector3.Distance(this.transform.position, this._startPos) > 6){
                _status = ENEMY_STATUS.MOVE_SPAWN;
                this.target = null;
                return;
            }
            
            if(Vector3.Distance(this.transform.position, this.target.position) < 1f){
                _status = ENEMY_STATUS.ACTACK;
            }else{
                var toward = Vector2.MoveTowards(transform.position, target.position, _speed * deltaTime);
                this.transform.position = toward;
                UpdateAnimator(toward);
            }
        }

        /// <summary>
        /// Action Enemy Actack
        /// </summary>
        public virtual void Actack(float deltaTime)
        {
            //check time actack
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
        /// Action enemy auto add Target enemy forward
        /// </summary>
        public virtual void AddTarget(Transform targetActack){
            if(targetActack == null)
                return;
            this.target = targetActack;
        }
        

        /// <summary>
        /// Player Click Raycash to enemy
        /// </summary>
        public virtual void PlayerActack(Transform targetActack){
            Debug.Log("PlayerActack "+targetActack.gameObject.name);
            if(targetActack == null)
                return;
            this.target = targetActack;
            _status = ENEMY_STATUS.CHASE;
        }

        public virtual void ClearTarget(){
            this.target = null;
        }

        public virtual int PlayerHit(int damage) {
            this.info.Heal -= damage;
            return this.info.Heal;
        }

        public virtual void Dispose() {
            target = null;
        }
        void UpdateAnimator(Vector3 direction)
        {
            if (animator)
            {
                
            }
        }

    }

    public enum ENEMY_TYPE_ACTACK
    {
        MELEE = 0,
        RANGLE = 1,
    }

    public enum ENEMY_STATUS
    {
        NONE = 0,
        IDLE = 1,
        MOVE_SPAWN = 2,
        CHASE = 3,
        ACTACK = 4,
        STUN = 5,
    }
}
