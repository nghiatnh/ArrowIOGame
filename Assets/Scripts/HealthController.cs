using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public Transform cam;
    public CharacterInfo playerInfo;
    public Gradient healthColor;
    private Transform healthSprite;
    private float maxScale;
    // Start is called before the first frame update
    void Start()
    {
        playerInfo = transform.parent.GetComponent<CharacterInfo>();
        healthSprite = transform.Find("health");
        maxScale = transform.Find("background").transform.localScale.x;
        if (cam == null){
            cam = GameObject.Find("Main Camera").transform;
        }
    }

    // Update is called once per frame
    void Update(){
        healthSprite.GetComponent<SpriteRenderer>().color = healthColor.Evaluate(playerInfo.health / playerInfo.maxHealth);
        healthSprite.localScale = new Vector3(maxScale * playerInfo.health / playerInfo.maxHealth, healthSprite.transform.localScale.y, healthSprite.transform.localScale.z);
        healthSprite.localPosition = new Vector3(-(maxScale - healthSprite.localScale.x)/2, 0, 0);
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
