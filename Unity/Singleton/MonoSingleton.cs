using UnityEngine;

namespace Singleton
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool destroyed = false;
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null || destroyed)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
                        go.AddComponent<DontDestroy>();
                    }
                }

                return instance;
            }
        }

        public void OnDestroy()
        {
            destroyed = true;
        }
    }
}