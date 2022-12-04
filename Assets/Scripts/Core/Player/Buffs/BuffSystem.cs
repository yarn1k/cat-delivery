using System.Collections.Generic;
using Zenject;

namespace Core.Player.Buffs
{
    public class BuffSystem : ITickable
    {
        private readonly LazyInject<PlayerModel> _playerModel;
        private readonly List<IBuff> _buffs = new List<IBuff>();

        public BuffSystem(LazyInject<PlayerModel> model)
        {
            _playerModel = model;
        }
        
        public void AddBuff(IBuff buff)
        {
            if (!_buffs.Contains(buff))
            {
                buff.Execute(_playerModel.Value);
                buff.Duration.Run(5f);
                _buffs.Add(buff);
            }
        }
        public void RemoveBuff(IBuff buff)
        {
            _buffs.Remove(buff);
            buff.Reset(_playerModel.Value);
        }

        void ITickable.Tick()
        {  
            for (int i = _buffs.Count - 1; i >= 0; i--)
            {
                if (_buffs[i].Duration.IsOver) RemoveBuff(_buffs[i]);
            }
        }
    }
}
