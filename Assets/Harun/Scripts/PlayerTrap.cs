using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrap : MonoBehaviour
{
    [SerializeField] PlayerMove playerMove;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            playerMove.Trap();
        }
    }
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer == 3)
    //    {
    //        playerMove.jumped = true;
    //    }
    //}
}
