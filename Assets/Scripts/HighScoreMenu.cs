using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;

    [SerializeField] TextMeshProUGUI HighScoreText;

    [SerializeField] GlobalData mGlobalData;

    private void OnEnable()
    {
        UpdateVisuals();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateVisuals()
    {
        ScoreText.SetText(((int)mGlobalData.mCurrentScore).ToString());
        HighScoreText.SetText(((int)mGlobalData.getHighScore()).ToString());
    }
}
