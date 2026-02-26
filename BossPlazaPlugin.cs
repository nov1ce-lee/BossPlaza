using BepInEx;

namespace BossPlaza;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.nov1ce-lee.bossplaza")]
public partial class BossPlazaPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Put your initialization logic here
        Logger.LogInfo($"Plugin {Name} ({Id}) has loaded!");
    }
}
