using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplorationAI : MonoBehaviour {

    public const string ExplorationAI_Search = "search";
    public const string ExplorationAI_Engaging = "engaging";
    public const string ExplorationAI_Patrolling = "Patrol";
    public const string ExplorationAI_Engaged = "Engaged";

    public List<Vector3> patrolLocations = new List<Vector3>();
    public float searchRadius;
    private string currentState = "";
    private bool movingToNewLocation = false;
    private Vector3 lastKnownLocation;
    GameObject currentPlayer;
    private List<Vector3> exisitingSearchWayPoints = new List<Vector3>();

    public void Start() {

        doPatrol();
    }

    public void Update() {

        //updated every frame to check if the enemy has reached the patrol location, this only runs when the movingToNewLocation is active
        if (movingToNewLocation && currentState.Equals(ExplorationAI_Patrolling)) {
            if (gameObject.GetComponent<NavMeshAgent>().remainingDistance<2f) {
                gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                movingToNewLocation = false;
                doPatrol();
            }
        }


        if (movingToNewLocation && currentState.Equals(ExplorationAI_Search)) {

            //put a distance against minute check to check if enemy is stuck

            if (gameObject.GetComponent<NavMeshAgent>().remainingDistance < 2f) {
                gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                movingToNewLocation = false;
                searchForPlayer();
            }
        }


        //used to check every frame if the enemy has followed and reached the player, if they have reached the player, then nothing needs to be done as the player will be dragged into the combat instance
        if (currentState.Equals(ExplorationAI_Engaging)) {
            movingToNewLocation = true;
            gameObject.GetComponent<NavMeshAgent>().SetDestination(currentPlayer.transform.position);

            if (gameObject.GetComponent<NavMeshAgent>().remainingDistance < 1.0) {
                currentState = ExplorationAI_Engaged;
                gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                //at this point player is dragged into combat and this enemy object is destroyed
            }
        }
    }

    public void engagePlayer(GameObject playerObject) {
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        movingToNewLocation = false;
        currentState = ExplorationAI_Engaging;
        currentPlayer = playerObject.gameObject;
        gameObject.GetComponent<NavMeshAgent>().SetDestination(currentPlayer.transform.position);
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;



    }

    public void searchForPlayer() {

        if (currentState!= ExplorationAI_Search) {
            //Generating search points
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            lastKnownLocation = currentPlayer.transform.position;
            currentPlayer = null;
            currentState = ExplorationAI_Search;
            exisitingSearchWayPoints = generateSearchWaypoints(lastKnownLocation, searchRadius);
            searchForPlayer();

        } else {

            if (exisitingSearchWayPoints.Count >0 ) {
                int nextPos = Random.Range(0, exisitingSearchWayPoints.Count);
                gameObject.GetComponent<NavMeshAgent>().SetDestination(exisitingSearchWayPoints[nextPos]);
                gameObject.GetComponent<NavMeshAgent>().isStopped = false;
                movingToNewLocation = true;
                exisitingSearchWayPoints.RemoveAt(nextPos);

            } else {
                movingToNewLocation = false;
                lastKnownLocation = new Vector3();
                doPatrol();
            }

        }

    }

    public void doPatrol() {
        currentState = ExplorationAI_Patrolling;
        if (!movingToNewLocation) {
            int selectedLocation = Random.Range(0, patrolLocations.Count);
            moveToNewLocation(patrolLocations[selectedLocation]);
        }
    }

    public void moveToNewLocation(Vector3 location) {
        movingToNewLocation = true;
        gameObject.GetComponent<NavMeshAgent>().SetDestination(location);
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public List<Vector3> generateSearchWaypoints(Vector3 centreLocation, float searchRadius) {

        List<Vector3> waypoints = new List<Vector3>();
        Vector3 temp = new Vector3();

        //-x = left
        //+x = right
        //-z = back
        //+z = forward

        temp.Set((centreLocation.x - searchRadius), centreLocation.y, centreLocation.z);
        waypoints.Add(temp);

        temp.Set((centreLocation.x + searchRadius), centreLocation.y, centreLocation.z);
        waypoints.Add(temp);

        temp.Set(centreLocation.x, centreLocation.y, (centreLocation.z - searchRadius));
        waypoints.Add(temp);

        temp.Set(centreLocation.x, centreLocation.y, (centreLocation.z + searchRadius));
        waypoints.Add(temp);

        return waypoints;
    }
}
