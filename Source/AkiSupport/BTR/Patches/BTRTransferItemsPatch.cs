﻿using Aki.Custom.BTR.Utils;
using EFT.UI;
using HarmonyLib;
using StayInTarkov;
using StayInTarkov.AkiSupport.Singleplayer.Utils.TraderServices;
using System.Reflection;

namespace Aki.Custom.BTR.Patches
{
    public class BTRTransferItemsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {

            return AccessTools.Method(typeof(TransferItemsInRaidScreen), nameof(TransferItemsInRaidScreen.Close));
        }

        [PatchPostfix]
        private static void PatchPostfix(bool ___bool_1)
        {
            // Didn't extract items
            if (!___bool_1)
            {
                return;
            }

            // Update the trader services information now that we've used this service
            TraderServicesManager.Instance.GetTraderServicesDataFromServer(BTRUtil.BTRTraderId);
        }
    }
}
