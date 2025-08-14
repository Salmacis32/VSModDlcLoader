using AudioImportLib;
using HarmonyLib;
using Il2CppDarkTonic.MasterAudio;
using Il2CppInterop.Runtime.Runtime;
using Il2CppNewtonsoft.Json;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Weapons;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.DLC.Types;
using Il2CppVampireSurvivors.Framework.Loading;
using Il2CppVampireSurvivors.Objects;
using Il2CppVampireSurvivors.Objects.Weapons;
using MelonLoader;
using MelonLoader.Utils;
using TestVSMod.Models;
using TestVSMod.Patches;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Il2CppDarkTonic.MasterAudio.MasterAudio;
using static Il2CppVampireSurvivors.Objects.Characters.CharacterController_Support;
using Il2Col = Il2CppSystem.Collections.Generic;

[assembly: MelonInfo(typeof(TestVSMod.Core), "TestVSMod", "1.0.0", "warde", null)]
[assembly: MelonGame("poncle", "Vampire Survivors")]
[assembly: MelonAdditionalDependencies("AudioImportLib")]

namespace TestVSMod
{

    public class Core : MelonMod
    {
        public static IEnumerable<WeaponInfo> ModdedWeaponInfo;
        public static Il2Col.Dictionary<WeaponType, Il2Col.List<WeaponData>> Il2CppModdedWeaponInfo;
        public static GameManager GameManager;
        public static string MusicJson;
        public static IDictionary<int, SongData[]> Music;
        private const int SongIdStart = 1410;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
            ModdedWeaponInfo = new List<WeaponInfo>();
            LoggerInstance.Msg("Created ModdedWeaponInfo.");
            Il2CppModdedWeaponInfo = new Il2Col.Dictionary<WeaponType, Il2Col.List<WeaponData>>();
            LoggerInstance.Msg("Created Il2CppModdedWeaponInfo.");
            HarmonyLib.Harmony harmony = HarmonyInstance;
            var methods = WeaponPatches.Methods;
            /*
            var prefix = typeof(WeaponPatches).GetMethod(nameof(WeaponPatches.Prefix));
            foreach (var method in methods)
            {
                if (method.Name == nameof(Weapon.HandlePlayerTeleport)) continue;
                harmony.Patch(method, new HarmonyMethod(prefix));
            }
            */
            var ass = MelonAssembly.Assembly;
            
            LoadWeaponJson(ass);
            LoadMusic(ass);
        }

        private static void LoadMusic(System.Reflection.Assembly ass)
        {
            var mdmj = ass.GetManifestResourceStream("TestVSMod.Data.musicData_Modded.json");
            var read2 = new StreamReader(mdmj);
            MusicJson = read2.ReadToEnd();
            Music = new Dictionary<int, SongData[]>();
            int id = SongIdStart;
            AddSong(id, "PAC TRONICA", ["BGM_Pactronica1.wav", "BGM_Pactronica2.wav"]); id++;
            AddSong(id, "PAC MADNESS", ["BGM_Pacmadness1.wav", "BGM_Pacmadness2.wav"]); id++;
            AddSong(id, "PAC TOY BOX", ["BGM_Pactoybox1.wav", "BGM_Pactoybox2.wav"]); id++;
            AddSong(id, "PAC BABY", ["BGM_Pacbaby1.wav", "BGM_Pacbaby2.wav"]);
        }

        private static void AddSong(int id, string name, string[] paths)
        {
            var clips = new SongData[paths.Length];
            for (var i = 0; i < paths.Length; i++)
            {
                clips[i] = new SongData(API.LoadAudioClip(MelonEnvironment.UserDataDirectory + "\\CustomAudio\\" + paths[i], true), name + i, (i != 0));
            }
            Music.Add(id, clips);
        }

        private static void LoadWeaponJson(System.Reflection.Assembly ass)
        {
            var wdj = ass.GetManifestResourceStream("TestVSMod.Data.WEAPON_DATA.json");
            var read = new StreamReader(wdj);
            var jobj = JsonConvert.DeserializeObject<Il2Col.Dictionary<WeaponType, Il2Col.List<WeaponData>>>(read.ReadToEnd());
            if (jobj != null) Il2CppModdedWeaponInfo = jobj;
            foreach (var il2obj in jobj)
            {
                ModdedWeaponInfo = ModdedWeaponInfo.AddItem(new WeaponInfo(il2obj));
            }
        }

