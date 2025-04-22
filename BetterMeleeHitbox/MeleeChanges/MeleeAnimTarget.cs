using Gear;

namespace BMH.MeleeChanges
{
    public sealed class MeleeAnimTarget
    {
        public readonly eMeleeWeaponState State;
        public readonly MeleeAnimData Target;

        public MeleeAnimTarget(eMeleeWeaponState state, MeleeAnimData target)
        {
            State = state;
            Target = target;
        }
    }
}
