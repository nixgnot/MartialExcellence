using MartialExcellence.Feats;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using UnityModManagerNet;
using MartialExcellence.RagePowers;
using MartialExcellence.Backgrounds;
using MartialExcellence.Races;
using Kingmaker.Blueprints.Root;
using MartialExcellence.Util;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using MartialExcellence.Races.Skinwalker;
using MartialExcellence.Races.Orc;
using MartialExcellence.Archetypes;


namespace MartialExcellence
{
    public static class Main
    {
        public static bool Enabled;
        private static readonly LogWrapper Logger = LogWrapper.Get("MartialExcellence");

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                modEntry.OnToggle = OnToggle;
                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll();
                Logger.Info("Finished patching.");
            }
            catch (Exception e)
            {
                Logger.Error("Failed to patch", e);
            }
            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        [HarmonyPatch(typeof(BlueprintsCache))]
        static class BlueprintsCaches_Patch
        {
            private static bool Initialized = false;

            [HarmonyPriority(Priority.First)]
            [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
            static void Init()
            {
                try
                {
                    if (Initialized)
                    {
                        Logger.Info("Already configured blueprints.");
                        return;
                    }
                    Initialized = true;

                    Logger.Info("Configuring race blueprints.");
                    // Configure Races
                    Skinwalker.Configure();
                    Orc.Configure();

                    Logger.Info("Configuring feat blueprints.");
                    // Configure Feats
                    DazingAssault.Configure();
                    StunningAssault.Configure();
                    ShieldSlam.Configure();
                    ViciousStomp.Configure();
                    RagingBrutality.Configure();
                    HandsOfValor.Configure();
                    ExtraFeature.Configure();
                    //KitsuneFCB.Configure();


                    Logger.Info("Configuring rage power blueprints.");
                    // Configure Rage Powers
                    ComeAndGetMe.Configure();
                    UnrestrainedRage.Configure();
                    //SpiritTotemLesser.Configure();

                    Logger.Info("Configuring background blueprints.");
                    // Configure Backgrounds
                    JungleExplorer.Configure();
                    //FeralChild.Configure();
                    //DesnaChosen.Configure();

                    Logger.Info("Configuring archetype blueprints.");
                    // Configure Archetypes
                    //Siegebreaker.Configure();

                }
                catch (Exception e)
                {
                    Logger.Error("Failed to configure blueprints.", e);
                }
            }
        }

        [HarmonyPatch(typeof(StartGameLoader))]
        static class StartGameLoader_Patch
        {
            private static bool Initialized = false;

            [HarmonyPatch(nameof(StartGameLoader.LoadPackTOC)), HarmonyPostfix]
            static void LoadPackTOC()
            {
                try
                {
                    if (Initialized)
                    {
                        Logger.Info("Already configured delayed blueprints.");
                        return;
                    }
                    Initialized = true;

                    RootConfigurator.ConfigureDelayedBlueprints();
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to configure delayed blueprints.", e);
                }
            }
        }
    }
}

