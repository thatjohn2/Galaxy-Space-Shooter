using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    private string _direction;

    public GameObject _shooter;

    // Start is called before the first frame update
    void Start()
    {
        if (_shooter == null)
        {
            Debug.LogError("The Laser's Shooter is NULL.");
        }
        else if (_shooter.tag == "Player")
        {
            _direction = "UP";
        }
        else if (_shooter.tag == "Enemy")
        {
            _direction = "DOWN";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_direction == "UP")
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        else if (_direction == "DOWN")
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

        if (transform.position.y <= -8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }   
}
