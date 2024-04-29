using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerMove : MonoBehaviour
{
    public PathCreator gravityPathCreator, ungravityPathCreator, middleTrianglePathCreator, middleSubmarinePathCreator;
    public Transform player;
    public float speed;
    public LayerMask groundLayer;
    bool jump;
    public bool gravity, jumped;
    float distanceTravelled;
    public float rotate = 0, currentRotationX;
    Rigidbody rb;
    Vector3  followRot, gravityPos;
    PathFollower pathFollower;
    private void Awake()
    {
        pathFollower = GetComponent<PathFollower>();
        rb = player.GetComponent<Rigidbody>();
        gravityPos = Physics.gravity;
    }
    public void CubeMove()
    {
        PathControl();
        //JumpControl();
        if (Input.GetKeyDown(KeyCode.Space) && jump && !gravity)
        {
            //rb.AddForce(Vector3.up * 200);
            if (pathFollower.gravityState == PathFollower.GravityState.Gravity)
            {
                player.DOMoveY(player.transform.position.y + .5f, .5f);
                JumpRotate();
                jumped = true;
            }
            else if (pathFollower.gravityState == PathFollower.GravityState.NonGravity)
            {
                Physics.gravity = gravityPos * -1;
                player.DOMoveY(player.position.y - .5f, .5f);
                JumpRotate();
                jumped = true;
            }
        }
        followRot = new Vector3(transform.localEulerAngles.x + rotate, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (jumped)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
            followRot = new Vector3(player.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        if (gravity)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        player.position = Vector3.Lerp(player.position, pathFollower.followPos, Time.deltaTime * 50);
        //player.position = pathFollower.followPos;
        player.localEulerAngles = followRot;
    }
    public void SphereMove()
    {
        //JumpControl();
        if (Input.GetKeyDown(KeyCode.Space) && jump && !gravity)
        {
            //rb.AddForce(Vector3.up * 200);
            if (pathFollower.gravityState == PathFollower.GravityState.Gravity)
            {
                player.DOMoveY(player.position.y + .5f, .5f);
                jumped = true;
            }
            else if (pathFollower.gravityState == PathFollower.GravityState.NonGravity)
            {
                Physics.gravity = gravityPos * -1;
                player.DOMoveY(player.position.y - .5f, .5f);
                jumped = true;
            }
        }
        PathControl();
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, transform.up);
        if (jumped)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        if (gravity)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
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
        followRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (gravity)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        player.position = Vector3.Lerp(player.position, pathFollower.followPos, Time.deltaTime * 5);
        //player.position = pathFollower.followPos;
        player.localEulerAngles = followRot;
    }
    public void SubmarineMove()
    {
        PathControl();
        followRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (gravity)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        player.position = Vector3.Lerp(player.position, pathFollower.followPos, Time.deltaTime * 50);
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
            else if (pathFollower.playerState == PathFollower.PlayerState.Submarine)
            {
                float posY = (Input.GetAxis("Vertical") / 5);
                pathFollower.followPos = pathFollower.followPos + new Vector3(0, posY, 0);
                transform.position = middleSubmarinePathCreator.path.GetPointAtDistance(distanceTravelled);
                transform.rotation = middleSubmarinePathCreator.path.GetRotationAtDistance(distanceTravelled);
            }
        }
    }
    public void GravityJumpControl()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.position, Vector3.down, out hit, .125f, groundLayer) && !jump)
        {
            Physics.gravity = gravityPos;
            jump = true;
            jumped = false;
        }
        else if (!Physics.Raycast(player.position, Vector3.down, out hit, .125f, groundLayer))
        {
            jump = false;
        }
    }
    public void NongravityJumpControl()
    {
        Collider[] colliders = Physics.OverlapBox(player.position + (Vector3.up * .06f), Vector3.one * .125f, Quaternion.identity, groundLayer);
        if (colliders.Length > 0 && !jump)
        {
            Physics.gravity = gravityPos;
            jump = true;
            jumped = false;
        }
        else if (colliders.Length == 0)
        {
            jump = false;
        }
        //RaycastHit hit;
        //if (Physics.Raycast(player.transform.position, Vector3.up, out hit, .5f, groundLayer) && !jump)
        //{
        //    jump = true;
        //    jumped = false;
        //}
        //else if (!Physics.Raycast(player.transform.position, Vector3.up, out hit, .125f, groundLayer))
        //{
        //    jump = false;
        //}
    }
    void JumpRotate()
    {
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
        player.DOLocalRotate(new Vector3(transform.localEulerAngles.x + rotate, transform.localEulerAngles.y, transform.localEulerAngles.z), .85f).SetEase(Ease.Linear);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(player.position + (Vector3.up * .06f), Vector3.one * .125f);
    }
    public void Gravity(PathFollower.GravityState gravity)
    {
        if (Input.GetKeyDown(KeyCode.C) && !this.gravity && !jumped)
        {
            pathFollower.gravityState = gravity;
            PathControl();
            this.gravity = true;
            switch (gravity)
            {
                case PathFollower.GravityState.Gravity:
                    player.DOMoveY(transform.position.y + .1f, .2f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        this.gravity = false;
                    });
                    break;
                case PathFollower.GravityState.NonGravity:
                    player.DOMoveY(transform.position.y - .1f, .2f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        this.gravity = false;
                    });
                    break;
                case PathFollower.GravityState.Middle:
                    player.DOMoveY(transform.position.y, .2f).SetEase(Ease.Linear).OnComplete(() =>
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
