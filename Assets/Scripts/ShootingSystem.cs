using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private KeyCode _keyShoot;

    private void Update()
    {
        if (Input.GetKeyDown(_keyShoot))
        {
            Instantiate(_bulletPrefab, transform.position, transform.rotation);
        }
    }
}
