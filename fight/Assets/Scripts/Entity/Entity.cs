using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    public GridInfo grid;
    public GameObject bulletPrefab;
    public GameObject bloodBar;
    public Vector3 pos;
    public int camp;
    public int bloodMax;
    public int blood;
    public float cd;
    public int damage;
    public int bulletSpeed;

	void Start () {
        
	}
	
	void Update () {

	}

    public void AddBlood(int b)
    {
        if (grid == null)
        {
            return;
        }
        blood = blood + b;
        //bloodBar.GetComponent<UISlider>().value = blood / bloodMax;
        if (blood <= 0)
        {
            GameStatic.entityMgr.RemoveEntity(gameObject);
            Destroy(bloodBar);
            //Destroy(gameObject);
        }
    }

    public void SetBloodBar()
    {
        //if (grid == null)
        //{
        //    return;
        //}
        //GameObject bloodPrefab = Resources.Load("BloodBar") as GameObject;
        //if (bloodPrefab == null)
        //{
        //    Debug.LogError("blood bar");
        //    return;
        //}
        //bloodBar = Instantiate(bloodPrefab);
        //bloodBar.GetComponent<UISlider>().value = 1;
        //bloodBar.transform.position = pos + new Vector3(0, 5, 0);
        ////bloodBar.GetComponent<UISlider>().transform.position = pos + new Vector3(0, 5, 0);
        ////bloodBar.GetComponent<UISlider>().transform.rotation = Camera.main.transform.rotation;
        //Debug.LogError(bloodBar.transform.position.x + " " + bloodBar.transform.position.y + " " + bloodBar.transform.position.z);
    }
}
