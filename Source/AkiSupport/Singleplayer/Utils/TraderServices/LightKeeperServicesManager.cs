using BepInEx.Logging;
using Comfort.Common;
using EFT;
using EFT.Weather;
using StayInTarkov.Coop.Components.CoopGameComponents;
using StayInTarkov.Coop.Matchmaker;
using StayInTarkov.Coop.SITGameModes;
using System.Numerics;
using UnityEngine;

namespace StayInTarkov.AkiSupport.Singleplayer.Utils.TraderServices
{
    /// <summary>
    /// Created by: SPT-Aki team
    /// Link: https://dev.sp-tarkov.com/SPT-AKI/Modules/src/branch/3.8.0/project/Aki.SinglePlayer/Utils/TraderServices/LightKeeperServicesManager.cs
    /// Modified by: KWJimWails. Modified to use SIT ModulePatch
    /// </summary>
    internal class LightKeeperServicesManager : MonoBehaviour
    {
        private static ManualLogSource logger;
        GameWorld gameWorld;
        BotsController botsController;
        private bool hasClientInit = false;

        private void Awake()
        {
            logger = BepInEx.Logging.Logger.CreateLogSource(nameof(LightKeeperServicesManager));
            if (SITMatchmaking.IsServer) Singleton<LightKeeperServicesManager>.Create(this);

            logger.LogInfo("[AKI-LKS] GameWorld loading");
            gameWorld = Singleton<GameWorld>.Instance;
            if (gameWorld == null || TraderServicesManager.Instance == null)
            {
                logger.LogError("[AKI-LKS] GameWorld or TraderServices null");
                Destroy(this);
                return;
            }

            //if (SITMatchmaking.IsClient)
            //{
            //    logger.LogError("[AKI-LKS] Your are client, wait for server load manager.");
            //    Destroy(this);
            //    return;
            //}

            logger.LogInfo("[AKI-LKS] botsController loading");
            botsController = Singleton<IBotGame>.Instance?.BotsController;
            logger.LogInfo("[AKI-LKS] botsController loaded");
            if (botsController == null)
            {
                logger.LogError("[AKI-LKS] BotsController null");
                if (SITMatchmaking.IsServer) Destroy(this);
                return;
            }

            logger.LogInfo("[AKI-LKS] OnTraderServicePurchased");
            TraderServicesManager.Instance.OnTraderServicePurchased += OnTraderServicePurchased;
            logger.LogInfo("[AKI-LKS] LightKeeperServicesManager.Awake");
        }

        public void FixedUpdate()
        {
            if (SITMatchmaking.IsServer) return;

            if (!hasClientInit)
            {
                logger.LogInfo("[AKI-LKS] Client.GameWorld loading");
                gameWorld = Singleton<GameWorld>.Instance;
                if (gameWorld == null || TraderServicesManager.Instance == null)
                {
                    logger.LogError("[AKI-LKS] Client.GameWorld or TraderServices null");
                    return;
                }

                //if (SITMatchmaking.IsClient)
                //{
                //    logger.LogError("[AKI-LKS] Your are client, wait for server load manager.");
                //    Destroy(this);
                //    return;
                //}

                logger.LogInfo("[AKI-LKS] Client.botsController loading");
                botsController = (BotsController)ReflectionHelpers.GetFieldFromTypeByFieldType(typeof(BaseLocalGame<GamePlayerOwner>), typeof(BotsController)).GetValue(Singleton<ISITGame>.Instance);
                //botsController = Singleton<IBotGame>.Instance?.BotsController;
                logger.LogInfo("[AKI-LKS] Client.botsController loaded");
                if (botsController == null)
                {
                    logger.LogError("[AKI-LKS] Client.BotsController null");
                    return;
                }

                logger.LogInfo("[AKI-LKS] Client.OnTraderServicePurchased");
                TraderServicesManager.Instance.OnTraderServicePurchased += OnTraderServicePurchased;
                logger.LogInfo("[AKI-LKS] Client.LightKeeperServicesManager.Awake");

                hasClientInit = true;
            }
        }

        private void OnTraderServicePurchased(ETraderServiceType serviceType, string subserviceId)
        {
            switch (serviceType)
            {
                case ETraderServiceType.ExUsecLoyalty:
                    botsController.BotTradersServices.LighthouseKeeperServices.OnFriendlyExUsecPurchased(gameWorld.MainPlayer);
                    break;
                case ETraderServiceType.ZryachiyAid:
                    botsController.BotTradersServices.LighthouseKeeperServices.OnFriendlyZryachiyPurchased(gameWorld.MainPlayer);
                    break;
            }
        }

        private void OnDestroy()
        {
            if (gameWorld == null || botsController == null)
            {
                return;
            }

            if (TraderServicesManager.Instance != null)
            {
                TraderServicesManager.Instance.OnTraderServicePurchased -= OnTraderServicePurchased;
            }
        }
    }
}
