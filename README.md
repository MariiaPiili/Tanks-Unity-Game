# Tanks 2-player game 
A 2-player shooter game, including simple game mechanics and UI, built using Unity/C#.
## Instructions
Each player controls 4 keys on the keyboard to move their tank. The first player to hit the other player 5 times wins the round.
## Movement сontrol
### Player 1:
- Shoot: Spacebar
- Move Forward: W 
- Move Backward: S
- Move Left: A
- Move Rigth: D
### Player 2:
- Shoot: Right Shift
- Move Forward: UpArrow
- Move Backward: Down Arrow
- Move Left: Left Arrow
- Move Rigth: Right Arrow

Control is made by script `TankMovement.cs`

```csharp
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private KeyCode _keyRight;
    [SerializeField] private KeyCode _keyLeft;
    [SerializeField] private KeyCode _keyUp;
    [SerializeField] private KeyCode _keyDown;

    private Rigidbody _rigidbody;
    private int _lastRotation;
    private float _horizont;
    private float _vertical;
    private float _direction;
    private float _angle;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateHorizontalMovement();
        UpdateVerticalMovement();
    }

    private void UpdateHorizontalMovement()
    {
        if (Input.GetKey(_keyLeft))
        {
            _lastRotation = -90;
            _horizont = -1;
        }
        else if (Input.GetKey(_keyRight))
        {
            _lastRotation = 90;
            _horizont = 1;
        }
        else
        {
            _horizont = 0;
        }
    }

    private void UpdateVerticalMovement()
    {
        if (Input.GetKey(_keyUp))
        {
            _lastRotation = 0;
            _vertical = 1;
        }
        else if (Input.GetKey(_keyDown))
        {
            _lastRotation = 180;
            _vertical = -1;
        }
        else
        {
            _vertical = 0;
        }
    }

    private void FixedUpdate()
    {
        transform.eulerAngles = Vector3.up * _lastRotation;

        _rigidbody.AddForce(Vector3.right * _speed * _horizont, ForceMode.VelocityChange);
        _rigidbody.AddForce(Vector3.forward * _speed * _vertical, ForceMode.VelocityChange);
    }
}
```

## Health control
The health of the game object is managed by the `HealthController.cs`. It has two properties: CurrentHealth, which tracks the object's current health, and MaxHealth, which defines the maximum health the object can have.

In the beginning of the game, the object's current health is set to the maximum value. When the object collides with something that has a `Bullet.cs` component, its health decreases by one. If the health reaches zero, the object is destroyed.

```csharp
using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public Action<int> ChangedHealth;

    [SerializeField] private int _maxHealth;

    private int _currentHealth;

    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        private set
        {
            _currentHealth = value;
            ChangedHealth.Invoke(_currentHealth);
        }
    }

    public int MaxHealth => _maxHealth;

    private void Start()
    {
        CurrentHealth = _maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            CurrentHealth--;
            if (CurrentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
```

## Bullet - Game Object

This script `Bullet.cs` manages the behavior of a bullet in the game. It controls the bullet's speed, movement, and ricochets when it hits other objects.

When the bullet spawns, it moves forward with some random horizontal deviation.
On collision, the bullet ricochets in a new direction until it reaches the maximum ricochet count. Once the limit is reached, the bullet is destroyed.

```csharp
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
```

## Shooting system

The script `ShootingSystem.cs`, attached to an empty object called "Gun", handles shooting mechanics in a game.

In the Update method, the script checks if the specified shoot key is pressed. If so, it creates a bullet at the current position and rotation of the object with this script.

```csharp
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
```

## Score manager

`ScoreManager.cs` is responsible for displaying the player's score on a text element in the UI.

The script listens for changes in health using the ChangedHealth event from the `HealthController.cs`.

When health changes, the score is updated based on the difference between the maximum health and the current health, and the result is displayed on the UI.

```csharp
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;    
    [SerializeField] private HealthController _healthController;

    private void OnEnable()
    {
        _healthController.ChangedHealth += UpdateScore;
    }

    private void OnDisable()
    {
        _healthController.ChangedHealth -= UpdateScore;
    }
    private void UpdateScore(int health)
    {
        _text.text = $"Score: {_healthController.MaxHealth - health}";        
    }
}
```

## References

https://yandex.ru/games/app/207145#app-id=207145&catalog-session-uid=catalog-eafe7c04-74d0-557d-ac21-3bdc5004e019-1727289577327-109c&rtx-reqid=5744098127717580016&pos=%7B%22listType%22%3A%22played%22%2C%22tabCategory%22%3A%22common%22%7D&redir-data=%7B%22block%22%3A%22played%22%2C%22block_index%22%3A0%2C%22card%22%3A%22your_games%22%2C%22col%22%3A0%2C%22first_screen%22%3A1%2C%22page%22%3A%22main%22%2C%22rn%22%3A108128833%2C%22row%22%3A0%2C%22rtx_reqid%22%3A%225744098127717580016%22%2C%22same_block_index%22%3A0%2C%22wrapper%22%3A%22played_games%22%2C%22http_ref%22%3A%22https%253A%252F%252Fyandex.ru%252Fgames%252F%22%7D

## Requirements

Unity Game Engine
