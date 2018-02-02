using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSystem : MonoBehaviour {

    private Rigidbody _myRB;

    [Header("Sliders")]
    public float e;
    public float m;
    public float s;

    private float _exh;
    public float exhaustion
    {
        get
        {
            return _exh;
        }

        set
        {
            _exh = Mathf.Clamp01(value);
        }
    }
    private float _mot;
    public float motivation
    {
        get
        {
            return _mot;
        }

        set
        {
            _mot = Mathf.Clamp01(value);
        }
    }
    private float _srn;
    public float strain
    {
        get
        {
            return _srn;
        }

        set
        {
            _srn = Mathf.Clamp01(value);
        }
    }
    [Header("Hard Values")]
    public float powerMax_kW;
    public float topSpeed_mps;

    [Header("Read Values")]
    public float currentSpeed;

    [Header("Output Values")]

    public float effort;
    public float powerAvailable_kW;
    public float powerLocomotion_kW;

    public float counterForceOfLocomotion_N;

    public float powerForEffort_kW;

    public float powerOfEffort_kW;

    public float forceOfEffort_N;

    void Start()
    {
        _myRB = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        currentSpeed = _myRB.velocity.magnitude;

        exhaustion = e;
        powerAvailable_kW = Mathf.Lerp(powerMax_kW, 0, exhaustion);

        powerLocomotion_kW = (1 - ((topSpeed_mps - currentSpeed) / topSpeed_mps)) * powerAvailable_kW;

        powerForEffort_kW = powerAvailable_kW - powerLocomotion_kW;

        powerOfEffort_kW = Mathf.Lerp(0, powerForEffort_kW, effort);

        forceOfEffort_N = Mathf.Clamp(1000 * powerOfEffort_kW / (currentSpeed + .1f), 0, 20000);

        counterForceOfLocomotion_N = ((currentSpeed - (effort * topSpeed_mps)) / topSpeed_mps) * 20000;
    }

    public void FixedUpdate()
    {
        motivation = m;
        strain = s;

        effort = Mathf.Lerp(effort, motivation, Time.fixedDeltaTime * motivation * .2f);
        effort = Mathf.Lerp(effort, 0, Time.fixedDeltaTime * strain);

        _myRB.AddForce(transform.forward * (forceOfEffort_N));
    }
}
