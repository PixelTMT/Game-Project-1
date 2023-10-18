using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    public float value = 1;

    [SerializeField]
    float spin_amount = 5f;

    [SerializeField]
    float y_amount = 1f;

    [SerializeField]
    public Color color;

    [SerializeField]
    public GameObject particle;
    IEnumerator Start()
    {
        while(gameObject.activeSelf)
        {
            float y = Mathf.Sin(Time.time) * y_amount;
            var coin = transform.GetChild(0);
            coin.localPosition = new Vector3(0, y, 0);
            coin.Rotate(Vector3.up * spin_amount);

            yield return new WaitForFixedUpdate();
        }
    }
    public void Collected()
    {
        var p = Instantiate(particle, transform.localToWorldMatrix.GetPosition(), Quaternion.identity);
        p.GetComponent<ParticleSystem>().startColor = color;
        Destroy(gameObject);
    }
}
