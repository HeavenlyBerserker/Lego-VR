using UnityEngine;
using System.Collections;

public class spawnBlocks : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 10; i++)
        {
            createBlock(0 + Random.Range(0, 3), 25 + Random.Range(0, 3), -88 + Random.Range(0, 3), "Blocks/red2x8");
            createBlock(7 + Random.Range(0, 3), 25 + Random.Range(0, 3), -88 + Random.Range(0, 3), "Blocks/black2x10");
        }
    }
	void createBlock(float x, float y, float z, string name)
    {
        Instantiate(Resources.Load(name), new Vector3(x,y,z), Quaternion.identity);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
