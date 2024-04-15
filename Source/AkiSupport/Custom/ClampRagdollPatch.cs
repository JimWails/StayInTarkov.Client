using EFT.Interactive;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace StayInTarkov.AkiSupport.Custom
{
    /// <summary>
    /// Created by: SPT-Aki team
    /// Link: https://dev.sp-tarkov.com/SPT-AKI/Modules/src/branch/master/project/Aki.Custom/Patches/ClampRagdollPatch.cs
    /// </summary>
    public class ClampRagdollPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Corpse), nameof(Corpse.method_16));
        }

        [PatchPrefix]
        private static void PatchPreFix(ref Vector3 velocity)
        {
            velocity.y = Mathf.Clamp(velocity.y, -1f, 1f);
        }
    }
}
