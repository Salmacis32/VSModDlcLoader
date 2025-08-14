using HarmonyLib;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppSystem.Reflection;
using Il2CppVampireSurvivors.App.Data;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Weapons;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.DLC;
using Il2CppVampireSurvivors.Objects.Projectiles;
using Il2CppVampireSurvivors.Objects.Weapons;
using Il2CppZenject;
using TestVSMod.Factories;
using UnityEngine;
using Il2Col = Il2CppSystem.Collections.Generic;

namespace TestVSMod.Patches
{
    [HarmonyPatch(typeof(GameManager))]
    public static class GameManagerPatches
    {
        public static WeaponFactory WeaponFactory;
        public static Projectile Prefab;
        [HarmonyPatch(nameof(GameManager.InitializeGameSessionPostLoad))]
        [HarmonyPostfix]
        public static void Load(GameManager __instance)
        {
            if (WeaponFactory == null) WeaponFactory = __instance.WeaponsFacade._weaponFactory;
            if (Prefab == null) Prefab = __instance.ProjectileFactory.GetProjectilePrefab(WeaponType.AXE);
            foreach (var item in Core.ModdedWeaponInfo) {
                WeaponFactory.GetWeaponPrefab(item.IdAsType, out WeaponType dead)._ProjectilePrefab = Prefab;
            }
        }
    }
}