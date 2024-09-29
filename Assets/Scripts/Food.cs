using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITENCE: base class for the different types of foods
public abstract class Food : MonoBehaviour
{
    [SerializeField] protected Vector3 startingPosition;
    [SerializeField] protected Vector3 startingRotation;
    [SerializeField] protected Vector3 vectorToRotateAround;
    [SerializeField] protected Vector3 liftToLocation;
    [SerializeField] protected Quaternion startingQuat;
    [SerializeField] protected Quaternion rotateToPointQuat;
    [SerializeField] protected float moveDistance;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float rotateToPointSpeed;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected bool isLifting;
    [SerializeField] protected bool isFalling;
    [SerializeField] protected bool isRotating;
    [SerializeField] protected bool isRotatingToPoint;
    [SerializeField] protected bool isRotatingBackToStart;
    [SerializeField] protected int numberOfRotations;
    [SerializeField] protected int currentNumberOfRotations;
    [SerializeField] protected float rotationDegreeCount;
    [SerializeField] protected float lastRotation;
    [SerializeField] public AudioSource foodSoundSource;
    [SerializeField] public AudioClip foodSound;

    // POLYMORPHISM: abstract function to be implemented in child classes and
    // virtual functions that can be overwritten in the child classes and/or call
    // the parent class functionality
    public virtual void Awake()
    {
        foodSoundSource = GetComponent<AudioSource>();
    }
    public abstract void Reveal();
    public virtual void PlaySound()
    {
        foodSoundSource.PlayOneShot(foodSound, 1);
    }

    public virtual void PlaySound(AudioClip sound, float volume = 1.0f)
    {
        foodSoundSource.PlayOneShot(sound, volume);
    }
}
