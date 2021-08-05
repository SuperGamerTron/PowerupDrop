using TMPro;
using UnityEngine;

namespace PowerupDrop
{
    public class Packets
    {
        public static void ClientSendCanPowerupDrop()
        {
            using (Packet packet = new Packet(203))
            {
                ClientSend.SendTCPData(packet);
            }
        }

        public static void ServerHandleCanPowerupDrop(int fromClient, Packet packet)
        {
            PowerupDrop.playersWithMod.Add(fromClient);
        }

        public static void ClientSendPowerupDrop(Powerup powerup)
        {
            using (Packet packet = new Packet(204))
            {
                packet.Write(powerup.name);
                packet.Write(powerup.id);
                packet.Write(PlayerStatus.Instance.transform.position);
                ClientSend.SendTCPData(packet);
            }
        }

        public static void ServerHandlePowerupDrop(int fromClient, Packet packet)
        {
            string powerupName = packet.ReadString();
            int powerupId = packet.ReadInt();
            Vector3 position = packet.ReadVector3();
            ServerSendPowerupDrop(fromClient, powerupName, powerupId, position);
        }

        public static void ServerSendPowerupDrop(int toClient, string powerupName, int powerupId, Vector3 position)
        {
            if (PowerupDrop.playersWithMod.Count >= GameManager.players.Count)
            {
                using (Packet packet = new Packet(205))
                {
                    packet.Write(powerupName);
                    packet.Write(powerupId);
                    ServerSend.SendTCPData(toClient, packet);
                }
                int objectID = ItemManager.Instance.GetNextId();
                ItemManager.Instance.DropPowerupAtPosition(powerupId, position, objectID);
                ServerSend.DropPowerupAtPosition(powerupId, objectID, position);
            }
        }

        public static void ClientHandlePowerupDrop(Packet packet)
        {
            string powerupName = packet.ReadString();
            int powerupId = packet.ReadInt();
            GameObject powerupObject = PowerupUI.Instance.powerups[powerupId];
            TextMeshProUGUI componentInChildren = powerupObject.GetComponentInChildren<TextMeshProUGUI>();
            int num = int.Parse(componentInChildren.text);
            num--;
            componentInChildren.text = string.Concat(num);
            PowerupInventory.Instance.powerups[powerupId]--;
            PlayerStatus.Instance.UpdateStats();
            if (PlayerStatus.Instance.hp > PlayerStatus.Instance.maxHp)
            {
                PlayerStatus.Instance.hp = PlayerStatus.Instance.maxHp;
            }
            if (PlayerStatus.Instance.shield > PlayerStatus.Instance.maxShield)
            {
                PlayerStatus.Instance.shield = PlayerStatus.Instance.maxShield;
            }
            string colorName = ItemManager.Instance.allPowerups[powerupId].GetColorName();
            ChatBox.Instance.SendMessage($"Dropped <color={colorName}>({powerupName})<color=white>");
            if (num == 0)
            {
                Object.Destroy(powerupObject);
                PowerupUI.Instance.powerups.Remove(powerupId);
                ItemInfo.Instance.Fade(0f, 0.2f);
            }
        }
    }
}
