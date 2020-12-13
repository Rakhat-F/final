using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedPerspective : MonoBehaviour
{
    public Transform camera;
    private RaycastHit hit;
    private RaycastHit hit_init;
    private int layerMask = 1 << 11;
    private Rigidbody rb;
    private float initial_distance;
    private Vector3 initial_scale;
    private Ray ray;
    private int temp = 0;
    public float mCorrectionForce = 50.0f;
    public float mPointDistance = 3.0f;
    private bool isBeingPicked = false;
    public GameObject ray_dest;
    private FixedJoint joint;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        PickUp();
        MoveObject();
    }
    // Update is called once per frame
    void Update()
    {
     
        ray = new Ray(camera.position, camera.forward);
        Debug.DrawRay(ray.origin, ray.direction * 200, Color.red);
    }

    private void PickUp()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(camera.position, camera.forward, out hit_init, 200, layerMask))
             {
                /*initial_distance = hit_init.distance;
                initial_scale = hit_init.transform.localScale;
                rb = hit_init.rigidbody;
                temp++;
                isBeingPicked = true;
                joint = ray_dest.AddComponent<FixedJoint>();
                joint.connectedBody = hit_init.rigidbody;*/
                print(hit_init.collider.bounds.size);
                /*if (Physics.Raycast(hit_init.transform.position, hit_init.transform.right, out hit, Mathf.Infinity, layerMask))
                {
                    print(hit.distance);
                }*/
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isBeingPicked)
            {
                isBeingPicked = false;
            }
        }
    }
    private void MoveObject()
    {

            /*if (Physics.Raycast(camera.position, camera.forward, out hit, Mathf.Infinity, ~layerMask))
            {
                ray_dest.transform.position = ray.GetPoint(hit.distance);
                *//*
                                Vector3 targetPoint = ray.GetPoint(hit.distance);
                                Vector3 force = targetPoint;

                                rb.velocity = force.normalized * rb.velocity.magnitude;
                                rb.AddForce(force * mCorrectionForce);

                                rb.velocity *= Mathf.Min(1.0f, force.magnitude / 2);*/
                /* if (temp != 1)
                     hit_init.transform.localScale = (initial_scale * hit.distance) / initial_distance;*//*

                print(hit.distance);
            }*/
    }
}
