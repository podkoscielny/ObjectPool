using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AoOkami.ObjectPool
{
    [CreateAssetMenu(fileName = "ObjectPool", menuName = "ScriptableObjects/ObjectPool")]
    public class ObjectPoolSO : ScriptableObject
    {
        [System.Serializable]
        public struct Pool
        {
            public PoolTags tag;
            public GameObject prefab;
            public int size;
        }

        [SerializeField] List<Pool> pools;
        [SerializeField] Dictionary<PoolTags, Queue<GameObject>> poolDictionary;

        private void OnDisable() => InitializePoolDictionary();

        public void InitializePool()
        {
            InitializePoolDictionary();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject instance = Instantiate(pool.prefab);
                    instance.AddComponent<PooledObject>().SetPoolTag(pool.tag);
                    instance.SetActive(false);
                    objectPool.Enqueue(instance);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public bool IsTagInDictionary(PoolTags tag) => poolDictionary.ContainsKey(tag);

        public void AddToPool(GameObject instance)
        {
            if (instance.TryGetComponent(out PooledObject pooledObject))
            {
                PoolTags pooledTag = pooledObject.PoolTag;

                if (!poolDictionary.ContainsKey(pooledTag)) return;

                Queue<GameObject> pool = poolDictionary[pooledTag];

                instance.SetActive(false);
                pool.Enqueue(instance);

            }
        }

        public GameObject GetFromPool(PoolTags tag)
        {
            if (!poolDictionary.ContainsKey(tag)) return null;

            else if (poolDictionary[tag].Count < 1) return InstantiateNewPrefab(tag);

            else return GetObjectToSpawn(tag);
        }

        public GameObject GetFromPool(PoolTags tag, Vector3 position)
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

        public GameObject GetFromPool(PoolTags tag, Vector3 position, Quaternion rotation)
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

        private GameObject InstantiateNewPrefab(PoolTags tag)
        {
            GameObject desiredPrefab = pools.Find(p => p.tag == tag).prefab;
            GameObject desiredObject = Instantiate(desiredPrefab);

            return desiredObject;
        }

        private GameObject GetObjectToSpawn(PoolTags tag)
        {
            Queue<GameObject> pool = poolDictionary[tag];
            GameObject objectToSpawn = pool.Dequeue();
            objectToSpawn.SetActive(true);

            return objectToSpawn;
        }

        private void InitializePoolDictionary() => poolDictionary = new Dictionary<PoolTags, Queue<GameObject>>();
    }
}
