using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using WhatTheHack;

namespace WTHadditionalmodules;

internal class MainTabWindow_Work_MechanoidsNew : MainTabWindow_Work
{
    protected override IEnumerable<Pawn> Pawns => from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
        where p.IsHacked()
        select p;

    protected override PawnTableDef PawnTableDef => PawnTableDefOf.Work;
}