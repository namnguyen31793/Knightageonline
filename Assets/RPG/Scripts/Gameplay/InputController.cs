using System;
using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;

namespace RPGM.UI
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

        void FixedUpdate()
        {
            switch (state)
            {
                case State.CharacterControl:
                    CharacterControl();
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
    }
}