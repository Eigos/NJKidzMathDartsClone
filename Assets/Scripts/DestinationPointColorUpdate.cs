using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DestinationPointColorUpdate : MonoBehaviour
{
    [SerializeField] Color ValidTarget;

    [SerializeField] Color InValidTarget;

    [SerializeField] string ChartTag;

    private Image mImage;

    private GameObject mCurrentChart = null;

    private void Awake()
    {
        mImage = GetComponent<Image>();

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        mImage.color = InValidTarget;

        mCurrentChart = null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != ChartTag)
        {
            mImage.color = InValidTarget;

            mCurrentChart = null;
        }
        else
        {
            mImage.color = ValidTarget;

            mCurrentChart = collision.gameObject;
        }
    }

    public GameObject getCurrentChart()
    {
        return mCurrentChart;
    }
}
