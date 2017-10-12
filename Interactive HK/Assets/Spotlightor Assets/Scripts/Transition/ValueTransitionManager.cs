using UnityEngine;
using System.Collections;

public abstract class ValueTransitionManager : TransitionManager
{
	public float durationIn = 0.8f;
	public float durationOut = 0.5f;
	public iTween.EaseType easeIn = iTween.EaseType.easeOutCubic;
	public iTween.EaseType easeOut = iTween.EaseType.easeOutCubic;
	private float progressValue = 0;
	private Tweener progressTweener;
	private bool isTweeningProgress = false;

	private Tweener ProgressTweener {
		get {
			if (progressTweener == null)
				progressTweener = new Tweener (0, 1, durationIn, easeIn);
			return progressTweener;
		}
	}

	public float ProgressValue {
		get { return progressValue; }
		set {
			this.progressValue = value;
			OnProgressValueUpdated (this.progressValue);
		}
	}

	protected abstract void OnProgressValueUpdated (float progress);

	protected override void DoTransitionIn (bool instant, StateTypes prevStateType)
	{
        if (instant) {
			isTweeningProgress = false;
			ProgressValue = 1;
			TransitionInComplete ();
		} else {
			ProgressTweener.from = ProgressValue;
			ProgressTweener.to = 1;
			ProgressTweener.EaseType = easeIn;
			ProgressTweener.time = durationIn;
			ProgressTweener.TimeElapsed = 0;

			if (ProgressTweener.IsCompleted) {
				TransitionInComplete ();
				isTweeningProgress = false;
			} else
				isTweeningProgress = true;
		}
	}

	protected override void DoTransitionOut (bool instant, StateTypes prevStateType)
	{
        Debug.Log("ValueTM: TransitionOut(bool) " + instant + "," + prevStateType);

        if (instant) {
			isTweeningProgress = false;
			ProgressValue = 0;
			TransitionOutComplete ();
		} else {
			ProgressTweener.from = ProgressValue;
			ProgressTweener.to = 0;
			ProgressTweener.EaseType = easeOut;
			ProgressTweener.time = durationOut;
			ProgressTweener.TimeElapsed = 0;

			if (ProgressTweener.IsCompleted) {
				TransitionOutComplete ();
				isTweeningProgress = false;
			} else
				isTweeningProgress = true;
		}
	}

	protected virtual void Update ()
	{
		if (isTweeningProgress) {
			ProgressTweener.TimeElapsed += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
			this.ProgressValue = ProgressTweener.Value;

			if (ProgressTweener.IsCompleted) {
				isTweeningProgress = false;
				if (this.State == StateTypes.TransitionIn)
					TransitionInComplete ();
				else if (this.State == StateTypes.TransitionOut)
					TransitionOutComplete ();
			}
		}
	}
}
