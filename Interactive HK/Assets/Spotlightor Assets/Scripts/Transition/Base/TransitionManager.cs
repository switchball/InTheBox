using UnityEngine;
using System.Collections;

public abstract class TransitionManager : MonoBehaviour, ITransition
{
	public enum StateTypes
	{
		Unkown,
		In,
		TransitionIn,
		Out,
		TransitionOut
	}

	public class TimeDelayCoroutinePlayer : MonoBehaviour
	{
		
	}

	public delegate void GenericEventHandler (TransitionManager source);

	public delegate void TransitionBegunEventHandler (TransitionManager source, bool isInstant, StateTypes prevStateType);

	public event TransitionBegunEventHandler TransitionInTriggered;
	public event TransitionBegunEventHandler TransitionInStarted;
	public event GenericEventHandler TransitionInCompleted;
	public event TransitionBegunEventHandler TransitionOutTriggered;
	public event TransitionBegunEventHandler TransitionOutStarted;
	public event GenericEventHandler TransitionOutCompleted;

	private static TimeDelayCoroutinePlayer coroutinePlayer;

	private static TimeDelayCoroutinePlayer CoroutinePlayer {
		get {
			if (coroutinePlayer == null) {
				GameObject go = new GameObject ("TransitionManager TimeDelayCoroutinePlayer");
				coroutinePlayer = go.AddComponent<TimeDelayCoroutinePlayer> ();
				DontDestroyOnLoad (coroutinePlayer);
			}
			return coroutinePlayer;
		}
	}

	public bool outWhenAwake = true;
	public bool autoActivate = true;
	[Space (10)]
	public float delayIn = 0;
	public float delayOut = 0;
	public bool ignoreTimeScale = false;
	private StateTypes state = StateTypes.Unkown;
	private StateTypes prevState = StateTypes.Unkown;
	private IEnumerator timeDelayRoutine = null;

	public bool IsInTransition {
		get { return State == StateTypes.TransitionIn || State == StateTypes.TransitionOut; }
	}

	public bool IsOutOrUnkown {
		get { return State == StateTypes.Out || State == StateTypes.Unkown; }
	}

	public StateTypes State { 
		get { return state; } 
		private set { 
			if (state != value) {
				prevState = state;
				state = value; 
			}
		}
	}

	protected virtual void Awake ()
	{
		if (outWhenAwake && State == StateTypes.Unkown)
			TransitionOut (true);
	}

	protected virtual void OnEnable ()
	{
		if (State == StateTypes.Out && autoActivate)
			gameObject.SetActive (false);
	}

	protected virtual void OnDisable ()
	{
		if (State == StateTypes.TransitionIn || State == StateTypes.TransitionOut)
			State = StateTypes.Unkown;

		StopAllDelayedTransition ();
	}

	public IEnumerator TransitionInCoroutine ()
	{
		TransitionIn (false);
		
		while (State != StateTypes.In)
			yield return null;
	}

	public IEnumerator TransitionInCoroutine (bool instant)
	{
		TransitionIn (instant);

		while (State != StateTypes.In)
			yield return null;
	}

	public void TransitionIn ()
	{
		TransitionIn (false);
	}

	public void TransitionIn (bool instant)
	{
        Debug.Log("TM: TransitionIn(bool)" + instant);
        if (outWhenAwake && State == StateTypes.Unkown)
			TransitionOut (true);
		
		switch (State) {
		case StateTypes.In:
			{
				TransitionInComplete ();
				break;
			}
		case StateTypes.TransitionIn:
			{
				if (instant) {
					if (autoActivate)
						gameObject.SetActive (true);

					if (timeDelayRoutine != null) {
						if (TransitionInStarted != null)
							TransitionInStarted (this, true, prevState);
					}

					StopAllDelayedTransition ();

					DoTransitionIn (true, prevState);
				}
				break;
			}
		case StateTypes.Unkown:
		case StateTypes.Out:
		case StateTypes.TransitionOut:
			{
				StopAllDelayedTransition ();

				State = StateTypes.TransitionIn;
				
				if (instant || delayIn <= 0) {
					if (autoActivate)
						gameObject.SetActive (true);

					if (TransitionInTriggered != null)
						TransitionInTriggered (this, instant, prevState);

					if (TransitionInStarted != null)
						TransitionInStarted (this, instant, prevState);

					DoTransitionIn (instant, prevState);
				} else {
					if (TransitionInTriggered != null)
						TransitionInTriggered (this, instant, prevState);

					timeDelayRoutine = DoTransitionInAfterDelay ();
					CoroutinePlayer.StartCoroutine (timeDelayRoutine);
				}
				
				break;
			}
		}
	}

