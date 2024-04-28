using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerMove : MonoBehaviour
{
    public PathCreator gravityPathCreator, ungravityPathCreator, middleTrianglePathCreator;
    public Transform player;
    public float speed;
    public LayerMask groundLayer;
    bool jump;
    public bool gravity, jumped;
    float distanceTravelled;
    public float rotate = 0, currentRotationX;
    Rigidbody rb;
    Vector3  followRot;
    PathFollower pathFollower;
    private void Awake()
    {
        pathFollower = GetComponent<PathFollower>();
        rb = player.GetComponent<Rigidbody>();
    }
    public void CubeMove()
    {
        JumpControl();
        Debug.DrawRay(player.transform.position, Vector3.down * .125f, Color.red);
        if (Input.GetKeyDown(KeyCode.Space) && jump && !gravity)
        {
            //rb.AddForce(Vector3.up * 200);
            player.transform.DOMoveY(player.transform.position.y + .5f, .5f);
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
            player.transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x + rotate, transform.localEulerAngles.y, transform.localEulerAngles.z), .85f).SetEase(Ease.Linear);
            jumped = true;
        }
        PathControl();
        followRot = new Vector3(transform.localEulerAngles.x + rotate, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (jumped)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            followRot = new Vector3(player.transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        if (gravity)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
        player.position = Vector3.Lerp(player.position, pathFollower.followPos, Time.deltaTime * 50);
        //player.position = pathFollower.followPos;
        player.localEulerAngles = followRot;
    }
    public void SphereMove()
    {
        JumpControl();
        if (Input.GetKeyDown(KeyCode.Space) && jump && !gravity)
        {
            //rb.AddForce(Vector3.up * 200);
            player.transform.DOMoveY(player.position.y + .5f, .5f);
            jumped = true;
        }
        PathControl();
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, transform.up);
        if (jumped)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
        if (gravity)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
        player.position = Vector3.Lerp(player.position, pathFollower.followPos, Time.deltaTime * 50);
        //player.position = pathFollower.followPos;
        player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        currentRotationX += Time.deltaTime * 50;
        player.rotation = Quaternion.Euler(currentRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
    public void TriangleMove()
    {
        PathControl();
        if (gravity)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
        player.position = Vector3.Lerp(player.position, pathFollower.followPos, Time.deltaTime * 50);
        //player.position = pathFollower.followPos;
        player.localEulerAngles = followRot;
    }
    void PathControl()
    {
        distanceTravelled += Time.deltaTime;
        if (pathFollower.gravityState == PathFollower.GravityState.Gravity)
        {
            transform.position = gravityPathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = gravityPathCreator.path.GetRotationAtDistance(distanceTravelled);
        }
        else if (pathFollower.gravityState == PathFollower.GravityState.NonGravity)
        {
            transform.position = ungravityPathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = ungravityPathCreator.path.GetRotationAtDistance(distanceTravelled);
        }
        else if (pathFollower.gravityState == PathFollower.GravityState.Middle)
        {
            if (pathFollower.playerState == PathFollower.PlayerState.Triangle)
            {
                transform.position = middleTrianglePathCreator.path.GetPointAtDistance(distanceTravelled);
                transform.rotation = middleTrianglePathCreator.path.GetRotationAtDistance(distanceTravelled);
            }
        }
    }
    void JumpControl()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, .125f, groundLayer) && !jump)
        {
            jump = true;
            jumped = false;
        }
        else if (!Physics.Raycast(player.transform.position, Vector3.down, out hit, .125f, groundLayer))
        {
            jump = false;
        }
    }
    public void Gravity(PathFollower.GravityState gravity)
    {
        Debug.Log(jumped);
        if (Input.GetKeyDown(KeyCode.C) && !this.gravity && !jumped)
        {
            pathFollower.gravityState = gravity;
            PathControl();
            this.gravity = true;
            switch (gravity)
            {
                case PathFollower.GravityState.Gravity:
                    player.transform.DOMoveY(transform.position.y + .1f, .2f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        this.gravity = false;
                    });
                    break;
                case PathFollower.GravityState.NonGravity:
                    player.transform.DOMoveY(transform.position.y - .1f, .2f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        this.gravity = false;
                    });
                    break;
                case PathFollower.GravityState.Middle:
                    player.transform.DOMoveY(transform.position.y, .2f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        this.gravity = false;
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
