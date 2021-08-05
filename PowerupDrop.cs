using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PowerupDrop
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess("Muck.exe")]
    public class PowerupDrop : BaseUnityPlugin
    {
        public const string
            GUID = "PowerupDrop",
            NAME = "PowerupDrop",
            VERSION = "1.0.0";

        public static List<int> playersWithMod = new List<int>();
        public static bool hasMod = Directory.GetFiles(Directory.GetCurrentDirectory(), "PowerupDrop.dll", SearchOption.AllDirectories).FirstOrDefault() != default;

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(MuckPatch));
            Logger.LogMessage("Loaded PowerupDrop");
        }
    }
}