        public override void OnDeinitializeMelon()
        {
            ModdedWeaponInfo = null;
            Il2CppModdedWeaponInfo = null;
            LoggerInstance.Msg("Set lists to null.");
            Music = null;
            MusicJson = null;
            GameManager = null;
        }
        /*
        [HarmonyPatch(typeof(LicenseManager))]
        static class PatchLicenseManager
        {
            [HarmonyPatch(nameof(LicenseManager.AddIncludedDlc))]
            [HarmonyPostfix]
            static void LoadingDLCSTytpe(ref LicenseManager __instance)
            {
                //__instance._OwnedDlc_k__BackingField.Add((DlcType)10000);
            }
        }

        [HarmonyPatch(typeof(DlcCatalog))]
        static class PatchDlcCatalog
        {
            
        }

        [HarmonyPatch(typeof(LoadingManager.__c__DisplayClass12_0))]
        static class PatchLMDisplayClass12
        {
            [HarmonyPatch(nameof(LoadingManager.__c__DisplayClass12_0._LoadManifestDirect_b__0))]
            [HarmonyPrefix]
            static void DisplayPre(LoadingManager.__c__DisplayClass12_0 __instance)
            {
                var test = __instance;
            }

            [HarmonyPatch(nameof(LoadingManager.__c__DisplayClass12_0._LoadManifestDirect_b__0))]
            [HarmonyPostfix]
            static void DisplayPost(LoadingManager.__c__DisplayClass12_0 __instance)
            {
                var test = __instance;
            }
        }

        [HarmonyPatch(typeof(LoadingManager))]
        static class PatchLoadingManager
        {

            [HarmonyPatch(nameof(LoadingManager.LoadDlc))]
            [HarmonyPrefix]
            static void LoadingDLCSPre(LoadingManager __instance)
            {
                var test = __instance;
            }

            [HarmonyPatch(nameof(LoadingManager.LoadDlc))]
            [HarmonyPostfix]
            static void LoadingDLCSPost(LoadingManager __instance)
            {
                var test = __instance;
            }

            [HarmonyPatch(nameof(LoadingManager.LoadIncludedDlc))]
            [HarmonyPrefix]
            static void LoadingInclDLCS(LoadingManager __instance, int index, Il2Col.List<DlcType> dlcsToLoad, Il2CppSystem.Action callback)
            {
                var test = __instance;
            }

            [HarmonyPatch(nameof(LoadingManager.LoadIncludedDlc))]
            [HarmonyPostfix]
            static void LoadingInclDLCSPost(LoadingManager __instance, int index, Il2Col.List<DlcType> dlcsToLoad, Il2CppSystem.Action callback)
            {
                var test = __instance;
            }

            [HarmonyPatch(nameof(LoadingManager.LoadManifestDirect))]
            [HarmonyPrefix]
            static void LoadingMani(LoadingManager __instance, DlcType dlcType, Il2CppSystem.String path, Il2CppSystem.Action<Il2CppSystem.Boolean> callback)
            {
                var test = __instance;
            }

            [HarmonyPatch(nameof(LoadingManager.LoadManifestDirect))]
            [HarmonyPostfix]
            static void LoadingManiPost(LoadingManager __instance, DlcType dlcType, Il2CppSystem.String path, Il2CppSystem.Action<Il2CppSystem.Boolean> callback)
            {
                var test = __instance;
            }
        }

        [HarmonyPatch(typeof(DlcLoader))]
        static class PatchDlcLoader
        {
            [HarmonyPatch(nameof(DlcLoader.LoadDlc))]
            [HarmonyPrefix]
            static void AddDlc(DlcLoader __instance, DlcType dlcType, Il2CppSystem.Action<BundleManifestData> onComplete)
            {
                var test = __instance;
            }

            [HarmonyPatch(nameof(DlcLoader.LoadDlc))]
            [HarmonyPostfix]
            static void AddDlcPost(DlcLoader __instance, DlcType dlcType, Il2CppSystem.Action<BundleManifestData> onComplete)
            {
                var test = __instance;
            }

            [HarmonyPatch(nameof(DlcLoader.LoadManifest))]
            [HarmonyPrefix]
            static void AddManifestDlc(DlcLoader __instance, Il2CppSystem.Action<BundleManifestData> onComplete)
            {
                var test = __instance;
                var test2 = onComplete;
                var test3 = DlcLoader.DlcType;
            }

            [HarmonyPatch(nameof(DlcLoader.LoadManifest))]
            [HarmonyPostfix]
            static void AddManifestDlcPost(DlcLoader __instance, Il2CppSystem.Action<BundleManifestData> onComplete)
            {
                var test = __instance;
                var test2 = onComplete;
                var test3 = DlcLoader.DlcType;
            }
        }


        [HarmonyPatch(typeof(DataManager))]
        [HarmonyWrapSafe]
        static class PatchDataManager
        {
            [HarmonyPatch(nameof(DataManager.GetConvertedWeapons))]
            [HarmonyPostfix]
            static void ConvertedWeapon(DataManager __instance, Il2Col.Dictionary<WeaponType, Il2Col.List<WeaponData>> __result)
            {
                var test = __result;
                return;
            }

            [HarmonyPatch(nameof(DataManager.LoadBaseJObjects))]
            [HarmonyPostfix]
            static void AddInstance(DataManager __instance, object[] __args, Il2CppSystem.Reflection.MethodBase __originalMethod)
            {
                
            }
            [HarmonyPatch(nameof(DataManager.MergeInJsonData))]
            [HarmonyPrefix]
            static void AddNewWeapons(ref DataManager __instance, DataManagerSettings settings, DlcType dlcType)
            {
                //if (dlcType == (DlcType)10000) WeaponAdder(ref __instance, settings, dlcType);
            }

            private static void WeaponAdder(ref DataManager __instance, DataManagerSettings settings, DlcType dlcType)
            {
                var AxeData = __instance._allWeaponDataJson.GetValue(WeaponType.AXE.ToString());
                var newWeapon = AxeData.ToObject<Il2Col.List<WeaponData>>();
                newWeapon[0].name = "NOTAXE"; newWeapon[0].description = "EVEN BETTER THAN THE AXE"; newWeapon[0].contentGroup = ContentGroupType.EXTRA;
                string jsonString = JsonConvert.SerializeObject(newWeapon,
                    Formatting.Indented);
                var jArray = JArray.Parse(jsonString);
                ModdedWeaponData.Add("1600", newWeapon);
                if (settings != null)
                {
                    JObject dlc = JObject.FromObject(ModdedWeaponData);
                    TextAsset textAsset = new TextAsset(dlc.ToString());
                    settings._WeaponDataJsonAsset = textAsset;
                }
            }

            [HarmonyPatch(nameof(DataManager.MergeInJsonData))]
            [HarmonyPostfix]
            static void CheckWeapons(DataManager __instance)
            {
                var test = __instance.AllWeaponData._entries;
                return;
            }
        }

        [HarmonyPatch(typeof(GameManager))]
        static class PatchGameManager
        {
            [HarmonyPatch(nameof(GameManager.LevelWeaponUp))]
            [HarmonyPrefix]
            static void prelevelupweap(GameManager __instance, WeaponType weaponType, CharacterController player)
            {
                var test = __instance;
                return;
            }

            [HarmonyPatch(nameof(GameManager.LevelWeaponUp))]
            [HarmonyPostfix]
            static void postlevelupweap(GameManager __instance, WeaponType weaponType, CharacterController player)
            {
                var test = __instance;
                return;
            }
        }

        [HarmonyPatch(typeof(Equipment), nameof(Equipment.GetDataForLevel))]
        [HarmonyWrapSafe]
        static class PatchEquipment
        {
            [HarmonyPrefix]
            static void DataLevel(Equipment __instance, WeaponType type, int level, ref JObject newLevelData, bool upgradeExistingData)
            {
                var test2 = GetDataForLevelFacade(__instance, type, level, out newLevelData, upgradeExistingData);
                return;
            }

            static bool GetDataForLevelFacade(Equipment instance, WeaponType type, int level, out JObject newLevelData, bool upgradeExistingData = true)
            {
                newLevelData = default;
                if (!instance.GetDataDictionary().TryGetValue(type, out JArray data)) return false;
                if (data.Count < level) return false;
                if (!data[level].HasValues) return false;
                var newData = data[level].Cast<JObject>();
                if (upgradeExistingData) DataHelper.UpgradeJsonData(instance._currentJsonDataObject, newData);
                else
                {
                    newLevelData = newData;
                }
                instance._currentJsonDataObject = newData;
                return true;
            }
        }

        [HarmonyPatch(typeof(Weapon), nameof(Weapon.InitWeapon))]
        [HarmonyWrapSafe]
        static class PatchWeapon
        {
            [HarmonyPrefix]
            static void Initialize(Weapon __instance, CharacterController characterController, WeaponType weaponType)
            {
                //InitWeapon(__instance, characterController, weaponType);
                return;
            }

            static void InitWeapon(Weapon instance, CharacterController characterController, WeaponType weaponType)
            {
                instance.FakeConstruct();
                instance.Owner = characterController;
                instance.Type = weaponType;
                instance.StatsInflictedDamage = 0;
                instance.StatsLifetime = 0;
                instance.TotalTime = 0;
                instance._beginningAmount = 0;
                //instance._projectilePool = new BulletPool() { UpperLimit = instance.ProjectilePoolSize };
                instance._isVisible = true;
                instance.MakeLevelOne();
                instance.OnStart();
                instance.GameMan.ParticleManager.RegisterParticleSystem(instance.GetComponentInChildren<ParticleSystem>());
            }
        }

        [HarmonyPatch(typeof(LevelUpFactory))]
        [HarmonyWrapSafe]
        static class PatchLevelUpFactory
        {
            [HarmonyPatch(nameof(LevelUpFactory.ProcessBaseWeaponData))]
            [HarmonyPrefix]
            static void PreProcess(LevelUpFactory __instance)
            {
                var test = __instance;
                var test2 = new List<WeaponType>();
                var weaponStore = LevelUpFactory._weaponStore.GetEnumerator();
                foreach (var w in weaponStore._list)
                {
                    test2.Add(w);
                }
                var test3 = new List<WeaponType>();
                var banishedWeapons = LevelUpFactory._banishedWeapons.GetEnumerator();
                foreach (var w in banishedWeapons._list)
                {
                    test3.Add(w);
                }
                return;
            }

            [HarmonyPatch(nameof(LevelUpFactory.ProcessBaseWeaponData))]
            [HarmonyPostfix]
            static void PostProcess(LevelUpFactory __instance)
            {
                var test = __instance;
                var test2 = new List<WeaponType>();
                var weaponStore = LevelUpFactory._weaponStore.GetEnumerator();
                foreach (var w in weaponStore._list)
                {
                    test2.Add(w);
                }
                var test3 = new List<WeaponType>();
                var banishedWeapons = LevelUpFactory._banishedWeapons.GetEnumerator();
                foreach (var w in banishedWeapons._list)
                {
                    test3.Add(w);
                }
                return;
            }
        }

        

        [HarmonyPatch(typeof(EquipmentManager), nameof(EquipmentManager.AddEquipment))]
        [HarmonyWrapSafe]
        static class PatchEquipmentManager
        {
            [HarmonyPrefix]
            static void Brian(EquipmentManager __instance, Equipment item)
            {
                var test = __instance;
                return;
            }

            [HarmonyPostfix]
            static void NewBrian(EquipmentManager __instance, Equipment item)
            {
                var test = __instance;
                return;
            }
        }

        /*
        [HarmonyPatch(typeof(DataHelper), nameof(DataHelper.GetWeaponDataForLevel))]
        [HarmonyWrapSafe]
        static class PatchDataHelper
        {
            [HarmonyPrefix]
            static bool WeaponData(ref WeaponData concreteData, bool __result, int level)
            {
                if (concreteData.name == "NOT AXE")
                {
                    concreteData = ModdedWeaponData[concreteData.name][level];
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(JsonSerializerInternalReader))]
        static class ParseWatch
        {
            [HarmonyPatch(nameof(JsonSerializerInternalReader.PopulateDictionary))]
            [HarmonyPrefix]
            static void checkParser(ref JsonSerializerInternalReader __instance, Il2CppSystem.Collections.IDictionary dictionary, JsonReader reader, JsonDictionaryContract contract, JsonProperty containerProperty, string id)
            {
                EnumInjector.InjectEnumValues<WeaponType>(new() { ["NOTAXE"] = 3000 });
                var fields = AccessTools.GetDeclaredFields(typeof(WeaponType));
                //AccessTools.StructFieldRefAccess<WeaponType, int>();
                var test = __instance;
                FakeInitializeValuesAndNames(new KeyValuePair<Il2CppSystem.Type, NamingStrategy?>(Il2CppType.Of<WeaponType>(), null));
                return;
            }

            static Il2CppNewtonsoft.Json.Utilities.EnumInfo FakeInitializeValuesAndNames(KeyValuePair<Il2CppSystem.Type, NamingStrategy?> key)
            {
                Il2CppSystem.Type enumType = key.Key;
                string[] names = Il2CppSystem.Enum.GetNames(enumType);
                string[] resolvedNames = new string[names.Length];
                ulong[] values = new ulong[names.Length];
                bool hasSpecifiedName;
                var test = enumType.GetFields();
                for (int i = 0; i < names.Length; i++)
                {
                    string name = names[i];
                    Il2CppSystem.Reflection.FieldInfo f = enumType.GetField(name, Il2CppSystem.Reflection.BindingFlags.NonPublic | Il2CppSystem.Reflection.BindingFlags.Public | Il2CppSystem.Reflection.BindingFlags.Static)!;
                    values[i] = ToUInt64(f.GetValue(null));
                    string resolvedName;
                    var custAttr = f.GetCustomAttributes(Il2CppType.Of<Il2CppSystem.Runtime.Serialization.EnumMemberAttribute>(), true);

                    resolvedName = name;
                    hasSpecifiedName = false;

                    if (Array.IndexOf(resolvedNames, resolvedName, 0, i) != -1)
                    {
                        throw new InvalidOperationException($"Enum name '{resolvedName}' already exists on enum '{enumType.Name}'.");
                    }

                    resolvedNames[i] = key.Value != null
                        ? key.Value.GetPropertyName(resolvedName, hasSpecifiedName)
                        : resolvedName;
                }

                bool isFlags = enumType.IsDefined(Il2CppType.Of<Il2CppSystem.FlagsAttribute>(), false);

                return new Il2CppNewtonsoft.Json.Utilities.EnumInfo(isFlags, values, names, resolvedNames);
            }

            private static ulong ToUInt64(Il2CppSystem.Object value)
            {
                var cppType = value.GetIl2CppType();
                PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(cppType, out bool _);
                if (cppType.Name != nameof(WeaponType)) return 0;
                var weaponVal = value.Unbox<WeaponType>();
                switch (typeCode)
                {
                    case PrimitiveTypeCode.SByte:
                        return (ulong)(sbyte)weaponVal;
                    case PrimitiveTypeCode.Byte:
                        return (byte)weaponVal;
                    case PrimitiveTypeCode.Boolean:
                        // direct cast from bool to byte is not allowed
                        return Convert.ToByte(weaponVal);
                    case PrimitiveTypeCode.Int16:
                        return (ulong)(short)weaponVal;
                    case PrimitiveTypeCode.UInt16:
                        return (ushort)weaponVal;
                    case PrimitiveTypeCode.Char:
                        return (char)weaponVal;
                    case PrimitiveTypeCode.UInt32:
                        return (uint)weaponVal;
                    case PrimitiveTypeCode.Int32:
                        return (ulong)(int)weaponVal;
                    case PrimitiveTypeCode.UInt64:
                        return (ulong)weaponVal;
                    case PrimitiveTypeCode.Int64:
                        return (ulong)(long)weaponVal;
                    // All unsigned types will be directly cast
                    default:
                        throw new InvalidOperationException("Unknown enum type.");
                }
            }
        }

        [HarmonyPatch(typeof(EnumUtils), nameof(EnumUtils.InitializeValuesAndNames))]
        static class ParseEnumWatch
        {
            [HarmonyPrefix]
            static void initParser(EnumUtils __instance, StructMultiKey<Type, NamingStrategy?> key, JsonReader reader, JsonDictionaryContract contract, JsonProperty containerProperty)
            {
                var test = __instance;
                return;
            }
        }
        [HarmonyPatch(typeof(DataManager))]
        [HarmonyWrapSafe]
        static class PatchDLCLoader
        {
            [HarmonyPatch(nameof(DataManager.BuildConvertedDlcData))]
            [HarmonyPrefix]
            static void Merge(DataManager __instance)
            {
                if (ModLoaded || __instance._allWeaponDataJson == null || __instance._allWeaponDataJson.Count == 0)
                {
                    return;
                }
                var ____allWeaponDataJson = __instance._allWeaponDataJson;
                var ____mergeSettings = __instance._mergeSettings;
                var converted = ____allWeaponDataJson.ToObject<Il2Col.Dictionary<string, JArray>>();
                var axe = converted["AXE"];
                var convertData = axe.ToObject<Il2Col.List<WeaponData>>();
                convertData[0].name = "NOT AXE"; convertData[0].description = "PLEASE GOD WORK";
                var modData = new Il2Col.Dictionary<string, Il2Col.List<WeaponData>>();
                modData.Add("NOTAXE", convertData);
                var dlcJson = JObject.FromObject(modData);
                __instance._allWeaponDataJson.ValidateContent(dlcJson);
                __instance._allWeaponDataJson.MergeItem(dlcJson, ____mergeSettings);

                ModLoaded = true;
            }
        }
        [HarmonyPatch(typeof(Il2CppSystem.Enum))]
        [HarmonyWrapSafe]
        static class PatchEnum
        {
            [HarmonyPatch(nameof(Il2CppSystem.Enum.Parse))]
            [HarmonyPatch(new[] { typeof(Il2CppSystem.Type), typeof(Il2CppSystem.String) } )]
            [HarmonyPrefix]
            static bool EnumParser(Il2CppSystem.Type enumType, Il2CppSystem.String value, ref object __result)
            {
                if (enumType.GetType() != typeof(WeaponType)) return true;
                if (value == "NOTAXE")
                {
                    __result = (WeaponType)1600;
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(GameManager))]
        [HarmonyWrapSafe]
        static class PatchGameManagerAddWeaponToPlayer
        {
            [HarmonyPatch(nameof(GameManager.AddWeaponToPlayer))]
            [HarmonyPostfix]
            static void CheckPlayerForWeapon(GameManager __instance, AddWeaponToCharacterSignal signal)
            {
                var test = __instance.LevelUpFactory;
                var postsignal = signal;

                return;
                //breakpoint
            }

            [HarmonyPatch(nameof(GameManager.AddWeaponToPlayer))]
            [HarmonyPrefix]
            static void CheckSignal(GameManager __instance, AddWeaponToCharacterSignal signal)
            {
                var test = signal;
                var chara = test.Character;
                var wepaon = test.Weapon;
                return;
                //breakpoint
            }

            [HarmonyPatch(nameof(GameManager.Construct))]
            [HarmonyPostfix]
            static void ConstructCheck(GameManager __instance, WeaponsFacade weaponsFacade)
            {
                var test = __instance.WeaponsFacade;
                return;
                //breakpoint
            }
        }
        
        [HarmonyPatch(typeof(LevelUpFactory), nameof(LevelUpFactory.TryParseType))]
        [HarmonyWrapSafe]
        static class PatchLevelUpFactoryParse
        {
            [HarmonyPrefix]
            static bool EnumsSuck(LevelUpFactory __instance, string type, ref WeaponType __result)
            {
                var enumParse = Enum.TryParse(type, true, out WeaponType parsed);
                if (!enumParse)
                {
                    __result = (WeaponType)(type.ToInt());
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(CharacterWeaponsManager), nameof(CharacterWeaponsManager.GetWeaponByType))]
        [HarmonyWrapSafe]
        static class PatchCharacterWeaponsManager
        {
            [HarmonyPostfix]
            static void Meg(CharacterWeaponsManager __instance, WeaponType weaponType, Weapon __result)
            {
                var test = __instance;
                return;
            }
        }

        

        

        [HarmonyPatch(typeof(Weapon), nameof(Weapon.InitWeapon))]
        [HarmonyWrapSafe]
        static class PatchWeapon
        {
            [HarmonyPostfix]
            static void Lois(Weapon __instance, WeaponType weaponType, CharacterController characterController)
            {
                var test = __instance;
                return;
            }
        }
        */
    }
}