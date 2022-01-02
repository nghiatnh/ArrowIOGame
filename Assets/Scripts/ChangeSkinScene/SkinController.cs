using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkinController : MonoBehaviour
{
    public Transform pnSkill;
    public SkinInfo[] Skins;
    public Text txtName;
    private Vector3 target;
    private readonly Vector3 Rotation = new Vector3(0, -200, 0);
    private List<Transform> ListSkins = new List<Transform>();
    private const float scale = 0.5f;
    private int currentSkin = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Skins.Length; i++){
            Transform skin = Instantiate(Skins[i].SkinObject, Vector3.zero, Quaternion.Euler(0, 0, 0 ), transform.GetChild(0)).transform;
            skin.localPosition = Vector3.right * 5f  * i;
            skin.transform.Rotate(Rotation);
            skin.transform.localScale = Vector3.one * scale;
            skin.name = Skins[i].name;
            ListSkins.Add(skin);
            if (GameInformation.Instance.PlayerSkin == Skins[i].SkinObject) currentSkin = i;
        }
        GameInformation.Instance.PlayerSkin = Skins[currentSkin];
        transform.position = new Vector3(-ListSkins[currentSkin].transform.localPosition.x, transform.position.y, transform.position.z);
        target = transform.position;
        DisplaySkin();
    }

    private void DisplaySkin(){
        transform.position = target;
        txtName.text = Skins[currentSkin].Name;
        for (int i = 0; i < pnSkill.childCount; i++){
            pnSkill.GetChild(i).GetComponent<Image>().sprite = Skins[currentSkin].Skills[i].SkillImage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, 0.1f);
        if (Mathf.Abs(transform.position.x - target.x) <= 0.2f) DisplaySkin();
    }

    public void MoveLeft(){
        if (currentSkin > 0) {
            currentSkin--;
            target = new Vector3(-ListSkins[currentSkin].transform.localPosition.x, transform.position.y, transform.position.z);
        }
    }

    public void MoveRight(){
        if (currentSkin < Skins.Length - 1) {
            currentSkin++;
            target = new Vector3(-ListSkins[currentSkin].localPosition.x, transform.position.y, transform.position.z);
        }
    }

    public void Exit(){
        SceneManager.LoadScene("StartScene");
    }

    public void ChooseSkin(){
        GameInformation.Instance.PlayerSkin = Skins[currentSkin];
        GetComponent<Animator>().SetTrigger("Execute");
        GetComponent<AudioSource>().Play();
    }
}
