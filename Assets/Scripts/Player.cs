using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _speed = 3;
    [SerializeField]
    private int _speedMultiplier = 2;
    [SerializeField]
    private GameObject _lasterPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _rightEngineDamage;
    [SerializeField]
    private GameObject _leftEngineDamage;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private GameObject _shields;
    [SerializeField]
    private AudioClip _laserClip;
    [SerializeField]
    private AudioClip _explosionClip;

    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isShieldActive;

    private float _canFire = -1f;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _laserClip;

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager Not Found");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager Not Found");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var direction = new Vector3(horizontalInput, verticalInput, 0);

        var speed = _speed;

        if (_isSpeedBoostActive)
        {
            speed *= _speedMultiplier;
        }

        transform.Translate(direction * speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_lasterPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shields.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _rightEngineDamage.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngineDamage.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives == 0)
        {
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }

    public void PowerupCollected(PowerupType powerupType)
    {

        switch (powerupType)
        {
            case PowerupType.TripleShot:
                _isTripleShotActive = true;
                StartCoroutine(TripleShotPowerDownRoutine());
                break;
            case PowerupType.Speed:
                _isSpeedBoostActive = true;
                StartCoroutine(SpeedPowerDownRoutine());
                break;
            case PowerupType.Shields:
                _isShieldActive = true;
                _shields.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ScoreUpdate(int value)
    {
        _score += value;

        _uiManager.UpdateScore(_score);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isTripleShotActive = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isSpeedBoostActive = false;
    }
}
