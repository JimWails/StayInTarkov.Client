using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace StayInTarkov.Bundles
{
	public class InternalBundleLoader
	{
		public static InternalBundleLoader Instance { get; private set; }

		public void Create()
		{
			InternalBundleLoader.Instance = this;
			this.Awake();
		}

		private void Awake()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			this._loadedBundles = new Dictionary<string, AssetBundleCreateRequest>();
			assembly.GetManifestResourceNames().ToList<string>().ForEach(delegate(string name)
			{
				using (Stream manifestResourceStream = assembly.GetManifestResourceStream(name))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						string text = name.Replace("StayInTarkov.Bundles.Files.", "").Replace(".bundle", "");
						manifestResourceStream.CopyTo(memoryStream);
						this._loadedBundles.Add(text, AssetBundle.LoadFromMemoryAsync(memoryStream.ToArray()));
					}
				}
			});
		}

		public AssetBundle GetAssetBundle(string bundleName)
		{
			AssetBundleCreateRequest assetBundleCreateRequest;
			bool flag = this._loadedBundles.TryGetValue(bundleName, out assetBundleCreateRequest) && assetBundleCreateRequest.isDone;
			AssetBundle assetBundle;
			if (flag)
			{
				assetBundle = assetBundleCreateRequest.assetBundle;
			}
			else
			{
				assetBundle = null;
			}
			return assetBundle;
		}

		public Dictionary<string, AssetBundleCreateRequest> _loadedBundles;
	}
}
