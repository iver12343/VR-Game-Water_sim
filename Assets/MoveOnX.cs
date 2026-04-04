using UnityEngine;

public class MoveOnX : MonoBehaviour
{
     public float _delay = 5f;
    public float _duration = 60f;
    public float _distance = 120f;
    
    public float _timer = 0f;
    public float _delayTimer = 0f;
    public bool _started = false;
    
    public float _startX;

    private void Start()
    {
        _startX = transform.position.x;
    }

    private void Update()
    {
        if (!_started)
        {
            _delayTimer += Time.deltaTime;
            if (_delayTimer >= _delay)
                _started = true;
            return;
        }

        if (_timer < _duration)
        {
            _timer += Time.deltaTime;
            float newX = Mathf.Lerp(_startX, _startX + _distance, _timer / _duration);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}