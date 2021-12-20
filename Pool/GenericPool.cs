using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomFeatures;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PoolPiece
{
    public List<Component> pool = new List<Component>();

    public Component this[int i]
    {
        get { return pool[i]; }
        set { pool[i] = value; }
    }
}

public class GenericPool : TaycpuSingleton<GenericPool>
{
    public List<PoolPiece> pools;
    [HideInInspector] public int[] indices;
    public string[][] names;

    public T GetFromPool<T>(int poolIndex) where T : Component
    {
        int i = 0;
        while (pools[poolIndex][i].gameObject.activeInHierarchy)
        {
            i++;
            if (i > pools[poolIndex].pool.Count - 1)
            {
                var newClone = Instantiate(pools[poolIndex][0], Vector3.zero, Quaternion.identity,
                    pools[poolIndex][0].transform.parent);
                newClone.gameObject.SetActive(false);
                pools[poolIndex].pool.Add(newClone);
            }
        }

        return pools[poolIndex][i] as T;
    }


    public T GetRandomObject<T>() where T : MonoBehaviour
    {
        int randPoolsIndex = Random.Range(0, pools.Count);
        int randPoolIndex = Random.Range(0, pools[randPoolsIndex].pool.Count);
        while (pools[randPoolsIndex][randPoolIndex].gameObject.activeInHierarchy)
        {
            randPoolsIndex = Random.Range(0, pools.Count);
            randPoolIndex = Random.Range(0, pools[randPoolsIndex].pool.Count);
        }

        return pools[randPoolsIndex][randPoolIndex] as T;
    }

    [ContextMenu("Get Childs")]
    public void GetChilds()
    {
        pools.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            pools.Add(new PoolPiece());
            for (int j = 0; j < child.childCount; j++)
            {
                var comps = child.GetChild(j).GetComponents<Component>();
                for (int k = 0; k < comps.Length; k++)
                {
                    if (comps[k].GetType().Name == names[i][indices[i]])
                        pools[pools.Count - 1].pool.Add(comps[k]);
                }
            }
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(GenericPool))]
public class GenericPoolEditor : Editor
{
    private GenericPool genericPool;

    private void OnEnable()
    {
        genericPool = target as GenericPool;

        genericPool.indices = new int[genericPool.transform.childCount];
        genericPool.names = new string[genericPool.indices.Length][];
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        for (int i = 0; i < genericPool.transform.childCount; i++)
        {
            Transform child = genericPool.transform.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                var comps = child.GetChild(j).GetComponents<Component>();
                genericPool.names[i] = new string[comps.Length];

                for (int k = 0; k < comps.Length; k++)
                {
                    genericPool.names[i][k] = comps[k].GetType().Name;
                }
            }
        }

        for (int i = 0; i < genericPool.indices.Length; i++)
        {
            genericPool.indices[i] = EditorGUILayout.Popup(genericPool.indices[i], genericPool.names[i]);
        }

        genericPool.GetChilds();
    }
}
#endif