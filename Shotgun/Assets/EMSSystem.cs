using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMSSystem : MonoBehaviour {

    private Rigidbody _myRB;

    public GameObject dotPrefab;

    [Header("Control Variables")]
    public float motivation;
    public float effort;
    public float exhaustion;

    [Header("Hard Values")]
    public float speedTop;
    public float accelerationMax;

    [Header("Read Values")]
    public float speedActual;

    [Header("Calculated Values")]
    public float speedDesired;
    public float speedAchievable;
    public float delVelDesiredActual;
    public float delVelTopActual;
    public float forceAcceleration;
    public float counterForceLocomotion;

    public float strain;

    public float a;


    void Start()
    {
        _myRB = GetComponent<Rigidbody>();

        for (int i = 0; i < 1000; i++)
            Instantiate(dotPrefab, new Vector3(-1, .1f, transform.position.z + i), Quaternion.identity);
    }

    public void Update()
    {
        _ReadValues();
        _CalculatedValues();

        if (Input.GetKeyDown(KeyCode.A))
            motivation += .05f;
        if (Input.GetKeyDown(KeyCode.D))
            motivation -= .05f;

    }

    public void FixedUpdate()
    {
        _myRB.AddForce(transform.forward * (forceAcceleration - counterForceLocomotion));
    }

    private void _ReadValues()
    {
        speedActual = _myRB.velocity.magnitude;
    }

    private void _CalculatedValues()
    {
        effort = (1 - exhaustion * exhaustion) * motivation;
        speedDesired = motivation * speedTop;
        speedAchievable = (1 - exhaustion * exhaustion) * speedTop;

        delVelDesiredActual = (speedDesired - speedActual) / speedTop;
        delVelTopActual = 1 - ((speedTop - speedActual) / speedTop);

        counterForceLocomotion = delVelTopActual * 700 * accelerationMax;

        forceAcceleration = 700 * accelerationMax * motivation * effort;

        //Instantiate(dotPrefab, new Vector3(-1, speedActual, transform.position.z), Quaternion.identity);

        strain = motivation * motivation * motivation;
        motivation = .995f * motivation + .005f * effort;
        exhaustion = .999f * exhaustion + .001f * strain;

    }
}
