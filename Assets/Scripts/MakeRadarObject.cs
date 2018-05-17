using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MakeRadarObject : MonoBehaviour {

    public Image image;

	// Use this for initialization
	void Start () {
        Radar.RegisterRaderObject(this.gameObject, image);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        Radar.RemoveRaderObject(this.gameObject);
    }

}
