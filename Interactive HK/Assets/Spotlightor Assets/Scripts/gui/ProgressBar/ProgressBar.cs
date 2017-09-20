using UnityEngine;
using System.Collections;

public abstract class ProgressBar : MonoBehaviour
{
	private const string ProgressTweenName = "tween progress";
	public delegate void BasicEventHandler (ProgressBar progressBar);

	public delegate void TweenStartedEventHandler (ProgressBar progressBar,float progressFrom,float progressTo);

	public event TweenStartedEventHandler TweenStarted;
	public event BasicEventHandler TweenCompleted;

	public float maxTweenTime = 0.5f;
	private float currentProgress = 0;
	private Tweener progressTweener = new Tweener (0, 1, 0.5f, iTween.EaseType.linear);

	public float CurrentProgress {
		set {
			StopCoroutine ("TweenProgress");
			this.currentProgress = value;
			UpdateProgressDisplay (this.currentProgress);
		}
		get { return currentProgress;}
	}

	public void TweenProgressTo (float targetProgress)
	{
		float deltaProgress = Mathf.Abs (targetProgress - currentProgress);
		float tweenTime = Mathf.Max (0.5f * maxTweenTime, maxTweenTime * deltaProgress);
		TweenProgressTo (targetProgress, tweenTime, iTween.EaseType.easeInOutQuad);
	}

	public void TweenProgressTo (float targetProgress, float time, iTween.EaseType easeType)
	{
		if (TweenStarted != null)
			TweenStarted (this, CurrentProgress, targetProgress);

		progressTweener.from = this.CurrentProgress;
		progressTweener.to = targetProgress;
		progressTweener.time = time;
		progressTweener.TimeElapsed = 0;
		progressTweener.EaseType = easeType;
		StartCoroutine ("TweenProgress");
	}

	private IEnumerator TweenProgress ()
	{
		while (progressTweener.IsCompleted == false) {
			yield return null;
			progressTweener.TimeElapsed += Time.deltaTime;

			this.currentProgress = progressTweener.Value;
			UpdateProgressDisplay (this.currentProgress);
		}
		if (TweenCompleted != null)
			TweenCompleted (this);
	}

	protected abstract void UpdateProgressDisplay (float progress);
}
