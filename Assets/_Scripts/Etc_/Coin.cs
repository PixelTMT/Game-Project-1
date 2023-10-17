using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    public float value = 1;

    [SerializeField]
    public Color color;

    [SerializeField]
    public GameObject particle;
    public void Collected()
    {
        var p = Instantiate(particle, transform.localToWorldMatrix.GetPosition(), Quaternion.identity);
        p.GetComponent<ParticleSystem>().startColor = color;
        Destroy(gameObject);
    }
}
