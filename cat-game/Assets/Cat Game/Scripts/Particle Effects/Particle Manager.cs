using UnityEngine;

public static class ParticleManager
{
    public static void SpawnParticle(GameObject particle, Vector3 position, Quaternion rotation)
    {
        ParticleSystem particleClone = GameObject.Instantiate(particle, position, rotation).GetComponent<ParticleSystem>();
        GameObject.Destroy(particleClone.gameObject, particleClone.main.startLifetime.constant + particleClone.main.duration);
    }
}
