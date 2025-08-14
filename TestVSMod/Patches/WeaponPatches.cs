using HarmonyLib;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Objects.Pools;
using Il2CppVampireSurvivors.Objects.Weapons;
using System.Reflection;
using TestVSMod.Util;

namespace TestVSMod.Patches
{
    public static class WeaponPatches
    {
        public static IEnumerable<MethodInfo> Methods = TargetMethods();
        public static BulletPool pool;
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            var methods = AccessTools.GetDeclaredMethods(typeof(Weapon)).Where(x => x.DeclaringType.Name == nameof(Weapon));
            var noFields = methods.Where(x => x.Name.StartsWith(nameof(Weapon.Fire)));
            return noFields;
        }

        // prefix all methods in someAssembly with a non-void return type and beginning with "Player"
        public static bool Prefix(object[] __args, MethodBase __originalMethod, object __instance)
        {
            if (__originalMethod.Name != nameof(Weapon.Fire) && __originalMethod.GetParameters().Length > 0) return true;
            if ((__instance as Weapon)?.Type != WeaponType.AXE) return true;
            var weapon = (__instance as Weapon);
            if (pool == null)
            {
                var projectiles = weapon.GameMan.ProjectileFactory._Projectiles.ToDictionary().First(x => x.Key == WeaponType.CROSS);
                pool = new BulletPool(projectiles.Value, 50);
            }
            weapon.FireOneProjectile(weapon.PlayerPos, 0, pool: pool);
            return true;
        }
    }
}
