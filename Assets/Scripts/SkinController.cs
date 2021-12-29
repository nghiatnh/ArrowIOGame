using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    public GameObject[] Skins;
    private Vector3 target;
    private readonly Vector3 Rotation = new Vector3(13, 160, -5);
    private List<Transform> ListSkins = new List<Transform>();
    private const float scale = 0.5f;
    private int currentSkin = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Skins.Length; i++){
            Transform skin = Instantiate(Skins[i], Vector3.zero, Quaternion.Euler(0, 0, 0 ), transform).transform;
            skin.localPosition = Vector3.right * 5f  * i;
            skin.transform.Rotate(Rotation);
            skin.transform.localScale = Vector3.one * scale;
            ListSkins.Add(skin);
            if (GameInformation.Instance.PlayerSkin == Skins[i]) currentSkin = i;
        }
        GameInformation.Instance.PlayerSkin = Skins[currentSkin].gameObject;
        transform.position = new Vector3(-ListSkins[currentSkin].transform.localPosition.x, transform.position.y, transform.position.z);
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, 0.1f);
        if (Mathf.Abs(transform.position.x - target.x) <= 0.2f) transform.position = target;
    }

    public void MoveLeft(){
        if (currentSkin > 0) {
            currentSkin--;
            target = new Vector3(-ListSkins[currentSkin].transform.localPosition.x, transform.position.y, transform.position.z);
            GameInformation.Instance.PlayerSkin = Skins[currentSkin].gameObject;
        }
    }

    public void MoveRight(){
        if (currentSkin < Skins.Length - 1) {
            currentSkin++;
            target = new Vector3(-ListSkins[currentSkin].localPosition.x, transform.position.y, transform.position.z);
            GameInformation.Instance.PlayerSkin = Skins[currentSkin].gameObject;
        }
    }
}
