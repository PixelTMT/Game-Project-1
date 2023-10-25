using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Block : MonoBehaviour
{
    [SerializeField] CapsuleCollider playerCollider;
    [SerializeField] Rigidbody rb;
    private PhysicMaterial playerPhysicMaterial;
    void Start()
    {
        playerPhysicMaterial = playerCollider.material;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPhysicMaterial.dynamicFriction = 20;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
