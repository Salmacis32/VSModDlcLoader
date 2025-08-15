﻿using HarmonyLib;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects.Projectiles;
using vsML.Models;

namespace vsML.Patches
{
    /// <summary>
    /// Harmony patches for the GameManager class
    /// </summary>
    [HarmonyPatch(typeof(GameManager))]
    public static class GameManagerPatches
    {
        public static WeaponFactory WeaponFactory;
        public static Projectile Prefab;

        public static void Deinitialize()
        {
            WeaponFactory = null;
            Prefab = null;
        }

        /// <summary>
        /// Patch to load custom content into a game session
        /// </summary>
        /// <remarks>
        /// Currently I have to use this to properly grab a Projectile prefab, as I still need to figure out how to create one from scratch.
        /// </remarks>
        [HarmonyPatch(nameof(GameManager.InitializeGameSessionPostLoad))]
        [HarmonyPostfix]
        public static void Load(GameManager __instance)
        {
            if (WeaponFactory == null) WeaponFactory = __instance.WeaponsFacade._weaponFactory;
            if (Prefab == null) Prefab = __instance.ProjectileFactory.GetProjectilePrefab(WeaponType.AXE);
            foreach (var item in vsMLCore.CustomWeapons) {
                WeaponFactory.GetWeaponPrefab(item.IdAsType, out WeaponType dead)._ProjectilePrefab = Prefab;
            }
            if (ProjectilePatches.ModPool == null) ProjectilePatches.ModPool = new ModProjectile[50, 500];
        }

        /// <summary>
        /// Cleanup for custom content
        /// </summary>
        /// <remarks>
        /// There might be more places that should be hooked for this, but this seemed safe enough for the time being.
        /// </remarks>
        [HarmonyPatch(nameof(GameManager.ResetGameSession))]
        [HarmonyPrefix]
        public static void ResetGameSession(GameManager __instance)
        {
            ResetCustomContent();
        }

        private static void ResetCustomContent()
        {
            Array.Clear(ProjectilePatches.ModPool);
            Prefab = null;
            WeaponFactory = null;
        }
    }
}