using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    AudioSourceManager asm;
    public AudioClip hurtSound;

    public UnityEvent<int> onLifeValueChanged;
    public UnityEvent<int> onMissileValueChanged;
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get => _instance;
    }

    public int _maxLives = 30;
    public int _lives = 30;

    public int lives
    {
        get { return _lives; }
        set
        {
            
            if (playerInstance && value < _lives)
            {
                playerInstance.GetComponent<AudioSourceManager>().PlayOneShot(hurtSound, false);
            }
            _lives = value;
            if (_lives > maxLives)
            {
                _lives = maxLives;
            }
            if(_lives <= 0)
            {
                GameOver();
            }

            onLifeValueChanged?.Invoke(_lives);

            Debug.Log(_lives.ToString());
        }
    }
    public int maxLives
    {
        get { return _maxLives; }
        set
        {
            _maxLives = value;
        }
    }

    public int _numMissiles = 1;
    public int numMissiles
    {
        get { return _numMissiles; }
        set
        {
            _numMissiles = value;
            onMissileValueChanged?.Invoke(_numMissiles);
        }
    }

    public PlayerController playerPrefab;
    [HideInInspector] public PlayerController playerInstance = null;
    [HideInInspector] public Level currentLevel = null;
    [HideInInspector] public Transform currentSpawnPoint;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        lives = _maxLives;
        asm = GetComponent<AudioSourceManager>();
    }

    public void SpawnPlayer(Transform spawnPoint)
    {
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        currentSpawnPoint = spawnPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void GameOver()
    {
        _maxLives = 30;
        SceneManager.LoadScene(2);
    }
}
