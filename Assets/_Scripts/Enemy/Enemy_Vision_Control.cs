using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Vision_Control : MonoBehaviour
{
    [SerializeField] Enemy_Controller controller;
    private void OnTriggerEnter(Collider other)
    {
        if (controller.died) return;
        if (other.tag == "Player")
        {
            if (controller.UnTarget_Coroutine != null) StopCoroutine(controller.UnTarget_Coroutine);
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

            if (Vector3.Distance(e_dist, p_dist) < controller._attackRange)
            {
                controller._animator.SetBool(Enemy_Animation.Walk, false);
                controller._animator.SetBool(Enemy_Animation.Attack, true);
                controller._attackHitBox.SetActive(true);
            }
            controller._Target = other.transform;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            controller.UnTarget_Coroutine = StartCoroutine(controller.UnChasing());
        }
    }
}
