using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAge
{
    public class PoolControl : MonoBehaviour
	{
		private Vector3 DEFAULT_POSITION_CREATE = Vector3.one * -9999;

		public void CreatePool(GameObject prefab, int lengthPool = 5 )
		{
			if (!ObjectPool.IsSpawned(prefab))
				ObjectPool.CreatePool(prefab, 10);
		}

		public GameObject GetSnareObject(GameObject prefab, GameObject parentObject)
		{
			GameObject obj = ObjectPool.Spawn(prefab, parentObject.transform);
			obj.SetActive(true);
			return obj;
		}

		public void Remove(GameObject obj)
		{
			ObjectPool.Recycle(obj);
		}

		public void ClearAllPool(int delayTimeSecond)
		{
			ObjectPool.ClearPool(delayTimeSecond);
		}

		public void RemoveAllByType(GameObject typeObject)
		{
			ObjectPool.RecycleAll(typeObject);
		}
	}
}
