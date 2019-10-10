using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapZoomAnimationController : MonoBehaviour {

    List<string> zoomLevels = new List<string>();
    int nextZoomTransitionIndex = 0;
    int nextZoomIconIndex = 0;

    //Minimap elements 
    public GameObject[] allMiniMapIcons;

    //positions for all the minimap icons to move to 
    List<float> scaleIconLevels = new List<float>();

	// Use this for initialization
	void Start () {
        //setting zoom triggers

        zoomLevels.Add("ZoomOutToMiddle");
        zoomLevels.Add("ZoomOutToFar");
        zoomLevels.Add("ZoomInToClose");

        //change to different scaling
        scaleIconLevels.Add(1.175927f);
        scaleIconLevels.Add(2.5f);
        scaleIconLevels.Add(6.15f);

        recursiveZoomMapIcons(1.175927f);
        //setting icon level Y Positions
    }

    //this will automatically toggle through the different zoom transition levels
    public void toggleZoom() {
        gameObject.GetComponent<Animator>().SetTrigger(zoomLevels[nextZoomTransitionIndex]);

        nextZoomTransitionIndex = nextZoomTransitionIndex + 1;
        nextZoomIconIndex = nextZoomIconIndex + 1;

        if (nextZoomTransitionIndex > 2) {
            nextZoomTransitionIndex = 0;
        }

        if (nextZoomIconIndex > 2) {
            nextZoomIconIndex = 0;
        }

        recursiveZoomMapIcons(scaleIconLevels[nextZoomIconIndex]);

    }

    public void recursiveZoomMapIcons(float zoomPosition) {
        Debug.Log("Changing icons to " + zoomPosition);

        for (int i = 0; i < allMiniMapIcons.Length; i++) {
            allMiniMapIcons[i].transform.localScale =new Vector3(zoomPosition, zoomPosition, zoomPosition);
            Debug.Log(allMiniMapIcons[i].transform.localScale);
        }
    }


}
