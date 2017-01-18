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

    Collider globalcol = new Collider();
    int yRot = 0;

    void Awake()
    {
        trackedobj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {

        

    }
    
    void FixedUpdate()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedobj.index);

        if(globalcol) globalcol.gameObject.transform.rotation = Quaternion.Euler(-90, yRot, 0);

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            //holdt = !holdt;
            //Debug.Log("You are holding down the trigger");

            Transform loc = gameObject.GetComponent<Transform>();
            Vector3 c = loc.position;

            if (c.x < -9.9 && c.x > -19.7 && c.z < -73.3 && c.z > -83.1 && c.y > 21.2) {
                double xx = c.x + 9.9; double zz = c.z + 73.3;
                xx = Math.Round(xx / .30625); zz = Math.Round(zz / .30625);
                xx *= .30625; zz *= .30625;
                xx -= 9.9; zz -= 73.3;
                c.x = (float)xx; c.z = (float)zz;

                if (globalcol) globalcol.gameObject.transform.position = c;
            }
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            //Debug.Log("TouchPad Up");
            yRot += 45;

            /*
            Collider coll = globalcol.gameObject.GetComponent<Collider>();
            Vector3 c = coll.bounds.center;
            //c = c - new Vector3(c.x %.1f, c.y%.1f, c.z%.1f);
            Vector3 ma = coll.bounds.max;
            Vector3 mi = coll.bounds.min;
            

            shadow = Instantiate(globalcol.gameObject, c, Quaternion.Euler(-90, yRot, 0)) as GameObject;

            Transform tra2 = shadow.GetComponent<Transform>();
            tra2.localScale = new Vector3(1,1,1);*/

        }

        
    }

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
            itemh = true;
        }
        else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            //Debug.Log("You have released PressUp while colliding with " + col.name);
            col.gameObject.transform.SetParent(null);
            col.attachedRigidbody.isKinematic = false;
            if (globalcol) Debug.Log("Release: " + globalcol.gameObject.name);
            else Debug.Log("No object selected");
            globalcol = null;
            itemh = false;
            //tossObject(col.attachedRigidbody);
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
