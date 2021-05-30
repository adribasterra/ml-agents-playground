using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;

public class AgentController : Agent
{
    [SerializeField] private GameObject target;
    [Range(30f, 50f)] public float speed;

    public override void OnEpisodeBegin()
    {
        Debug.Log(SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name.Contains("1") || SceneManager.GetActiveScene().name.Contains("2"))
        {
            // Level 1 & 2
            float xDist = Random.Range(-20.0f, 20.0f);
            float zDist = Random.Range(-20.0f, 20.0f);
            target.transform.position = new Vector3(xDist, this.transform.position.y, zDist);
            xDist = Random.Range(-20.0f, 20.0f);
            zDist = Random.Range(-20.0f, 20.0f);
            this.transform.position = new Vector3(xDist, this.transform.position.y, zDist);
        }
        else if (SceneManager.GetActiveScene().name.Contains("3"))
        {
            // Level 3
            this.transform.position = new Vector3(-15f, this.transform.position.y, 0f);
        }
        else if (SceneManager.GetActiveScene().name.Contains("4") || SceneManager.GetActiveScene().name.Contains("Final"))
        {
            // Final level
            this.transform.position = new Vector3(235.8f, this.transform.position.y, -338f);
        }
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
            SetReward(+10f);
            EndEpisode();
        }
    }
}
