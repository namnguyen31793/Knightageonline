using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KnightAge.UICanvas
{
    public class EnemyUI : MonoBehaviour
    {
        public Image progressHealPlayer;
        public TextMeshProUGUI nameTxt;

        public void UpdateInfo(string name, float heal){
            this.gameObject.SetActive(true);
            nameTxt.text = name;
            progressHealPlayer.fillAmount = heal;
        }

        public void UpdateHeal(float heal){
            progressHealPlayer.fillAmount = heal;
        }

        public void Dispose(){
            progressHealPlayer.fillAmount = 0;
            this.gameObject.SetActive(false);
        }
    }
}
