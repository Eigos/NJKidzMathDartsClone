using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotationScript : MonoBehaviour
{
    [SerializeField] float mRotationSpeed;

    [SerializeField] bool mClockWise;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentAngle = transform.localRotation.eulerAngles;

        if(mClockWise)
            currentAngle.z -= mRotationSpeed * Time.deltaTime;
        else
            currentAngle.z += mRotationSpeed * Time.deltaTime;
        
        transform.localRotation = Quaternion.Euler(currentAngle);

    }
}
