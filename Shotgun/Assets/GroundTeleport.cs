using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTeleport : MonoBehaviour {

    public Transform player;
    private Vector3 newPos;

	void Update () {
		if (Vector3.Distance(player.position, transform.position) > 100)
        {
            newPos.x = player.position.x;
            newPos.z = player.position.z;
            transform.position = newPos;
        }
	}
}
