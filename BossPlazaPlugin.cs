using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossPlaza;

[BepInAutoPlugin(id: "io.github.nov1ce-lee.bossplaza")]
public partial class BossPlazaPlugin : BaseUnityPlugin
{
    private string triggerSceneName = "Town";        // 触发所在场景名
    private string targetSceneName = "BossRoom01";   // 传送目标场景名
    
    private Vector3 triggerPosition = new Vector3(10f, 5f, 0f); // 触发中心点
    private float triggerRadius = 2.0f; // 触发范围半径
    
    private KeyCode triggerKey = KeyCode.F; // 模拟“弹琴”按键
    private GameObject player;

    private void Awake()
    {
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
    }

    void Update()
    {
        // 1. 场景检查
        if (SceneManager.GetActiveScene().name != triggerSceneName)
            return;

        // 2. 找玩家对象（根据游戏实际名称修改）
        if (player == null)
        {
            player = GameObject.FindWithTag("Hero");
            if (player == null)
                return;
        }

        // 3. 检查玩家是否在指定范围
        float distance = Vector3.Distance(player.transform.position, triggerPosition);
        if (distance > triggerRadius)
            return;

        // 4. 检测按键（模拟弹琴）
        if (Input.GetKeyDown(triggerKey))
        {
            TeleportToBossRoom();
        }
    }

    private void TeleportToBossRoom()
    {
        Logger.LogInfo("触发传送到 Boss 房间！");
        SceneManager.LoadScene(targetSceneName);
    }
}
