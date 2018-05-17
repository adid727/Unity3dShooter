using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadarObject
{
    public Image icon { get; set; }
    public GameObject owner { get; set; }
}

public class Radar : MonoBehaviour {

    public Transform playerPos;
    float mapScale = 2.0f;

    public static List<RadarObject> radObjects = new List<RadarObject>();

    public static void RegisterRaderObject(GameObject o, Image i)
    {
        Image image = Instantiate(i);
        radObjects.Add(new RadarObject() { owner = o, icon = image });
    }

    public static void RemoveRaderObject(GameObject o)
    {
        List<RadarObject> newList = new List<RadarObject>();
        for(int i = 0; i < radObjects.Count; i++)
        {
            if (radObjects[i].owner == o)
            {
                Destroy(radObjects[i].icon);
                continue;
            }
            else
                newList.Add(radObjects[i]);

        }
        radObjects.RemoveRange(0, radObjects.Count);
        radObjects.AddRange(newList);
    }

    void DrawRadarDot()
    {
        foreach (RadarObject ro in radObjects)
        {
            Vector3 radarPos = (ro.owner.transform.position - playerPos.position);
            float distToObject = Vector3.Distance(playerPos.position, ro.owner.transform.position) * mapScale;
            float deltay = Mathf.Atan2(radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;
            radarPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            radarPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

            ro.icon.transform.SetParent(this.transform);
            if(radarPos.x != Mathf.Infinity && radarPos.z != Mathf.Infinity )
            ro.icon.transform.position = new Vector3(radarPos.x, radarPos.z, 0) + this.transform.position;

        }
    }
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        DrawRadarDot();
	}
}
