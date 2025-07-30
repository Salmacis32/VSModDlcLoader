using HarmonyLib;
using Il2CppNewtonsoft.Json;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppVampireSurvivors.App.Data;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Weapons;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.DLC;
using Il2CppVampireSurvivors.Objects.Projectiles;
using Il2CppVampireSurvivors.Objects.Weapons;
using Il2CppZenject;
using TestVSMod.Util;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using Il2Col = Il2CppSystem.Collections.Generic;

namespace TestVSMod.Patches
{
    [HarmonyPatch(typeof(ManifestLoader))]
    public static class ManifestLoaderPatches
    {
        private static Action<BundleManifestData> DlcLoaderLoadDlc = (bmd) =>
        {
            DlcLoader._manifest = bmd;
            DlcLoader._manifestState = DlcLoadState.Complete;
            DlcLoader.UpdateProgress();
        };

        [HarmonyPatch(nameof(ManifestLoader.LoadManifest))]
        [HarmonyPrefix]
        private static void AddManifest(BundleManifestData bundleManifestData, DlcType dlcType, Il2CppSystem.Action<BundleManifestData> onComplete)
        {
            var test = bundleManifestData;
        }

        [HarmonyPatch(nameof(ManifestLoader.LoadManifest))]
        [HarmonyPostfix]
        private static void AddManifestPost(BundleManifestData bundleManifestData, DlcType dlcType, Il2CppSystem.Action<BundleManifestData> onComplete)
        {
            if (dlcType != DlcType.Emeralds) return;
            DlcType modDlcType = (DlcType)10000;
            var weaponData = ManifestLoader._sInstance._dataManager;
            var modDlcData = ScriptableObject.CreateInstance<BundleManifestData>();
            modDlcData._Version = "1.0.0"; modDlcData.name = "BundleManifestData - Modded"; modDlcData._DataFiles = new DataManagerSettings(); modDlcData._WeaponFactory = ScriptableObject.CreateInstance<WeaponFactory>();
            WeaponAdder(weaponData, modDlcData, modDlcType);
            ManifestLoader.LoadManifest(modDlcData, modDlcType, onComplete);
        }

        private static void WeaponAdder(Il2CppVampireSurvivors.Data.DataManager __instance, BundleManifestData modDlcData, DlcType dlcType)
        {
            foreach (var newWeapon in Core.Il2CppModdedWeaponInfo)
            {
                AxeWeapon comp = GetPrefab(newWeapon);
                modDlcData._WeaponFactory._weapons.Add(newWeapon.Key, comp);
            }
            if (modDlcData.DataFiles != null)
            {
                JObject dlc = JObject.FromObject(Core.Il2CppModdedWeaponInfo);
                TextAsset textAsset = new TextAsset(dlc.ToString());
                modDlcData.DataFiles._WeaponDataJsonAsset = textAsset;
            }
        }

        private static AxeWeapon GetPrefab(Il2Col.KeyValuePair<WeaponType, Il2Col.List<WeaponData>> newWeapon)
        {
            var container = ProjectContext._instance._container;
            var comp = container.InstantiateComponentOnNewGameObject<AxeWeapon>();
            comp.name = newWeapon.Value[0].name;
            comp._ProjectilePrefab = ProjectContext._instance.Container.InstantiateComponentOnNewGameObject<AxeProjectile>();
            return comp;
        }
    }
}