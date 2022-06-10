using KnightAge.Core;
using KnightAge.Gameplay;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace KnightAge
{
    public class CampEnemy : MonoBehaviour
    {
        [SerializeField]
        private int CampId;
        [SerializeField]
        private List<Enemy> listEnemy = new List<Enemy>();
        [SerializeField]
        private GameObject EnemyPrefab;
        [SerializeField]
        private int MaxStackEnemy;
        [SerializeField]
        private float SizeCampSpawn;

        GameModel model = Schedule.GetModel<GameModel>();


        /// <summary>
        /// Init Info Camp in function
        /// </summary>
        public void Init(int CampId){
            this.Dispose();
            this.CampId = CampId;
            //create Pool
            model.poolControl.CreatePool(EnemyPrefab, MaxStackEnemy);
        }

        public void UpdateFrame(float deltaTime) {
            if (CampId <= 0 )
                return;
            //add time frame for Enemy
            if( listEnemy.Count > 0)
                for (int i = listEnemy.Count -1; i >= 0; i--)
                {
                    listEnemy[i].CheckTime(deltaTime);
                }
            //check create Enemy
            if (listEnemy.Count < MaxStackEnemy){
                int random = RandomHelper.NextInt(10) - 8;
                if (random < 0)
                    random = 0;
                for (int i = 0; i < random; i++){
                    SpawnEnemy();
                }
            }
        }

        private void SpawnEnemy() { 
            var enemyNew = model.poolControl.GetSnareObject(EnemyPrefab, this.gameObject);
            enemyNew.transform.localScale = Vector3.one;
            enemyNew.active = true;
            Vector3 random = new Vector3(((float)(RandomHelper.NextInt(81) - 80) / 40) * SizeCampSpawn, ((float)(RandomHelper.NextInt(81) - 80) / 40) * SizeCampSpawn);
            enemyNew.transform.position = this.transform.position + random;
            this.listEnemy.Add(enemyNew.GetComponent<Enemy>());
            enemyNew.GetComponent<Enemy>().Init(this.CampId);
        }

        /// <summary>
        /// Action Call clear enemy obj
        /// </summary>
        public void KillEnemy(Enemy enemyView)
        {
            model.poolControl.Remove(enemyView.gameObject);
            enemyView.Dispose();
            this.listEnemy.Remove(enemyView);
        }

        public void Dispose()
        {
            model.poolControl.RemoveAllByType(this.EnemyPrefab);
            if (listEnemy == null)
                return;
            if (listEnemy.Count > 0){
                for (int i = listEnemy.Count - 1; i >= 0; i--)
                {
                    listEnemy[i].Dispose();
                    listEnemy.RemoveAt(i);
                }
            }
            listEnemy = new List<Enemy>();
            CampId = 0;
        }
    }
}
 