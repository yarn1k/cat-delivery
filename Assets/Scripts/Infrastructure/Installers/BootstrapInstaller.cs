using UnityEngine;
using Zenject;
using Core.Audio;
using Core.Loading;
using Core.UI;

namespace Core.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [Inject]
        private Models.AudioSettings _audioSettings;

        public override void InstallBindings()
        {
            BindGlobalDependencies();
        }

        private void BindGlobalDependencies()
        {
            Container.Bind<AudioListener>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<SoundManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<IPauseProvider>().To<PauseController>().AsSingle().NonLazy();
            Container.Bind<ILoadingScreenProvider>().To<LoadingScreenProvider>().AsSingle().NonLazy();
            Container.Bind<ICoroutineRunner>().To<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.BindFactory<AudioClip, float, bool, DisposableAudioClip, DisposableAudioClip.Factory>()
                .FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(_audioSettings.PoolCapacity)
                .FromNewComponentOnNewGameObject()
                .UnderTransformGroup("Sounds"));
        }
    }
}
