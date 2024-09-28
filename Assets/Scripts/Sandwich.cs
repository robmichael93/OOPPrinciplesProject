using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandwich : Food
{
    public override void Awake()
    {
        base.Awake();
        isLifting = false;
        isFalling = false;
        isRotating = false;
        isRotatingToPoint = false;
        isRotatingBackToStart = false;
        rotateToPointSpeed = 540;
        rotationSpeed = 1080;
        moveSpeed = 8;
        moveDistance = 2;
        numberOfRotations = 2;
        currentNumberOfRotations = 0;
        startingPosition = transform.position;
        startingQuat = transform.rotation;
        liftToLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
        rotationDegreeCount = 0;
        lastRotation = GetCurrentForwardRotation();
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
                isRotatingToPoint = true;
            }
        }
        if (isRotatingToPoint)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateToPointQuat, rotateToPointSpeed * Time.deltaTime);
            float thisRotation = GetCurrentForwardRotation();
            if (Mathf.Abs(thisRotation - lastRotation) <= 0.01)
            {
                transform.rotation = rotateToPointQuat;
                vectorToRotateAround = transform.InverseTransformDirection(transform.forward) + transform.InverseTransformDirection(transform.right);
                isRotatingToPoint = false;
                isRotating = true;
            }
            lastRotation = thisRotation;
        }
        if (isRotating)
        {
            transform.Rotate(vectorToRotateAround, rotationSpeed * Time.deltaTime, Space.Self);
            float thisRotation = GetCurrentRightRotation();
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
                transform.rotation = rotateToPointQuat;
                isRotating = false;
                isRotatingBackToStart = true;
            }
            lastRotation = thisRotation;
        }
        if (isRotatingBackToStart)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startingQuat, rotateToPointSpeed * Time.deltaTime);
            float thisRotation = GetCurrentForwardRotation();
            if (Mathf.Abs(thisRotation - lastRotation) <= 0.01)
            {
                transform.rotation = startingQuat;
                isRotatingBackToStart = false;
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

    private float GetCurrentForwardRotation()
    {
        Vector3 forward = transform.forward;
        float angleInDegrees = Mathf.Atan2(forward.z, forward.x) * Mathf.Rad2Deg;
        angleInDegrees = angleInDegrees - (float)Math.Floor(angleInDegrees / 360) * 360;
        return angleInDegrees;
    }

    private float GetCurrentRightRotation()
    {
        Vector3 right = transform.right;
        float angleInDegrees = Mathf.Atan2(right.z, right.x) * Mathf.Rad2Deg;
        angleInDegrees = angleInDegrees - (float)Math.Floor(angleInDegrees / 360) * 360;
        return angleInDegrees;
    }

    public override void Reveal()
    {
        isLifting = true;
        rotateToPointQuat = Quaternion.AngleAxis(-45, transform.InverseTransformDirection(transform.up));      
    }

    public override void PlaySound()
    {
        foodSoundSource.pitch = 0.5f;
        foodSoundSource.volume = 0.75f;
        base.PlaySound();
    }
}
