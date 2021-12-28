using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    public Joystick rotateJoystick;
    public GameObject bullet;
    public GameObject pnRestart;
    private Stack<Vector3> queuePosition = new Stack<Vector3>();
    private Stack<Quaternion> queueRotation = new Stack<Quaternion>();
    private float lastTimeAttack = 0f;
    private Vector2 lastJoystick = new Vector2(0, 0);
    
    void Start(){
        GetComponent<CharacterController>().afterDie = new CharacterController.AfterDie(AfterDie);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<CharacterInfo>().health <= 0) return;

        if (rotateJoystick.Direction.Equals(Vector2.zero) && !lastJoystick.Equals(rotateJoystick.Direction) && queuePosition.Count == 0){
            int countbullet = GetComponent<CharacterInfo>().status.FindAll(x => x == STATUS.BULLET_UP).Count;
            for (int i = 0; i <= countbullet; i++){
                queuePosition.Push(transform.position);
                queueRotation.Push(transform.rotation);
            }
            lastJoystick = rotateJoystick.Direction;
        }
        if (queuePosition.Count > 0 && Time.realtimeSinceStartup - lastTimeAttack >= 0.05f){
            GetComponent<CharacterController>().Attack(bullet, queuePosition.Pop(), queueRotation.Pop());
            lastTimeAttack = Time.realtimeSinceStartup;
        }
        if (!rotateJoystick.Equals(Vector2.zero))
            lastJoystick = new Vector2(rotateJoystick.Direction.x, rotateJoystick.Direction.y);
    }

    public void AfterDie(GameObject killer){
        pnRestart.SetActive(true);
    }
}
