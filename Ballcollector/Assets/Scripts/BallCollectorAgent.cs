using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

/// <summary>
/// Ball Collector Machine Learning Agent
/// </summary>

public class BallCollectorAgent : Agent
{
    [Tooltip("Goal object to be spawned")]
    public GameObject goalGameobj;

    [Tooltip("Maximum Number of objects to be spawned")]
    public int iMaxNumber;

    [Tooltip("Area where the goal to be spawned")]
    public float fRange;
   
    [Tooltip("Speed for agents movement")]
    public float speed;

    //Agent's Rigid body
    private Rigidbody agentRb;

   //List of goalObjects created
    private List<GameObject> goalObjects;

    /// <summary>
    /// Initialize agent
    /// </summary>
    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();       
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        GameObject shoretestGoalPos = FindClosestGoalObj();
        if (shoretestGoalPos == null)
        {
            sensor.AddObservation(new float[7]);
            return;
        }

        //agents local position (3 observations)
        sensor.AddObservation(this.transform.position.normalized);    
       
        //shortest goal position (3 observations)
        sensor.AddObservation(shoretestGoalPos.transform.position.normalized);

        //Relative distance between Goal and object (1 observation)
        sensor.AddObservation(Vector3.Distance(this.transform.position, shoretestGoalPos.transform.position));

        //Draw line to the nearest goal
        Debug.DrawLine(this.transform.position, shoretestGoalPos.transform.position, Color.red);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 move = new Vector3(actions.ContinuousActions[0], 0f, actions.ContinuousActions[1]);
        agentRb.AddForce(move * speed, ForceMode.VelocityChange);
        if (agentRb.velocity.sqrMagnitude > 25f) // slow it down
        {
            agentRb.velocity *= 0.95f;
        }       

        //start new game once all the goals are eliminated
        if (goalObjects.Count <= 0)
        {
            EndEpisode();
        }
        
        //Restart when the agent goes out of the fields
        if(this.transform.position.y < 0)
        {
            EndEpisode();
        }

    }

   /// <summary>
   /// Forward/Backward, Left/Right Movement
   /// </summary>
   /// <param name="actionsOut"></param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    /// <summary>
    /// Reset the agent when episode begins
    /// </summary>
    public override void OnEpisodeBegin()
    {
        agentRb.velocity = Vector3.zero;
        agentRb.angularVelocity = Vector3.zero;

        //reset position and rotation of the agent
        transform.position = new Vector3(Random.Range(-fRange, fRange), 0.5f, Random.Range(-fRange, fRange));
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
        goalObjects = new List<GameObject>();
        
        //spawn the goal objects
        SpawnGoalObjects();       
    }
    
    /// <summary>
    /// Spawn and place the goal objectes in ball collectable area Min  1 and maximum number can be controlled
    /// </summary>
       
    private void SpawnGoalObjects()
    {
        int iGoal = UnityEngine.Random.Range(1, iMaxNumber);
        for (int i = 0; i <iGoal; i++)
        {
            GameObject goalObj = Instantiate(goalGameobj, new Vector3(Random.Range(-fRange, fRange), 0.25f, Random.Range(-fRange, fRange)), Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
            goalObjects.Add(goalObj);
        }

    }
    /// <summary>
    /// Find the closeset goal object
    /// </summary>
    /// <returns> nearest gameobject</returns>
    private GameObject FindClosestGoalObj()
    {
        float distanceToClosestGoal = Mathf.Infinity;
        GameObject closesetObject = null;
        foreach (GameObject currentgoal in goalObjects)
        {
            float distanceToGoal = (currentgoal.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToGoal < distanceToClosestGoal)
            {
                distanceToClosestGoal = distanceToGoal;
                closesetObject = currentgoal;
            }
        }        

        return closesetObject;
    }

    /// <summary>
    /// Collides and eliminates the goal objects and get rewards
    /// </summary>
    /// <param name="other"> tag name goal</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("goal"))
        {
            Destroy(other.gameObject);
            goalObjects.Remove(other.gameObject);            
            AddReward(1f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("border"))
        {
           AddReward(-.01f);
        }
    }

}
