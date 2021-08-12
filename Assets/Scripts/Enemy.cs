using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;

    private Animator _animator;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private  AudioClip _explosionClip;

    [SerializeField]
    private AudioClip _laserClip;

    private AudioSource _audioSource;

    private RaycastHit2D _hitLeft;

    private RaycastHit2D _hitRight;

    [SerializeField]
    private Vector3 _adjustment;

    private Vector2 _colliderHalfWidth;

    [SerializeField]
    private float _speed = 4f;

    private bool _shotLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _colliderHalfWidth.x = GetComponent<BoxCollider2D>().size.x / 2;

        if (_player == null)
        {
            Debug.LogError("The Player is Null.");
        }
        
        if (_animator == null)
        {
            Debug.LogError("The Enemy's Animator is Null.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Enemy's Audio Source is NULL.");
        }
        else
        {
            _audioSource.clip = _laserClip;
        }

        StartCoroutine(FireLaser());
    }

    // Update is called once per frame
    void Update()
    {
        _hitLeft = Physics2D.Raycast((Vector2)transform.position - _colliderHalfWidth, (Vector2.down));
        _hitRight = Physics2D.Raycast((Vector2)transform.position + _colliderHalfWidth, (Vector2.down));
        if (_hitLeft && _hitRight)
        {
            Debug.Log("Left Ray Hit: " + _hitLeft.transform.name + "Right Ray Hit: " + _hitRight.transform.name);
            if (_hitLeft.transform.tag == "Enemy" && _hitRight.transform.tag == "Enemy")
            {
                int randomDirection = Random.Range(-1, 2);
                if (randomDirection == 0)
                {
                    randomDirection = 1;
                }
                _adjustment.x = randomDirection;
            }
            else if (_hitLeft.transform.tag == "Enemy")
            {
                _adjustment.x = 1;
            }
            else if (_hitRight.transform.tag == "Enemy")
            {
                _adjustment.x = -1;
            }
        }
        else if (_hitLeft)
        {
            Debug.Log("Left Ray Hit: " + _hitLeft.transform.name);
            if (_hitLeft.transform.tag == "Enemy")
            {
                _adjustment.x = 1;
            }
        }
        else if (_hitRight)
        {
            Debug.Log("Right Ray Hit: " + _hitRight.transform.name);
            if (_hitRight.transform.tag == "Enemy")
            {
                _adjustment.x = -1;
            }
        }
        else
        {

            Debug.Log("We dont see anything");

        }

        transform.Translate(_speed * Time.deltaTime * (Vector3.down + _adjustment));
        _adjustment.x = 0;

        if (transform.position.y <= -6)
        {
            float _randomX = Random.Range(-8, 8);
            transform.position = new Vector3(_randomX, 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (_player != null)
            {
                _player.Damage();
                _player.AddScore(10);
            }

            DeathSequence();
        }
        else if (other.tag == "Laser")
        {
            if (other.GetComponent<Laser>()._shooter.tag == "Player")
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                DeathSequence();
            }
        }
    }

    void DeathSequence()
    {
        Destroy(GetComponent<BoxCollider2D>());
        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _audioSource.clip = _explosionClip;
        _audioSource.Play();
        StopAllCoroutines();
        Destroy(gameObject, 2.5f);
    }

    IEnumerator FireLaser()
    {
        while(true)
        {
            float _spriteHeightHalf = GetComponent<BoxCollider2D>().size.y / 2;
            if (_shotLeft)
            {
                GameObject laser = Instantiate(_laserPrefab, transform.position + (Vector3.down * _spriteHeightHalf) + (Vector3.right * 0.18f), Quaternion.identity);
                Laser script = laser.GetComponent<Laser>();
                script._shooter = this.gameObject;
                script._direction = "DOWN";
                _shotLeft = false;
            }
            else
            {
                GameObject laser = Instantiate(_laserPrefab, transform.position + (Vector3.down * _spriteHeightHalf) + (Vector3.left * 0.18f), Quaternion.identity);
                Laser script = laser.GetComponent<Laser>();
                script._shooter = this.gameObject;
                script._direction = "DOWN";
                _shotLeft = true;
            }
            
            _audioSource.clip = _laserClip;
            _audioSource.Play();
            float _randomTime = Random.Range(3, 7);
            yield return new WaitForSeconds(_randomTime);
        }
    }
}