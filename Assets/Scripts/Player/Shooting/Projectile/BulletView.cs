using Particles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Shooting.Projectile
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private Bullet bullet;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private float rotationSpeed;
        
        private ParticleFactory _particleFactory;
        private float _rotation;
        private void Start()
        {
            _particleFactory = new ParticleFactory(particles);
            bullet.BulletDestroyed += OnBulletDestroyed;
            _rotation = Random.Range(-rotationSpeed, rotationSpeed);
        }
        
        private void OnBulletDestroyed()
        {
            ParticleSystem particleSystem = _particleFactory.CreateParticleSystem(transform.position);
        }

        private void Update()
        {
            transform.Rotate(0, 0, zAngle: _rotation *Time.deltaTime);
        }
    }
}



