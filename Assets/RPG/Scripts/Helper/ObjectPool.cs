using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightAge
{
    public class ObjectPool : MonoBehaviour
	{
		private static List<GameObject> tempList = new List<GameObject>();
		private static ObjectPool _instance;
		private Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();
		private Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();
		public ObjectPool.StartupPoolMode startupPoolMode;
		public ObjectPool.StartupPool[] startupPools;
		private bool startupPoolsCreated;

		public static ObjectPool instance
		{
			get
			{
				if ((UnityEngine.Object)ObjectPool._instance != (UnityEngine.Object)null)
					return ObjectPool._instance;
				ObjectPool._instance = (ObjectPool)UnityEngine.Object.FindObjectOfType<ObjectPool>();
				if ((UnityEngine.Object)ObjectPool._instance != (UnityEngine.Object)null)
					return ObjectPool._instance;
				GameObject gameObject = new GameObject("ObjectPool");
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				ObjectPool._instance = (ObjectPool)gameObject.AddComponent<ObjectPool>();

				return ObjectPool._instance;
			}
		}

		public ObjectPool()
		{
			//base.ctor();
		}

		void Awake()
		{
			ObjectPool._instance = this;
			if (this.startupPoolMode != StartupPoolMode.Awake)
				return;
			ObjectPool.CreateStartupPools();
		}

		void Start()
		{
			if (this.startupPoolMode != StartupPoolMode.Start)
				return;
			ObjectPool.CreateStartupPools();
		}

		public static void CreateStartupPools()
		{
			if (ObjectPool.instance.startupPoolsCreated)
				return;
			ObjectPool.instance.startupPoolsCreated = true;
			ObjectPool.StartupPool[] startupPoolArray = ObjectPool.instance.startupPools;
			if (startupPoolArray == null || startupPoolArray.Length <= 0)
				return;
			for (int index = 0; index < startupPoolArray.Length; index++)
			{
				ObjectPool.CreatePool(startupPoolArray[index].prefab, startupPoolArray[index].size);
			}
		}

		public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
		{
			ObjectPool.CreatePool(prefab.gameObject, initialPoolSize);
		}

		public static void CreatePool(GameObject prefab, int initialPoolSize)
		{
			if (!((UnityEngine.Object)prefab != (UnityEngine.Object)null) || ObjectPool.instance.pooledObjects.ContainsKey(prefab))
				return;
			List<GameObject> list = new List<GameObject>();
			ObjectPool.instance.pooledObjects.Add(prefab, list);
			if (initialPoolSize <= 0)
				return;
			bool activeSelf = prefab.activeSelf;
			prefab.SetActive(false);
			Transform transform = ((Component)ObjectPool.instance).transform;
			while (list.Count < initialPoolSize)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate((UnityEngine.Object)prefab);
				gameObject.transform.SetParent(transform);
				list.Add(gameObject);
			}
			// Set lai gia tri cua prefab
			prefab.SetActive(activeSelf);
		}

		public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotate) where T : Component
		{
			return ObjectPool.Spawn(prefab.gameObject, parent, position, rotate).GetComponent<T>();
		}

		public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotate) where T : Component
		{
			return ObjectPool.Spawn(prefab.gameObject, null, position, rotate).GetComponent<T>();
		}

		public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
		{
			return ObjectPool.Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
		}

		public static T Spawn<T>(T prefab, Vector3 position) where T : Component
		{
			return ObjectPool.Spawn(prefab.gameObject, (Transform)null, position, Quaternion.identity).GetComponent<T>();
		}

		public static T Spawn<T>(T prefab, Transform parent) where T : Component
		{
			return ObjectPool.Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
		}

		public static T Spawn<T>(T prefab) where T : Component
		{
			return ObjectPool.Spawn(prefab.gameObject, (Transform)null, Vector3.zero, Quaternion.identity).GetComponent<T>();
		}

		public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
		{
			List<GameObject> list;
			if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out list))
			{
				GameObject key1 = (GameObject)null;
				if (list.Count > 0)
				{
					// xoa taoan bo list
					while ((UnityEngine.Object)key1 == (UnityEngine.Object)null && list.Count > 0)
					{
						key1 = list[0];
						list.RemoveAt(0);
					}
					if ((UnityEngine.Object)key1 != (UnityEngine.Object)null)
					{
						Transform transform = key1.transform;
						transform.SetParent(parent);
						transform.localPosition = position;
						transform.localRotation = rotation;
						key1.SetActive(true);
						ObjectPool.instance.spawnedObjects.Add(key1, prefab);
						return key1;
					}

				}

				GameObject key2 = (GameObject)UnityEngine.Object.Instantiate((UnityEngine.Object)prefab);
				Transform transform1 = key2.transform;
				transform1.SetParent(parent);
				transform1.localPosition = position;
				transform1.localRotation = rotation;
				ObjectPool.instance.spawnedObjects.Add(key2, prefab);
				return key2;
			}

			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate((UnityEngine.Object)prefab);
			Transform transform2 = gameObject.GetComponent<Transform>();
			transform2.SetParent(parent);
			transform2.localPosition = position;
			transform2.localRotation = rotation;
			return gameObject;
		}


		public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
		{
			return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
		}

		public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			return ObjectPool.Spawn(prefab, (Transform)null, position, rotation);
		}

		public static GameObject Spawn(GameObject prefab, Transform parent)
		{
			return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
		}

		public static GameObject Spawn(GameObject prefab, Vector3 position)
		{
			return ObjectPool.Spawn(prefab, (Transform)null, position, Quaternion.identity);
		}

		public static GameObject Spawn(GameObject prefab)
		{
			return ObjectPool.Spawn(prefab, (Transform)null, Vector3.zero, Quaternion.identity);
		}

		public static void Recycle<T>(T obj) where T : Component
		{
			ObjectPool.Recycle(obj.gameObject);
		}

		public static void Recycle(GameObject obj)
		{
			GameObject prefab;
			if (ObjectPool.instance.spawnedObjects.TryGetValue(obj, out prefab))
			{
				ObjectPool.Recycle(obj, prefab);
			}
			else
				UnityEngine.Object.Destroy((UnityEngine.Object)obj);
		}

		public static void Recycle(GameObject obj, GameObject prefab)
		{
			ObjectPool.instance.pooledObjects[prefab].Add(obj);
			ObjectPool.instance.spawnedObjects.Remove(obj);
			obj.transform.SetParent(ObjectPool.instance.gameObject.transform);
			obj.SetActive(false);
			//		obj.transform.position = Config.HIDE_POSITION;
		}

		public static void RecycleAll(GameObject prefab)
		{
			using (Dictionary<GameObject, GameObject>.Enumerator enumerator = ObjectPool.instance.spawnedObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<GameObject, GameObject> current = enumerator.Current;
					if ((UnityEngine.Object)current.Value == (UnityEngine.Object)prefab)
						ObjectPool.tempList.Add(current.Key);

				}
			}

			for (int index = 0; index < ObjectPool.tempList.Count; index++)
			{
				ObjectPool.Recycle(ObjectPool.tempList[index]);
			}
			ObjectPool.tempList.Clear();
		}

		public static void RecycleAll()
		{
			ObjectPool.tempList.AddRange((IEnumerable<GameObject>)ObjectPool.instance.spawnedObjects.Keys);
			for (int index = 0; index < ObjectPool.tempList.Count; ++index)
				ObjectPool.Recycle(ObjectPool.tempList[index]);
			ObjectPool.tempList.Clear();
		}

		public static void RecycleAll<T>(T prefab) where T : Component
		{
			ObjectPool.RecycleAll(prefab.gameObject);
		}

		// xoa toan bo object pool
		public static void ClearPool()
		{
			ObjectPool.instance.ClearAllPool();

		}

		public void ClearAllPool()
		{
			CleanAllChilds(this.gameObject);
			tempList.Clear();
			pooledObjects.Clear();
			spawnedObjects.Clear();
		}

		public void ClearAllPool(float delayTimeSecond)
		{
			Invoke("ClearAllPool", delayTimeSecond);
		}

		public void RemoveAllDelay()
		{
			CancelInvoke();
		}

		public static void ClearAllRemoveDelay()
		{
			ObjectPool.instance.RemoveAllDelay();
		}

		public static void ClearPool(float delayTimeSecond)
		{
			ObjectPool.instance.ClearAllPool(delayTimeSecond);
		}

		public static void CleanAllChilds(GameObject parent)
		{
			foreach (Transform childTransform in parent.transform)
			{
				Destroy(childTransform.gameObject);
			}
		}

		public static bool IsSpawned(GameObject obj)
		{
			return ObjectPool.instance.spawnedObjects.ContainsKey(obj);
		}

		public static int CountPooled<T>(T prefab) where T : Component
		{
			return ObjectPool.CountPooled(prefab.gameObject);
		}

		public static int CountPooled(GameObject prefab)
		{
			List<GameObject> list;
			if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out list))
			{
				return list.Count;
			}
			return 0;
		}

		public static int CountSpawned<T>(T prefab) where T : Component
		{
			return ObjectPool.CountSpawned(prefab.gameObject);
		}

		public static int CountSpawned(GameObject prefab)
		{
			int num = 0;
			using (Dictionary<GameObject, GameObject>.ValueCollection.Enumerator enumerator = ObjectPool.instance.spawnedObjects.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					if ((UnityEngine.Object)prefab == (UnityEngine.Object)current)
						++num;
				}
			}
			return num;
		}

		public static int CountAllPooled()
		{
			int num = 0;
			using (Dictionary<GameObject, List<GameObject>>.ValueCollection.Enumerator enumerator = ObjectPool.instance.pooledObjects.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<GameObject> current = enumerator.Current;
					num += current.Count;
				}
			}
			return num;
		}

		public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
		{
			if (list == null)
				list = new List<GameObject>();
			if (!appendList)
				list.Clear();
			List<GameObject> list1;
			if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out list1))
				list.AddRange((IEnumerable<GameObject>)list1);
			return list;
		}

		public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
		{
			if (list == null)
				list = new List<T>();
			if (!appendList)
				list.Clear();
			List<GameObject> list1;
			if (ObjectPool.instance.pooledObjects.TryGetValue(prefab.gameObject, out list1))
			{
				for (int index = 0; index <= list1.Count; index++)
				{
					list.Add(list1[index].GetComponent<T>());
				}
			}
			return list;
		}

		public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
		{
			if (list == null)
				list = new List<GameObject>();
			if (!appendList)
			{
				list.Clear();
			}

			using (Dictionary<GameObject, GameObject>.Enumerator enumerator = ObjectPool.instance.spawnedObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<GameObject, GameObject> current = enumerator.Current;
					if ((UnityEngine.Object)current.Value == (UnityEngine.Object)prefab)
						list.Add(current.Key);
				}
			}
			return list;
		}

		public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
		{
			if (list == null)
				list = new List<T>();
			if (!appendList)
				list.Clear();
			GameObject gameObject = prefab.gameObject;
			using (Dictionary<GameObject, GameObject>.Enumerator enumerator = ObjectPool.instance.spawnedObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<GameObject, GameObject> current = enumerator.Current;
					if ((UnityEngine.Object)current.Value == (UnityEngine.Object)gameObject)
						list.Add(current.Key.GetComponent<T>());
				}
			}
			return list;
		}

		public enum StartupPoolMode
		{
			Awake,
			Start,
			CallManually,
		}

		[Serializable]
		public class StartupPool
		{
			public int size;
			public GameObject prefab;
		}
	}
}
