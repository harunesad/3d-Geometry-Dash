using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrash : MonoBehaviour
{
    [SerializeField] PlayerMove playerMove;
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
            playerMove.Jump();
            playerMove.JumpRotate();
        }
    }
}
