using Assets.Player;
using UnityEngine;
using Zenject;

namespace Assets.Installer
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerLogic _playerOne;
        [SerializeField] private PlayerLogic _playerTwo;

        public override void InstallBindings()
        {
            Container.Bind<PlayerLogic>().FromInstance(_playerOne).AsSingle();
            Container.QueueForInject(_playerOne);

            Container.Bind<PlayerLogic>().FromInstance(_playerTwo).AsSingle();
            Container.QueueForInject(_playerTwo);
        }
    }
}