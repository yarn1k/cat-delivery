using System;
using UnityEngine;
using Zenject;

namespace Core.Models
{
    [Serializable]
    public class AudioSettings
    {
        public AudioClip GameBackground;
        public AudioClip GameLevelStart;
        public AudioClip GameBonus;
        public AudioClip GameOver;
    }

    [Serializable]
    public class AudioUISettings
    {
        public AudioClip UIClick;
    }

    [Serializable]
    public class AudioPlayerSettings
    {
        public AudioClip PlayerShoot;
        public AudioClip PlayerOnHit;
    }

    [Serializable]
    public class AudioCatsSettings
    {
        public AudioClip CatGrabbed;
    }

    [CreateAssetMenu(fileName = "Audio Settings", menuName = "Installers/Audio Settings")]
    public class AudioSettingsInstaller : ScriptableObjectInstaller<AudioSettingsInstaller>
    {
        [SerializeField]
        private AudioSettings _audioSettings;
        [SerializeField]
        private AudioUISettings _audioUISettings;
        [SerializeField]
        private AudioPlayerSettings _audioPlayerSettings;
        [SerializeField]
        private AudioCatsSettings _audioCatsSettings;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<AudioSettings>().FromInstance(_audioSettings).IfNotBound();
            Container.Bind<AudioUISettings>().FromInstance(_audioUISettings).IfNotBound();
            Container.Bind<AudioPlayerSettings>().FromInstance(_audioPlayerSettings).IfNotBound();
            Container.Bind<AudioCatsSettings>().FromInstance(_audioCatsSettings).IfNotBound();
        }
    }
}
