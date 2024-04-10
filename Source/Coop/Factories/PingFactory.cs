using System;
using EFT;
using EFT.Interactive;
using StayInTarkov.Bundles;
using StayInTarkov.Configuration;
using UnityEngine;
using UnityEngine.UI;

namespace StayInTarkov.Coop.Factories
{
	public static class PingFactory
	{
		public static PingFactory.AbstractPing FromPingType(PingFactory.EPingType type, GameObject gameObject)
		{
			if (!true)
			{
			}
			PingFactory.AbstractPing abstractPing;
			switch (type)
			{
			case PingFactory.EPingType.Point:
				abstractPing = gameObject.AddComponent<PingFactory.PointPing>();
				break;
			case PingFactory.EPingType.Player:
				abstractPing = gameObject.AddComponent<PingFactory.PlayerPing>();
				break;
			case PingFactory.EPingType.DeadBody:
				abstractPing = gameObject.AddComponent<PingFactory.DeadBodyPing>();
				break;
			case PingFactory.EPingType.LootItem:
				abstractPing = gameObject.AddComponent<PingFactory.LootItemPing>();
				break;
			case PingFactory.EPingType.LootContainer:
				abstractPing = gameObject.AddComponent<PingFactory.LootContainerPing>();
				break;
			case PingFactory.EPingType.Door:
				abstractPing = gameObject.AddComponent<PingFactory.DoorPing>();
				break;
			case PingFactory.EPingType.Interactable:
				abstractPing = gameObject.AddComponent<PingFactory.InteractablePing>();
				break;
			default:
				abstractPing = null;
				break;
			}
			if (!true)
			{
			}
			return abstractPing;
		}

		// Token: 0x020001D1 RID: 465
		public enum EPingType : byte
		{
			Point,
			Player,
			DeadBody,
			LootItem,
			LootContainer,
			Door,
			Interactable
		}

		public abstract class AbstractPing : MonoBehaviour
		{
			private void Awake()
			{
				this.image = base.GetComponentInChildren<Image>();
				this.image.color = Color.clear;
				Destroy(base.gameObject, 3f);
			}

			private void Update()
			{
				bool flag = FPSCamera.Instance.OpticCameraManager.Boolean_0 && FPSCamera.Instance.OpticCameraManager.CurrentOpticSight != null;
				if (flag)
				{
					this.image.color = Color.clear;
				}
				else
				{
					Camera camera = FPSCamera.Instance.Camera;
					bool flag2 = FPSCamera.Instance.SSAA != null && FPSCamera.Instance.SSAA.isActiveAndEnabled;
					if (flag2)
					{
						this.screenScale = (float)FPSCamera.Instance.SSAA.GetOutputWidth() / (float)FPSCamera.Instance.SSAA.GetInputWidth();
					}
					Vector3 vector = camera.WorldToScreenPoint(this.hitPoint);
					bool flag3 = vector.z > 0f;
					if (flag3)
					{
						float num = Vector3.Distance(vector, new Vector3((float)Screen.width, (float)Screen.height, 0f) / 2f);
						bool flag4 = num < 200f;
						if (flag4)
						{
							this.image.color = new Color(this._pingColor.r, this._pingColor.g, this._pingColor.b, Mathf.Max(0.05f, num / 200f));
						}
						else
						{
							this.image.color = this._pingColor;
						}
						this.image.transform.position = vector * this.screenScale;
					}
				}
			}

			public virtual void Initialize(ref Vector3 point, object userObject, Color pingColor)
			{
				this.hitPoint = point;
				base.transform.position = point;
				this._pingColor = pingColor;
				float num = Mathf.Clamp(Vector3.Distance(FPSCamera.Instance.Camera.transform.position, base.transform.position) / 100f, 0.4f, 0.6f);
				float value = PluginConfigSettings.Instance.CoopSettings.PingSize;
				this.image.rectTransform.localScale = new Vector3(value, value, value) * num;
			}

			internal static readonly AssetBundle pingBundle = InternalBundleLoader.Instance.GetAssetBundle("ping");

			protected Image image;

			protected Vector3 hitPoint;

			private float screenScale = 1f;

			private Color _pingColor = Color.white;
		}

		public class InteractablePing : PingFactory.AbstractPing
		{
			public override void Initialize(ref Vector3 point, object userObject, Color pingColor)
			{
				base.Initialize(ref point, userObject, pingColor);
				this.image.sprite = PingFactory.AbstractPing.pingBundle.LoadAsset<Sprite>("PingPoint");
			}
		}

		public class PlayerPing : PingFactory.AbstractPing
		{
			public override void Initialize(ref Vector3 point, object userObject, Color pingColor)
			{
				base.Initialize(ref point, userObject, pingColor);
				EFT.Player player = userObject as EFT.Player;
				this.image.sprite = PingFactory.AbstractPing.pingBundle.LoadAsset<Sprite>("PingPlayer");
			}
		}

		public class LootContainerPing : PingFactory.AbstractPing
		{
			public override void Initialize(ref Vector3 point, object userObject, Color pingColor)
			{
				base.Initialize(ref point, userObject, pingColor);
				LootableContainer lootableContainer = userObject as LootableContainer;
				this.image.sprite = PingFactory.AbstractPing.pingBundle.LoadAsset<Sprite>("PingLootableContainer");
			}
		}

		public class DoorPing : PingFactory.AbstractPing
		{
			public override void Initialize(ref Vector3 point, object userObject, Color pingColor)
			{
				base.Initialize(ref point, userObject, pingColor);
				this.image.sprite = PingFactory.AbstractPing.pingBundle.LoadAsset<Sprite>("PingDoor");
			}
		}

		public class PointPing : PingFactory.AbstractPing
		{
			public override void Initialize(ref Vector3 point, object userObject, Color pingColor)
			{
				base.Initialize(ref point, userObject, pingColor);
				this.image.sprite = PingFactory.AbstractPing.pingBundle.LoadAsset<Sprite>("PingPoint");
			}
		}

		public class DeadBodyPing : PingFactory.AbstractPing
		{
			public override void Initialize(ref Vector3 point, object userObject, Color pingColor)
			{
				base.Initialize(ref point, userObject, Color.white);
				base.transform.localScale *= 0.5f;
				this.image.sprite = PingFactory.AbstractPing.pingBundle.LoadAsset<Sprite>("PingDeadBody");
			}
		}

		public class LootItemPing : PingFactory.AbstractPing
		{
			public override void Initialize(ref Vector3 point, object userObject, Color pingColor)
			{
				base.Initialize(ref point, userObject, pingColor);
				LootItem lootItem = userObject as LootItem;
				this.image.sprite = PingFactory.AbstractPing.pingBundle.LoadAsset<Sprite>("PingLootItem");
			}
		}
	}
}
