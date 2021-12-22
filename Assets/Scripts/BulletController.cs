using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject parent;
    public float initSpeed = 10f;
    public float ATK = 0f;
    private float stayTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        stayTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * initSpeed * Time.deltaTime);
        if (Time.realtimeSinceStartup - stayTime >= 10f)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider){
        if ((collider.CompareTag("Wall") && !parent.GetComponent<CharacterInfo>().status.Contains(STATUS.BULLET_THROUGH_WALL)) || collider.CompareTag("Ground")){
            Destroy(gameObject);
        }
    }
}
