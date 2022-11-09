using System;
using UnityEngine;
using Zenject;

namespace Core.Models
{
    [Serializable]
    public struct AudioSound
    {
        public AudioClip Clip;
        [Range(0f, 1f)]
        public float Volume;
    }

    [Serializable]
    public class AudioSettings
    {
        public byte PoolCapacity;
        [Range(0f, 1f)]
        public float GlobalVolume;
    }

    [Serializable]
    public class GameSounds
    {
        public AudioSound GameBackground;
        public AudioSound GameLevelStart;
        public AudioSound GameBonus;
        public AudioSound GameOver;
    }

    [Serializable]
    public class UISounds
    {
        public AudioSound UIClick;
    }

    [Serializable]
    public class PlayerSounds
    {
        public AudioSound PlayerOnHit;
    }

    [Serializable]
    public class CatsSounds
    {
        public AudioSound CatGrabbed;
    }


    [CreateAssetMenu(menuName = "Configuration/Settings/Audio Settings")]
    public class AudioSettingsInstaller : ScriptableObjectInstaller<AudioSettingsInstaller>
    {
        [SerializeField]
        private AudioSettings _audioSettings;

        [Header("Sounds")]
        [SerializeField]
        private GameSounds _gameSounds;
        [SerializeField]
        private UISounds _UISounds;
        [SerializeField]
        private PlayerSounds _playerSounds;
        [SerializeField]
        private CatsSounds _catsSounds;

        public override void InstallBindings()
        {
            Container.Bind<AudioSettings>().FromInstance(_audioSettings).IfNotBound();
            Container.Bind<GameSounds>().FromInstance(_gameSounds).IfNotBound();
            Container.Bind<UISounds>().FromInstance(_UISounds).IfNotBound();
            Container.Bind<PlayerSounds>().FromInstance(_playerSounds).IfNotBound();
            Container.Bind<CatsSounds>().FromInstance(_catsSounds).IfNotBound();
        }
    }
}