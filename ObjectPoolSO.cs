using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tags = AoOkami.MultipleTagSystem.TagSystem.Tags;

namespace AoOkami.ObjectPool
{
    [CreateAssetMenu(fileName = "ObjectPool", menuName = "ScriptableObjects/ObjectPool")]
    public class ObjectPoolSO : ScriptableObject
    {
        [System.Serializable]
        public struct Pool
        {
            public Tags tag;
            public GameObject prefab;
            public int size;
        }

        [SerializeField] List<Pool> pools;
        [SerializeField] Dictionary<Tags, Queue<GameObject>> poolDictionary;

        private void OnDisable() => poolDictionary = new Dictionary<Tags, Queue<GameObject>>();

        public void InitializePool()
        {
            poolDictionary = new Dictionary<Tags, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.AddComponent<PooledObject>().SetPoolTag(pool.tag);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public bool IsTagInDictionary(Tags tag) => poolDictionary.ContainsKey(tag);

        public void AddToPool(GameObject instance)
        {
            if (instance.TryGetComponent(out PooledObject pooledObject))
            {
                Tags pooledTag = pooledObject.PoolTag;

                if (!poolDictionary.ContainsKey(pooledTag)) return;

                Queue<GameObject> pool = poolDictionary[pooledTag];

                instance.SetActive(false);
                pool.Enqueue(instance);

            }
        }

        public GameObject GetFromPool(Tags tag)
        {
            if (!poolDictionary.ContainsKey(tag)) return null;

            else if (poolDictionary[tag].Count < 1) return InstantiateNewPrefab(tag);

            else return GetObjectToSpawn(tag);
        }

        public GameObject GetFromPool(Tags tag, Vector3 position)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                return null;
            }
            else if (poolDictionary[tag].Count < 1)
            {
                GameObject newInstance = InstantiateNewPrefab(tag);
                newInstance.transform.position = position;

                return newInstance;
            }
            else
            {
                GameObject objectToSpawn = GetObjectToSpawn(tag);
                objectToSpawn.transform.position = position;

                return objectToSpawn;
            }
        }

        public GameObject GetFromPool(Tags tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                return null;
            }
            else if (poolDictionary[tag].Count < 1)
            {
                GameObject newInstance = InstantiateNewPrefab(tag);
                newInstance.transform.SetPositionAndRotation(position, rotation);

                return newInstance;
            }
            else
            {
                GameObject objectToSpawn = GetObjectToSpawn(tag);
                objectToSpawn.transform.SetPositionAndRotation(position, rotation);

                return objectToSpawn;
            }
        }

        private GameObject InstantiateNewPrefab(Tags tag)
        {
            GameObject desiredPrefab = pools.Find(p => p.tag == tag).prefab;
            GameObject desiredObject = Instantiate(desiredPrefab);

            return desiredObject;
        }

        private GameObject GetObjectToSpawn(Tags tag)
        {
            Queue<GameObject> pool = poolDictionary[tag];
            GameObject objectToSpawn = pool.Dequeue();
            objectToSpawn.SetActive(true);

            return objectToSpawn;
        }
    }
}
