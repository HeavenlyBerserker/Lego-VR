using UnityEngine;
using System.Collections;

public class spawnBlocks0 : MonoBehaviour {

	// Use this for initialization
	void Start () {

        string[] blockNames = { "Blocks/black2x10" , "Blocks/red2x8" , "Blocks/green1x8", "Blocks/green2x6", "Blocks/grey2x4", "Blocks/purple1x3x2", "Blocks/black2x2", "Blocks/green1x6 (1)", "Blocks/blue1x4", "Blocks/green1x3", "Blocks/orange2x2x.33", "Blocks/orange1x3", "Blocks/1x1 (2)", };
        int[] blockNums = { 2,2,3,4,5,6,7,8,9,10,20,11,12};
        int[] blockNums2 = { 2, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 11, 12 };
        int xx = -20;
        int zz = -86;
        int number = 0;
        int remaining = 0;

        for (int i = 0; i < 13; i++)
        {
            number += blockNums[i];
            if(blockNums[i] != 0)remaining++;
        }

        //Debug.Log(remaining);
        string s = "";
        
        for (int j = 0; j < number; j++){
            int count = 0;
            int rand = Random.Range(0, remaining);
            int index = 0;
            remaining = 0;
            for (int i = 0; i < 13; i++)
            {
                if (blockNums[i] > 0) {
                    remaining++;
                    count++;
                }
                if (count == rand)
                {
                    index = i;
                }
            }
            if (blockNums[index] <= 0) {
                for (int i = 0; i < 13; i++)
                {
                    if (blockNums[i] > 0)
                    {
                        index = i;
                    }
                }
            }
            s += index.ToString() + ", ";
            if (xx < -37)
            {
                xx = -20;
                zz ++;
            }
            if (index <= 3)
            {
                createBlock(xx -= 3, 25, zz, blockNames[index]);
            }
            else {
                createBlock(xx -= 2, 25, zz, blockNames[index]);
            }
            blockNums[index] = blockNums[index] -1;
        }

        s = "";
        
        for (int i = 0; i < 13; i++)
        {
            s += blockNums[i].ToString() + ", ";
        }
        //Debug.Log(s);
    }
	void createBlock(float x, float y, float z, string name)
    {
        Instantiate(Resources.Load(name), new Vector3(x,y,z), Quaternion.Euler(-90, 0, 0));
    }
	// Update is called once per frame
	void Update () {
	
	}
}
