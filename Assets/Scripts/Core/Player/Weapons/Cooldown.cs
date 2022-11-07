﻿using UnityEngine;

namespace Core.Weapons
{
    public class Cooldown
    {
        private float _duration;
        private float _startTime;

        public float Duration => _duration;
        public float CurrentTime => Time.time - _startTime;
        public float RemainingTime => _duration - CurrentTime;
        public bool IsOver => CurrentTime > _duration;

        public void Run(float duration)
        {
            _duration = duration;
            _startTime = Time.time;
        }
    }
}
