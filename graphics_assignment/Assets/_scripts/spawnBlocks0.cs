﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawnBlocks0 : MonoBehaviour {

    public int ind;

	// Use this for initialization
	void Start () {

        string[] blockNames = { "Blocks/black2x10" , "Blocks/red2x8" , "Blocks/green1x8", "Blocks/green2x6", "Blocks/grey2x4", "Blocks/purple1x3x2", "Blocks/black2x2", "Blocks/green1x6 (1)", "Blocks/blue1x4", "Blocks/blue1x3", "Blocks/orange2x2x.33", "Blocks/orange1x3", "Blocks/1x1 (2)", };

        //comment or uncomment block set below to switch between sets

        int[] blockNums = { 3, 1, 6, 3, 3, 8, 5, 0, 3, 4, 6, 2, 4 };
        List<int[]> sets = new List<int[]>();
        sets.Add(blockNums);
        int[] blockNums2 = { 2, 4, 4, 3, 1, 5, 3, 3, 4, 5, 4, 5, 3 };
        sets.Add(blockNums2);
        int[] blockNums3 = { 1, 6, 6, 3, 1, 4, 3, 1, 3, 3, 5, 3, 8 };
        sets.Add(blockNums3);
        int[] blockNums4 = { 7, 3, 2, 1, 2, 1, 2, 1, 4, 5, 2, 3, 3 };
        sets.Add(blockNums4);
        int[] blockNums5 = { 4, 4, 3, 2, 3, 2, 3, 4, 1, 8, 2, 0, 2 };
        sets.Add(blockNums5);

        blockNums = sets[ind];

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
