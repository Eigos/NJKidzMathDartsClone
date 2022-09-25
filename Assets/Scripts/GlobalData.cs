using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public QuestionType mCurrentQuestionType;

    public float mCurrentScore;

    private float mHighScore;

    public void SetScore(float newScore)
    {
        mCurrentScore = newScore;
    }

    public void SetQuestionType(QuestionType type)
    {
        mCurrentQuestionType = type;
    }

    public void SetQuestionTypeSubtraction()
    {
        mCurrentQuestionType = QuestionType.Subtraction;
    }

    public void SetQuestionTypeAddition()
    {
        mCurrentQuestionType = QuestionType.Addition;
    }

    public float getHighScore()
    {
        return mHighScore;
    }

    public void setHighScore(float newScore)
    {
        if (newScore > mHighScore)
            mHighScore = newScore;
    }


}

public enum QuestionType
{
    Addition,
    Subtraction
}