	private IEnumerator DoTransitionInAfterDelay ()
	{
		float timeElapsed = 0;
		while (timeElapsed < delayIn) {
			yield return null;
			float deltaTime = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
			timeElapsed += deltaTime;
		}
			
		if (autoActivate)
			gameObject.SetActive (true);

		if (TransitionInStarted != null)
			TransitionInStarted (this, false, prevState);

		DoTransitionIn (false, prevState);
	}

	public IEnumerator TransitionOutCoroutine ()
	{
		TransitionOut (false);
		
		while (State != StateTypes.Out)
			yield return null;
	}

	public IEnumerator TransitionOutCoroutine (bool instant)
	{
		TransitionOut (instant);
		
		while (State != StateTypes.Out)
			yield return null;
	}

	public void TransitionOut ()
	{
		TransitionOut (false);
	}

	public void TransitionOut (bool instant)
	{
        Debug.Log("TM: TransitionOut(bool)" + instant);
		if (!outWhenAwake && State == StateTypes.Unkown)
			TransitionIn (true);

		switch (State) {
		case StateTypes.Out:
			{
				TransitionOutComplete ();
				break;
			}
		case StateTypes.TransitionOut:
			{
				if (instant) {
					if (timeDelayRoutine != null) {
						if (TransitionOutStarted != null)
							TransitionOutStarted (this, true, prevState);
					}
				
					StopAllDelayedTransition ();
				
					DoTransitionOut (true, prevState);
				}
				break;
			}
		case StateTypes.Unkown:
		case StateTypes.In:
		case StateTypes.TransitionIn:
			{
				if (state == StateTypes.Unkown)
					instant = true;
				
				StopAllDelayedTransition ();
				State = StateTypes.TransitionOut;

				if (TransitionOutTriggered != null)
					TransitionOutTriggered (this, instant, prevState);

				if (instant || delayOut <= 0) {
					if (TransitionOutStarted != null)
						TransitionOutStarted (this, instant, prevState);

					DoTransitionOut (instant, prevState);
				} else {
					timeDelayRoutine = DoTransitionOutAfterDelay ();
					CoroutinePlayer.StartCoroutine (timeDelayRoutine);
				}

				break;
			}
		}
	}

	private IEnumerator DoTransitionOutAfterDelay ()
	{
		float timeElapsed = 0;
		while (timeElapsed < delayOut) {
			yield return null;
			float deltaTime = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
			timeElapsed += deltaTime;
		}

		if (TransitionOutStarted != null)
			TransitionOutStarted (this, false, prevState);
		
		DoTransitionOut (false, prevState);
	}

	private void StopAllDelayedTransition ()
	{
		if (timeDelayRoutine != null)
			CoroutinePlayer.StopCoroutine (timeDelayRoutine);
	}

	/// <summary>
	/// Call me when transition in completed.
	/// </summary>
	protected void TransitionInComplete ()
	{
		State = StateTypes.In;
		if (TransitionInCompleted != null)
			TransitionInCompleted (this);
	}

	/// <summary>
	/// Call me when transition out completed
	/// </summary>
	protected void TransitionOutComplete ()
	{
		State = StateTypes.Out;
		
		if (autoActivate)
			gameObject.SetActive (false);
		
		if (TransitionOutCompleted != null)
			TransitionOutCompleted (this);
	}

	protected abstract void DoTransitionIn (bool instant, StateTypes prevStateType);

	protected abstract void DoTransitionOut (bool instant, StateTypes prevStateType);

}
