using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    [SerializeField] InputField ipfName;
    // Start is called before the first frame update
    void Start()
    {
        ipfName.text = GameInformation.Instance.PlayerName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
