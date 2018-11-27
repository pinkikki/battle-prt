using db;
using dotween;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameStartingInitializer : MonoBehaviour, Initializer, DatabaseListener
    {
        void Awake()
        {
            Init();
        }

        public void Init()
        {
            gameObject.AddComponent<DOTWeenInitializer>().Init();
            DbManager.Init(this, this);
        }

        public void OnDatabaseInit()
        {
            Debug.Log("db initialized successfully");
        }
    }
}