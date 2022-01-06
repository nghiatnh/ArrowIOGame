using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerOnlineController : MonoBehaviour
{
    private Vector3 moveDir;
    public LayerMask Wall;
    public LayerMask Outer;
    public GameObject cam;
    public Transform Skin;
    public GameObject bullet;
    public OnlineUIController UI;
    public FixedJoystick rotateJoystick;
    public FixedJoystick moveJoystick;
    public AudioSource collectItemSource;
    public AudioSource levelUpSource;
    public CharacterInfo info;
    public GameObject levelUp;
    public GameObject Name;
    public SkinInfo[] Skins;
    
    private Stack<Vector3> queuePosition = new Stack<Vector3>();
    private Stack<Quaternion> queueRotation = new Stack<Quaternion>();
    private float lastTimeAttack = 0f;
    private Vector2 lastJoystick = new Vector2(0, 0);
    private PhotonView view;
    private bool IsMine = false;

    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<CharacterInfo>();
        info.MAX_EXP = GameConstant.MAX_EXP_LEVEL[info.level];
        info.characterName = GameInformation.Instance.PlayerName;
        info.onUpdateInfo = new CharacterInfo.OnUpdateInfo(UpdateInfo);
        GetComponent<CharacterOnlineController>().afterDie = new CharacterOnlineController.AfterDie(AfterDie);
        if (Skin == null)
            Skin = transform.Find("Skin");
        if (UI == null)
            UI = GameObject.FindGameObjectWithTag("Controller").GetComponent<OnlineUIController>();
        if (cam == null)
            cam = GameObject.Find("MainCam");
        view = gameObject.GetPhotonView();
        IsMine = view.IsMine;
        if (!IsMine)
            NotMine();
        else
            Mine();
    }
    void Update()
    {
        if (rotateJoystick == null)
            rotateJoystick = UI.rotateJoystick.GetComponent<FixedJoystick>();
        if (moveJoystick == null)
           moveJoystick = UI.moveJoystick.GetComponent<FixedJoystick>();
    
        if (!IsMine || GetComponent<CharacterInfo>().health <= 0) return;

        float moveSpeed = GetComponent<CharacterInfo>().speed;
        if (GetComponent<CharacterInfo>().health > 0){
            moveDir = new Vector3(moveJoystick.Direction.x, GetComponent<Rigidbody>().velocity.y / moveSpeed, moveJoystick.Direction.y);
            GetComponent<Rigidbody>().velocity = moveDir * moveSpeed;
            if (Mathf.Abs(rotateJoystick.Direction.x) >= 0.5f || Mathf.Abs(rotateJoystick.Direction.y) >= 0.5f) 
                transform.rotation = Quaternion.LookRotation(new Vector3(rotateJoystick.Direction.x, 0, rotateJoystick.Direction.y));
        }        
        if ((moveDir.x != 0 || moveDir.z != 0) && GetComponent<CharacterInfo>().status.Contains(STATUS.MAKE_SPIKE)){
            GetComponent<CharacterOnlineController>().CreateSpike();
        }
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + 25, transform.position.z - 9);

        if (rotateJoystick.Direction.Equals(Vector2.zero) && !lastJoystick.Equals(rotateJoystick.Direction) && queuePosition.Count == 0){
            int countbullet = GetComponent<CharacterInfo>().status.FindAll(x => x == STATUS.BULLET_UP).Count;
            for (int i = 0; i <= countbullet; i++){
                queuePosition.Push(transform.position);
                queueRotation.Push(transform.rotation);
            }
            lastJoystick = rotateJoystick.Direction;
        }
        if (queuePosition.Count > 0 && Time.realtimeSinceStartup - lastTimeAttack >= 0.05f){
            GetComponent<CharacterOnlineController>().Attack(bullet, queuePosition.Pop(), queueRotation.Pop());
            lastTimeAttack = Time.realtimeSinceStartup;
        }
        if (!rotateJoystick.Equals(Vector2.zero))
            lastJoystick = new Vector2(rotateJoystick.Direction.x, rotateJoystick.Direction.y);
    }

    void OnTriggerEnter(Collider collider){
        if (!IsMine) return;
        if (collider.CompareTag("Heart") || collider.CompareTag("Diamond"))
            view.RPC("CollectItem", RpcTarget.All, collider.tag, collider.gameObject.GetPhotonView().ViewID);
    }

    [PunRPC]
    void CollectItem(string itemName, int objID){
        GameObject obj = PhotonNetwork.GetPhotonView(objID).gameObject;
        switch(itemName){
            case "Heart":
                collectItemSource.Play();
                GameInformation.Instance.ItemCount--;
                info.GainHealth(obj.GetComponent<ItemInfo>().Health);
                obj.GetComponent<ItemOnlineController>().Collected();
                break;
            case "Diamond":
                collectItemSource.Play();
                if (IsMine)
                    GainMineEXP(obj.GetComponent<ItemInfo>().EXP);
                else
                    GainOtherEXP(obj.GetComponent<ItemInfo>().EXP);
                obj.GetComponent<ItemOnlineController>().Collected();
                GameInformation.Instance.ItemCount--;
                break;
        }
    }

    public void GainMineEXP(int EXP){
        info.EXP += EXP;
        UI.UpdateExperience((float)info.EXP / info.MAX_EXP, "LV " + info.level);
        if (info.EXP >= info.MAX_EXP && info.level < GameConstant.MAX_EXP_LEVEL.Length - 1){
            info.level++;
            GameObject.Find("UI").GetComponent<UIController>().AddQueueSkill();
            info.MAX_EXP = GameConstant.MAX_EXP_LEVEL[info.level];
            int exp = info.EXP;
            info.EXP = 0;
            info.speed = Mathf.Max(2.5f, info.speed - 0.1f);
            UI.ShowLevelUp(levelUp);
            levelUpSource.Play();

            GainMineEXP(exp - GameConstant.MAX_EXP_LEVEL[info.level - 1]);     
        }
        else if (info.level >= GameConstant.MAX_EXP_LEVEL.Length - 1){
            UI.UpdateExperience(1, "LV Max");
        }
    }

    public void GainOtherEXP(int EXP){
        info.EXP += EXP;
        if (info.EXP >= info.MAX_EXP && info.level < GameConstant.MAX_EXP_LEVEL.Length - 1){
            info.level++;
            info.MAX_EXP = GameConstant.MAX_EXP_LEVEL[info.level];
            int exp = info.EXP;
            info.EXP = 0;
            info.speed = Mathf.Max(2.5f, info.speed - 0.1f);

            GainOtherEXP(exp - GameConstant.MAX_EXP_LEVEL[info.level - 1]);     
        }
    }

    [PunRPC]
    public void ChangeSkin(string skName){
        Destroy(Skin.GetChild(0).gameObject);
        foreach (var sk in Skins){
            if (sk.Name != skName) continue;
            GameObject skin = Instantiate(sk.SkinObject, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), Skin);
            skin.transform.localPosition = new Vector3(0, 0, 0);
            break;
        }
    }

    void NotMine(){
        Destroy(GetComponent<AudioListener>());
        Name.GetComponent<TextMesh>().text = gameObject.GetPhotonView().Owner.NickName;
    }

    void Mine(){
        Destroy(Name);
        if (GameInformation.Instance.PlayerSkin == null)
            foreach (var sk in Skins)
                if (sk.SkinObject.name == Skin.GetChild(0).name){
                    GameInformation.Instance.PlayerSkin = sk;
                    break;
                }
        view.RPC("ChangeSkin", RpcTarget.All, GameInformation.Instance.PlayerSkin.Name);
        GameObject.Find("UI").GetComponent<UIController>().player = transform;
    }

    public void AfterDie(GameObject killer){
        if (IsMine){
            UI.ShowPanelRestart();
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void UpdateInfo(SKILLS skill){
        if (IsMine)
            view.RPC("RPCUpdateInfo", RpcTarget.Others, skill);
    }

    [PunRPC]
    private void RPCUpdateInfo(SKILLS skill){
        SkillController.ChooseSkill(skill, transform, false);
    }
}
