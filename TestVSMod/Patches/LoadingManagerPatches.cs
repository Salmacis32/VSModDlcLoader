using HarmonyLib;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppSystem.Reflection;
using Il2CppVampireSurvivors.App.Data;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Weapons;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.DLC;
using Il2CppVampireSurvivors.Objects.Weapons;
using Il2CppZenject;
using TestVSMod.Factories;
using UnityEngine;
using Il2Col = Il2CppSystem.Collections.Generic;

namespace TestVSMod.Patches
{
    [HarmonyPatch(typeof(LoadingManager))]
    public static class LoadingManagerPatchesk
    {
        public static bool ModLoaded;

        [HarmonyPatch(nameof(LoadingManager.ValidateVersion))]
        [HarmonyPostfix]
        private static void AddManifest(object[] __args, MethodBase __originalMethod, object __instance)
        {
            if (ModLoaded) return;
            AddManifestPost();
            ModLoaded = true;
        }

        private static void AddManifestPost()
        {
            DlcType modDlcType = (DlcType)10000;
            var modDlcData = ScriptableObject.CreateInstance<BundleManifestData>();
            modDlcData._Version = "1.0.0"; modDlcData.name = "BundleManifestData - Modded"; modDlcData._DataFiles = new DataManagerSettings();
            WeaponAdder(modDlcData, modDlcType);
            MusicAdder(modDlcData);
            DlcSystem.MountedPaths.Add(modDlcType, "");
            DlcSystem.LoadedDlc.TryAdd(modDlcType, modDlcData);
            Action<BundleManifestData> DlcLoaderLoadDlc = (bmd) =>
            {
                bmd = modDlcData;
                DlcLoader._manifest = bmd;
                DlcLoader._manifestState = DlcLoadState.Complete;
                UnityEngine.Debug.Log("Loaded Modded Dlc");
            };
            ManifestLoader.ApplyBundleCore(modDlcType, modDlcData, DlcLoaderLoadDlc);
            ManifestLoader.DoRuntimeReload();
        }

        private static Weapon GetPrefab(Il2Col.KeyValuePair<WeaponType, Il2Col.List<WeaponData>> newWeapon)
        {
            var comp = ProjectContext.Instance.Container.InstantiateComponentOnNewGameObject<Weapon>();
            comp.name = newWeapon.Value[0].name;
            return comp;
        }

        private static void MusicAdder(BundleManifestData manifestData)
        {
            TextAsset textAsset = new TextAsset(Core.MusicJson);
            manifestData.DataFiles._MusicDataJsonAsset = textAsset;
            manifestData._DynamicSoundGroup = DynamicSoundGroupFactory.DefaultModdedGroup();
        }

        private static void WeaponAdder(BundleManifestData modDlcData, DlcType dlcType)
        {
            if (Core.Il2CppModdedWeaponInfo == null || Core.Il2CppModdedWeaponInfo.Count == 0) return;
            modDlcData._WeaponFactory = ScriptableObject.CreateInstance<WeaponFactory>();
            foreach (var newWeapon in Core.Il2CppModdedWeaponInfo)
            {
                Weapon comp = GetPrefab(newWeapon);
                modDlcData._WeaponFactory._weapons.Add(newWeapon.Key, comp);
            }
            if (modDlcData.DataFiles != null)
            {
                JObject dlc = JObject.FromObject(Core.Il2CppModdedWeaponInfo);
                TextAsset textAsset = new TextAsset(dlc.ToString());
                modDlcData.DataFiles._WeaponDataJsonAsset = textAsset;
            }
        }
        
    }
}