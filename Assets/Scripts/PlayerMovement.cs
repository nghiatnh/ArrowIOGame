using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveDir;
    public LayerMask Wall;
    public LayerMask Outer;
    public GameObject cam;
    public FixedJoystick rotateJoystick;
    public FixedJoystick moveJoystick;

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = GetComponent<CharacterInfo>().speed;
        if (GetComponent<CharacterInfo>().health > 0){
            moveDir = new Vector3(0, GetComponent<Rigidbody>().velocity.y / moveSpeed, 0);
            if (!IsCollide(new Vector3(moveJoystick.Direction.x, 0, moveJoystick.Direction.y)))
                moveDir += new Vector3(moveJoystick.Direction.x, 0, moveJoystick.Direction.y);
            else if (GetComponent<CharacterInfo>().status.Contains(STATUS.THROUGH_WALL))
                ThroughWall(new Vector3(moveJoystick.Direction.x, 0, moveJoystick.Direction.y));
            GetComponent<Rigidbody>().velocity = moveDir * moveSpeed;
            if (Mathf.Abs(rotateJoystick.Direction.x) >= 0.5f || Mathf.Abs(rotateJoystick.Direction.y) >= 0.5f) 
                transform.rotation = Quaternion.LookRotation(new Vector3(rotateJoystick.Direction.x, 0, rotateJoystick.Direction.y));
        }        
        if ((moveDir.x != 0 || moveDir.z != 0) && GetComponent<CharacterInfo>().status.Contains(STATUS.MAKE_SPIKE)){
            GetComponent<CharacterController>().CreateSpike();
        }
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + 25, transform.position.z - 9);
    }

    private bool IsCollide(Vector3 moveDir){
        return Physics.Raycast(gameObject.transform.position, moveDir, 0.55f, Wall) || Physics.Raycast(gameObject.transform.position, moveDir, 0.55f, Outer);
    }

    private void ThroughWall(Vector3 moveDir){
        if (!Physics.Raycast(transform.position + moveDir * 2f, moveDir, 1f, Wall) && !Physics.Raycast(transform.position, moveDir, 0.55f, Outer))
            transform.position += moveDir * 2f;
    }
}
