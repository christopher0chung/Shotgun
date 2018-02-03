using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMeshTest : MonoBehaviour {

    public Material matToUse;

    void Start()
    {

        MeshFilter mF = this.gameObject.AddComponent<MeshFilter>();
        MeshRenderer mR = this.gameObject.AddComponent<MeshRenderer>();

        MeshFilter[] subFilters = GetComponentsInChildren<MeshFilter>();

        List<CombineInstance> cIHold = new List<CombineInstance>();

        Vector3 oldPos = transform.position;
        Quaternion oldRot = transform.rotation;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        for (int i = 0; i < subFilters.Length; i++)
        {
            if (subFilters[i].transform == this.transform)
            {
                Debug.Log(i);
                continue;
            }
            CombineInstance cISingle = new CombineInstance();
            cISingle.mesh = subFilters[i].sharedMesh;
            cISingle.transform = subFilters[i].transform.localToWorldMatrix;

            cIHold.Add(cISingle);

            subFilters[i].gameObject.SetActive(false);
        }

        CombineInstance[] combinedInstances = new CombineInstance[cIHold.Count];

        for (int i = 0; i < cIHold.Count; i++)
        {
            combinedInstances[i] = cIHold[i];
        }

        mF.mesh = new Mesh();
        mF.mesh.name = "Steve";
        Debug.Log(combinedInstances.Length);
        mF.mesh.CombineMeshes(combinedInstances, true, true, false);

        mR.material = matToUse;

        transform.rotation = oldRot;
        transform.position = oldPos;
    }
}
