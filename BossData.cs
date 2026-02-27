using System.Collections.Generic;
using UnityEngine;

namespace BossPlaza;

public class BossData 
{ 
    public string Name; 
    public string SceneName; 
    public string GateName; 
    public string PlayerDataFlag; 
    public string[] AdditionalFlags; 
    public Vector3? CustomPosition; 

    public BossData(string name, string scene, string gate, string flag, params string[] additionalFlags) 
    { 
        Name = name; 
        SceneName = scene; 
        GateName = gate; 
        PlayerDataFlag = flag; 
        AdditionalFlags = additionalFlags; 
        CustomPosition = null; 
    } 

    public BossData(string name, string scene, Vector3 pos, string flag, params string[] additionalFlags) 
    { 
        Name = name; 
        SceneName = scene; 
        GateName = null; 
        PlayerDataFlag = flag; 
        AdditionalFlags = additionalFlags; 
        CustomPosition = pos; 
    } 
} 

public static class BossRegistry
{
    public static readonly List<BossData> act1Bosses = new List<BossData> 
    { 
        new BossData("Moss Mother", "Tut_03", "right1", "defeatedMossMother"),//good 
        new BossData("Bell Beast", "Bone_05", new Vector3(83.40305f,3.567686f,0.004f), "defeatedBellBeast"),//good 
        new BossData("Lace", "Bone_East_12", new Vector3(92.13885f,7.567686f,0.004f), "defeatedLace1", "laceLeftDocks"),//good 
        new BossData("Fourth Chorus", "Bone_East_08",new Vector3(82.46f,12.26745f,0.004f), "defeatedFourthChorus"),// good 
        new BossData("Savage Beastfly", "Ant_19", new Vector3(50.61314f,34.56768f,0.004f), "defeatedSavageBeastfly"),//good 
        new BossData("Sister Splinter", "Shellwood_18", new Vector3(55.44654f,8.567684f,0.004f), "defeatedSplinterQueen"),//good 
        new BossData("Skull Tyrant", "Bone_15", new Vector3(74.93963f,14.56769f,0.004f), "skullKingDefeated"),//good 
        new BossData("moorwing", "Greymoor_08", new Vector3(37.91465f, 4.567685f, 0.004f), ""),//check 
        new BossData("Widow", "Belltown_Shrine", new Vector3(51.51193f,8.567684f,0.004f), "hasNeedolin"),//good 
        new BossData("MossEvolver", "Weave_03", new Vector3(32.89833f,20.56768f,0.004f), "defeatedMossEvolver"),//good 
        new BossData("Great Conchflies", "Coral_11", new Vector3(54.95525f, 14.56769f, 0.004f), "defeatedCoralDrillers"),//good 
        new BossData("Phantom", "Organ_01", new Vector3(83.79173f,104.5677f,0.004f), "defeatedPhantom"),//good 
        new BossData("Last Judge", "Coral_Judge_Arena", new Vector3(45.96228f,24.56768f,0.004f), "defeatedLastJudge"),//good 
    }; 
}
