using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalking : MonoBehaviour
{
    public Transform player;
    public Transform rotation_orientation_forward;
    public Transform rotation_orientation_back;
    public Transform rotation_orientation_right;
    public Transform rotation_orientation_left;
    public LayerMask whatIsWallkable;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        CheckWallkable();
    }
    private void Update()
    {
        Debug.DrawRay(rotation_orientation_left.position, -rotation_orientation_left.right, Color.red);
        Debug.DrawRay(rotation_orientation_forward.position, rotation_orientation_forward.forward, Color.black);
        Debug.DrawRay(rotation_orientation_back.position, -rotation_orientation_back.forward, Color.blue);
        Debug.DrawRay(rotation_orientation_right.position, rotation_orientation_right.right, Color.green);

    }
    private void CheckWallkable()
    {
        float right = 2;
        float left = 2;
        float forward = 2;
        float back = 2;
        RaycastHit hit;
        int layerMask = whatIsWallkable.value;
        if (Physics.Raycast(player.position, rotation_orientation_right.right, out hit, 2, layerMask))
        {
            right = hit.distance - 0.5f;
        }
        if (Physics.Raycast(player.position, -rotation_orientation_left.right, out hit, 2, layerMask))
        {
            left = hit.distance - 0.5f;
        }
        if (Physics.Raycast(player.position, rotation_orientation_forward.forward, out hit, 2, layerMask))
        {
            forward = hit.distance - 0.5f;
        }
        if (Physics.Raycast(player.position, -rotation_orientation_back.forward, out hit, 2, layerMask))
        {
            back = hit.distance - 0.5f;
        }
        RotatePlayer(right, left, forward, back);

    }
    private void RotatePlayer(float distanceR, float distanceL, float distanceF, float distanceB)
    {
        Vector3 rotation_total = new Vector3(0, 0, 0);
        rotation_total += new Vector3(0, 0, (1f - (distanceR / 1.5f)) * 90);
        rotation_total += new Vector3(0, 0, (1f - (distanceL / 1.5f)) * -90);
        rotation_total += new Vector3((1f - (distanceB / 1.5f)) * 90, 0, 0);
        rotation_total += new Vector3((1f - (distanceF / 1.5f)) * -90, 0, 0);
        player.localRotation = Quaternion.Euler(rotation_total);
    }
    private void RotatePlayerInitial()
    {
        player.localRotation = Quaternion.Euler(0, 0, 0);
    }
    /* private Vector3 RotatePlayerX(float distance)
 {
     if (distance > 2 || distance == 0)
     {
         return new Vector3(0, 0, 0);
     }
     else
     {
         float rotation = (1f - (distance / 2f)) * 90;
         return new Vector3(rotation, 0, 0);
     }

 }
 private Vector3 RotatePlayerZ(float distance)
 {
     if (distance > 2 || distance == 0)
     {
         return new Vector3(0, 0, 0);
     }
     else
     {
         float rotation = (1f - (distance / 2f)) * 90;
         return new Vector3(0, 0, rotation);
     }
 }*/
}
