using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject[] mSceneList;

    private int mCurrentIndex = 0;

    private void Start()
    {
        if(mSceneList.Length == 0)
        {
            mCurrentIndex = -1;
        }
    }

    public void NextScene()
    {
        if (mCurrentIndex == -1)
            return;

        mSceneList[mCurrentIndex].gameObject.SetActive(false);

        if (mCurrentIndex < mSceneList.Length)
        {
            mCurrentIndex++;
        }
        else
        {
            mCurrentIndex = 0;
        }

        mSceneList[mCurrentIndex].gameObject.SetActive(true);

    }

    public void PreviousScene()
    {
        if (mCurrentIndex == -1)
            return;

        mSceneList[mCurrentIndex].gameObject.SetActive(false);

        if (mCurrentIndex > 0)
        {
            mCurrentIndex--;
        }
        else
        {
            mCurrentIndex = mSceneList.Length;
        }

        mSceneList[mCurrentIndex].gameObject.SetActive(true);
    }

    public void GotoScene(int index)
    {
        if (mCurrentIndex == -1)
            return;


        if (index < 0 || index >= mSceneList.Length)
            return;

        mSceneList[mCurrentIndex].gameObject.SetActive(false);

        mCurrentIndex = index;
        
        mSceneList[mCurrentIndex].gameObject.SetActive(true);

    }
}
