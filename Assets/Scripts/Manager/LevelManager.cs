using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region singleton
    public static LevelManager _instance = null;
    public static LevelManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
            }

            return _instance;
        }
    }
    #endregion

    [Header("Enemies")]
    public Transform[] enemiesPoint;
    public GameObject[] enemies;

    [Header("Items")]
    public Transform[] itemsPoint;
    public GameObject[] items;

    private int allEnemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateAllEnemies()
    {
        
    }
}
