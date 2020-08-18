using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyExplosionPrefab;
    //variable for your speed
    private float _speed = 3.0f;
    //use this for initialization
    
    private UIManager _uiManager;
    
    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //move down
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //when off the screen on the bottom
        //respawn back on top with a new x position between the bounds of the screen
        if (transform.position.y < -7)
        {
            transform.position = new Vector3(Random.Range(-7, 7), 7,0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if(other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            Destroy(other.gameObject);
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            _uiManager.UpdateScore();
            AudioSource.PlayClipAtPoint(_clip,Camera.main.transform.position,0.8f);
            Destroy(this.gameObject);

        }
        else if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 0.8f);
            Destroy(this.gameObject);
        }
    }
}
