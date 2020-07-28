using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float _topWall = 0f;
    public float _bottomWall = -4f;
    public float _sideWall = 10f;

    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _powerUpClip;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _normalSpeed = 5.0f;
    [SerializeField]
    private float _thrustingSpeed = 7.5f;
    [SerializeField]
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    private float _laserOffset = 1.5f;

    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private bool _speedBoostActive = false;
    [SerializeField]
    private bool _shieldActive = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
        
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Player's Audio Source is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (other.GetComponent<Laser>()._shooter.tag != "Player")
            {
                Damage();
                Destroy(other.gameObject);
            }
        }
    }

    void Move()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        Vector3 _moveAmount = new Vector3(_horizontalInput, _verticalInput, 0);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = _thrustingSpeed;
        }
        else
        {
            _speed = _normalSpeed;
        }

        _moveAmount *= _speed * Time.deltaTime;
        transform.Translate(_moveAmount);

        float _playerX = transform.position.x;
        float _playerY = transform.position.y;
        float _playerZ = transform.position.z;
        transform.position = new Vector3(_playerX, Mathf.Clamp(_playerY, _bottomWall, _topWall), _playerZ);
        if (_playerX > _sideWall)
        {
            transform.position = new Vector3(-_sideWall, _playerY, _playerZ);
        }
        else if (_playerX < -_sideWall)
        {
            transform.position = new Vector3(_sideWall, _playerY, _playerZ);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_tripleShotActive == true)
        {
            GameObject tripleShot = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            foreach (Transform child in tripleShot.transform)
            {
                Laser script = child.GetComponent<Laser>();
                script._shooter = this.gameObject;
                script._direction = "UP"; 
            }
        }
        else
        {
            GameObject laser = Instantiate(_laserPrefab, transform.position + (Vector3.up * _laserOffset), Quaternion.identity);
            Laser script = laser.GetComponent<Laser>();
            script._shooter = this.gameObject;
            script._direction = "UP";
        }

        _audioSource.clip = _laserSoundClip;
        _audioSource.Play();

    }

    public void Damage()
    {
        if (_shieldActive == false)
        {

            if (_lives == 3)
            {
                int _hitEngine = Random.Range(0, 2);
                if (_hitEngine == 0)
                {
                    _leftEngine.SetActive(true);
                }
                else
                {
                    _rightEngine.SetActive(true);
                }
            }
            else if (_lives == 2)
            {
                if (_leftEngine.activeInHierarchy == true)
                {
                    _rightEngine.SetActive(true);
                }
                else if (_rightEngine.activeInHierarchy == true)
                {
                    _leftEngine.SetActive(true);
                }
            }

            _lives--;

            _uiManager.UpdateLives(_lives);

            if (_lives < 1)
            {
                _spawnManager.onPlayerDeath();
                Destroy(this.gameObject);
            }
        }
        else
        {
            _shieldActive = false;
            _shieldVisual.SetActive(false);
        }
    }

    public void ActivateTripleShot()
    {
        _audioSource.clip = _powerUpClip;
        _audioSource.Play();
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    
    public void ActivateSpeedBoost()
    {
        _audioSource.clip = _powerUpClip;
        _audioSource.Play();
        _speedBoostActive = true;
        _normalSpeed *= _speedMultiplier;
        _thrustingSpeed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ActivateShield()
    {
        _audioSource.clip = _powerUpClip;
        _audioSource.Play();
        _shieldActive = true;
        _shieldVisual.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }
    
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _normalSpeed /= _speedMultiplier;
        _thrustingSpeed /= _speedMultiplier;
        _speedBoostActive = false;
    }
    
}
