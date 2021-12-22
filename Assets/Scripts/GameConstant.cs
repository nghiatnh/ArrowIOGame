using System;
using System.Collections.Generic;

public class GameConstant{
    public const float INIT_ATK = 12;
    public static readonly int[] MAX_EXP_LEVEL = {0, 20, 35, 60, 100, 150, 180, 220, 300, 350, 450, 550, 700};
    public static readonly Array LIST_SKILL = Enum.GetValues(typeof(SKILLS));
    public static readonly Dictionary<SKILLS, string> SKILL_DESCRIPTIONS = new Dictionary<SKILLS, string>(){
        {SKILLS.MAGNET, "You will get more DIAMOND around you!"},
        {SKILLS.RANGE_UP, "You RANGE is extended!"},
        {SKILLS.CRIT, "You will take more DAMAGE during attacking!"},
        {SKILLS.BLOOD_SUCKING, "You will get HEALTH when you attack accurately!"},
        {SKILLS.ALL_STATUS_UP, "Your ATK, DEF and SPEED are increased!"},
        {SKILLS.THROUGH_WALL, "You can THROUGH the wall now!"},
        {SKILLS.MORE_EXP, "You will get more EXP when killing an enemy!"},
        {SKILLS.ATK_UP, "Your ATK increased!"},
        {SKILLS.ATK_UP_HP_DOWN, "Your ATK increased and you HEALTH descreased!"},
        {SKILLS.DEF_UP, "Your DEF increased!"},
        {SKILLS.HP_UP, "Your HEALTH increased!"},
        {SKILLS.SPEED_UP, "Your SPEED increased!"},
        {SKILLS.BULLET_UP, "You will take more BULLET!"},
        {SKILLS.KILL_HP_UP, "You will get HEALTH when you kill an enemy!"},
        {SKILLS.BULLET_THROUGH_WALL, "Your BULLET can THROUGH the wall now!"},
        {SKILLS.MAKE_SPIKE, "You will make spike while moving!"},
    };
}

public enum SKILLS{
    MAGNET,
    RANGE_UP,
    CRIT,
    BLOOD_SUCKING,
    ALL_STATUS_UP,
    THROUGH_WALL,
    MORE_EXP,
    ATK_UP,
    ATK_UP_HP_DOWN,
    DEF_UP,
    HP_UP,
    SPEED_UP,
    BULLET_UP,
    KILL_HP_UP,
    BULLET_THROUGH_WALL,
    MAKE_SPIKE,
}

public enum STATUS
{
    CRIT,
    BLOOD_SUCKING,
    KILL_HP_UP,
    BULLET_THROUGH_WALL,
    BULLET_UP,
    MAKE_SPIKE,
    MORE_EXP,
    THROUGH_WALL,
}
