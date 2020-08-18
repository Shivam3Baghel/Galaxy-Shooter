using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameOver = true;
    public GameObject player;

    private UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    //if game over is true
    //if space key pressed
    //spawn the player
    //gameOver is false
    //hide title screen
    void Update()
    {
        if (gameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(player, Vector3.zero, Quaternion.identity);
                gameOver = false;
                _uiManager.HideTitleScreen();
            }
        }
    }

}
