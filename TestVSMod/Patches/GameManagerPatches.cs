using HarmonyLib;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects.Projectiles;
using TestVSMod.Models;

namespace TestVSMod.Patches
{
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

        [HarmonyPatch(nameof(GameManager.InitializeGameSessionPostLoad))]
        [HarmonyPostfix]
        public static void Load(GameManager __instance)
        {
            if (WeaponFactory == null) WeaponFactory = __instance.WeaponsFacade._weaponFactory;
            if (Prefab == null) Prefab = __instance.ProjectileFactory.GetProjectilePrefab(WeaponType.AXE);
            foreach (var item in Core.ModdedWeaponInfo) {
                WeaponFactory.GetWeaponPrefab(item.IdAsType, out WeaponType dead)._ProjectilePrefab = Prefab;
            }
            if (ProjectilePatches.ModPool == null) ProjectilePatches.ModPool = new ModProjectile[50, 500];
        }

        [HarmonyPatch(nameof(GameManager.ResetGameSession))]
        [HarmonyPrefix]
        public static void ResetGameSession(GameManager __instance)
        {
            Array.Clear(ProjectilePatches.ModPool);
            Prefab = null;
            WeaponFactory = null;
        }
    }
}