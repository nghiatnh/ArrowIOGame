using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 moveDir;
    public LayerMask Wall;
    public GameObject cam;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<CharacterInfo>().health > 0){
        moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.A)){
            if (!IsCollide(Vector3.left))
                moveDir.x += -1;
        }
        if (Input.GetKey(KeyCode.D)){
            if (!IsCollide(Vector3.right))
                moveDir.x += 1; 
        }
        if (Input.GetKey(KeyCode.W)){
            if (!IsCollide(Vector3.forward))
                moveDir.z += 1; 
        }
        if (Input.GetKey(KeyCode.S)){
            if (!IsCollide(Vector3.back))
                moveDir.z += -1; 
        }
        GetComponent<Rigidbody>().velocity = moveDir * moveSpeed;
        transform.rotation = Quaternion.LookRotation(new Vector3((Input.mousePosition.x - Screen.width / 2), 0, (Input.mousePosition.y - Screen.height / 2)));
        }        
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + 25, transform.position.z - 9);
    }

    private bool IsCollide(Vector3 moveDir){
        return Physics.Raycast(gameObject.transform.position, moveDir, 0.55f, Wall);
    }

    public void UpdateSpeed(){
        moveSpeed = GetComponent<CharacterInfo>().speed;
    }
}
