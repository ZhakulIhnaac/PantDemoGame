using System;
using System.Collections.Generic;
using Assets.Scripts.Classes.Playables;
using Assets.Scripts.Constants;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes.Gameplay
{
    public class ObjectPooling : MonoBehaviour, IObjectPooling
    {
        /*
         ObjectPool class is only related to and being used in the ObjectPooling class.
         It acts as a pool for game objects to be stored.
         */
        [System.Serializable]
        public class ObjectPool
        {
            public string ObjTag;
            public GameObject ObjToStore;
            public int PoolSize;
        }

        /* Events */
        public static event Action<string> GiveWarning; // Event for giving warning to GUI controller.
        public static event Action<float> ReducePowerAmount; // Event for reducing power amount when producing a new playable.

        public static ObjectPooling Instance; // Singleton
        public List<ObjectPool> PoolList; // Used to feed Pool objects from Unity interface.
        public Dictionary<string, Queue<GameObject>> PoolDictionary; // Each pool of objects will be stored inside a dictionary.

        private void Awake()
        {
            if (Instance == null) // Making the singleton.
            {
                Instance = this;
            }
        }

        private void Start()
        {
            CreatePool();
        }


        /*
        Initial action of the object pool instance. It creates the
        pools for the given units in "PoolList" list variable.
         */
        public void CreatePool()
        {
            PoolDictionary = new Dictionary<string, Queue<GameObject>>();
            foreach (var pool in PoolList)
            {
                var gameObjPool = new Queue<GameObject>();

                for (var index = 0; index < pool.PoolSize; index++)
                {
                    var poolObj = Instantiate(pool.ObjToStore, transform.position, Quaternion.identity);
                    poolObj.SetActive(false);
                    gameObjPool.Enqueue(poolObj);
                }

                PoolDictionary.Add(pool.ObjTag, gameObjPool);
            }
        }

        /*
         Any playable game object created by the player is being spawned
         from the pool initially defined by the CreatePool method. SpawnFromPool
         method does the spawning after checking the available resources and required
         resources by the game object to be spawned.
         */
        public void SpawnFromPool(GameObject gameObjectToSpawn, Vector2 spawnPosition, Quaternion spawnRotation, GameObject template = null)
        {
            if (gameObjectToSpawn.GetComponent<Playable>().PowerCost > GameController.Instance.PowerAmount) // Checking if the amount of power in hand can meet the required power amount.
            {
                GiveWarning?.Invoke(InGameDictionary.InsufficientPowerWarning);
            }
            else // There is enough resource for the game object to be spawned.
            {
                // Spawn
                var objectToSpawn = PoolDictionary[gameObjectToSpawn.tag].Dequeue();

                /*
                 Before spawning the new object into the scene, object to be spawned must be checked if it was already in the scene.
                 In case of not moving the object in its first spawn, its first position's nodes remain obstructed and it becomes
                 impossible to move to those nodes or place any building.
                 */
                if (objectToSpawn.activeSelf)
                {
                    NodeObstructionChange(objectToSpawn.transform.position, objectToSpawn, gameObjectToSpawn);
                }

                objectToSpawn.SetActive(true);
                objectToSpawn.transform.position = spawnPosition;
                objectToSpawn.transform.rotation = spawnRotation;

                PoolDictionary[gameObjectToSpawn.tag].Enqueue(objectToSpawn);

                ReducePowerAmount?.Invoke(gameObjectToSpawn.GetComponent<Playable>().PowerCost);

                if (template != null)
                {
                    Destroy(template);
                }

                // Obstruct the nodes
                NodeObstructionChange(spawnPosition, objectToSpawn, gameObjectToSpawn);
            }
        }

        /*
         NodeObstructionChange method changes the "availability" of the nodes below the
         game object to be spawned or to be taken from the pool to be the new spawned.
         When a game object spawned onto a node or node group, or when it is taken from
         the game area because of being the next in pool, the nodes below the game object
         changes their status for being obstructed.
         */
        private void NodeObstructionChange(Vector2 pSpawnPosition, GameObject pObjectToSpawn, GameObject pGameObjectToSpawn)
        {
            var objToSpawnBottomLeftPoint = (Vector2)pSpawnPosition + Vector2.left * pObjectToSpawn.GetComponent<Playable>().PlayableSize.x / 2 + Vector2.down * pObjectToSpawn.GetComponent<Playable>().PlayableSize.y / 2 + new Vector2(GameController.GridSystem.NodeRadius, GameController.GridSystem.NodeRadius); // transform.position will give the middle point and we will subtract the halves of the height and width to find bottom left point
            for (var i = 0; i < pGameObjectToSpawn.GetComponent<Playable>().PlayableSize.x; i++) // For each grid square in width...
            {
                for (var j = 0; j < pGameObjectToSpawn.GetComponent<Playable>().PlayableSize.y; j++) // For each grid square in height...
                {
                    var node = GameController.GridSystem.NodeFromWorldPosition(objToSpawnBottomLeftPoint + new Vector2(i * GameController.GridSystem.NodeDiameter, j * GameController.GridSystem.NodeDiameter));

                    if (node != null)
                    {
                        node.IsObstructed = !node.IsObstructed;
                    }
                }
            }
        }
    }
}
