using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAge
{
    public class CampControl : List<EnemyControl>
    {
        private GameLogic gameLogic;

        public CampControl(GameLogic gameLogic)
        {
            this.gameLogic = gameLogic;
        }

    }
}
