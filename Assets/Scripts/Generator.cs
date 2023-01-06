using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private GameObject instance;

    private void Awake()
    {
        Generate();
        GetComponent<SpriteRenderer>().color = Color.clear;
    }

    public void Generate()
    {
        instance = Instantiate(prefab, transform.position, transform.rotation);
    }

    public void RemoveInstance()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
    }
}
