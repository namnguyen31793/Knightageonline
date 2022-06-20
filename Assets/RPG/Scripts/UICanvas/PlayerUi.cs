using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KnightAge.UICanvas
{
    public class PlayerUi : MonoBehaviour
    {  
        public Image progressHealPlayer;
        public TextMeshProUGUI nameTxt;

        public void UpdateInfo(string name){
            nameTxt.text = name;
        }
    }
}
