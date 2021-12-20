using UnityEngine;

namespace CustomFeatures
{
    public class TaycpuSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;


        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {

                        GameObject go = new GameObject();
                        instance = go.AddComponent<T>();
                        go.name = typeof(T).Name;
                    }
                }


                return instance;
            }
            set => instance = value;
        }

        public bool dontDestroyOnLoad;

        public void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(Instance);
            }
        }
    }
}