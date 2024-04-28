using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : MonoBehaviour
{
    [SerializeField] Mesh newMesh;
    [SerializeField] PathFollower.PlayerState playerState;
    [SerializeField] PathFollower pathFollower;
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
                    break;
                case PathFollower.PlayerState.Submarine:
                    break;
                default:
                    break;
            }
            other.GetComponent<MeshFilter>().mesh = newMesh;
            pathFollower.playerState = playerState;
            pathFollower.GetComponent<PlayerMove>().rotate = 0;
        }
    }
}
