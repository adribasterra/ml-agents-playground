using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentController : Agent
{
    [SerializeField] private GameObject target;
    [Range(30f, 50f)] public float speed;

    public override void OnEpisodeBegin()
    {
        this.transform.position = new Vector3(0f, this.transform.position.y, 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(target.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        
        this.transform.position += new Vector3(moveX, 0f, moveZ) * speed * Time.deltaTime;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "wall")
        {
            SetReward(-1f);
            EndEpisode();
        }
        if (other.gameObject.tag == "target")
        {
            SetReward(+1f);
            EndEpisode();
        }
    }
}
