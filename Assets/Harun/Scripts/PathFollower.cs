using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathFollower : MonoBehaviour
{
    public PathCreator gravityPathCreator, ungravityPathCreator;
    public Transform player;
    public float speed;
    public LayerMask groundLayer;
    bool jump, jumped, gravity = true;
    float distanceTravelled;
    Rigidbody rb;
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, .3f, groundLayer) && !jump)
        {
            jump = true;
            jumped = false;
        }
        else if (!Physics.Raycast(player.transform.position, Vector3.down, out hit, .3f, groundLayer))
        {
            jump = false;
        }
        Debug.DrawRay(player.transform.position, Vector3.down * .3f, Color.red);
        if (Input.GetKeyDown(KeyCode.Space) && jump)
        {
            rb.AddForce(Vector3.up * 200);
            jumped = true;
        }
        distanceTravelled += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.C))
        {
            gravity = !gravity;
        }
        if (gravity)
        {
            transform.position = gravityPathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = gravityPathCreator.path.GetRotationAtDistance(distanceTravelled);
        }
        else
        {
            transform.position = ungravityPathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = ungravityPathCreator.path.GetRotationAtDistance(distanceTravelled);
        }
        Vector3 followPos = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z);
        if (jumped)
        {
            followPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
        player.position = followPos;
        player.rotation = transform.rotation;
    }
    //IEnumerator JumpFinish()
    //{

    //}
}
