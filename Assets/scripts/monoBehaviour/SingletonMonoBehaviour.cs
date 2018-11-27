using UnityEngine;

namespace monoBehaviour
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogError(typeof(T) + " is none");
                }
                return _instance;
            }
        }

        public static bool Exist()
        {
            return _instance != null;
        }
    }
}
