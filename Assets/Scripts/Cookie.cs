using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

// INHERITENCE: child class of Food that uses inherited member variables and overridden functions
public class Cookie : Food
{
    // POLYMORPHISM: overriden virtual function
    public override void Awake()
    {
        base.Awake();
        isLifting = false;
        isFalling = false;
        isRotating = false;
        moveSpeed = 8;
        moveDistance = 2;
        rotationSpeed = 720;
        numberOfRotations = 2;
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
                isRotating = true;
            }
        }
        if (isRotating)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
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
                isFalling = true;
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
        Vector3 forward = transform.forward;
        float angleInDegrees = Mathf.Atan2(forward.z, forward.x) * Mathf.Rad2Deg;
        angleInDegrees = angleInDegrees - Mathf.Floor(angleInDegrees / 360) * 360;
        return angleInDegrees;
    }

    // POLYMORPHISM: overriden abstract function
    public override void Reveal()
    {
        isLifting = true;
    }

    // POLYMORPHISM: overriden virtual function
    public override void PlaySound()
    {
        foodSoundSource.pitch = 0.5f;
        base.PlaySound();
    }
}
