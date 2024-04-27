using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using DG.Tweening;

public class PathFollower : MonoBehaviour
{
    public PathCreator gravityPathCreator, ungravityPathCreator;
    public Transform player;
    public float speed;
    public LayerMask groundLayer;
    bool jump, jumped, gravity;
    float distanceTravelled, rotate = 0;
    Rigidbody rb;
    Vector3 followPos, followRot;
    public PlayerState playerState;
    public enum PlayerState
    {
        Cube,
        Sphere,
        Triangle,
        Submarine
    }
    public GravityState gravityState, lastGravityState;
    public enum GravityState
    {
        Gravity,
        NonGravity,
        Middle
    }
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gravityState)
        {
            case GravityState.Gravity:
                followPos = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z);
                if (playerState == PlayerState.Cube || playerState == PlayerState.Sphere)
                {
                    Gravity(GravityState.NonGravity);
                }
                else
                {
                    lastGravityState = GravityState.Gravity;
                    Gravity(GravityState.Middle);
                }
                break;
            case GravityState.NonGravity:
                followPos = new Vector3(transform.position.x, transform.position.y - .25f, transform.position.z);
                if (playerState == PlayerState.Cube || playerState == PlayerState.Sphere)
                {
                    Gravity(GravityState.Gravity);
                }
                else
                {
                    lastGravityState = GravityState.NonGravity;
                    Gravity(GravityState.Middle);
                }
                break;
            case GravityState.Middle:
                if (lastGravityState == GravityState.NonGravity)
                {
                    gravityState = GravityState.Gravity;
                }
                else if (lastGravityState == GravityState.Gravity)
                {
                    gravityState = GravityState.NonGravity;
                }
                break;
            default:
                break;
        }
        Debug.Log(player.transform.localEulerAngles.x);
        switch (playerState)
        {
            case PlayerState.Cube:
                Cube();
                break;
            case PlayerState.Sphere:
                break;
            case PlayerState.Triangle:
                break;
            case PlayerState.Submarine:
                break;
            default:
                break;
        }
    }
    void Cube()
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
            rb.AddForce(Vector3.up * 250);
            switch (rotate)
            {
                case 0:
                    rotate = 90;
                    break;
                case 90:
                    rotate = 180;
                    break;
                case 180:
                    rotate = 90;
                    break;
                default:
                    break;
            }
            player.transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x + rotate, transform.localEulerAngles.y, transform.localEulerAngles.z), .75f).SetEase(Ease.Linear);
            jumped = true;
        }
        distanceTravelled += Time.deltaTime;
        followRot = new Vector3(transform.localEulerAngles.x + rotate, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (gravityState == GravityState.Gravity)
        {
            transform.position = gravityPathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = gravityPathCreator.path.GetRotationAtDistance(distanceTravelled);
        }
        else if (gravityState == GravityState.NonGravity)
        {
            transform.position = ungravityPathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = ungravityPathCreator.path.GetRotationAtDistance(distanceTravelled);
        }
        if (jumped)
        {
            followPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            followRot = new Vector3(player.transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        if (gravity)
        {
            followPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            followRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        //if (rotate)
        //{
        //    followPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //    followRot = new Vector3(player.transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        //}
        player.position = followPos;
        player.localEulerAngles = followRot;
    }
    void Gravity(GravityState gravity)
    {
        if (Input.GetKeyDown(KeyCode.C) && !this.gravity)
        {
            gravityState = gravity;
            this.gravity = true;
            switch (gravity)
            {
                case GravityState.Gravity:
                    player.transform.DOMoveY(gravityPathCreator.transform.position.y + .25f, .5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        this.gravity = false;
                    });
                    break;
                case GravityState.NonGravity:
                    player.transform.DOMoveY(ungravityPathCreator.transform.position.y - .25f, .5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        this.gravity = false;
                    });
                    break;
                case GravityState.Middle:
                    break;
                default:
                    break;
            }
        }
    }
}
