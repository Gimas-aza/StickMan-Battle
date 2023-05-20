
using UnityEngine;
using Zenject;

namespace Assets.Installer
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameEventsServise _gameEvents;

        public override void InstallBindings()
        {
            BindGameEvents();
        }

        private void BindGameEvents()
        {
            Container
                .Bind<GameEventsServise>()
                .FromComponentInNewPrefab(_gameEvents)
                .AsSingle();
        }
    }
}