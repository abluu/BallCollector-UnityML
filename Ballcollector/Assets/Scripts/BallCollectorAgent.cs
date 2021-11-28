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

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
    }

    /// <summary>
    /// Reset the agent when episode begins
    /// </summary>
    public override void OnEpisodeBegin()
    {
        //spawn the goal objects
        SpawnGoalObjects();
    }

    /// <summary>
    /// Spawn and place the goal objectes in ball collectable area Min - 1 and maximum number can be controlled
    /// </summary>
       
    private void SpawnGoalObjects()
    {
        int iGoal = UnityEngine.Random.Range(1, iMaxNumber);
        for (int i = 0; i <iGoal; i++)
        {
            GameObject goalObj = Instantiate(goalGameobj, new Vector3(Random.Range(-fRange, fRange), 0.25f, Random.Range(-fRange, fRange)), Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
        }

    }

}
