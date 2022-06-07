using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAge
{
	public class GameLogic : MonoBehaviour
	{
		public CampControl campControl;
		public void Init()
		{
			campControl = new CampControl(this);
		}

		public void SpawnTurn(){
			
		}

		public void HandleClear()
		{
			campControl = null;
		}
	}
}
