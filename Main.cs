using System;
using System.Collections.Generic;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.Bridge;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using RandomPathBlocker;

[assembly:
    MelonInfo(typeof(RandomPathBlocker.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace RandomPathBlocker;

[HarmonyPatch]
public class Main : BloonsTD6Mod
{
    private readonly Random _random = new();
    private static readonly Dictionary<ObjectId, int> TowerPathCount = new();
    public override void OnTowerSaved(Tower tower, TowerSaveDataModel saveData)
    {
        saveData.metaData["RandomPath"] = TowerPathCount[tower.Id].ToString();
    }
    public override void OnTowerLoaded(Tower tower, TowerSaveDataModel saveData)
    {
        TowerPathCount[tower.Id] = int.Parse(saveData.metaData["RandomPath"]);
    }
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        TowerPathCount[tower.Id] = _random.Next(0, 3);
    }
    [HarmonyPatch(typeof(TowerSelectionMenu), nameof(TowerSelectionMenu.SelectTower))]
    [HarmonyPostfix]
    private static void TowerSelectionMenu_TowerSelect(TowerSelectionMenu __instance,TowerToSimulation tower)
    {
        for (var i = 0; i < 3; i++)
        {
            __instance.upgradeButtons[i].gameObject.SetActive(false);
        }
        __instance.upgradeButtons[TowerPathCount[tower.Id]].gameObject.SetActive(true);
    }
}