using Zenject;
using UnityEngine;
using GameFlow;
using System.Collections.Generic;

namespace ShootEmUp
{
    public class LevelInstaler : MonoInstaller
    {
        [SerializeField] private Transform _worldTransform;

        [Header("BulletSystem")]
        [SerializeField] private Bullet _bulletPrefab;

        [Space(20)]
        [Header("Player")]
        [SerializeField] private Character _character;
        [SerializeField] private HitPointsComponent _playerHitPointComponent;

        [Space(20)]
        [Header("Enemy")]
        [SerializeField] private Enemy _enemyPrefab;

        public override void InstallBindings()
        {
            BindGameFlow();
            BindBulletSystem();
            BindCharacterSystems();
            BindEnemySystems();
        }

        private void BindGameFlow()
        {
            Container.BindInterfacesTo<CoroutineStarter>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<GameFlowManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }

        private void BindBulletSystem()
        {
            Container.Bind<Transform>().FromInstance(_worldTransform).AsSingle();
            Container.Bind<LevelBounds>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BulletContainer>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<Bullet>().FromInstance(_bulletPrefab).AsSingle();
            Container.BindInterfacesAndSelfTo<BulletSystem>().AsSingle();
        }

        private void BindCharacterSystems()
        {
            Container.Bind<HitPointsComponent>().FromInstance(_playerHitPointComponent);
            Container.Bind<Character>().FromInstance(_character).AsSingle();
            Container.BindInterfacesAndSelfTo<OnCharacterKilledObserver>().AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterController>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        }

        private void BindEnemySystems()
        {
            Container.Bind<Enemy>().FromInstance(_enemyPrefab).AsSingle();
            Container.Bind<EnemiesContainer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EnemyPositions>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyPool>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();
            
        }
    }
}