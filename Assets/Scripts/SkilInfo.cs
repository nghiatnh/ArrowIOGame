using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkilInfo : ScriptableObject
{
    public string SkillName;
    public string Description;
    public SKILLS SkillTag;
    public Sprite SkillImage;
}
