using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrash : MonoBehaviour
{
    [SerializeField] PlayerMove playerMove;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            playerMove.Jump();
        }
    }
}
