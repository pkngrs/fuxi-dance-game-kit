using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fuxi.Infrastructure.Utility
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T m_instance;
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    m_instance = go.AddComponent<T>();
                }
                return m_instance;
            }
        }

        protected void Awake()
        {
            m_instance = gameObject.GetComponent<T>();
        }
    }

    [System.Serializable]
    public abstract class JsonIO<T>
    {
#if UNITY_EDITOR
        public void Save(string directoryPath, string targetMeshName, string extensionName)
        {
            var path = EditorUtility.SaveFilePanel("save", directoryPath, targetMeshName, extensionName);
            Save(path);
        }

        public static T Load(string directoryPath, string extensionName)
        {
            var path = EditorUtility.OpenFilePanel("load", directoryPath, extensionName);
            return Load(path);
        }
#endif

        public void Save(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var s = ToString();
                if (s != null)
                {
                    File.WriteAllText(path, s);
                    Debug.LogFormat("Saved. Path:{0}", path);
                }
            }
        }

        public static T Load(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string json = File.ReadAllText(path);
                var v = JsonUtility.FromJson<T>(json);
                if (v != null)
                {
                    Debug.LogFormat("Loaded. Path:{0}", path);
                }
                return v;
            }
            return default(T);
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}