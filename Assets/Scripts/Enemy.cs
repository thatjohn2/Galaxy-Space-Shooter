using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;

    private Animator _animator;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private  AudioClip _explosionClip;

    [SerializeField]
    private AudioClip _laserClip;

    private AudioSource _audioSource;

    private bool _shotLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

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
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4)
        {
            float _randomX = Random.Range(-8, 8);
            transform.position = new Vector3(_randomX, 7, 0);
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
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            DeathSequence();
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
                GameObject _laser = Instantiate(_laserPrefab, transform.position + (Vector3.down * _spriteHeightHalf) + (Vector3.right * 0.18f), Quaternion.identity);
                _laser.GetComponent<Laser>()._shooter = this.gameObject;
                _shotLeft = false;
            }
            else
            {
                GameObject _laser = Instantiate(_laserPrefab, transform.position + (Vector3.down * _spriteHeightHalf) + (Vector3.left * 0.18f), Quaternion.identity);
                _laser.GetComponent<Laser>()._shooter = this.gameObject;
                _shotLeft = true;
            }
            
            _audioSource.clip = _laserClip;
            _audioSource.Play();
            float _randomTime = Random.Range(3, 7);
            yield return new WaitForSeconds(_randomTime);
        }
    }
}