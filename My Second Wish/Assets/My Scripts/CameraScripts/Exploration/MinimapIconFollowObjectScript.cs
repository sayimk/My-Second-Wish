using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconFollowObjectScript : MonoBehaviour {

    public GameObject minimapIcon;
    public GameObject playerObject;
	
	// Update is called once per frame
	void Update () {
        minimapIcon.transform.position = new Vector3(gameObject.transform.position.x, minimapIcon.transform.position.y, gameObject.transform.position.z);
        minimapIcon.transform.forward = playerObject.transform.forward;
    }
}
