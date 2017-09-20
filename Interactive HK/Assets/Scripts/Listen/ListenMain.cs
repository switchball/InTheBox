using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ListenQuestion {
	public AudioClip audio;
	public string question;
	public string correctAnswer;
	public string wrongAnswer1;
	public string wrongAnswer2;
    [HideInInspector]
    public string wrongAnswer3;

    /* read only*/
    [HideInInspector]
	public string[] ans; 
	int correctIndex;

    public void Shuffle3()
    {
        correctIndex = Random.Range(0, 3);
        ans = new string[] { wrongAnswer1, wrongAnswer2, correctAnswer };
        //		int ia = Random.Range (0, 3);
        Swap(ans, 0, Random.Range(0, 2));
        //		int ib = Random.Range (1, 3);
        Swap(ans, 2, correctIndex);
    }

    public void Shuffle4() {
		correctIndex = Random.Range (0, 4);
		ans = new string[]{wrongAnswer1, wrongAnswer2, wrongAnswer3, correctAnswer};
		//		int ia = Random.Range (0, 3);
		Swap (ans, 0, Random.Range (0, 3));
		//		int ib = Random.Range (1, 3);
		Swap (ans, 1, Random.Range (1, 3));
		Swap (ans, 3, correctIndex);
	}

	public int GetCorrectIndex() {
		return correctIndex;
	}

	private void Swap(string[] array, int a, int b) {
		if (a == b)
			return;
		string t = array[a];
		array[a] = array[b];
		array[b] = t;
	}

	public string AnswerText() {
		return string.Format ("A. {0}, B. {1}, C. {2}", 
			ans [0], ans [1], ans [2]);
	}
}

public class ListenMain : MonoBehaviour {

	/** The leading audio before the question audio. */
	public AudioClip promptAudio;

	//[UnityEngine.SerializeField]
	public List<ListenQuestion> questions;

	public List<Text> answerLocations;

	// public reference
	public Canvas wall;

	public float correctRatio = 1.0f;

	// referenced UI elements
	public Text questionText;
	public Text choiceText;
	public Text statusText;

	// referenced component
	AudioSource audioSource;

	// private variables
	int totalNumber;
	int rightNumber;
	int wrongNumber;

	bool hasCompleted;
	bool activeStatus;
	int ongoingIndex;
	bool clickSignal;

    bool triggerByHand = false; // 是否是手动触发（至少一次失败之后）

	// Use this for initialization
	void Start () {
		//questionText = GameObject.Find ("QuestionText").GetComponent<Text> ();
		//choiceText = GameObject.Find ("ChoiceText").GetComponent<Text>();
		//statusText = GameObject.Find ("StatusText").GetComponent<Text> ();
		audioSource = gameObject.GetComponent<AudioSource> ();




		ResetStatus ();
	}

	void ResetStatus () {
		totalNumber = questions.Count;
		rightNumber = 0;
		wrongNumber = 0;
		hasCompleted = false;
		activeStatus = false;
		ongoingIndex = 0;
		clickSignal = false;
	}

	public void Play () {
        //int correctIndex = Random.Range (0, 4);
        //string[] answerTexts = ShuffleAnswers (questions [0], correctIndex);
        //for (int k = 0; k < 4; k++) {
        //	((TextMesh)answerLocations [k].GetComponentInChildren<TextMesh> ()).text = answerTexts [k];
        //}
        //statusText.text = "Score: " + rightNumber + " / " + totalNumber;
        statusText.text = "O : 0    X : 0  ";
        StartCoroutine(PlayQuestionNo ());
        triggerByHand = true;
	}

