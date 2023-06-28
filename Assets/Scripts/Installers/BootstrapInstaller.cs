
using UnityEngine;
using Zenject;

namespace Assets.Installer
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GlobalEventsSystem _gameEvents;

        public override void InstallBindings()
        {
            BindGlobalEvents();
        }

        private void BindGlobalEvents()
        {
            Container
                .Bind<GlobalEventsSystem>()
                .FromComponentInNewPrefab(_gameEvents)
                .AsSingle();
        }
    }
}