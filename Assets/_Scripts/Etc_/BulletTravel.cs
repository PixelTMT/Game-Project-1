using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BulletTravel : MonoBehaviour
{
    [SerializeField] string TagExeption = "Player";
    [SerializeField] float speed = 20f;
    [SerializeField] GameObject _ImpactParticle;
    [SerializeField] Color _ImpactParticleColor = Color.white;
    Transform sfx;
    private void Awake()
    {
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        transform.Rotate(new Vector3(0, 0, Time.time));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag(TagExeption))
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        sfx = transform.Find("SoundFX");
    }
    private void OnDisable()
    {
        var p = Instantiate(_ImpactParticle, transform.localToWorldMatrix.GetPosition(), Quaternion.identity);
        if (!sfx.gameObject.activeInHierarchy) return;
        sfx.SetParent(p.transform);
        sfx.Find("Exploded").GetComponent<AudioSource>().Play();
        var particle_main = p.GetComponent<ParticleSystem>().main;
        particle_main.startColor = _ImpactParticleColor;
    }
}
