using UnityEngine;
using System.Collections;

public class LoadIntro : MonoBehaviour {

    // Use this for initialization
    public void GotoGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update () {
	
	}
}
