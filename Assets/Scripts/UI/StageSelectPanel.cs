using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPanel : SelectPanel
{

    [SerializeField] private Color inactiveColor;

    protected override void GetInventory()
    {
        itemList = new List<SelectChoice>();
        for (int i = 0; i < 14; i++)
        {
            itemList.Add(new StageSelectChoice(i));
        }
        int lastCompletedStage = GetHighestLevelIndex();
        for (int j = lastCompletedStage; j < 14; j++)
        {
            selectPanelChoices[j].SetDefaultColor(inactiveColor);
            Destroy(selectPanelChoices[j]);
        }
    }

    public static int GetHighestLevelIndex()
    {
        if (PlayerPrefs.HasKey("Highest Level"))
        {
            return PlayerPrefs.GetInt("Highest Level");
        }
        else
        {
            return 1;
        }
    }
}
