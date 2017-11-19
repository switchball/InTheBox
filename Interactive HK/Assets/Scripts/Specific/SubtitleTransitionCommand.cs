using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SubtitleTransitionCommand : CoroutineCommandBehavior
{

    public string subtitle;
    public string subtitleTag = "SubstitleText";
    private GenericTextDisplayer subtitleDisplayer;

    public enum TransitionTypes
    {
        In,
        Out,
        InAndOut,
    }

    private TransitionManager transition;
    public TransitionTypes transitionType = TransitionTypes.InAndOut;
    public float inAndOutWaitTime = 2;
    [UnityEngine.Serialization.FormerlySerializedAs("waitForTransitionOut")]
    public bool waitTransitionComplete = true;

    private void OnEnable()
    {
        GameObject x = GameObject.FindGameObjectWithTag(subtitleTag);
        if (x != null)
        {
            subtitleDisplayer = x.GetComponent<GenericTextDisplayer>();
            transition = x.GetComponent<TransitionManager>();
        }
    }

    protected override IEnumerator CoroutineCommand()
    {
        // set text before transition
        ChangeSubtitle();

        if (transitionType == TransitionTypes.In || transitionType == TransitionTypes.InAndOut)
        {
            transition.TransitionIn();
            if (waitTransitionComplete)
            {
                while (transition.State != TransitionManager.StateTypes.In)
                    yield return null;
            }
        }

        if (transitionType == TransitionTypes.InAndOut)
            yield return new WaitForSeconds(inAndOutWaitTime);

        if (transitionType == TransitionTypes.Out || transitionType == TransitionTypes.InAndOut)
        {
            transition.TransitionOut();
            if (waitTransitionComplete)
            {
                while (transition.State != TransitionManager.StateTypes.Out)
                    yield return null;
            }
        }
    }
	
    public void ChangeSubtitle()
    {
        //subtitle = subtitle.Replace("\\n", "\n");
        GameObject x = GameObject.FindGameObjectWithTag(subtitleTag);
        subtitleDisplayer = x.GetComponent<GenericTextDisplayer>();
        subtitleDisplayer.Text = subtitle.Replace("|", "\n");
    }

    void Reset()
    {
        //commandSequence = new List<CoroutineCommandBehavior>(GetComponentsInChildren<CoroutineCommandBehavior>(true));
        //commandSequence.Remove(this);
    }
}
