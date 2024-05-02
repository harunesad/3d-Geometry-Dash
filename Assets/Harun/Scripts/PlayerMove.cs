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
    public LayerMask groundLayer, trapLayer;
    bool jump;
    public bool gravityChange, jumped, gravity, nongravity;
    public float distanceTravelled, startDistanceTravelled;
    public float rotate = 0, currentRotationX;
    Rigidbody rb;
    Vector3  followRot, gravityPos;
    PathFollower pathFollower;
    [SerializeField] CanvasGroup gameOverPanel;
    private void Awake()
    {
        pathFollower = GetComponent<PathFollower>();
        rb = player.GetComponent<Rigidbody>();
        gravityPos = Physics.gravity;
    }
    public void CubeMove()
    {
        PathControl();
        if (Input.GetKeyDown(KeyCode.Space) && jump && !gravityChange)
        {
            Jump();
            JumpRotate();
        }
        followRot = new Vector3(transform.localEulerAngles.x + rotate, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (!jump)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        else if (gravityChange)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        else if (jumped)
        {
            followRot = new Vector3(player.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        player.position = pathFollower.followPos;
        player.localEulerAngles = followRot;
    }
    public void SphereMove()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jump && !gravityChange)
        {
            Jump();
        }
        PathControl();
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, transform.up);
        if (!jump)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        else if (gravityChange)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        player.position = pathFollower.followPos;
        player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        currentRotationX += Time.deltaTime * 50;
        player.rotation = Quaternion.Euler(currentRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
    public void TriangleMove()
    {
        PathControl();
        followRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (gravityChange)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        player.position = pathFollower.followPos;
        player.localEulerAngles = followRot;
    }
    public void SubmarineMove()
    {
        PathControl();
        followRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (gravityChange)
        {
            pathFollower.followPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        player.position = pathFollower.followPos;
        player.localEulerAngles = followRot;
    }
    public void PathControl()
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
        if (Physics.Raycast(player.position, Vector3.down, .125f, groundLayer) && !jump)
        {
            Physics.gravity = gravityPos;
            jump = true;
            jumped = false;
        }
        else if (!Physics.Raycast(player.position, Vector3.down, .125f, groundLayer))
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
    public void JumpRotate()
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
    public void Jump()
    {
        if (pathFollower.gravityState == PathFollower.GravityState.Gravity)
        {
            //rb.AddForce(Vector3.up * 200);
            player.DOMoveY(player.position.y + .5f, .5f);
            jumped = true;
        }
        else if (pathFollower.gravityState == PathFollower.GravityState.NonGravity)
        {
            Physics.gravity = gravityPos * -1;
            //rb.AddForce(Vector3.down * 200);
            player.DOMoveY(player.position.y - .5f, .5f);
            jumped = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(player.position + (Vector3.up * .06f), Vector3.one * .125f);
    }
    public void Gravity(PathFollower.GravityState gravity)
    {
        if (Input.GetKeyDown(KeyCode.C) && !gravityChange && !jumped)
        {
            pathFollower.gravityState = gravity;
            PathControl();
            gravityChange = true;
            switch (gravity)
            {
                case PathFollower.GravityState.Gravity:
                    PathUpdate(transform.position.y + .1f);
                    break;
                case PathFollower.GravityState.NonGravity:
                    PathUpdate(transform.position.y - .1f);
                    break;
                case PathFollower.GravityState.Middle:
                    PathUpdate(transform.position.y);
                    break;
                default:
                    break;
            }
        }
    }
    public void PathUpdate(float pos)
    {
        player.DOMoveY(pos, .2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            this.gravityChange = false;
        });
        player.DORotate(transform.rotation.eulerAngles + new Vector3(rotate, 0, 0), .2f).SetEase(Ease.Linear);
    }
    public void TrapCrash()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.position, Vector3.right, out hit, .125f, trapLayer))
        {
            gameOverPanel.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                distanceTravelled = startDistanceTravelled;
                transform.position = pathFollower.startPos;
                player.position = pathFollower.startPos;
                player.GetComponent<MeshFilter>().mesh = pathFollower.startMesh;
                pathFollower.gravityState = pathFollower.startGravityState;
                pathFollower.playerState = pathFollower.startPlayerState;
                player.GetComponent<BoxCollider>().enabled = false;
                player.GetComponent<SphereCollider>().enabled = false;
                switch (pathFollower.playerState)
                {
                    case PathFollower.PlayerState.Cube:
                        player.GetComponent<BoxCollider>().enabled = true;
                        break;
                    case PathFollower.PlayerState.Sphere:
                        player.GetComponent<SphereCollider>().enabled = true;
                        break;
                    case PathFollower.PlayerState.Triangle:
                        player.GetComponent<BoxCollider>().enabled = true;
                        break;
                    case PathFollower.PlayerState.Submarine:
                        player.GetComponent<BoxCollider>().enabled = true;
                        break;
                    default:
                        break;
                }
                gameOverPanel.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
                {
                    pathFollower.enabled = true;
                });
            });
            pathFollower.enabled = false;
        }
        Debug.DrawRay(player.position, Vector3.right * .125f, Color.red);
    }
}
