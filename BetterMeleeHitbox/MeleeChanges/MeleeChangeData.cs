using Gear;
using MSC.CustomMeleeData;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BMH.MeleeChanges
{
    public sealed class MeleeChangeData
    {
        private readonly List<(MeleeAnimTarget target, MeleeAnimData data)>? _animDatas;
        private readonly (MeleeArchData target, MeleeArchData data)? _archData;
        private readonly MeleeData? _hitboxData;

        public MeleeChangeData(List<(MeleeAnimTarget target, MeleeAnimData anim)>? animDatas = null, (MeleeArchData target, MeleeArchData arch)? archData = null, MeleeData? hitboxData = null)
        {
            _animDatas = animDatas;
            _archData = archData;
            _hitboxData = hitboxData;
        }

        public bool TryGetHitboxData([MaybeNullWhen(false)] out MeleeData hitboxData)
        {
            if (_hitboxData == null)
            {
                hitboxData = null;
                return false;
            }
            hitboxData = _hitboxData;
            return true;
        }

        public void Apply(MeleeWeaponFirstPerson melee)
        {
            var arch = melee.MeleeArchetypeData;
            if (_archData != null && _archData.Value.target.Equals(arch))
                _archData.Value.data.Apply(arch);

            if (_animDatas == null) return;

            var states = melee.m_states;
            foreach ((var animTarget, _) in _animDatas)
            {
                var data = states[(int)animTarget.State].m_data;
                if (!animTarget.Target.Equals(data))
                    return;
            }

            foreach ((var animTarget, var animData) in _animDatas)
            {
                var data = states[(int)animTarget.State].m_data;
                animData.Apply(data);
            }
        }

        public readonly static Dictionary<string, MeleeChangeData> ChangeDatas = new()
        {
            {
                "Assets/AssetPrefabs/Items/Melee/MeleeWeaponFirstPerson.prefab",
                new(animDatas: null,
                    archData: (new(cameraDamageRayLength:1.8f), new(cameraDamageRayLength:1.9f)),
                    hitboxData: new()
                    {
                        AttackOffset = new(new(0, -0.12f, 0.55f), new(0, 0f, -0.3f))
                        {
                            CapsuleSize = 0.05f,
                            CapsuleUseCamFwd = true,
                            CapsuleCamFwdAdd = -1.3f,
                            CapsuleDelay = 0.1f
                        },
                        LightAttackSpeed = 1.3f,
                        AttackSphereCenterMod = 1.5f
                    }
                )
            },
            {
                "Assets/AssetPrefabs/Items/Melee/MeleeWeaponFirstPersonKnife.prefab",
                new(animDatas: new()
                    {
                        (
                        new(eMeleeWeaponState.AttackMissRight, new(attackLength:1.0f, attackHitTime:0.1667f,  damageStartTime:0.1f, damageEndTime:0.2667f, attackCamFwdHitTime:0.1667f, comboEarlyTime: 0.5f)),
                        new(damageStartTime:0.1167f, attackCamFwdHitTime:0.1167f, comboEarlyTime: 0.4667f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackMissLeft, new(attackLength:1.0f, attackHitTime:0.1667f,  damageStartTime:0.1f, damageEndTime:0.2667f, attackCamFwdHitTime:0.1667f, comboEarlyTime: 0.5f)),
                        new(damageStartTime:0.1167f, attackCamFwdHitTime:0.1167f, comboEarlyTime: 0.4667f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackHitRight, new(attackLength:1.0f, attackHitTime:0.1667f, comboEarlyTime: 0.3f)),
                        new(attackHitTime: 0.1167f, comboEarlyTime: 0.3333f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackHitLeft, new(attackLength:1.0f, attackHitTime:0.1667f, comboEarlyTime: 0.3f)),
                        new(attackHitTime: 0.1167f, comboEarlyTime: 0.3333f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackChargeReleaseRight, new(attackLength:1.0f, attackHitTime:0.1667f, damageStartTime:0f, damageEndTime:0.3667f, attackCamFwdHitTime:0.1667f, comboEarlyTime: 0.3f)),
                        new(attackHitTime:0.1f, damageStartTime:0.08f, attackCamFwdHitTime:0.1f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackChargeReleaseLeft, new(attackLength:1.0f, attackHitTime:0.1667f, damageStartTime:0f, damageEndTime:0.3667f, attackCamFwdHitTime:0.1667f, comboEarlyTime: 0.3f)),
                        new(attackHitTime:0.1f, damageStartTime:0.0667f, attackCamFwdHitTime:0.1f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackChargeHitRight, new(attackLength:1f, attackHitTime:0.1334f, comboEarlyTime: 0.3f)),
                        new(comboEarlyTime: 0.3333f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackChargeHitLeft, new(attackLength:1f, attackHitTime:0.1334f, comboEarlyTime: 0.3f)),
                        new(comboEarlyTime: 0.3333f)
                        )
                    },
                    archData: (new(cameraDamageRayLength:1.75f, attackSphereRadius:0.25f), new(cameraDamageRayLength:2f, attackSphereRadius:0.15f)),
                    hitboxData: new()
                    {
                        AttackOffset = new(new(0, 0.35f, 0.076f), new(0, -0.4f, 0f))
                        {
                            CapsuleSize = 0.05f,
                            CapsuleUseCamFwd = true,
                            CapsuleCamFwdAdd = -1.7f,
                            CapsuleStateDelay = new()
                            {
                                { eMeleeWeaponState.AttackChargeReleaseLeft, 0.05f },
                                { eMeleeWeaponState.AttackChargeReleaseRight, 0.05f }
                            }
                        },
                        AttackSphereCenterMod = 1f
                    }
                )
            },
            {
                "Assets/AssetPrefabs/Items/Melee/MeleeWeaponFirstPersonSpear.prefab",
                new(animDatas: null,
                    hitboxData: new()
                    {
                        AttackOffset = new(new(0, 1.1f, -0.002f), new(0f, -0.3f, 0f))
                        {
                            CapsuleSize = 0.05f,
                            CapsuleUseCamFwd = true,
                            CapsuleCamFwdAdd = -2.2f,
                            CapsuleDelay = 0.1f
                        }
                    }
                )
            },
            {
                "Assets/AssetPrefabs/Items/Melee/MeleeWeaponFirstPersonBat.prefab",
                new(animDatas: new()
                    {
                        (
                        new(eMeleeWeaponState.AttackMissRight, new(attackLength:1.3334f, attackHitTime:0.2334f,  damageStartTime:0.1334f, damageEndTime:0.667f, comboEarlyTime: 0.5f)),
                        new(attackLength:0.6667f, damageStartTime:0.2f, damageEndTime:0.6f, comboEarlyTime: 0.45f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackMissLeft, new(attackLength:1.3334f, attackHitTime:0.2334f, damageStartTime:0.1334f, damageEndTime:0.667f, comboEarlyTime: 0.5f)),
                        new(attackLength:0.6667f, damageStartTime:0.2f, damageEndTime:0.6f, comboEarlyTime: 0.45f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackHitRight, new(attackLength:1.3334f, attackHitTime:0.2334f, comboEarlyTime: 0.4f)),
                        new(comboEarlyTime: 0.33f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackHitLeft, new(attackLength:1.3334f, attackHitTime:0.2334f, comboEarlyTime: 0.4f)),
                        new(comboEarlyTime: 0.33f)
                        ),
                        (
                        new(eMeleeWeaponState.Push, new(attackLength:1.1667f, attackHitTime:0.2334f, damageStartTime:0.2f, damageEndTime:0.2667f, attackCamFwdHitTime:0.2334f, comboEarlyTime: 0.667f)),
                        new(damageStartTime:0.1667f, attackCamFwdHitTime:0.1667f, comboEarlyTime: 0.467f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackChargeReleaseRight, new(attackLength:0.7f, attackHitTime:0.2334f, damageStartTime:0f, damageEndTime:0.3667f, attackCamFwdHitTime: 0.2334f, comboEarlyTime: 0.3f)),
                        new(comboEarlyTime: 0.4f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackChargeReleaseLeft, new(attackLength:0.7f, attackHitTime:0.2334f, damageStartTime:0f, damageEndTime:0.3667f, attackCamFwdHitTime: 0.2334f, comboEarlyTime: 0.3f)),
                        new(comboEarlyTime: 0.4f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackChargeHitRight, new(attackLength:1.334f, attackHitTime:0.2334f, damageStartTime:0f, damageEndTime:0.0f, attackCamFwdHitTime: 0f, comboEarlyTime: 0.4f)),
                        new(comboEarlyTime: 0.3333f)
                        ),
                        (
                        new(eMeleeWeaponState.AttackChargeHitLeft, new(attackLength:1.334f, attackHitTime:0.2334f, damageStartTime:0f, damageEndTime:0.0f, attackCamFwdHitTime: 0f, comboEarlyTime: 0.4f)),
                        new(comboEarlyTime: 0.3333f)
                        )
                    },
                    archData: (new(cameraDamageRayLength:1.75f), new(cameraDamageRayLength:1.85f)),
                    hitboxData: new()
                    {
                        AttackOffset = new(new UnityEngine.Vector3(0, 0.6f, 0), new(0, -0.2f, -0.1f), new(0, 0.55f, -0.1f))
                        {
                            CapsuleSize = 0.05f,
                            CapsuleUseCamFwd = true,
                            CapsuleCamFwdAdd = -1.2f,
                            CapsuleDelay = 0.05f
                        },
                        AttackSphereCenterMod = 1f
                    }
                )
            }
        };
    }
}
