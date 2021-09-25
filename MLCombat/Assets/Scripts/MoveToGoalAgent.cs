using StarterAssets;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform goal;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Material winMat;
    [SerializeField] private Material loseMat;
    [SerializeField] private MeshRenderer floorMesh;
    [SerializeField] private StarterAssetsInputs inputs;

    private float possibleSpace = 50f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-possibleSpace, possibleSpace), 1f,
            Random.Range(-possibleSpace, possibleSpace));
        goal.localPosition = new Vector3(Random.Range(-possibleSpace, possibleSpace), 1f,
            Random.Range(-possibleSpace, possibleSpace));

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(goal.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        print(moveX);

        transform.localPosition += new Vector3 { x = moveX, y = 0, z = moveZ } * Time.deltaTime * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            SetReward(-1f);
            floorMesh.material = loseMat;
            EndEpisode();
        }
        if (other.CompareTag("Goal"))
        {
            SetReward(+1f);
            floorMesh.material = winMat;
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        ActionSegment<float> contActions = actionsOut.ContinuousActions;
        contActions[0] = inputs.move.x;
        contActions[1] = inputs.move.y;
    }

    
}
