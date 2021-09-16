using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform goal;
    [SerializeField] private Transform resetTo;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Material winMat;
    [SerializeField] private Material loseMat;
    [SerializeField] private MeshRenderer floorMesh;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(goal.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3 { x = moveX, y = 0, z = moveZ } * Time.deltaTime * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
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
        contActions[0] = Input.GetAxisRaw("Horizontal");
        contActions[1] = Input.GetAxisRaw("Vertical");
    }

    public override void OnEpisodeBegin()
    {
        transform.position = resetTo.position;
        transform.rotation = resetTo.rotation;
    }
}
