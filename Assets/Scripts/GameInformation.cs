using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation 
{
    private static GameInformation instance;

    public static GameInformation Instance{
        get {
            if (instance == null) instance = new GameInformation();
            return instance; 
        }
    }

    public int ItemCount = 0;
    public int EnemyCount = 0;
    public string PlayerName = "character";
    public SkinInfo PlayerSkin;
    public List<CharacterInfo> characters = new List<CharacterInfo>();
    public List<SKILLS> Skills = new List<SKILLS>();

    public void Reload(){
        instance = new GameInformation() {PlayerName = instance.PlayerName, PlayerSkin = instance.PlayerSkin};
    }
}