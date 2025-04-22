using GameData;
using System;

namespace BMH.MeleeChanges
{
    public sealed class MeleeArchData
    {

        private readonly float _cameraDamageRayLength;
        private readonly float _attackSphereRadius;

        public MeleeArchData(
            float cameraDamageRayLength = -1,
            float attackSphereRadius = -1)
        {
            _cameraDamageRayLength = cameraDamageRayLength;
            _attackSphereRadius = attackSphereRadius;
        }

        public void Apply(MeleeArchetypeDataBlock data)
        {
            if (_cameraDamageRayLength >= 0)
                data.CameraDamageRayLength = _cameraDamageRayLength;
            if (_attackSphereRadius >= 0)
                data.AttackSphereRadius = _attackSphereRadius;
        }

        private bool Approximately(float a, float b) => Math.Abs(a - b) <= 0.01f;

        public bool Equals(MeleeArchetypeDataBlock data)
        {
            if (_cameraDamageRayLength >= 0 && !Approximately(data.CameraDamageRayLength, _cameraDamageRayLength))
                return false;
            if (_attackSphereRadius >= 0 && !Approximately(data.AttackSphereRadius, _attackSphereRadius))
                return false;
            return true;
        }
    }
}
