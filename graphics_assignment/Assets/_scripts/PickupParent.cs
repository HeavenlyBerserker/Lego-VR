using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour
{

    SteamVR_TrackedObject trackedobj;
    SteamVR_Controller.Device device;
    public Transform sphere;
    public GameObject platform;
    private GameObject shadow;
    public float speed = 10.0f;
    private bool shad = false;
    private bool holdt = false;
    private bool itemh = false;
    public GameObject sc;
    private Transform tra2;
    public bool[,,] blocks = new bool[32,32*3+1,32];

    Collider globalcol = new Collider();
    int yRot = 0;

    void Awake()
    {
        trackedobj = GetComponent<SteamVR_TrackedObject>();
        for (int i = 0; i < 32; i++)
        {
            for (int j=0; j<32*3+1; j++)
            {
                for (int k=0; k<32; k++)
                {
                    blocks[i, j, k] =false;
                }
            }
        }
    }

    bool abovePlat() {
        if (globalcol)
        {
            Transform loc = gameObject.GetComponent<Transform>();
            Vector3 c = loc.position;

            if (c.x < -9.9 && c.x > -19.7 && c.z < -73.3 && c.z > -83.1 && c.y > 21.2)
            {
                return true;
            }
        }
        return false;
    }

    bool printMat() {
        int maxH = 0;
        //Debug.Log("Before Calc maxH: " + maxH + "\n");
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32 * 3 + 1; j++)
            {

                for (int k = 0; k < 32; k++)
                {
                    if(blocks[i, j, k]) maxH = j;
                }
            }
        }

        //Debug.Log("maxH: " + maxH + "\n");

        for (int j = 0; j < maxH+3 + 1; j++)
        {
            string deb = "Height: " + j + "\n";
            for (int k = 0; k < 32; k++)
            {
                for (int i = 0; i < 32; i++)
                {
                    if (blocks[i, j, k]) deb += "+";
                    else deb += "=";
                }
                deb += "\n";
            }
           // Debug.Log(deb);
        }
        

        return true;
    }

    //Joel: given origin and size of a block, returns closest location for it to snap to in blocks[] (currently just origin)
    Vector3 getBlockPlacement(int ox, int oy, int oz, int xs, int zs, int ys)
    {
        Vector3 n = new Vector3(ox,oy,oz);
        //Debug.Log(n);
        for (int kk=oy-1; kk>=0; kk--)
        {
            if ((blocks[ox,kk,oz] == true && blocks[ox, kk+1, oz] == false) || kk == 0)
            {
                n = new Vector3(ox,kk+1,oz);
                //Debug.Log(n);
                return n;
            }
        }
        return n;
    }

    //Given a collider, it determines the boundaries.
    //return Vector4 (min x to check in array, max x, min z, max z)
    Vector4 getBlockXYCoor(GameObject go)
    {
        Collider colsize = go.GetComponent<Collider>();
        Vector3 max = colsize.bounds.max;
        Vector3 min = colsize.bounds.min;
        Vector4 check = new Vector4(Mathf.Abs(Mathf.Round((min.x - (-9.9f))/.30625f)),
            Mathf.Abs(Mathf.Round((max.x - (-9.9f)) / .30625f)),
            Mathf.Abs(Mathf.Round((min.z-(-73.3f))/.30625f)),
            Mathf.Abs(Mathf.Round((max.z - (-73.3f)) / .30625f)));
        Debug.Log(max + " " + min);
        Debug.Log("Curr block coordinate boundaries: x(" + check.x + ", " + check.y + ") z(" + check.z + ", " + check.w + ")");
        return check;
    }

    //Given a collider, it determines the boundaries.
    //return int [] {min x to check in array, max x, min y, max y, min z, min z}
    int[] getBlockXYZCoor(GameObject go, bool print)
    {
        Collider colsize = go.GetComponent<Collider>();
        Vector3 max = colsize.bounds.max;
        Vector3 min = colsize.bounds.min;

        //Switch y and z
        int[] check = new int[6]{(int)Mathf.Abs(Mathf.Round((max.x - (-9.9f)) / .30625f)),
            (int)(Mathf.Abs(Mathf.Round((min.x - (-9.9f)) / .30625f))),
            (int)Mathf.Abs(Mathf.Round((min.y - (21.3f)) / .12533f)),
            (int)Mathf.Abs(Mathf.Round((max.y - (21.3f)) / .12533f)),
            (int)Mathf.Abs(Mathf.Round((max.z - (-73.3f)) / .30625f)),
            (int)Mathf.Abs(Mathf.Round((min.z - (-73.3f)) / .30625f))
        };
        //Debug.Log(max + " " + min);
        if(print)Debug.Log("Curr block coordinate boundaries: x(" + check[0] + ", " + check[1] + ") y(" + check[2] + ", " + check[3] + ")" + ") z(" + check[4] + ", " + check[5] + ")");

        if (check[1] > 32) check[1] = 32;
        if (check[2] > 32 * 3 + 1) check[2] = 32 * 3 + 1;
        if (check[3] > 32) check[3] = 32;

        return check;
    }

    //Adds the block to the matrix
    bool addBlock2Array(GameObject ob)
    {
        int[] coors = getBlockXYZCoor(ob, true);
        //Debug.Log("Hi");

        int coun = 0;

        for (int i = coors[0]; i < coors[1]; i++)
        {
            for (int j = coors[2]; j < coors[3]; j++)
            {
                for (int k = coors[4]; k < coors[5]; k++)
                {
                    blocks[i, j, k] = true;
                    coun++;
                    //Debug.Log("Added "+ i + ", " + j + ", " + k + "to matrix");
                }
            }
        }

        Debug.Log("Added " + coun + " units to matrix");

        return true;
    }

    //################In Progress######################
    //Determines the y location the shadow should be at
    float determineY(GameObject ob) {
        float finalY = 21.3f;

        int[] coors = getBlockXYZCoor(ob,false);

        //Debug.Log("X [" + coors[0] + " " + coors[1] + "] " + " Z [" + coors[4] + " " + coors[5] + "] ");

        int max = 0;
        for (int i = coors[0]; i < coors[1]; i++)
        {
            for (int j = coors[4]; j < coors[5]; j++)
            {
                int localmax = 0;
                for (int k = 0; k < coors[2]; k++)
                {
                    if (blocks[i, k, j] == true)
                        localmax = k + 1;
                }
                if (localmax > max)
                    max = localmax;
            }
        }



        //max += 1;
        finalY += max * .12533f;

        return finalY;
    }

    /*
    //given origin and size of a block, set region in blocks[] to 1 (currently just origin)
    void updateBlocks(int ox, int oy, int oz, int xs, int zs, int ys)
    {
        blocks[ox, oy, oz] = false;
    }*/


    //Update----------------------------------------
    //Function with object modification functions
    //----------------------------------------------
    void Update()
    {

        Vector3 locvec = new Vector3();
        //Get controller
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedobj.index);

        //set rotations to yRot
        if (globalcol) globalcol.gameObject.transform.rotation = Quaternion.Euler(-90, yRot, 0);
        if (globalcol) shadow.transform.rotation = Quaternion.Euler(-90, yRot, 0);

        //If device trigger is pressed
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            //getting location
            Transform loc = gameObject.GetComponent<Transform>();
            Vector3 c = loc.position;

            //If lego is above the platform
            if (abovePlat())
            {
                double xx = c.x + 9.9; double zz = c.z + 73.3;
                xx = Math.Round(xx / .30625); zz = Math.Round(zz / .30625);
                xx *= .30625; zz *= .30625;                
                xx -= 9.9; zz -= 73.3;
                c.x = (float)xx; c.z = (float)zz;

                Vector3 p = c;

                int xxx = -(int)Math.Round((c.x+9.9) / .30625);
                int yyy = (int)Math.Round((c.y-21.3)/ .12533f);  //currently divided by same value as xwidth instead of height
                int zzz = -(int)Math.Round((c.z+73.3)/ .30625);

                //Modify the height of the shadow cast
                p = getBlockPlacement(xxx, yyy, zzz, 1, 1, 1);
                locvec = p; //used when block is placed

                p.x = -.30625f * (float)p.x - 9.9f;
                p.y = determineY(globalcol.gameObject);//Determines y coordinate for current block                     //.12533f * (float)p.y + 21.3f;
                p.z = -.30625f * (float)p.z - 73.3f;
                //p.y = getBlockPlacement(xx,zz,p.y);//21.2f;
                //
                tra2.position = p;

                if (globalcol) globalcol.gameObject.transform.position = c;
                if(itemh)shadow.SetActive(true);
            }
            else if(itemh)shadow.SetActive(false);
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("TouchPad Up");
            yRot += 90;
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            //Debug.Log("You have released PressUp while colliding with " + col.name);
            globalcol.gameObject.transform.SetParent(null);
            globalcol.attachedRigidbody.isKinematic = false;
            if (globalcol) Debug.Log("Release: " + globalcol.gameObject.name);
            else Debug.Log("No object selected");

            //updateBlocks((int)locvec.x,(int)locvec.y,(int)locvec.z, 1, 1, 1);
            addBlock2Array(shadow);
            //Debug.Log("Block Added");
            printMat();

            //if (abovePlat()) {
            Destroy(globalcol.gameObject);
            //}
            globalcol = null;
            shadow.SetActive(true);
            itemh = false;
            //tossObject(col.attachedRigidbody);
        }
    }

    //OnTriggerStay---------------------------------
    //Function with pickup funtions
    //----------------------------------------------
    void OnTriggerStay(Collider col)
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedobj.index);
        //Debug.Log("You have collided with " + col.name + " and activated OnTriggerStay");
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && itemh == false)
        {
            //Debug.Log("You have collided with " + col.name + " while holding down touch");
            col.attachedRigidbody.isKinematic = true;
            col.gameObject.transform.SetParent(gameObject.transform);
            globalcol = col;
            if (globalcol) Debug.Log("Grab: " + globalcol.gameObject.name);

            //Creating shadow---------
            Collider coll = globalcol.gameObject.GetComponent<Collider>();
            Vector3 c = coll.bounds.center;
            Vector3 ma = coll.bounds.max;
            Vector3 mi = coll.bounds.min;
            shadow = Instantiate(globalcol.gameObject, c, Quaternion.Euler(-90, yRot, 0)) as GameObject;
            

            tra2 = shadow.GetComponent<Transform>();
            Vector3 p = tra2.position;
            p.y = 22;
            tra2.position = p;
            tra2.localScale = new Vector3(1, 1, 1);
            /*
            Color cc = shadow.gameObject.GetComponent<Renderer>().material.color;
            cc.a = 0.5f;
            shadow.gameObject.GetComponent<Renderer>().material.color = cc;*/
            //----------------------

            itemh = true;
        }
        
    }

    //void tossObject (Rigidbody rb)

    //{
    //SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedobj.index);
    ///Transform origin = trackedobj.origin ? trackedobj.origin : trackedobj.transform.parent;
    //if (origin != null)
    //{
    // rb.velocity = origin.TransformVector(device.velocity * speed);
    //rb.angularVelocity = origin.TransformVector(device.angularVelocity * speed);

    //}
    //else
    //{
    //rb.velocity = device.velocity*speed;
    //rb.angularVelocity = device.angularVelocity*speed;
    //}
    //}

}
