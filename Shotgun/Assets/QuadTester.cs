using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTester : MonoBehaviour {

    public int quadCount;
    public int quadCountActua;
    public GameObject quad;

    public float xMin, xMax, yMin, yMax, zMin, zMax;

    private List<GameObject> quads;
    private Vector3 loc;

    public int del;

    void Start()
    {
        quadCount = 1;
        quads = new List<GameObject>();
        loc.x = Random.Range(xMin, xMax);
        loc.y = Random.Range(yMin, yMax);
        loc.z = Random.Range(zMin, zMax);
        quads.Add(Instantiate(quad, loc, Quaternion.identity, this.transform));
    }

    void Update()
    {
        quadCountActua = quads.Count;
        if (quadCount > quads.Count)
        {
            del = (int)(((float)quadCount - (float)quadCountActua) / 100);
            if (del <= 1)
                del = 1;

            for (int i = 0; i < del; i++)
            {
                Debug.Log(i);
                loc.x = Random.Range(xMin, xMax);
                loc.y = Random.Range(yMin, yMax);
                loc.z = Random.Range(zMin, zMax);
                quads.Add(Instantiate(quad, loc, Quaternion.identity, this.transform));
            }
        }
        else if (quadCount < quads.Count)
        {
            GameObject toDestroy = quads[Random.Range(0, quads.Count)];
            quads.Remove(toDestroy);
            Destroy(toDestroy);
            return;
        }
    }
}
