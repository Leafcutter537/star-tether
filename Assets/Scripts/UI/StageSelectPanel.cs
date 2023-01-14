using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPanel : SelectPanel
{
    protected override void GetInventory()
    {
        itemList = new List<SelectChoice>();
        for (int i = 0; i < 14; i++)
        {
            itemList.Add(new StageSelectChoice(i));
        }
    }
}
