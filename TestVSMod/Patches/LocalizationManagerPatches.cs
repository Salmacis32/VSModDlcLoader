using HarmonyLib;
using Il2CppI2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVSMod.Patches
{
    [HarmonyPatch(typeof(LocalizationManager))]
    public static class LocalizationManagerPatches
    {
        [HarmonyPatch(nameof(LocalizationManager.UpdateSources))]
        [HarmonyPostfix]
        public static void GetTransPost()
        {
            var test = LocalizationManager.GetTermsList();
            if (test == null || test.Count == 0) return;
            foreach (var weapon in Core.ModdedWeaponInfo)
            {
                var weapondata = LocalizationManager.GetSourceContaining("weaponLang/{HELLFIRE}name");
                StringBuilder sb = new StringBuilder("weaponLang/{");
                sb.Append(weapon.WeaponId); sb.Append("}name");
                if (!weapondata.ContainsTerm(sb.ToString()))
                {
                    var name = weapondata.AddTerm(sb.ToString(), eTermType.Text);
                    name.Languages[0] = weapon.WeaponName;
                }
                sb.Replace("name", "description");
                if (!weapondata.ContainsTerm(sb.ToString()))
                {
                    var desc = weapondata.AddTerm(sb.ToString(), eTermType.Text);
                    desc.Languages[0] = weapon.WeaponDescription;
                }
                sb.Replace("description", "tips");
                if (!weapondata.ContainsTerm(sb.ToString()))
                {
                    var desc = weapondata.AddTerm(sb.ToString(), eTermType.Text);
                    desc.Languages[0] = weapon.WeaponTips;
                }
            }
        }
    }
}
