using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType
{
    TripleShot = 0,
    Speed = 1,
    Shields = 2
}

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private PowerupType _powerupType;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GameObject.Find("AudioManager/Powerup").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = other.transform.GetComponent<Player>();

            _audioSource.Play();

            if (player != null)
                player.PowerupCollected(this._powerupType);

            Destroy(this.gameObject);
        }
    }
}
