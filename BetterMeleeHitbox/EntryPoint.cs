using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using BMH.Dependencies;
using MSC.API;
using BMH.MeleeChanges;

namespace BMH
{
    [BepInPlugin("Dinorush." + MODNAME, MODNAME, "1.0.0")]
    [BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(MSCWrapper.GUID, BepInDependency.DependencyFlags.HardDependency)]
    internal sealed class EntryPoint : BasePlugin
    {
        public const string MODNAME = "BetterMeleeHitbox";

        public override void Load()
        {
            new Harmony(MODNAME).PatchAll();

            foreach ((var prefab, var changeData) in MeleeChangeData.ChangeDatas)
                if (changeData.TryGetHitboxData(out var hitboxData))
                {
                    DinoLogger.Log("Added hitbox data for prefab " + prefab);
                    MeleeDataAPI.AddData(prefab, hitboxData);
                }
            Log.LogMessage("Loaded " + MODNAME);
        }
    }
}