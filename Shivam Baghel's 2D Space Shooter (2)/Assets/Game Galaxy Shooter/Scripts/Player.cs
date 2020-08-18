using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;

    /*public(can be accessed by other scripts) or 
     * private(cannot be accessed by other scripts) identifier, 
     * data type(int,float(Eg.:-4.2f),bool,string),
     * every variable has a name, option value assigned*/
    [SerializeField]
    //variable _speed to change the 
    //movement speed of the player
    private float _speed = 5.0f;
    
    [SerializeField]/*create a variable of type GameObject 
    named _laserPrefab
     to store the laser Prefab*/
    private GameObject _laserPrefab;
    
    [SerializeField]//just below 2 variables to limit 
    //the frame rate
    private float _fireRate = 0.25f;
    
    private float _canFire = 0.0f;
    
    public bool canTripleShot=false;
    
    [SerializeField]/*create a variable of type GameObject 
    named _tripleShotPrefab
    to store the TripleShot Prefab*/
    private GameObject _tripleShotPrefab;
    
    public bool isSpeedBoostActive = false;
    
    public bool shieldsActive = false;
    
    [SerializeField]
    private GameObject _shieldGameObject;


    public int lives = 3;

    private UIManager _uiManager;

    private GameManager _gameManager;
    
    private SpawnManager _spawnManager;

    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //current position=new position
        transform.position = new Vector3(0, 0, 0);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager != null)
        {
            _uiManager.UpdateLives(lives);

        }
        
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutine();
        }

        _audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        //function called so that it can move 
        // & restricting its area of movement 
        Movement();
        /*if((space key is pressed) or (Mouse0 key is pressed )) ,
        spawn laser just above the player position*/
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (canTripleShot == false) { SingleShoot(); }
            else                        { TripleShoot(); }
   
        }
    }
    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (isSpeedBoostActive == false)
        {
         transform.Translate(Vector3.right * _speed * horizontalInput *
             Time.deltaTime);
         transform.Translate(Vector3.up * _speed * verticalInput *
             Time.deltaTime);
        }
        else if (isSpeedBoostActive == true)
        {
        transform.Translate(Vector3.right * _speed * horizontalInput * 
            Time.deltaTime * 3.0f);
        transform.Translate(Vector3.up * _speed * verticalInput * 
            Time.deltaTime * 3.0f);
        }
        //if (player y position>0) then set y position=0
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0); 
        }
        if (transform.position.y < -4.4f) 
        { 
            transform.position = new Vector3(transform.position.x, -4.4f, 0); 
        }
        if (transform.position.x > 9.97f)
        { 
            transform.position = new Vector3(-8.8f, transform.position.y, 0); 
        }
        if (transform.position.x < -9.97f)
        { 
            transform.position = new Vector3(8.8f, transform.position.y, 0); 
        }
    }
    public void Damage()
    {
        //if player has shields do nothing
        if (shieldsActive == true)
        {
            shieldsActive = false;
            _shieldGameObject.SetActive(false);
            return;
        }
        
        //subtract 1 life from the player
        lives = lives - 1;
        _uiManager.UpdateLives(lives);


        //if(lives<1) then destroy player
        if (lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position,Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }
    }
    private void SingleShoot()    
        //function is to spawn or instantiate an object(one laser)
        //Time.time & _canFire is used to limit firerate
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();

            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.2f, 0), 
                Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }
    private void TripleShoot()    
        //function is to spawn or instantiate an object(_tripleShotPrefab)
        //Time.time & _canFire is used to limit firerate
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();

            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }
    public void TripleShotPowerupOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        canTripleShot = false;
    }
    public void SpeedBoostPowerupOn()
    {
        isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostDownRoutine());
    }
    public IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        isSpeedBoostActive = false;
    }
    public void EnableShields()
    {
        shieldsActive = true;
        _shieldGameObject.SetActive(true);
    }
}
