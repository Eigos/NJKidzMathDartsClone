using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    [SerializeField] float dragDistance = 0;  //minimum distance for a swipe to be registered
    [SerializeField] GameObject boxObject;
    [SerializeField] float sensitivity = 1.0f;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position

    private RectTransform mRectTransform;
    private RectTransform mBoxRectTransform;

    void Start()
    {
        mRectTransform = GetComponent<RectTransform>();

        mBoxRectTransform = boxObject.GetComponent<RectTransform>();
        //dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
    }

    void Update()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {

                //is circle inside box 

                Vector2 size = mBoxRectTransform.rect.size;
                size = size / 2;

                Vector2 centerPoint = mBoxRectTransform.localPosition;

                transform.localPosition += new Vector3((touch.deltaPosition * sensitivity).x, (touch.deltaPosition * sensitivity).y, 0);

                transform.localPosition = KeepPointInsideBox(centerPoint, size, transform.localPosition);
            }

        }


    }

    bool PointBoxCollision(Vector2 boxPoint, Vector2 boxSize, Vector2 pointTarget)
    {
        boxSize = boxSize / 2;

        bool isInsideBox = true;

        //check x
        if (boxPoint.x + boxSize.x < pointTarget.x)
        {
            isInsideBox = false;
        }
        else if (boxPoint.x - boxSize.x > pointTarget.x)
        {
            isInsideBox = false;
        }

        //check y
        if (boxPoint.y + boxSize.y < pointTarget.y)
        {
            isInsideBox = false;

        }
        else if (boxPoint.y - boxSize.y > pointTarget.y)
        {
            isInsideBox = false;
        }

        return isInsideBox;
    }

    Vector2 KeepPointInsideBox(Vector2 boxPoint, Vector2 boxSize, Vector2 pointTarget)
    {
        Vector2 newPoint = pointTarget;

        //check x

        //right
        if (boxPoint.x + boxSize.x < pointTarget.x)
        {
            newPoint.x -= pointTarget.x - (boxPoint.x + boxSize.x); 
        }

        //left
        else if (boxPoint.x - boxSize.x > pointTarget.x)
        {
            newPoint.x += Mathf.Abs(pointTarget.x) - Mathf.Abs(boxPoint.x - boxSize.x);

        }

        //check y

        //top
        if (boxPoint.y + boxSize.y < pointTarget.y)
        {
            newPoint.y -= pointTarget.y - (boxPoint.y + boxSize.y);

        }

        //bottom
        else if (boxPoint.y - boxSize.y > pointTarget.y)
        {
            newPoint.y += Mathf.Abs(pointTarget.y) - Mathf.Abs(boxPoint.y - boxSize.y);
        }

        return newPoint;
    }
}


