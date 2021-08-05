using UnityEngine;
using UnityEngine.EventSystems;

namespace PowerupDrop
{
	public class ClickablePowerupInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IEventSystemHandler
	{
		public Powerup powerup { get; set; }

		public void OnPointerEnter(PointerEventData eventData)
		{
			ItemInfo.Instance.SetText(powerup.name + "\n<size=50%><i>" + powerup.description, true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			ItemInfo.Instance.Fade(0f, 0.2f);
		}

		public void OnPointerClick(PointerEventData eventData)
        {
			Packets.ClientSendPowerupDrop(powerup);
		}
	}
}
