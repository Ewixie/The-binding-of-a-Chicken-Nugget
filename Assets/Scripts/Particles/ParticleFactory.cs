using UnityEngine;
using UnityEngine.Pool;

namespace Player.Shooting
{
    public class ParticleFactory
    {
        private readonly ParticleHolder _particleSystemPrefab;
        private readonly ObjectPool<ParticleHolder> _particleSystemsPool;

        public ParticleFactory(ParticleSystem particleSystemPrefab)
        {
            _particleSystemsPool = new ObjectPool<ParticleHolder>(CreateNewParticleSystem, OnParticleSystemGet, OnParticleSystemRelease, OnDestroyParticleSystem, true,
                20, 1000);

            _particleSystemPrefab = particleSystemPrefab.GetComponent<ParticleHolder>();
        }
        
        public ParticleSystem CreateParticleSystem(Vector3 position)
        {
            var particleHolder = _particleSystemsPool.Get();
            particleHolder.transform.position = position;
            return particleHolder.GetParticleSystem();
        }

        private ParticleHolder CreateNewParticleSystem()
        {
            var particleHolder = Object.Instantiate(_particleSystemPrefab, Vector2.zero, Quaternion.identity);
             particleHolder.ParticleSystemStopped += () =>
            {
                _particleSystemsPool.Release(particleHolder);
            };
            return particleHolder;
        }
        
        private void OnDestroyParticleSystem(ParticleHolder obj)
        {
            Object.Destroy(obj.gameObject);
        }
        
        private void OnParticleSystemRelease(ParticleHolder obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnParticleSystemGet(ParticleHolder obj)
        {
            obj.gameObject.SetActive(true);
        }
    }
}