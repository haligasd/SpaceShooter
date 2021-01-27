using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _fireRate = 3.0f;
    [SerializeField]
    private AudioClip _explosionClip;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _canFire = -1f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("No Player found");
        }

        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explosionClip;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -5f)
        {
            var randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.transform.name);

        if (other.tag == "Player")
        {
            if (_player != null)
                _player.Damage();

            DestroyMe();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
                _player.ScoreUpdate(10);

            DestroyMe();
        }
    }

    private void DestroyMe()
    {
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        _speed = 0;
        _animator.SetTrigger("OnEnemyDeath");
        Destroy(this.gameObject, 2.8f);
    }
}
