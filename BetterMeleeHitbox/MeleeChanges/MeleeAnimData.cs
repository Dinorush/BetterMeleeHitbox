using Gear;
using System;

namespace BMH.MeleeChanges
{
    public sealed class MeleeAnimData
    {
        private readonly float _attackLength;
        private readonly float _attackHitTime;
        private readonly float _damageStartTime;
        private readonly float _damageEndTime;
        private readonly float _attackCamFwdHitTime;
        private readonly float _comboEarlyTime;

        public MeleeAnimData(
            float attackLength = -1,
            float attackHitTime = -1,
            float damageStartTime = -1,
            float damageEndTime = -1,
            float attackCamFwdHitTime = -1,
            float comboEarlyTime = -1)
        {
            _attackLength = attackLength;
            _attackHitTime = attackHitTime;
            _damageStartTime = damageStartTime;
            _damageEndTime = damageEndTime;
            _attackCamFwdHitTime = attackCamFwdHitTime;
            _comboEarlyTime = comboEarlyTime;
        }

        public void Apply(MeleeAttackData data)
        {
            if (_attackLength >= 0)
                data.m_attackLength = _attackLength;
            if (_attackHitTime >= 0)
                data.m_attackHitTime = _attackHitTime;
            if (_damageStartTime >= 0)
                data.m_damageStartTime = _damageStartTime;
            if (_damageEndTime >= 0)
                data.m_damageEndTime = _damageEndTime;
            if (_attackCamFwdHitTime >= 0)
                data.m_attackCamFwdHitTime = _attackCamFwdHitTime;
            if (_comboEarlyTime >= 0)
                data.m_comboEarlyTime = _comboEarlyTime;
        }

        private bool Approximately(float a, float b) => Math.Abs(a - b) <= 0.01f;

        public bool Equals(MeleeAttackData data)
        {
            if (_attackLength >= 0 && !Approximately(data.m_attackLength, _attackLength))
                return false;
            if (_attackHitTime >= 0 && !Approximately(data.m_attackHitTime, _attackHitTime))
                return false;
            if (_damageStartTime >= 0 && !Approximately(data.m_damageStartTime, _damageStartTime))
                return false;
            if (_damageEndTime >= 0 && !Approximately(data.m_damageEndTime, _damageEndTime))
                return false;
            if (_attackCamFwdHitTime >= 0 && !Approximately(data.m_attackCamFwdHitTime, _attackCamFwdHitTime))
                return false;
            if (_comboEarlyTime >= 0 && !Approximately(data.m_comboEarlyTime, _comboEarlyTime))
                return false;
            return true;
        }
    }
}