	IEnumerator PlayQuestionNo() {
		if (hasCompleted)
			yield return null;
		activeStatus = true;
		if (rightNumber + wrongNumber < totalNumber) {
			int qID = ongoingIndex;
			Debug.Log ("Ready to play question: " + qID);
			questions [qID].Shuffle3 ();
			audioSource.clip = promptAudio;
			audioSource.Play ();
			yield return new WaitForSeconds (1);

			// Retrive question data
			ListenQuestion question = questions [qID];
			questionText.text = question.question.Replace("|", "\n");
			choiceText.text = question.AnswerText ();
            answerLocations[0].text = question.ans[0];
            answerLocations[1].text = question.ans[1];
            answerLocations[2].text = question.ans[2];
            //answerLocations[3].text = question.ans[3];

            audioSource.Stop ();
			audioSource.clip = questions [qID].audio;
			audioSource.Play ();
			yield return new WaitForSeconds (audioSource.clip.length + 0.5f);

			// wait
			/*while (!HasClicked()) {
				Debug.Log ("waiting for qid=" + ongoingIndex + clickSignal);
				yield return new WaitForSeconds (1);
			}*/

			//clickSignal = false;
		} else {
			if (rightNumber >= totalNumber * correctRatio - 1e-10) {
				// Completed
				hasCompleted = true;
				Debug.Log ("All question finished, completed, send onfinish request");
				audioSource.Stop ();
				questionText.text = "棒~ (｡･∀･)ﾉﾞ";
                answerLocations[0].text = " ";
                answerLocations[1].text = " ";
                answerLocations[2].text = " ";
                StateTrigger st = GameObject.Find("SecondRoomControl").GetComponent<StateTrigger>();
                st.OuterChangeState(true);
            }
            else {
				// reset
				rightNumber = 0;
				wrongNumber = 0;
				hasCompleted = false;
				activeStatus = false;
				ongoingIndex = 0;
				clickSignal = false;
				//statusText.text = " (FAIL) ";
                questionText.text = "小朋友不专心哦 ⸜(* ॑꒳ ॑* )⸝";
                answerLocations[0].text = " (*_*) ";
                answerLocations[1].text = "再来一次吧";
                answerLocations[2].text = " (*_*) ";
                audioSource.Stop ();
				Debug.Log ("All question finished, but not completed");
			}
			// after all
			activeStatus = false;
		}
		Debug.Log ("right=" + rightNumber + ", wrong=" + wrongNumber);


	}

	bool HasClicked() {
		return clickSignal;
	}

	void ConcludeQuestionNo(int qID, int choice) {
		if (choice == questions [qID].GetCorrectIndex())
			rightNumber++;
		else
			wrongNumber++;
		if (rightNumber + wrongNumber < totalNumber) {
			ongoingIndex++;
		}
		statusText.text = "O : " + rightNumber + "    X : " + wrongNumber + " ";
		this.StartCoroutine (PlayQuestionNo ());
	}
	
	public void ClickButtonAnswer(int which) {
		if (hasCompleted)
			return;
		if (!activeStatus) {
            // trigger has been modified by event 
            // only if trigger by hand
            if (triggerByHand)
			    Play ();
		} else {
			Debug.Log ("Clicked " + which); 
			clickSignal = true;
			ConcludeQuestionNo (ongoingIndex, which);
		}
	}

    /*
	string[] ShuffleAnswers(ListenQuestion q, int correctIndex) {
		string ac  = q.correctAnswer;
		string wa1 = q.wrongAnswer1;
		string wa2 = q.wrongAnswer2;
		string wa3 = q.wrongAnswer3;
		string[] ans = {wa1, wa2, wa3, ac};
//		int ia = Random.Range (0, 3);
		Swap (ans, 0, Random.Range (0, 3));
//		int ib = Random.Range (1, 3);
		Swap (ans, 1, Random.Range (1, 3));
		Swap (ans, 3, correctIndex);
		Debug.Log (string.Format("ans: A. {0}, B. {1}, C. {2}, D. {3}, [{4}]", 
			ans[0], ans[1], ans[2], ans[3], correctIndex));
		return ans;
	}

	private void Swap(string[] array, int a, int b) {
		if (a == b)
			return;
		string t = array[a];
		array[a] = array[b];
		array[b] = t;
	}*/
}
