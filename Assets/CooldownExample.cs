using UnityEngine;

public class CooldownExample : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField] private float redColorDuration = 1;

    private float _currentTimeInGame;
    private float _lastTimeWasDamaged;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ChangeColorIfNeeded();
    }

    private void ChangeColorIfNeeded()
    {
        _currentTimeInGame = Time.time;
        if (_currentTimeInGame - _lastTimeWasDamaged > redColorDuration && _sr.color != Color.white)
        {
            TurnWhite();
        }
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(gameObject.name + " takes " + amount);
        _lastTimeWasDamaged = Time.time;
        _sr.color = Color.red;
    }

    private void TurnWhite()
    {
        _sr.color = Color.white;
    }
}