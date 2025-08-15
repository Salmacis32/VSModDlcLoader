using HarmonyLib;
using Il2CppVampireSurvivors.Objects.Pools;
using Il2CppVampireSurvivors.Objects.Projectiles;
using Il2CppVampireSurvivors.Objects.Weapons;
using MelonLoader;
using System.Reflection;
using System.Text;
using TestVSMod.Models;
using TestVSMod.Models.Projectiles;
using TestVSMod.Util;

namespace TestVSMod.Patches
{
    public static class ProjectilePatches
    {
        public static MethodInfo[] Methods;
        public static byte[] ModWeaponType;
        public static ModProjectile[,] ModPool;
        public static StringBuilder StringBuilder;

        public static void Initialize()
        {
            Methods = TargetMethods();
            ModWeaponType = new byte[2600];
            StringBuilder = new StringBuilder();
        }

        public static void Deinitialize()
        {
            Methods = null;
            ModWeaponType = null;
            StringBuilder = null;
        }

        public static MethodInfo[] TargetMethods()
        {
            var arr = new MethodInfo[16];
            var methods = AccessTools.GetDeclaredMethods(typeof(Projectile));
            var onlyTypes = methods.Where(x => x.DeclaringType.Name == nameof(Projectile));
            arr[0] = onlyTypes.Single(x => x.Name.Equals(nameof(Projectile.InitProjectile)));
            arr[1] = onlyTypes.Single(x => x.Name.Equals(nameof(Projectile.Despawn)));
            var methtwo = AccessTools.GetDeclaredMethods(typeof(AxeProjectile)).Where(x => x.DeclaringType.Name == nameof(AxeProjectile));
            arr[2] = methtwo.Single(x => x.Name.Equals(nameof(AxeProjectile.InternalUpdate)));
            return arr;
        }

        public static bool InitPrefix(Projectile __instance, Weapon weapon, BulletPool pool, int index, MethodBase __originalMethod)
        {
            if (ModWeaponType[(int)weapon.Type] == 0)
            {
                if (!Core.ModdedWeaponInfo.Any(x => x.IdAsType.Equals(weapon.Type))) ModWeaponType[(int)weapon.Type] = 1;
                else ModWeaponType[(int)weapon.Type] = 2;
            }
            if (ModWeaponType[(int)weapon.Type] < 2) return true;
            var modIndex = (int)weapon.Type - Constants.WEAPON_START_ID;
            var modWeapon = ModPool[modIndex, index];
            if (modWeapon == null) modWeapon = new ModKnifeProjectile();
            
            try
            {
                /* temp logging
                StringBuilder.AppendFormat(Constants.INITPROJECTILE_1 + (int)weapon.Type + Constants.INITPROJECTILE_3 + index);
                MelonLogger.MsgPastel(StringBuilder.ToString());
                StringBuilder.Clear();
                */
                modWeapon.InitProjectile(ref __instance, pool, weapon, index);
                /*
                StringBuilder.AppendFormat(Constants.INITPROJECTILE_2 + (int)weapon.Type + Constants.INITPROJECTILE_3 + index);
                MelonLogger.MsgPastel(StringBuilder.ToString());
                StringBuilder.Clear();
                */
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.Message);
                return true;
            }
            finally
            {
                ModPool[modIndex, index] = modWeapon;
            }
        }

        public static void DespawnPrefix(Projectile __instance)
        {
            if (ModPool == null) return;
            var safeWeapon = SafeAccess.GetProperty<Weapon>(__instance, nameof(__instance.Weapon));
            if (safeWeapon == null) return;
            if (ModWeaponType[(int)__instance._weapon.Type] < 2) return;
            var modIndex = (int)__instance._weapon.Type - Constants.WEAPON_START_ID;
            ModPool[modIndex, __instance.IndexInWeapon] = null;
        }

        public static bool InternalUpdatePrefix(AxeProjectile __instance)
        {
            if (ModPool == null) return true;
            var safeWeapon = SafeAccess.GetProperty<Weapon>(__instance, nameof(__instance.Weapon));
            if (safeWeapon == null) return true;
            if (ModWeaponType[(int)__instance._weapon.Type] < 2) return true;
            return false;
        }
    }
}
