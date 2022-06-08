using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace KnightAge
{
    public class CampControl : MonoBehaviour
    {
        [SerializeField]
        private List<CampEnemy> listCamp;
        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// Init Info Camp in function
        /// </summary>
        private void Init() {
            for (int i = 0; i < listCamp.Count; i++) {
                listCamp[i].Init(i + 1);
            }
        }

        /// <summary>
        /// Update Frame camp
        /// </summary>
        private void FixedUpdate()
        {
            var time = Time.deltaTime;
            foreach (var camp in listCamp) {
                 camp.UpdateFrame(time);
            }
        }

        /// <summary>
        /// Clear data
        /// </summary>
        public void Dispose()
        {
            foreach (var camp in listCamp)
            {
                camp.Dispose();
            }
        }
    }
}
