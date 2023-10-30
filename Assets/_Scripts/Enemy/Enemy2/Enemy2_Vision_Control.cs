using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2_Vision_Control : MonoBehaviour
{
    [SerializeField] Enemy2_Controller controller;
    private void OnTriggerEnter(Collider other)
    {
        if (controller.died) return;
        if (other.tag == "Player")
        {
            if (controller.UnTarget_Coroutine != null) StopCoroutine(controller.UnTarget_Coroutine);
            controller._animator.SetTrigger("Taunt");
            controller._Target = other.transform;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (controller.died) return;
        if (other.tag == "Player")
        {
            controller._Target = other.transform;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            controller.UnTarget_Coroutine = StartCoroutine(controller.UnTarget());
        }
    }
}
