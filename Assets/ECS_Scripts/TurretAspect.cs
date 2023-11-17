using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

//Bundled Wrapper Abstraction
namespace ECS_Scripts
{
    //This was not working for me? Seems useful if I can figure it out though...
    
    public readonly partial struct TurretAspect : IAspect
    {
        private readonly RefRO<Turret> m_Turret;
        private readonly RefRO<URPMaterialPropertyBaseColor> m_BaseColor;

        public Entity BulletSpawnPoint => m_Turret.ValueRO.FirePoint;
        public Entity ProjectilePrefab => m_Turret.ValueRO.Projectile;
        public float4 Color => m_BaseColor.ValueRO.Value;
    }
}
