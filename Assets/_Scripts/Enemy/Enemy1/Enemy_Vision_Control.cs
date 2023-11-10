using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Vision_Control : MonoBehaviour
{
    [SerializeField] Enemy_Controller controller;
    [SerializeField] Transform enemy;

    Transform _transform;
    private void Awake()
    {
        _transform = transform;
    }
    private void LateUpdate()
    {
        if (controller.died)
        {
            Destroy(gameObject);
            return;
        }
        _transform.position = enemy.position;
        _transform.rotation = enemy.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (controller.died) return;
        if (other.tag == "Player")
        {
            controller._Target = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (controller.died) return;
        if (other.tag == "Player")
        {
            Vector3 p_dist = other.transform.position;
            p_dist.y = 0;

            Vector3 e_dist = transform.position;
            e_dist.y = 0;

            controller._Target = other.transform;
        }

    }
    private void OnTriggerExit(Collider other)
    {

    }
}
