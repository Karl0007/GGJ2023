using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GGJ
{

    public class Singleton<T> : SerializedMonoBehaviour where T : Singleton<T>
    {
        private static T m_Instance;
        public static T Instance
        {
            get { return m_Instance; }
        }

        protected virtual void Awake()
        {
            if (m_Instance != null && m_Instance != this)
                Destroy(gameObject);
            else
                m_Instance = (T)this;
        }

        public static bool IsInitialized
        {
            get { return m_Instance != null; }
        }

        protected virtual void OnDestroy()
        {
            if (m_Instance == this)
            {
                m_Instance = null;
            }
        }
    }
}
