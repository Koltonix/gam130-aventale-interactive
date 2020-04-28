using UnityEngine;

public static class ParticleManager
{
    /// <summary>Spawns an instance of a particle prefab and handles destroying.</summary>
    /// <param name="particle">GameObject particle to spawn.</param>
    /// <param name="position">Point at which to spawn it.</param>
    /// <param name="rotation">Rotation at which to spawn it.</param>
    /// <returns>The time it will take to destroy the particle effect.</returns>
    public static float SpawnParticle(GameObject particle, Vector3 position, Quaternion rotation)
    {
        ParticleSystem particleClone = GameObject.Instantiate(particle, position, rotation).GetComponent<ParticleSystem>();
        GameObject.Destroy(particleClone.gameObject, particleClone.main.duration);

        return particleClone.main.duration;
    }
}
