using HarmonyLib;
using UnityEngine;

namespace PowerupDrop
{
    public class MuckPatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(Server), nameof(Server.InitializeServerPackets))]
        public static void InitializeServerPackets()
        {
            Server.PacketHandlers.Add(203, new Server.PacketHandler(Packets.ServerHandleCanPowerupDrop));
            Server.PacketHandlers.Add(204, new Server.PacketHandler(Packets.ServerHandlePowerupDrop));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LocalClient), nameof(LocalClient.InitializeClientData))]
        public static void InitializeClientData()
        {
            LocalClient.packetHandlers.Add(205, new LocalClient.PacketHandler(Packets.ClientHandlePowerupDrop));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static void OnStartGame()
        {
            Packets.ClientSendCanPowerupDrop();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PowerupUI), nameof(PowerupUI.AddPowerup))]
        public static void OnAddPowerup(ref int powerupId)
        {
            if (PowerupUI.Instance.powerups[powerupId].GetComponent<ClickablePowerupInfo>() == null)
            {
                GameObject powerup = PowerupUI.Instance.powerups[powerupId];
                Object.Destroy(powerup.GetComponent<PowerupInfo>());
                ClickablePowerupInfo powerupInfo = powerup.AddComponent<ClickablePowerupInfo>();
                powerupInfo.powerup = ItemManager.Instance.allPowerups[powerupId];    
            }
        }
    }
}
