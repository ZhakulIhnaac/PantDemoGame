using System.Collections.Generic;
using Assets.Scripts.Classes.Gameplay;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class ObjectPooling : MonoBehaviour, IObjectPooling
    {
        public static ObjectPooling Instance;

        [System.Serializable]
        public class ObjectPool // Pool to store the game objects.
        {
            public string objTag;
            public GameObject objToStore;
            public int poolSize;
        }

        public List<ObjectPool> poolList; // Used to feed Pool objects from Unity interface.
        public Dictionary<string, Queue<GameObject>> poolDictionary; // Each pool of objects will be stored inside a dictionary.

        void Awake()
        {
            if (Instance == null) // We will only have one ObjectPooling (Attached to MainGame Singleton) object in out scene. Thus, we just make it unique (Singleton)
            {
                Instance = this;
            }
        }

        void Start()
        {
            CreatePool();
        }

        public void CreatePool()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            foreach (var pool in poolList)
            {
                Queue<GameObject> gameObjPool = new Queue<GameObject>();

                for (int index = 0; index < pool.poolSize; index++)
                {
                    GameObject poolObj = Instantiate(pool.objToStore, transform.position, Quaternion.identity);
                    poolObj.SetActive(false);
                    gameObjPool.Enqueue(poolObj);
                }

                poolDictionary.Add(pool.objTag, gameObjPool);
            }
        }

        public void SpawnFromPool(GameObject gameObjectToSpawn, Vector2 spawnPosition, Quaternion spawnRotation, GameObject template = null)
        {
            // Spawn
            GameObject objectToSpawn = poolDictionary[gameObjectToSpawn.tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = spawnPosition;
            objectToSpawn.transform.rotation = spawnRotation;

            poolDictionary[gameObjectToSpawn.tag].Enqueue(objectToSpawn);

            GameController.Instance.powerAmount -= gameObjectToSpawn.GetComponent<Playable>().BuildingCost;
            GameController.Instance.UpdatePowerAmount();

            if (template != null)
            {
                Destroy(template);
            }

            // Obstruct the nodes
            Vector2 objBottomLeftPoint = (Vector2)spawnPosition + Vector2.left * objectToSpawn.GetComponent<Playable>().PlayableSize.x / 2 + Vector2.down * objectToSpawn.GetComponent<Playable>().PlayableSize.y / 2; // transform.position will give the middle point and we will subtract the halves of the height and width to find bottom left point
            for (int i = 0; i < gameObjectToSpawn.GetComponent<Playable>().PlayableSize.x; i++)
            {
                for (int j = 0; j < gameObjectToSpawn.GetComponent<Playable>().PlayableSize.y; j++)
                {
                    var node = GameController.GridSystem.NodeFromWorldPosition(
                        objBottomLeftPoint + new Vector2(i * GameController.GridSystem._nodeDiameter,
                            j * GameController.GridSystem._nodeDiameter));
                    
                    if (node != null)
                    {
                        node.IsObstructed = true;
                    }
                }
            }
        }
    }
}
