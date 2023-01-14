using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectChoice : SelectChoice
{
    public int stageIndex;

    public StageSelectChoice(int stageIndex)
    {
        this.stageIndex = stageIndex;
    }

    public override string GetTitle()
    {
        return (stageIndex + 1).ToString();
    }
}
