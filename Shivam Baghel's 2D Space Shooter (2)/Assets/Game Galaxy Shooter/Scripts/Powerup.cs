using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private int powerupID;//0=triple shot, 1=speed boost, 2=shield

    [SerializeField]
    private AudioClip _clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with:"+other.name);
        if (other.tag == "Player")
        {
            //access the player
            Player player = other.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 0.8f);
            if (player != null)
            {
                if (powerupID == 0)
                {
                    //enable triple shot
                    player.TripleShotPowerupOn();
                }
                else if (powerupID == 1)
                {
                    //enable speed boost
                    player.SpeedBoostPowerupOn();
                }
                else if (powerupID == 2)
                {
                    //enable shield
                    player.EnableShields();
                }

            }
            //destroy ourself
            Destroy(this.gameObject);
        }
    }
}
