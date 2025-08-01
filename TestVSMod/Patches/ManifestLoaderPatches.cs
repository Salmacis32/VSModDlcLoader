using HarmonyLib;
using Il2CppDarkTonic.MasterAudio;
using Il2CppDoozy.Engine.Utils;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNewtonsoft.Json.Linq;
using Il2CppSystem.IO;
using Il2CppVampireSurvivors.App.Data;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Weapons;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.DLC;
using Il2CppVampireSurvivors.Objects.Projectiles;
using Il2CppVampireSurvivors.Objects.Weapons;
using Il2CppZenject;
using TestVSMod.Util;
using Unity.Services.Core.Configuration;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using static Il2CppDarkTonic.MasterAudio.MasterAudio;
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
            modDlcData._Version = "1.0.0"; modDlcData.name = "BundleManifestData - Modded"; modDlcData._DataFiles = new DataManagerSettings();
            modDlcData._WeaponFactory = ScriptableObject.CreateInstance<WeaponFactory>();
            WeaponAdder(weaponData, modDlcData, modDlcType);
            MusicAdder(modDlcData);
            DlcSystem.MountedPaths.Add((DlcType)10000, "");
            ManifestLoader.LoadManifest(modDlcData, modDlcType, onComplete);
        }

        private static AxeWeapon GetPrefab(Il2Col.KeyValuePair<WeaponType, Il2Col.List<WeaponData>> newWeapon)
        {
            var container = ProjectContext._instance._container;
            var comp = container.InstantiateComponentOnNewGameObject<AxeWeapon>();
            comp.name = newWeapon.Value[0].name;
            comp._ProjectilePrefab = ProjectContext._instance.Container.InstantiateComponentOnNewGameObject<AxeProjectile>();
            return comp;
        }

        private static void MusicAdder(BundleManifestData modDlcData)
        {
            var sounds = ProjectContext._instance._container.InstantiateComponentOnNewGameObject<DynamicSoundGroupCreator>();
            

            foreach (var song in Core.Music)
            {
                Playlist playlist = new Playlist() { playlistName = song.Key.ToString() };
                playlist.MusicSettings.Add(new MusicSetting() { clip = song.Value.Clip, songName = song.Key.ToString(), alias = song.Value.Name, isLoop = true, audLocation = AudioLocation.Clip });
                sounds.musicPlaylists = new Il2Col.List<Playlist>();
                MasterAudio.CreatePlaylist(playlist, sounds);
                sounds.musicPlaylists.Add(playlist);
            }
            
            TextAsset textAsset = new TextAsset(Core.MusicJson);
            modDlcData.DataFiles._MusicDataJsonAsset = textAsset;
            modDlcData._DynamicSoundGroup = sounds;
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
    }
}