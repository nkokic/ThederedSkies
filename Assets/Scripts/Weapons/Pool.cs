using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Pool<T> where T : MonoBehaviour
{
    Transform parent;
    T elementPrefab;
    List<T> pool;
    int capacity;
    int cursor = -1;
    float elementLifeTime = 1;

    public Pool(int capacity, T elementPrefab, float elementLifeTime, Transform poolTransform)
    {
        parent = poolTransform;
        this.capacity = capacity;
        this.elementPrefab = elementPrefab;
        this.elementLifeTime = elementLifeTime;

        pool = new List<T>();

        for (int i = 0; i < capacity; i++)
            Add(elementPrefab);
    }

    public void SetCapacity(int capacity)
    {
        this.capacity = capacity;
        while(capacity > pool.Count)
            Add(elementPrefab);
    }

    void Remove(T element)
    {
        Object.Destroy(element.gameObject);

        if (pool.IndexOf(element) < cursor)
            cursor--;
        pool.Remove(element);
    }

    void Add(T elementPrefab)
    {
        T element = Object.Instantiate(elementPrefab, parent);
        element.gameObject.SetActive(false);
        pool.Insert(cursor + 1, element);
    }

    void Next()
    {
        cursor = (cursor + 1) % pool.Count;
    }

    public T GetObject()
    {
        Next();
        T element = pool[cursor];
        if (element.gameObject.activeSelf)
        {
            throw new System.OverflowException($"Selected pool element {element.name} is already active and in use.");
        }   

        element.gameObject.SetActive(true);
        element.StartCoroutine(CountDownLifeTime(element));

        return element;
    }

    IEnumerator CountDownLifeTime(T element)
    {
        yield return new WaitForSeconds(elementLifeTime);
        element.gameObject.SetActive(false);
        if (capacity < pool.Count)
            Remove(element);
    }
}

