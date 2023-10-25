using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CombiningCollider : MonoBehaviour
{
    public bool CombinePoint = false;

    BoxCollider box;
    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }
    int d = 2;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") && CombinePoint)
        {
            var otherBoxCollider = collision.gameObject.GetComponent<BoxCollider>();
            otherBoxCollider.enabled = false;
            Debug.Log("Combine");
            var boxBound = box.bounds;
            var otherBounds = otherBoxCollider.bounds;
            boxBound.Encapsulate(otherBounds);

            box.size = boxBound.size;

            //box.center = (transform.localPosition - collision.transform.localPosition)/2;

        }
    }
}
