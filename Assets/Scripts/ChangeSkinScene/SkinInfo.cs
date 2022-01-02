using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skin")]
public class SkinInfo : ScriptableObject
{
    public string Name;
    public string Description;
    public GameObject SkinObject;
    public SkilInfo[] Skills = new SkilInfo[3];
}
