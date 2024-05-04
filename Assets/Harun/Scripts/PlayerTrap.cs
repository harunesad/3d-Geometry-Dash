using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrap : MonoBehaviour
{
    [SerializeField] PlayerMove playerMove;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            playerMove.Trap();
        }
    }
}
