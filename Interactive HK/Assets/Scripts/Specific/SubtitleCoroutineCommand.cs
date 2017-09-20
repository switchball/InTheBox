using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleCoroutineCommand : CoroutineCommandBehavior
{
    public List<string> subtitleTexts = new List<string>();

    public List<SubtitleTransitionCommand> commandSequence;

    protected override IEnumerator CoroutineCommand()
    {
        foreach (SubtitleTransitionCommand command in commandSequence)
            yield return StartCoroutine(command.ExecuteCoroutine());
    }

    void Reset()
    {
        commandSequence = new List<SubtitleTransitionCommand>(GetComponentsInChildren<SubtitleTransitionCommand>(true));
        subtitleTexts = new List<string>();
        foreach (SubtitleTransitionCommand command in commandSequence)
        {
            subtitleTexts.Add(command.subtitle);
        }
    }

    [ContextMenu("Apply Change to Children")]
    private void ApplyChangeToChildren()
    {
        int index = 0;
        foreach (SubtitleTransitionCommand command in commandSequence)
        {
            if (index < subtitleTexts.Count)
                command.subtitle = subtitleTexts[index];
            index++;
        }
    }
}
