using HarmonyLib;
using Il2CppDarkTonic.MasterAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVSMod.Patches
{
    [HarmonyPatch(typeof(MasterAudio))]
    public static class PlaylistPatches
    {
        [HarmonyPatch(nameof(MasterAudio.AddCustomEventReceiver))]
        [HarmonyPrefix]
        public static void Init()
        {

        }

        [HarmonyPatch(nameof(MasterAudio.CreatePlaylist))]
        [HarmonyPrefix]
        public static void CreateEvents()
        {

        }
    }
}
