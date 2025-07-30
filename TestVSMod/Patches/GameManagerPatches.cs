using HarmonyLib;
using Il2CppI2.Loc;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Framework.DLC;
using Il2CppVampireSurvivors.Objects.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVSMod.Patches
{
    [HarmonyPatch(typeof(GameManager))]
    public static class GameManagerPatches
    {
        [HarmonyPatch(nameof(GameManager.InitializeGame))]
        [HarmonyPrefix]
        public static void Awake(GameManager __instance)
        {
        }
    }
}
