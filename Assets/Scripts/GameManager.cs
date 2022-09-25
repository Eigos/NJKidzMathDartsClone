using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("General Objects")]

    [SerializeField] GameObject mDestinationPointObject;

    [SerializeField] GameObject mDartObject;

    [SerializeField] GameObject mDartPoint;

    [SerializeField] GameObject[] mChartList;

    [Space(10)]


    [Header("Time Objects & Values")]

    [SerializeField] GameObject mRemainingTimeLevelText;

    [SerializeField] GameObject mRemainingTimeQuestionText;

    [SerializeField] GameObject mQuestionText;

    [SerializeField] float mDartDestinationDuration;

    [SerializeField] float mDartResetDelay;

    [SerializeField] float mQuestionDuration;

    [SerializeField] float mLevelTimeDuration;

    //Remaining time to end level
    private float mRemainingTimeDuration;

    //Remaining time for the next question
    private float mRemainingQuestionDuration;

    [Header("Score")]

    [SerializeField] float mScoreIncrementAmmount = 20;


    [HideInInspector]
    public QuestionType mCurrentQuestionType = QuestionType.Addition;

    private Vector3 mDartPositionBeforeThrown;

    private float mCurrentScore = 0;

    [Header("Global Data Object")]
    [SerializeField] GlobalData mGlobalData;

    [Header("Global Data Object")]
    [SerializeField] UIManager mUIManager;

    [Space(10)]

    [Header("Pause")]

    [SerializeField] bool mPauseTimer = false;

    struct Question
    {
        public int mAnswer;
        public int mFirstNubmer;
        public int mSecondNumber;
    }

    private Question mQuestion = new Question();

    // Start is called before the first frame update
    void Start()
    {
        UpdateQuestionType();
        UpdateCharts();
    }


    private void OnEnable()
    {
        UpdateQuestionType();
        UpdateCharts();
        ResetRemaniningTimeLevel();
        ResetRemainingTimeQuestion();
        ResetScore();
    }

    private void Awake()
    {
        UpdateQuestionType();
        UpdateCharts();
        ResetRemaniningTimeLevel();
        ResetRemainingTimeQuestion();
        ResetScore();
    }

    // Update is called once per frame
    void Update()
    {

        //Update timers
        if (!mPauseTimer)
        {
            UpdateRemainingTime();
            UpdateRemainingTimeDisplay();

            UpdateRemainingTimeQuestion();
            UpdateRemainingTimeQuestionDisplay();

            //If the question time is up reset question
            if (mRemainingQuestionDuration < 0)
            {
                ResetRemainingTimeQuestion();

                UpdateCharts();
            }

            if (mRemainingTimeDuration < 0)
            {
                ResetRemaniningTimeLevel();

                mGlobalData.mCurrentScore = mCurrentScore;

                mGlobalData.setHighScore(mCurrentScore);

                mUIManager.NextScene();
            }
        }

    }

    public void OnDartPressed()
    {
        if (mDestinationPointObject.GetComponent<DestinationPointColorUpdate>().getCurrentChart() == null)
        {
            return;
        }

        mDartObject.GetComponent<Image>().raycastTarget = false;

        //dont allow destination point move until reset
        mDestinationPointObject.GetComponent<SwipeControl>().enabled = false;

        Vector3 finalDestinationPosition = mDestinationPointObject.transform.position - mDartPoint.transform.localPosition;

        Sequence dartSequence = DOTween.Sequence();

        //Dart throw beginin to destination point
        dartSequence.Append(
            mDartObject.transform.DOMove(finalDestinationPosition, mDartDestinationDuration)
            .OnStart(() =>
            {
                mDartPositionBeforeThrown = mDartObject.transform.position;

                //Pause timer to prevent undesired chart update
                mPauseTimer = true;
            })
            .OnComplete(() =>
            {

                //if chart is set disable rotation
                if (mChartList.Length != 0)
                {
                    mChartList[0].transform.parent.GetComponent<RotationScript>().enabled = false;
                }


                //Check whether the dart point thrown to correct answer or not
                GameObject currentChart = mDestinationPointObject.GetComponent<DestinationPointColorUpdate>().getCurrentChart();

                if (currentChart != null)
                {
                    int chartValue = int.Parse(currentChart.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().GetParsedText());

                    //On correct answer
                    if (chartValue == mQuestion.mAnswer)
                    {
                        IncreaseScore();

                        mQuestionText.GetComponent<TextMeshProUGUI>().color = Color.green;

                    }

                    //On Incorrect answer
                    else
                    {
                        mQuestionText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                }


            })
            );

        //Reset delay
        dartSequence.AppendInterval(mDartResetDelay);

        //On restart
        dartSequence.AppendCallback(
            () =>
            {
                //Resume timer
                mPauseTimer = false;

                //Reset dart object
                mDartObject.GetComponent<Image>().raycastTarget = true;
                mDartObject.transform.position = mDartPositionBeforeThrown;

                //let dart destination point move
                mDestinationPointObject.GetComponent<SwipeControl>().enabled = true;

                //new Question and Update chart visual
                UpdateCharts();

                //restart chart rotation script
                if (mChartList.Length != 0)
                {
                    mChartList[0].transform.parent.GetComponent<RotationScript>().enabled = true;
                }

                //reset question timer
                ResetRemainingTimeQuestion();

                mQuestionText.GetComponent<TextMeshProUGUI>().color = Color.white;
            }
            );

    }

    #region CHARTS_AND_QUESTION
    private void UpdateQuestionType()
    {
        mCurrentQuestionType = mGlobalData.mCurrentQuestionType;
    }

    private void UpdateQuestionDisplay(Question question)
    {

        string questionSymbol = mCurrentQuestionType == QuestionType.Addition ? "+" : "-";
        string newText = question.mFirstNubmer + " " + questionSymbol + " " + question.mSecondNumber + " = ";

        mQuestionText.GetComponent<TextMeshProUGUI>().SetText(newText);
    }

    private Question GenerateQuestion(QuestionType type)
    {
        Question newQuestion = new Question();

        if (type == QuestionType.Addition)
        {
            newQuestion.mAnswer = Random.Range(3, 19);

            newQuestion.mFirstNubmer = Random.Range(0, newQuestion.mAnswer);

            newQuestion.mSecondNumber = newQuestion.mAnswer - newQuestion.mFirstNubmer;

        }
        else if (type == QuestionType.Subtraction)
        {
            newQuestion.mFirstNubmer = Random.Range(2, 10);

            newQuestion.mAnswer = Random.Range(0, newQuestion.mFirstNubmer - 1);

            newQuestion.mSecondNumber = newQuestion.mFirstNubmer - newQuestion.mAnswer;
        }

        return newQuestion;
    }

    private void UpdateCharts()
    {
        mQuestion = GenerateQuestion(mCurrentQuestionType);

        UpdateQuestionDisplay(mQuestion);

        List<int> numbers = new List<int>();

        numbers.Add(mQuestion.mAnswer);

        for (int i = 0; i < 2;)
        {
            int newValue = Random.Range(1, 4);

            newValue = (Random.Range(2, 100) % 2) == 0 ? newValue -= mQuestion.mAnswer : newValue += mQuestion.mAnswer;

            if (newValue <= 1)
                continue;

            if (newValue == mQuestion.mAnswer)
                continue;


            if (numbers.Count > 1)
            {
                if (numbers[1] == newValue)
                {
                    continue;
                }
            }

            numbers.Add(newValue);
            i++;
        }

        int[] numberArr = numbers.ToArray();

        Utils.Shuffle(numberArr);

        UpdateChartsDisplay(numberArr);

    }

    private void UpdateChartsDisplay(int[] numbers)
    {
        mChartList[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(numbers[0].ToString());
        mChartList[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(numbers[1].ToString());
        mChartList[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(numbers[2].ToString());

    }

    #endregion

    #region SCORE

    private void IncreaseScore()
    {
        mCurrentScore += mScoreIncrementAmmount;
    }

    public float getScore()
    {
        return mCurrentScore;
    }

    public void ResetScore()
    {
        mCurrentScore = 0;
    }

    #endregion

    #region INGAME_TIME
    private void ResetRemaniningTimeLevel()
    {
        mRemainingTimeDuration = mLevelTimeDuration;
    }

    private void UpdateRemainingTime()
    {
        mRemainingTimeDuration -= Time.deltaTime;
    }

    private void UpdateRemainingTimeDisplay()
    {
        mRemainingTimeLevelText.GetComponent<TextMeshProUGUI>().SetText(((int)mRemainingTimeDuration).ToString());
    }

    private void ResetRemainingTimeQuestion()
    {
        mRemainingQuestionDuration = mQuestionDuration;
    }

    private void UpdateRemainingTimeQuestion()
    {
        mRemainingQuestionDuration -= Time.deltaTime;
    }


    private void UpdateRemainingTimeQuestionDisplay()
    {
        mRemainingTimeQuestionText.GetComponent<TextMeshProUGUI>().SetText(((int)mRemainingQuestionDuration).ToString());
    }

    private string TimeToString(int seconds)
    {
        int hour = seconds / (60 * 60);
        int minute = seconds / 60;
        int second = seconds % 60;

        string newTimeStr = "";

        if (hour != 0)
        {
            newTimeStr += hour.ToString() + ":" + minute.ToString() + ":" + second.ToString();
        }
        else if (minute != 0)
        {
            newTimeStr += minute.ToString() + ":" + second.ToString();
        }
        else
        {
            newTimeStr += second.ToString();
        }

        return newTimeStr;
    }

    #endregion
}
