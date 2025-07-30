using HarmonyLib;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.DLC;
using Il2CppVampireSurvivors.Objects.Weapons;
using TestVSMod.Util;
using Il2Col = Il2CppSystem.Collections.Generic;

namespace TestVSMod.Patches
{
    [HarmonyPatch(typeof(WeaponFactory))]
    public static class WeaponFactoryPatches
    {
        /*
        private static Weapon GetWeaponPrefab(WeaponFactory instance, WeaponType weaponType, out WeaponType forcedWeaponType)
        {
            var weaponList = new Dictionary<WeaponType, Weapon>();
            forcedWeaponType = weaponType;
            foreach (var weapon in instance._weapons)
            {
                weaponList.Add(weapon.Key, weapon.Value);
            }
            foreach (var factory in instance._LinkedFactories)
            {
                foreach (var weapon in factory._weapons)
                {
                    weaponList.Add(weapon.Key, weapon.Value);
                }
            }
            foreach (var dlc in DlcSystem.LoadedDlc)
            {
                foreach (var weapon in dlc.Value._WeaponFactory._weapons)
                {
                    weaponList.Add(weapon.Key, weapon.Value);
                }
            }

            if (!weaponList.ContainsKey(forcedWeaponType)) return null;

            return weaponList[forcedWeaponType];
        }
        */

        [HarmonyPatch(nameof(WeaponFactory.GetWeaponPrefab))]
        [HarmonyPrefix]
        private static void Prefab(WeaponFactory __instance, WeaponType weaponType, WeaponType forcedWeaponType, Weapon __result)
        {
            
        }

        [HarmonyPatch(nameof(WeaponFactory.GetWeaponPrefab))]
        [HarmonyPostfix]
        private static void PrefabPost(WeaponFactory __instance, WeaponType weaponType, WeaponType forcedWeaponType, ref Weapon __result)
        {
            if (!Core.Il2CppModdedWeaponInfo.ContainsIl2CppKey(weaponType)) return;
            var axePrefab = GM.Core.ProjectileFactory.GetProjectilePrefab(WeaponType.AXE);
            if (axePrefab == null) return;
            __result._ProjectilePrefab = axePrefab;
        }
    }
}