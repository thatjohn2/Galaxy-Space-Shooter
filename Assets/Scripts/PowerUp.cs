using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField] //0 = TripleShot, 1 = Speed, 2 = Shield, 3 = Ammo, 4 = Health, 5 = SideShot, 6 = Slow
    private int _powerupID;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    case 3:
                        player.ActivateAmmoBoost();
                        break;
                    case 4:
                        player.ActivateHealthBoost();
                        break;
                    case 5:
                        player.ActivateSideShot();
                        break;
                    case 6:
                        player.ActivateSlow();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
