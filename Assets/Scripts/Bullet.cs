using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _maxCountRicochet;
    [SerializeField] private float _bulletSpeed;

    private Rigidbody _rigidbody;
    private int _countRicochet;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        BulletMoves();
    }

    private void Ricochet(Vector3 direction)
    {
        _rigidbody.velocity = direction * _bulletSpeed;
    }

    private void BulletMoves()
    {
        _rigidbody.velocity = transform.forward * _bulletSpeed + Vector3.right * Random.Range(-1.5f, 1.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_countRicochet < _maxCountRicochet)
        {
            _countRicochet++;
            Ricochet(collision.contacts[0].normal);
            return;
        }
        Destroy(gameObject);
    }


}
