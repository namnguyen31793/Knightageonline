using System;
using System.Collections;
using System.Collections.Generic;
using KnightAge.Gameplay;
using UnityEngine;
using UnityEngine.U2D;

namespace KnightAge.Gameplay
{
    /// <summary>
    /// A simple controller for animating a 4 directional sprite using Physics.
    /// </summary>
    public class CharacterController2D : MonoBehaviour
    {
        public float speed = 1;
        public float acceleration = 2;
        public Vector3 nextMoveCommand;
        public Animator animator;
        public bool flipX = false;

        new Rigidbody2D rigidbody2D;
        SpriteRenderer spriteRenderer;
        PixelPerfectCamera pixelPerfectCamera;

        enum State
        {
            Idle, Moving, AutoMove
        }

        State state = State.Idle;
        Vector3 start, end;
        [SerializeField]
        Vector2 currentVelocity;
        float startTime;
        float distance;
        float velocity;

        Transform targetObject;

        void IdleState()
        {
            if (nextMoveCommand != Vector3.zero)
            {
                start = transform.position;
                end = start + nextMoveCommand;
                distance = (end - start).magnitude;
                velocity = 0;
                UpdateAnimator(nextMoveCommand);
                nextMoveCommand = Vector3.zero;
                state = State.Moving;
            }
        }

        void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            pixelPerfectCamera = GameObject.FindObjectOfType<PixelPerfectCamera>();
        }

        void MoveState()
        {
            velocity = Mathf.Clamp01(velocity + Time.deltaTime * acceleration);
            UpdateAnimator(nextMoveCommand);
            rigidbody2D.velocity = Vector2.SmoothDamp(rigidbody2D.velocity, nextMoveCommand * speed, ref currentVelocity, acceleration, speed);
            spriteRenderer.flipX = rigidbody2D.velocity.x >= 0 ? true : false;
        }

        void UpdateAnimator(Vector3 direction)
        {
            if (animator)
            {
                animator.SetInteger("WalkX", direction.x < 0 ? -1 : direction.x > 0 ? 1 : 0);
                animator.SetInteger("WalkY", direction.y < 0 ? 1 : direction.y > 0 ? -1 : 0);
            }
        }

        void FixedUpdate()
        {
            switch (state)
            {
                case State.Idle:
                    IdleState();
                    break;
                case State.Moving:
                    MoveState();
                    break;
                case State.AutoMove:
                    AutoMoveState();
                    break;
            }
        }

        void LateUpdate()
        {
            if (pixelPerfectCamera != null)
            {
                transform.position = pixelPerfectCamera.RoundToPixel(transform.position);
            }
        }

        public void SelectObject(Transform objSelect){
            this.targetObject = objSelect;
            state = State.AutoMove;
        }

        void AutoMoveState()
        {
            if (nextMoveCommand != Vector3.zero){
                start = transform.position;
                end = start + nextMoveCommand;
                distance = (end - start).magnitude;
                velocity = 0;
                UpdateAnimator(nextMoveCommand);
                nextMoveCommand = Vector3.zero;
                state = State.Moving;
            }
            var _vectorMove = Vector3.zero;
            var direction = CalculateMidVector(this.targetObject.position, this.transform.position);
            if(Math.Abs(direction.x) >= Math.Abs(direction.y)){
                if(direction.x < 0)
                    _vectorMove.x = -0.1f;
                else
                    _vectorMove.x = 0.1f;
            }
            else
                if(direction.y < 0)
                    _vectorMove.y = -0.1f;
                else
                    _vectorMove.y = 0.1f;

            velocity = Mathf.Clamp01(velocity + Time.deltaTime * acceleration);
            UpdateAnimator(_vectorMove);
            rigidbody2D.velocity = Vector2.SmoothDamp(rigidbody2D.velocity, _vectorMove * speed, ref currentVelocity, acceleration, speed);
            spriteRenderer.flipX = rigidbody2D.velocity.x >= 0 ? true : false;

            if(Vector3.Distance(this.transform.position, this.targetObject.position) < 1f){
                state = State.Moving;
            }
        }

        void Dispose(){
            this.targetObject = null;
        }

        Vector3 CalculateMidVector(Vector3 start, Vector3 end){
            return start - end;
        }
    }
}