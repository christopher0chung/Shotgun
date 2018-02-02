using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseMovementForceTest : MonoBehaviour {

    private Rigidbody _myRB;

    public float _rbVel;
    private float _timer;


    public float outputVel;
    public float outputVelAlt;

    public float outputAccel;

    public float outputPower;

    public float maxVel;
    public float rate;

    public GameObject dotPrefab;

    void Start()
    {
        _myRB = GetComponent<Rigidbody>();
    }

	void FixedUpdate () {
        //_myRB.AddForce(transform.forward * runForce, ForceMode.Force);
        _rbVel = _myRB.velocity.magnitude;

        _timer += Time.deltaTime;
        //if (_rbVel >= 18.77f)
        //    Debug.Log(_timer);

        outputVel = VelocityOverTime(_timer);
        outputAccel = AccelerationOverTime(_timer);
        outputPower = PowerOverTime(outputVel, outputAccel);

        _myRB.AddForce(transform.forward * 700 * outputAccel);
        //if (outputVel >= maxVel - .25f)
        //    Debug.Log(_timer);

        Debug.Log(outputPower + " " + _timer);
        Instantiate(dotPrefab, new Vector3(_timer * 10, outputPower / 1000, 0), Quaternion.identity);
        Instantiate(dotPrefab, new Vector3(_timer * 10, (.0005f * _myRB.mass * outputVel * outputVel) / _timer, 0), Quaternion.identity);

        //Debug.Log(outputPower);

    }

    private float VelocityOverTime(float t)
    {
        return (2 * maxVel) / (1 + Mathf.Exp(-rate * t)) - maxVel;
    }

    private float AccelerationOverTime(float t)
    {
        return (2 * maxVel * rate * Mathf.Exp(rate * t)) / ((Mathf.Exp(rate * t) + 1) * (Mathf.Exp(rate * t) + 1));
    }

    private float PowerOverTime(float v, float a)
    {
        return _myRB.mass * v * a;
    }
}
