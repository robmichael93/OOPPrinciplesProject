using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITENCE: child class of Food that uses protected member variables and overridden functions
public class Pizza : Food
{
    // POLYMORPHISM: overriden virtual function
    public override void Awake()
    {
        base.Awake();
        isLifting = false;
        isFalling = false;
        isRotating = false;
        rotationSpeed = 540;
        moveSpeed = 8;
        moveDistance = 1.5f;
        numberOfRotations = 1;
        currentNumberOfRotations = 0;
        startingPosition = transform.position;
        startingRotation = transform.eulerAngles;
        liftToLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveDistance);
        rotationDegreeCount = 0;
        lastRotation = GetCurrentRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLifting)
        {
            if (Vector3.Distance(transform.position, liftToLocation) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, liftToLocation, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = liftToLocation;
                isLifting = false;
                isFalling = true;
            }
        }
        if (isRotating)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
            float thisRotation = GetCurrentRotation();
            while (thisRotation < lastRotation - 180f)
            {
                thisRotation += 360f;
            }
            while (thisRotation > lastRotation + 180f)
            {
                thisRotation -= 360f;
            }
            rotationDegreeCount += Math.Abs(thisRotation - lastRotation);
            currentNumberOfRotations = Mathf.FloorToInt(rotationDegreeCount / 360);
            if (currentNumberOfRotations >= numberOfRotations)
            {
                transform.eulerAngles = startingRotation;
                isRotating = false;

            }
            lastRotation = thisRotation;
        }
        if (isFalling)
        {
            if (Vector3.Distance(transform.position, startingPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = startingPosition;
                isFalling = false;
            }
        }
    }

    private float GetCurrentRotation()
    {
        Vector3 right = transform.right;
        float angleInDegrees = Mathf.Atan2(right.z, right.x) * Mathf.Rad2Deg;
        angleInDegrees = angleInDegrees - Mathf.Floor(angleInDegrees / 360) * 360;
        return angleInDegrees;
    }

    // POLYMORPHISM: overriden abstract function
    public override void Reveal()
    {
        isLifting = true;
        isRotating = true;   
    }

    // POLYMORPHISM: overriden virtual function
    public override void PlaySound()
    {
        foodSoundSource.volume = 0.5f;
        base.PlaySound();
    }
}
