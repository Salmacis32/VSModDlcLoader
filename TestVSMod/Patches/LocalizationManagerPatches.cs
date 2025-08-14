using HarmonyLib;
using Il2CppI2.Loc;
using System.Text;

namespace TestVSMod.Patches
{
    [HarmonyPatch(typeof(LocalizationManager))]
    public static class LocalizationManagerPatches
    {
        public static LanguageSourceData Source;
        
        [HarmonyPatch(nameof(LocalizationManager.UpdateSources))]
        [HarmonyPostfix]
        public static void GetTransPost()
        {
            if (LocalizationManager.Sources.Count == 0) return;
            if (Source == null) Source = LocalizationManager.GetSourceContaining("weaponLang/{HELLFIRE}name");
            foreach (var weapon in Core.ModdedWeaponInfo)
            {
                StringBuilder sb = new StringBuilder("weaponLang/{");
                sb.Append(weapon.WeaponId); sb.Append("}name");
                if (!Source.ContainsTerm(sb.ToString()))
                {
                    var name = Source.AddTerm(sb.ToString(), eTermType.Text);
                    name.Languages[0] = weapon.WeaponName;
                }
                sb.Replace("name", "description");
                if (!Source.ContainsTerm(sb.ToString()))
                {
                    var desc = Source.AddTerm(sb.ToString(), eTermType.Text);
                    desc.Languages[0] = weapon.WeaponDescription;
                }
                sb.Replace("description", "tips");
                if (!Source.ContainsTerm(sb.ToString()))
                {
                    var desc = Source.AddTerm(sb.ToString(), eTermType.Text);
                    desc.Languages[0] = weapon.WeaponTips;
                }
            }
        }
    }
}
