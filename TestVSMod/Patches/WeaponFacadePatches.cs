using HarmonyLib;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects.Characters;
using Il2CppVampireSurvivors.Objects.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVSMod.Patches
{
    [HarmonyPatch(typeof(WeaponsFacade))]
    [HarmonyWrapSafe]
    static class PatchWeaponFacade
    {
        [HarmonyPatch(nameof(WeaponsFacade.AddWeapon))]
        [HarmonyPrefix]
        static void What(WeaponsFacade __instance, Weapon __result, WeaponType weaponType, CharacterController character, bool removeFromStore)
        {
            var test = __instance;
            return;
        }

        [HarmonyPatch(nameof(WeaponsFacade.AddWeapon))]
        [HarmonyPostfix]
        static void TheDuece(WeaponsFacade __instance, Weapon __result, WeaponType weaponType, CharacterController character, bool removeFromStore)
        {
        }
    }
}
