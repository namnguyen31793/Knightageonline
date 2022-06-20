using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAge.UICanvas
{
    public class UICanvasControl : MonoBehaviour
    {
        public PlayerUi playerUi;
        public EnemyUI enemyUI;
        private void Awake() {
            this.playerUi.UpdateInfo("User1");
            HideUiEnemy();
        }

        public void ShowUIEnemy(string name, float heal){
            enemyUI.UpdateInfo(name, heal);
        }

        public void UpdateHealEnemy(float heal){
            enemyUI.UpdateHeal(heal);
        }

        public void HideUiEnemy(){
            enemyUI.Dispose();
        }
    }
}
