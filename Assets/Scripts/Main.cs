using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public GameObject Player;
    public GameObject Camera;
    public GameObject PauseView;
    public GameObject MotusMan;
    public GameObject target;
    public GameObject Canvas;
    public playerController playerContScript;
    public enemyAI enemyScript;
    Vector3 cameraPosition;
    // Use this for initialization
    bool pauseToggle = true;
    bool gamestarted = true;
    void Awake ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        target.SetActive(false);

    }

    
	
	// Update is called once per frame
	void Update () {
        if (playerContScript.CurrentHealth > 0 && enemyScript.EnemiesKilled < 100)
        {
            if (gamestarted)
            {
                Time.timeScale = 0;
                Player.SetActive(false);
                cameraPosition = Camera.transform.position;
                Camera.transform.parent = PauseView.transform;
                Camera.transform.rotation = PauseView.transform.rotation;
                Camera.transform.position = PauseView.transform.position;
                MotusMan.SetActive(false);
                Camera.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.C) && gamestarted == true)
            {
                gamestarted = false;
                Time.timeScale = 1;
                Player.SetActive(true);
                MotusMan.SetActive(true);
                cameraPosition = new Vector3(Player.transform.position.x - 0.04101563f, Player.transform.position.y + 2.723999f, Player.transform.position.z + 0.5819702f);
                Camera.transform.parent = Player.transform;
                Camera.transform.rotation = Player.transform.rotation;
                Camera.transform.position = cameraPosition;
                Canvas.SetActive(false);
                target.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.P) && gamestarted != true)
            {
                if (pauseToggle)
                {
                    target.SetActive(false);
                    Time.timeScale = 0;
                    Player.SetActive(false);
                    cameraPosition = Camera.transform.position;
                    Camera.transform.parent = PauseView.transform;
                    Camera.transform.rotation = PauseView.transform.rotation;
                    Camera.transform.position = PauseView.transform.position;
                    MotusMan.SetActive(false);
                    Camera.SetActive(true);
                    Canvas.SetActive(true);
                }
                else
                {
                    target.SetActive(true);
                    Time.timeScale = 1;
                    Player.SetActive(true);
                    MotusMan.SetActive(true);
                    Camera.transform.parent = Player.transform;
                    Camera.transform.rotation = Player.transform.rotation;
                    Camera.transform.position = cameraPosition;
                    Canvas.SetActive(false);
                }

                pauseToggle = !pauseToggle;
            }
        }
        if(enemyScript.EnemiesKilled >= 100)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
        }
        else if (playerContScript.CurrentHealth <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
        }

    }
}
