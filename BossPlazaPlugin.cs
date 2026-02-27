using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace BossPlaza;

[BepInAutoPlugin(id: "io.github.nov1ce-lee.bossplaza")]
public partial class BossPlazaPlugin : BaseUnityPlugin
{
    // --- 配置参数 ---
    private string triggerSceneName = "Belltown"; // 入口触发场景
    private string plazaSceneName = "Belltown_Shrine"; // 中转广场场景
    
    private Vector3 triggerPosition = new Vector3(68.868f, 7.567f, 0f); // 弹琴位置
    private Vector3 consolePosition = new Vector3(51.5f, 8.5f, 0f);     // 控制台位置
    private Vector3 stagePosition = new Vector3(45f, 8.5f, 0f);       // 舞台位置
    
    private float triggerRadius = 2.0f;
    private float interactRadius = 2.5f;
    private float stageRadius = 3.0f;

    private KeyCode instrumentKey = KeyCode.F8; // 弹琴键
    private KeyCode interactKey = KeyCode.E;    // 交互键 (坐下/确认)

    // --- 状态变量 ---
    private GameObject player;
    private BossData selectedBoss = BossRegistry.act1Bosses[0];
    private bool isTransitioning = false;
    private bool isSitting = false;
    private bool showUI = false;
    private float stageTimer = 0f;
    private const float COUNTDOWN_TIME = 3.0f;

    private void Awake()
    {
        Logger.LogInfo("BossPlaza Loaded");
    }

    void Update()
    {
        if (isTransitioning) return;

        string currentScene = SceneManager.GetActiveScene().name;

        // 1. 在普通场景：检测弹琴进入广场
        if (currentScene == triggerSceneName)
        {
            HandleInstrumentTrigger();
        }
        // 2. 在广场场景：处理坐下、选择和舞台逻辑
        else if (currentScene == plazaSceneName)
        {
            HandlePlazaLogic();
        }
    }

    private void HandleInstrumentTrigger()
    {
        if (player == null) player = GameObject.Find("Hero_Hornet(Clone)");
        if (player == null) return;

        if (Vector3.Distance(player.transform.position, triggerPosition) < triggerRadius)
        {
            if (Input.GetKeyDown(instrumentKey))
            {
                Logger.LogInfo("在指定地点弹琴，准备进入 Boss 广场...");
                StartCoroutine(TeleportToPlaza());
            }
        }
    }

    private void HandlePlazaLogic()
    {
        if (player == null) player = GameObject.Find("Hero_Hornet(Clone)");
        if (player == null) return;

        float distToConsole = Vector3.Distance(player.transform.position, consolePosition);
        float distToStage = Vector3.Distance(player.transform.position, stagePosition);

        // A. 控制台交互 (坐下/打开 UI)
        if (distToConsole < interactRadius && Input.GetKeyDown(interactKey))
        {
            ToggleSit();
        }

        // B. 舞台检测与倒计时
        if (!isSitting && distToStage < stageRadius)
        {
            stageTimer += Time.deltaTime;
            if (stageTimer >= COUNTDOWN_TIME)
            {
                stageTimer = 0;
                StartCoroutine(TeleportToBossRoom(selectedBoss));
            }
        }
        else
        {
            stageTimer = 0;
        }
    }

    private void ToggleSit()
    {
        isSitting = !isSitting;
        showUI = isSitting;
        
        if (HeroController.instance != null)
        {
            if (isSitting)
            {
                HeroController.instance.transform.position = consolePosition;
                // 这里可以播放坐下动画，目前先限制移动
                Logger.LogInfo("坐下，打开 Boss 列表...");
            }
            else
            {
                Logger.LogInfo("站起，关闭 Boss 列表。");
            }
        }
    }

    private void OnGUI()
    {
        if (!showUI) return;

        GUI.Box(new Rect(10, 10, 300, 500), "Boss 广场 - 展览框");

        for (int i = 0; i < BossRegistry.act1Bosses.Count; i++)
        {
            var boss = BossRegistry.act1Bosses[i];
            if (GUI.Button(new Rect(20, 40 + (i * 35), 280, 30), 
                (selectedBoss == boss ? "> " : "") + boss.Name))
            {
                selectedBoss = boss;
                Logger.LogInfo($"已选择 Boss: {boss.Name}");
            }
        }

        if (GUI.Button(new Rect(20, 460, 280, 30), "确认并离开"))
        {
            ToggleSit();
        }
    }

    private IEnumerator TeleportToPlaza()
    {
        isTransitioning = true;
        GameManager.instance.BeginSceneTransition(new GameManager.SceneLoadInfo
        {
            SceneName = plazaSceneName,
            EntryGateName = "left1", // 广场默认入口
            PreventCameraFadeOut = false,
            WaitForSceneTransitionCameraFade = true,
            Visualization = GameManager.SceneLoadVisualizations.Default
        });
        yield return new WaitForSeconds(1f);
        isTransitioning = false;
    }

    private IEnumerator TeleportToBossRoom(BossData boss)
    {
        if (boss == null || isTransitioning) yield break;
        isTransitioning = true;

        Logger.LogInfo($"[舞台准备就绪] 正在传送到: {boss.Name}");

        GameManager.instance.BeginSceneTransition(new GameManager.SceneLoadInfo
        {
            SceneName = boss.SceneName,
            EntryGateName = boss.GateName ?? "left1",
            PreventCameraFadeOut = false,
            WaitForSceneTransitionCameraFade = true,
            Visualization = GameManager.SceneLoadVisualizations.Default
        });

        // 如果是自定义位置，需要等待场景加载
        if (boss.CustomPosition.HasValue)
        {
            while (SceneManager.GetActiveScene().name != boss.SceneName)
                yield return null;
            
            yield return new WaitForSeconds(0.3f);
            if (HeroController.instance != null)
            {
                HeroController.instance.transform.position = boss.CustomPosition.Value;
            }
        }

        isTransitioning = false;
    }
}