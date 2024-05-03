using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrash : MonoBehaviour
{
    [SerializeField] PlayerMove playerMove;
    bool jump;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jump)
        {
            Debug.Log("ssss");
            //playerMove.player.Rotate(new Vector3(90, 0, 0) * Time.deltaTime);
            //playerMove.followRot = new Vector3(playerMove.player.localEulerAngles.x, playerMove.transform.localEulerAngles.y, playerMove.transform.localEulerAngles.z);
            //playerMove.JumpRotate();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            playerMove.Jump();
            //playerMove.rotate = playerMove.player.localEulerAngles.z - playerMove.transform.localEulerAngles.z;
            //playerMove.followRot = new Vector3(transform.localEulerAngles.x + playerMove.rotate, playerMove.transform.localEulerAngles.y, playerMove.transform.localEulerAngles.z);
            //playerMove.jumpPlatform = true;
            //jump = true;
            //playerMove.JumpRotate();
        }
    }
}
