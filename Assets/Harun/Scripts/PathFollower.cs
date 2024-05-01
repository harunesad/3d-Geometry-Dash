using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using DG.Tweening;

public class PathFollower : MonoBehaviour
{
    PlayerMove playerMove;
    public Vector3 followPos;
    public Mesh startMesh;
    public Vector3 startPos;
    public PlayerState playerState, startPlayerState;
    public enum PlayerState
    {
        Cube,
        Sphere,
        Triangle,
        Submarine
    }
    public GravityState gravityState, lastGravityState, startGravityState;
    public enum GravityState
    {
        Gravity,
        NonGravity,
        Middle
    }
    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
    }
    void Update()
    {
        switch (gravityState)
        {
            case GravityState.Gravity:
                followPos = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
                if (playerState == PlayerState.Cube || playerState == PlayerState.Sphere)
                {
                    playerMove.GravityJumpControl();
                    playerMove.Gravity(GravityState.NonGravity);
                }
                else
                {
                    lastGravityState = GravityState.Gravity;
                    playerMove.Gravity(GravityState.Middle);
                }
                break;
            case GravityState.NonGravity:
                followPos = new Vector3(transform.position.x, transform.position.y - .1f, transform.position.z);
                if (playerState == PlayerState.Cube || playerState == PlayerState.Sphere)
                {
                    playerMove.NongravityJumpControl();
                    playerMove.Gravity(GravityState.Gravity);
                }
                else
                {
                    lastGravityState = GravityState.NonGravity;
                    playerMove.Gravity(GravityState.Middle);
                }
                break;
            case GravityState.Middle:
                followPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                if (lastGravityState == GravityState.NonGravity)
                {
                    playerMove.Gravity(GravityState.Gravity);
                    //gravityState = GravityState.Gravity;
                }
                else if (lastGravityState == GravityState.Gravity)
                {
                    playerMove.Gravity(GravityState.NonGravity);
                    //gravityState = GravityState.NonGravity;
                }
                break;
            default:
                break;
        }
        switch (playerState)
        {
            case PlayerState.Cube:
                playerMove.CubeMove();
                break;
            case PlayerState.Sphere:
                playerMove.SphereMove();
                break;
            case PlayerState.Triangle:
                playerMove.TriangleMove();
                break;
            case PlayerState.Submarine:
                playerMove.SubmarineMove();
                break;
            default:
                break;
        }
        playerMove.TrapCrash();
    }
}
