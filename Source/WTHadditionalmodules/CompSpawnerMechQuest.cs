using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace WTHadditionalmodules;

public class CompSpawnerMechQuest : ThingComp
{
    private int ticksUntilSpawn;

    public CompProperties_MechQuest PropsSpawner => (CompProperties_MechQuest)props;

    private bool PowerOn => parent.GetComp<CompPowerTrader>()?.PowerOn ?? false;

    public int RandomNumber(int min, int max)
    {
        var random = new Random();
        return random.Next(min, max);
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        if (!respawningAfterLoad)
        {
            ResetCountdown();
        }
    }

    public override void CompTick()
    {
        TickInterval(1);
    }

    public override void CompTickRare()
    {
        TickInterval(250);
    }

    private void TickInterval(int interval)
    {
        if (!parent.Spawned)
        {
            return;
        }

        var comp = parent.GetComp<CompCanBeDormant>();
        if (comp != null)
        {
            if (!comp.Awake)
            {
                return;
            }
        }
        else if (parent.Position.Fogged(parent.Map))
        {
            return;
        }

        if (PropsSpawner.requiresPower && !PowerOn)
        {
            return;
        }

        ticksUntilSpawn -= interval;
        CheckShouldSpawn();
    }

    private void CheckShouldSpawn()
    {
        if (ticksUntilSpawn > 0)
        {
            return;
        }

        TryDoSpawn();
        ResetCountdown();
    }

    public void TryDoSpawn()
    {
        if (RandomNumber(1, 3) == 2)
        {
            var vars = new Slate();
            var quest = QuestUtility.GenerateQuestAndMakeAvailable(
                IncidentDefOfMechQuest.WTH_LongRangeMineralScannerMechParts, vars);
            Find.LetterStack.ReceiveLetter(quest.name, quest.description, LetterDefOf.PositiveEvent, null, null, quest);
        }
        else
        {
            var vars2 = new Slate();
            var quest2 =
                QuestUtility.GenerateQuestAndMakeAvailable(IncidentDefOfMechQuest.WTH_RoamingMechanoidsEncounterQuest,
                    vars2);
            Find.LetterStack.ReceiveLetter(quest2.name, quest2.description, LetterDefOf.PositiveEvent, null, null,
                quest2);
        }
    }

    private void ResetCountdown()
    {
        ticksUntilSpawn = PropsSpawner.spawnIntervalRange.RandomInRange;
    }

    public override void PostExposeData()
    {
        var text = PropsSpawner.saveKeysPrefix.NullOrEmpty() ? null : $"{PropsSpawner.saveKeysPrefix}_";
        Scribe_Values.Look(ref ticksUntilSpawn, $"{text}ticksUntilSpawn");
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        if (Prefs.DevMode)
        {
            yield return new Command_Action
            {
                defaultLabel = "DEBUG: Spawn ",
                icon = TexCommand.DesirePower,
                action = delegate
                {
                    TryDoSpawn();
                    ResetCountdown();
                }
            };
        }
    }
}