using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;
    private Transform path;

    void Start()
    {
        if(transform.childCount > 0)
        {
            path = transform.GetChild(0);
        }

        if (enemyPrefab != null)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, transform, false);
            newEnemy.GetComponent<Enemy_Controller>().setPatrolPath(path);
        }

    }
}
