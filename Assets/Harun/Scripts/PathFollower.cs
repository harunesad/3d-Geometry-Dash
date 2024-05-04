using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using DG.Tweening;
using TMPro;

public class PathFollower : MonoBehaviour
{
    PlayerMove playerMove;
    public Vector3 followPos;
    public Mesh startMesh;
    public Vector3 startPos;
    public PlayerState playerState, startPlayerState;
    [SerializeField] CanvasGroup startPanel;
    float time = 3.5f;
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
    private void Start()
    {
        startPos = playerMove.player.transform.position;
    }
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            startPanel.GetComponentInChildren<TextMeshProUGUI>().text = ((int)time).ToString();
            startPanel.DOFade(0, 1).SetEase(Ease.Linear);
        }
        else
        {
            startPanel.GetComponentInChildren<TextMeshProUGUI>().text = ((int)time).ToString();
            return;
        }
        switch (gravityState)
        {
            case GravityState.Gravity:
                followPos = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
                if (playerState == PlayerState.Cube || playerState == PlayerState.Sphere)
                {
                    playerMove.GravityJumpControl();
                    if (playerMove.nongravity)
                    {
                        playerMove.Gravity(GravityState.NonGravity);
                    }
                }
                else if (playerState == PlayerState.Triangle || playerState == PlayerState.Submarine)
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
                    if (playerMove.gravity)
                    {
                        playerMove.Gravity(GravityState.Gravity);
                    }
                }
                else if(playerState == PlayerState.Triangle || playerState == PlayerState.Submarine)
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
                }
                else if (lastGravityState == GravityState.Gravity)
                {
                    playerMove.Gravity(GravityState.NonGravity);
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
