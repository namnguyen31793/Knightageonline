using System;
using KnightAge.Core;
using KnightAge.Gameplay;
using UnityEngine;

namespace KnightAge.UI
{
    /// <summary>
    /// Sends user input to the correct control systems.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        public float stepSize = 0.1f;
        GameModel model = Schedule.GetModel<GameModel>();
        public Joystick joystick;

        public enum State
        {
            CharacterControl,
            DialogControl,
            Pause
        }

        State state;

        public void ChangeState(State state){  
            this.state = state;
            if(state == State.CharacterControl){
                joystick.gameObject.SetActive(true);
            }else{
                joystick.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            switch (state)
            {
                case State.CharacterControl:
                    CharacterControl();
                    CheckRaycash();
                    break;
                case State.DialogControl:
                    DialogControl();
                    break;
            }
        }

        void DialogControl()
        {
            model.player.nextMoveCommand = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                model.dialog.FocusButton(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                model.dialog.FocusButton(+1);
            if (Input.GetKeyDown(KeyCode.Space))
                model.dialog.SelectActiveButton();
        }

        void CharacterControl()
        {
            var _vectorMove = Vector3.zero;
            if(Math.Abs(joystick.Horizontal) >= Math.Abs(joystick.Vertical))
                _vectorMove = new Vector3(joystick.Horizontal, 0);
            else
                _vectorMove = new Vector3( 0, joystick.Vertical);
            model.player.nextMoveCommand = _vectorMove * stepSize;
        }

        void CheckRaycash(){
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
                if(hit.collider != null && hit.collider.gameObject.tag == "Enemy")
                {
                    // actack
                    model.player.SelectObject(hit.collider.transform, TYPE_PLAYER_SELECT.ENEMY);
                    //enemy actack
                    var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                    if(enemy == null)
                        return;
                    enemy.PlayerActack(model.player.transform);
                }
                if(hit.collider != null && hit.collider.gameObject.tag == "NPC")
                {
                    // actack
                    model.player.SelectObject(hit.collider.transform, TYPE_PLAYER_SELECT.NPC);
                }
            }
        }
    }
}