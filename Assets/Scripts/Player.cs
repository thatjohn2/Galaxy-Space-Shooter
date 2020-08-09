using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /*
    * Create a UI element
    to visualize the charge
    element of your
    thrusters:
    Create a hollow box to which contains another box that fills it, this box will change scale and position as the thrusters charge (+) and are used (-) to appear to be emptying a container
    create a text element over the box that says thruster charge

    ● Cool Down System
    required:
    thrusters charge up to a max of 100 while not in use
    they use up charge as the shift key is held
    charge should last 2 seconds and take 4 seconds to fully recharge
    float maxcharge
    float currentcharge
    currentcharge += 1/sec
    if shiftpressed and currentcharge > 0
    then activate boost and currentcharge -= 2/sec
    */

    public float _topWall = 0f;
    public float _bottomWall = -4f;
    public float _sideWall = 10f;

    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private SpriteRenderer _shieldSpriteRenderer;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _sideShotPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _powerUpClip;
    [SerializeField]
    private AudioClip _noAmmoClip;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private int _shieldLives = 0;
    [SerializeField]
    private int _ammoCount = 15;

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
    public float maxCharge = 100f;
    [SerializeField]
    private float _currentCharge = 100f;
    [SerializeField]
    private float _chargingSpeed = 25f;
    [SerializeField]
    private float _depletionFactor = 2f;

    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private bool _speedBoostActive = false;
    [SerializeField]
    private bool _sideShotActive = false;

    [SerializeField]
    private Color _shieldColor;
    [SerializeField]
    private Color _firstColor;
    [SerializeField]
    private Color _secondColor;
    [SerializeField]
    private Color _thirdColor;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldSpriteRenderer = _shieldVisual.GetComponent<SpriteRenderer>();

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

        if (_shieldSpriteRenderer == null)
        {
            Debug.LogError("The Shield's sprite renderer is NULL.");
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
            if (_currentCharge > 0)
            {
                _currentCharge -= _chargingSpeed * _depletionFactor * Time.deltaTime;
                _speed = _thrustingSpeed;
            }
            else
            {
                _speed = _normalSpeed;
            }
            if (_currentCharge < 0)
            {

                _currentCharge = 0;
            }
        }
        else
        {
            if (_currentCharge < maxCharge)
            {
                _currentCharge += _chargingSpeed * Time.deltaTime;
            }
            if (_currentCharge > maxCharge)
            {
                _currentCharge = maxCharge;
            }
            _speed = _normalSpeed;
        }

        _uiManager.UpdateCharge(_currentCharge);

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
        if (_ammoCount > 0)
        {
            _canFire = Time.time + _fireRate;
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);

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
            else if (_sideShotActive == true)
            {
                GameObject sideShot = Instantiate(_sideShotPrefab, transform.position, Quaternion.identity);
                foreach (Transform child in sideShot.transform)
                {
                    Laser script = child.GetComponent<Laser>();
                    script._shooter = this.gameObject;
                    if (child.name == "laser_Left")
                    {
                        script._direction = "LEFT";
                    }
                    else if (child.name == "laser_Right")
                    {
                        script._direction = "RIGHT";
                    }
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
        else
        {
            _canFire = Time.time + _fireRate;
            _audioSource.clip = _noAmmoClip;
            _audioSource.Play();
        }
    }

    public void Damage()
    {
        if (_shieldLives == 0)
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
            _shieldLives--;
            if (_shieldLives == 2)
            {
                _shieldSpriteRenderer.color = _secondColor;
            }
            else if (_shieldLives == 1)
            {
                _shieldSpriteRenderer.color = _thirdColor;
            }
            else if (_shieldLives == 0)
            {
                _shieldSpriteRenderer.color = _firstColor;
                _shieldVisual.SetActive(false);
            }
        }
    }

    public void ActivateTripleShot()
    {
        _audioSource.clip = _powerUpClip;
        _audioSource.Play();
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void ActivateSideShot()
    {
        _audioSource.clip = _powerUpClip;
        _audioSource.Play();
        _sideShotActive = true;
        StartCoroutine(SideShotPowerDownRoutine());
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
        _shieldLives = 3;
        _shieldSpriteRenderer.color = _firstColor;
        _shieldVisual.SetActive(true);
    }

    public void ActivateAmmoBoost()
    {
        _audioSource.clip = _powerUpClip;
        _audioSource.Play();
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void ActivateHealthBoost()
    {
        if (_lives == 3)
        {
            _audioSource.clip = _noAmmoClip;
            _audioSource.Play();
        }
        else if (_lives == 2)
        {
            _audioSource.clip = _powerUpClip;
            _audioSource.Play();
            _lives++;
            _uiManager.UpdateLives(_lives);
            if (_leftEngine.activeInHierarchy == true)
            {
                _leftEngine.SetActive(false);
            }
            else
            {
                _rightEngine.SetActive(false);
            }
        }
        else if (_lives == 1)
        {
            _audioSource.clip = _powerUpClip;
            _audioSource.Play();
            _lives++;
            _uiManager.UpdateLives(_lives);
            int healEngine = Random.Range(0, 2);
            if (healEngine == 0)
            {
                _leftEngine.SetActive(false);
            }
            else
            {
                _rightEngine.SetActive(false);
            }
        }

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

    IEnumerator SideShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _sideShotActive = false;
    }

}
