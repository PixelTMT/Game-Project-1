using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    public float value = 0;

    [SerializeField]
    public CollectedType collectedType = CollectedType.coin;

    [SerializeField]
    public Color color;

    [SerializeField]
    public GameObject particle;

    [SerializeField]

    public enum CollectedType
    {
        coin, gun
    }
    public void Collected()
    {
        var p = Instantiate(particle, transform.localToWorldMatrix.GetPosition(), Quaternion.identity);
        var particle_main = p.GetComponent<ParticleSystem>().main;
        particle_main.startColor = color;
        Destroy(gameObject);
    }
}
