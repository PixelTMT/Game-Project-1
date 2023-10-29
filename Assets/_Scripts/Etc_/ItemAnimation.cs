using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    [SerializeField]
    float spin_amount = 5f;

    [SerializeField]
    float y_amount = .5f;
    IEnumerator Start()
    {
        while (gameObject.activeSelf)
        {
            float y = Mathf.Sin(Time.time) * y_amount;
            var coin = transform.GetChild(0);
            coin.localPosition = new Vector3(0, y, 0);
            coin.Rotate(Vector3.up * spin_amount);

            yield return new WaitForFixedUpdate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
