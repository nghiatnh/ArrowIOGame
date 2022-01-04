using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public Transform cam;
    public CharacterInfo playerInfo;
    public Gradient healthColor;
    [SerializeField] private Transform healthSprite;
    [SerializeField] private Transform healthBorder;
    [SerializeField] private Transform healthBackground;
    [SerializeField] private Transform healthScale;
    // Start is called before the first frame update
    void Start()
    {
        playerInfo = transform.parent.GetComponent<CharacterInfo>();
        updateHealth();
        if (cam == null){
            cam = GameObject.Find("MainCam").transform;
        }
    }

    // Update is called once per frame
    void Update(){
        healthSprite.GetComponent<SpriteRenderer>().color = healthColor.Evaluate(playerInfo.health / playerInfo.maxHealth);
        healthSprite.localScale = new Vector3(healthBackground.localScale.x * playerInfo.health / playerInfo.maxHealth, healthSprite.transform.localScale.y, healthSprite.transform.localScale.z);
        healthSprite.localPosition = new Vector3(-(healthBackground.localScale.x - healthSprite.localScale.x)/2, 0, 0);
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    public void updateHealth(){
        healthScale.localScale = new Vector3(0.0005f * playerInfo.maxHealth, healthScale.localScale.y, healthScale.localScale.z);
    }
}
