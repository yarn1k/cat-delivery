using Core.Match;
using System;
using UnityEngine;
using Zenject;

namespace Core.Models
{
    [Serializable]
    public class Audio
    {
        public AudioClip Clip;
        [Range(0f, 1f)]
        public float Volume;
    }

    [Serializable]
    public class AudioGameSettings
    {
        public Audio GameBackground;
        public Audio GameLevelStart;
        public Audio GameBonus;
        public Audio GameOver;
    }

    [Serializable]
    public class AudioUISettings
    {
        public Audio UIClick;
    }

    [Serializable]
    public class AudioPlayerSettings
    {
        public Audio PlayerShoot;
        public Audio PlayerOnHit;
    }

    [Serializable]
    public class AudioCatsSettings
    {
        public Audio CatGrabbed;
    }

    [CreateAssetMenu(fileName = "Audio Settings", menuName = "Installers/Audio Settings")]
    public class AudioSettingsInstaller : ScriptableObjectInstaller<AudioSettingsInstaller>
    {
        [SerializeField]
        private AudioGameSettings _audioSettings;
        [SerializeField]
        private AudioUISettings _audioUISettings;
        [SerializeField]
        private AudioPlayerSettings _audioPlayerSettings;
        [SerializeField]
        private AudioCatsSettings _audioCatsSettings;

        public override void InstallBindings()
        {
            Container.Bind<AudioGameSettings>().FromInstance(_audioSettings).IfNotBound();
            Container.Bind<AudioUISettings>().FromInstance(_audioUISettings).IfNotBound();
            Container.Bind<AudioPlayerSettings>().FromInstance(_audioPlayerSettings).IfNotBound();
            Container.Bind<AudioCatsSettings>().FromInstance(_audioCatsSettings).IfNotBound();
        }
    }
}
