using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Block : MonoBehaviour
{

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player_Control>().initialPosition = transform.position + Vector3.up * 3f;
        }
    }
}
