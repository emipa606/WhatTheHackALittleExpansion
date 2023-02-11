using RimWorld;

namespace WTHadditionalmodules;

[DefOf]
public static class IncidentDefOfMechQuest
{
    public static QuestScriptDef WTH_LongRangeMineralScannerMechParts;

    public static QuestScriptDef WTH_RoamingMechanoidsEncounterQuest;

    static IncidentDefOfMechQuest()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(IncidentDefOf));
    }
}