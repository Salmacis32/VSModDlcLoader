using Il2Cpp;
using Il2CppVampireSurvivors.Framework;
using Il2CppVampireSurvivors.Objects.Pools;
using Il2CppVampireSurvivors.Objects.Projectiles;
using Il2CppVampireSurvivors.Objects.Weapons;
using TestVSMod.Util;
using UnityEngine;

namespace TestVSMod.Models
{
    public class ModProjectile
    {
        public virtual void InitProjectile(ref Projectile proj, BulletPool pool, Weapon weapon, int index)
        {
            proj._gameSessionData = GM.Core.GameSessionData;
            proj._pool = pool;
            proj._weapon = weapon;

            var objectsHit = proj._objectsHit.ToHashSet();
            if (objectsHit.Count > 0)
            {
                objectsHit.Clear();
            }
            proj._objectsHit = objectsHit.ToIl2CppHashSet();
            proj._indexInWeapon = index;

            proj._penetrating = proj._weapon.Penetrating;
            proj._bounces = proj._weapon.PBounces();
            if (proj.body == null)
            {
                ArcadePhysics.s_scene.add._world.enableBody(proj, PhysicsType.DYNAMIC_BODY);
            }

            proj.body._enable = true;
            PhysicsManager instance = PhysicsManager._sInstance;

            instance._bulletGroup?.add(proj._sprite);
            proj._spriteTrail?.Reset();

            var spawnedProj = weapon._spawnedProjectiles.ToSystemList();
            if (spawnedProj.Count == 0 || !spawnedProj.Contains(proj))
            {
                spawnedProj.Add(proj);
            }
            weapon._spawnedProjectiles = spawnedProj.ToIl2CppList();
            GM.Core.ParticleManager.RegisterParticleSystem(proj.GetComponentInChildren<ParticleSystem>());
            
        }
    }
}
