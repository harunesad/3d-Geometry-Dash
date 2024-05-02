using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : MonoBehaviour
{
    [SerializeField] Mesh newMesh;
    [SerializeField] PathFollower.PlayerState playerState;
    [SerializeField] PathFollower pathFollower;
    [SerializeField] bool gravity, nongravity, lightUpdate;
    [SerializeField] Color lightColor;
    [SerializeField] Light light;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            PlayerMove playerMove = pathFollower.GetComponent<PlayerMove>();
            other.GetComponent<BoxCollider>().enabled = false;
            other.GetComponent<SphereCollider>().enabled = false;
            switch (pathFollower.playerState)
            {
                case PathFollower.PlayerState.Cube:
                    other.GetComponent<BoxCollider>().enabled = true;
                    break;
                case PathFollower.PlayerState.Sphere:
                    other.GetComponent<SphereCollider>().enabled = true;
                    break;
                case PathFollower.PlayerState.Triangle:
                    other.GetComponent<BoxCollider>().enabled = true;
                    break;
                case PathFollower.PlayerState.Submarine:
                    other.GetComponent<BoxCollider>().enabled = true;
                    break;
                default:
                    break;
            }
            pathFollower.gravityState = PathFollower.GravityState.Gravity;
            playerMove.PathControl();
            playerMove.gravityChange = true;
            playerMove.gravity = gravity;
            playerMove.nongravity = nongravity;
            if (gravity)
            {
                playerMove.PathUpdate(playerMove.transform.position.y + .1f);
            }
            else
            {
                playerMove.PathUpdate(playerMove.transform.position.y - .1f);
            }
            other.GetComponent<MeshFilter>().mesh = newMesh;
            pathFollower.playerState = playerState;
            playerMove.rotate = 0;
            playerMove.currentRotationX = 0;
            playerMove.jumped = false;
            pathFollower.startGravityState = PathFollower.GravityState.Gravity;
            pathFollower.startPlayerState = playerState;
            pathFollower.startMesh = newMesh;
            pathFollower.startPos = other.transform.position;
            playerMove.startDistanceTravelled = playerMove.distanceTravelled;
            if (lightUpdate)
            {
                light.color = lightColor;
            }
        }
    }
}
