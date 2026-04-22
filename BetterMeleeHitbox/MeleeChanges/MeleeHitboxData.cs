using GameData;
using Gear;
using ModifierAPI;
using MSC.CustomMeleeData;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BMH.MeleeChanges
{
    public sealed class MeleeHitboxData
    {
        private readonly float _targetCameraDamageRayLength;
        private readonly float _targetAttackSphereRadius;
        private readonly float _cameraDamageRayLength;
        private readonly float _attackSphereRadius;
        private readonly MeleeData _baseData;
        private readonly Dictionary<uint, (float ray, float radius, MeleeData data)> _seenArchs;
        private static readonly List<PropertyInfo> s_offsetProps = new();
        private const float RayConversionMod = 0.75f;
        private const float CapsuleConversionMod = 0.33f;

        public MeleeHitboxData(
            float targetCameraDamageRayLength,
            float targetAttackSphereRadius,
            float cameraDamageRayLength,
            float attackSphereRadius,
            MeleeData hitboxData)
        {
            _targetCameraDamageRayLength = targetCameraDamageRayLength;
            _targetAttackSphereRadius = targetAttackSphereRadius;
            _cameraDamageRayLength = cameraDamageRayLength;
            _attackSphereRadius = attackSphereRadius;
            _baseData = hitboxData;
            _seenArchs = new();
        }

        public bool TryGetMeleeData(MeleeWeaponFirstPerson melee, out MeleeData data)
        {
            var arch = melee.MeleeArchetypeData;
            if (_seenArchs.TryGetValue(arch.persistentID, out (float ray, float radius, MeleeData data) oldData))
            {
                if (Approximately(arch.CameraDamageRayLength, oldData.ray) && Approximately(arch.AttackSphereRadius, oldData.radius))
                {
                    data = oldData.data;
                    return true;
                }
            }

            float rayDiff = arch.CameraDamageRayLength - _targetCameraDamageRayLength;
            float sizeDiff = arch.AttackSphereRadius - _targetAttackSphereRadius;

            if (Approximately(rayDiff, 0) && Approximately(sizeDiff, 0))
            {
                _seenArchs[arch.persistentID] = (_cameraDamageRayLength, _attackSphereRadius, _baseData);
                MeleeRangeAPI.SetBaseRange(_cameraDamageRayLength);
                arch.AttackSphereRadius = _attackSphereRadius;
                data = _baseData;
                return true;
            }

            data = CreateCopyData();
            if (!Approximately(rayDiff, 0))
            {
                data.AttackOffset.EntityRayLengthAdd = Math.Max(0, data.AttackOffset.EntityRayLengthAdd + rayDiff * RayConversionMod);
                oldData.ray = Math.Max(0.2f, _cameraDamageRayLength + rayDiff * (1f - RayConversionMod));
            }
            else
                oldData.ray = _cameraDamageRayLength;

            if (!Approximately(sizeDiff, 0))
            {
                data.AttackOffset.EntitySize = Math.Max(0f, data.AttackOffset.EntitySize + sizeDiff);
                data.AttackOffset.CapsuleSize = Math.Max(0f, data.AttackOffset.CapsuleSize + sizeDiff * CapsuleConversionMod);
                oldData.radius = Math.Max(0f, _attackSphereRadius + sizeDiff * CapsuleConversionMod);
            }
            else
                oldData.radius = _attackSphereRadius;

            _seenArchs[arch.persistentID] = (oldData.ray, oldData.radius, data);
            MeleeRangeAPI.SetBaseRange(oldData.ray);
            arch.AttackSphereRadius = oldData.radius;
            return true;
        }

        private MeleeData CreateCopyData()
        {
            if (s_offsetProps.Count == 0)
            {
                var props = typeof(OffsetData).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                s_offsetProps.EnsureCapacity(props.Length);
                foreach (var prop in props)
                {
                    MethodInfo? mget = prop.GetGetMethod(false);
                    MethodInfo? mset = prop.GetSetMethod(false);
                    if (mget == null || mset == null || !prop.CanWrite) continue;

                    s_offsetProps.Add(prop);
                }
                s_offsetProps.TrimExcess();
            }

            MeleeData data = new()
            {
                LightAttackSpeed = _baseData.LightAttackSpeed,
                ChargedAttackSpeed = _baseData.ChargedAttackSpeed,
                PushSpeed = _baseData.PushSpeed,
                PushOffset = _baseData.PushOffset,
                AttackSphereCenterMod = _baseData.AttackSphereCenterMod
            };

            foreach (var prop in s_offsetProps)
                prop.SetValue(data.AttackOffset, prop.GetValue(_baseData.AttackOffset));
            return data;
        }

        private static bool Approximately(float a, float b) => Math.Abs(a - b) <= 0.01f;
    }
}
