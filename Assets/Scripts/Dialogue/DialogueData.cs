using System.Collections.Generic;

[System.Serializable]
public class DialogueData 
{
    public int uid;
    public string text;
    public int selectionGroupNum;
    public int nextUid;
    public bool isEnd;
}

[System.Serializable]
public class DialogueSelectionData
{
    public int uid;
    public string text;
    public int nextDialogueUid;
    public int groupNum;
    public int groupIdx;
}