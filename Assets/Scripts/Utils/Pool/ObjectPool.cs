using System.Collections.Generic;
using UnityEngine;

namespace Utils.Pool
{
    public class ObjectPool
    {
        private GameObject prefab;
        private List<GameObject> pool;
        private GameObject parent = null;

        public ObjectPool(GameObject prefab, int initialSize, string parentName = null)
        {
            this.prefab = prefab;
            this.pool = new List<GameObject>();

            if (parentName != null)
            {
                parent = GameObject.Find(parentName);
            }
            
            for (var i = 0; i < initialSize; i++)
            {
                AllocateInstance();
            }
        }

        public GameObject GetInstance()
        {
            if (pool.Count == 0)
            {
                AllocateInstance();
            }

            var lastIndex = pool.Count - 1;
            var instance = pool[lastIndex];
            instance.SetActive(true);
            pool.RemoveAt(lastIndex);

            return instance;
        }

        public void ReturnInstance(GameObject instance)
        {
            instance.SetActive(false);
            pool.Add(instance);
        }

        private void AllocateInstance()
        {
            var instance = Object.Instantiate(prefab);
            instance.SetActive(false);

            if (parent != null)
            {
                instance.transform.parent = parent.transform;
            }
            
            pool.Add(instance);
        }
    }
}