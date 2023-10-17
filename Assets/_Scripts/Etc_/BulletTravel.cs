using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BulletTravel : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] GameObject _particle;
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            var p = Instantiate(_particle, transform.localToWorldMatrix.GetPosition(), Quaternion.identity);
            p.GetComponent<ParticleSystem>().startColor = Color.white;
            Destroy(gameObject);
        }
    }
}
