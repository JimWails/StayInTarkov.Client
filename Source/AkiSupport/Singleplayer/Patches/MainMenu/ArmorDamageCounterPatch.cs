using EFT;
using EFT.InventoryLogic;
using HarmonyLib;
using StayInTarkov;
using System;
using System.Reflection;

namespace StayInTarkov.AkiSupport.Singleplayer.Patches.MainMenu
{
    /// <summary>
    /// Created by: SPT-Aki team
    /// Link: https://dev.sp-tarkov.com/SPT-AKI/Modules/src/branch/3.8.0/project/Aki.SinglePlayer/Patches/MainMenu/ArmorDamageCounterPatch.cs
    /// Modified by: KWJimWails. Modified to use SIT ModulePatch
    /// </summary>
    public class ArmorDamageCounterPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player), nameof(Player.ApplyDamageInfo));
        }

        [PatchPostfix]
        private static void PatchPostfix(DamageInfo damageInfo)
        {
            if (damageInfo.Player == null)
            {
                return;
            }

            if (damageInfo.Player.iPlayer == null)
            {
                return;
            }

            if (damageInfo.Player.iPlayer.IsYourPlayer == null)
            {
                return;
            }

            if (!damageInfo.Player.iPlayer.IsYourPlayer)
            {
                return;
            }

            if (damageInfo.Weapon is Weapon weapon && weapon.Chambers[0].ContainedItem is BulletClass bullet)
            {
                float newDamage = (float)Math.Round(bullet.Damage - damageInfo.Damage);
                damageInfo.Player.iPlayer.Profile.EftStats.SessionCounters.AddFloat(newDamage, GClass2200.CauseArmorDamage);
            }
        }
    }
}
