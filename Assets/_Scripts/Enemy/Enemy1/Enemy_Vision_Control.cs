using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Vision_Control : MonoBehaviour
{
    [SerializeField] Enemy_Controller controller;
    [SerializeField] Transform enemy;

    Transform _transform;
    Coroutine LoseTrack;
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

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (LoseTrack != null) StopCoroutine(LoseTrack);
            LoseTrack = StartCoroutine(loseTarget(5));
        }
    }
    IEnumerator loseTarget(float time)
    {
        yield return new WaitForSeconds(time);
        controller._Target = null;
    }
}
