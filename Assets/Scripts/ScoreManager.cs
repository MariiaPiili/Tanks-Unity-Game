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
