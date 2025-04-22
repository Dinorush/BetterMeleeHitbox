using Gear;
using HarmonyLib;
using BMH.Dependencies;
using BMH.MeleeChanges;

namespace BMH.Patches
{
    [HarmonyPatch(typeof(MeleeWeaponFirstPerson))]
    internal static class MeleeSetupPatches
    {
        [HarmonyPatch(nameof(MeleeWeaponFirstPerson.SetupMeleeAnimations))]
        [HarmonyAfter(MSCWrapper.GUID)]
        [HarmonyWrapSafe]
        [HarmonyPostfix]
        private static void Post_MeleeSetup(MeleeWeaponFirstPerson __instance)
        {
            var prefabs = __instance.ItemDataBlock.FirstPersonPrefabs;
            if (prefabs == null || prefabs.Count == 0) return;

            MeleeChangeData.ChangeDatas[prefabs[0]].Apply(__instance);
        }
    }
}
