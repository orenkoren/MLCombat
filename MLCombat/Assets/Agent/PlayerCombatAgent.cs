using MiddleAges.Combat;
using MiddleAges.Events;
using MiddleAges.Motion;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerCombatAgent : Agent
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform startPos;

    private ThirdPersonMovement movement;
    private PaladinCombat combat;
    private AbilityCombos combos;
    private GameEvents playerEvents;
    private GameEvents enemyEvents;
    private bool isFirstTime = true;

    private void Start()
    {
        CreateComponents();
    }

    private void CreateComponents()
    {
        movement = player.GetComponent<ThirdPersonMovement>();
        combat = player.GetComponent<PaladinCombat>();
        combos = player.GetComponent<AbilityCombos>();
        playerEvents = player.GetComponent<GameEvents>();
        enemyEvents = enemy.GetComponent<GameEvents>();
        playerEvents.DamageDealtListeners += DealDamage;
        playerEvents.DamageTakenListeners += TakeDamage;
    }

    private void TakeDamage(object sender, DamageTakenEventArgs e)
    {
        SetReward(-1f);
        EndEpisode();
    }

    private void DealDamage(object sender, DamageDealtEventArgs e)
    {
        SetReward(1f);
        EndEpisode();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (player)
            sensor.AddObservation(player.transform.position);
        if (enemy)
            sensor.AddObservation(enemy.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        bool shouldAttack = actions.DiscreteActions[0] == 1;
        bool shouldUseQ = actions.DiscreteActions[1] == 1;

        // apply ThirdPersonMovement logic
        movement.agentMoveX = moveX;
        movement.agentMoveZ = moveZ;

        //apply combat logic
        if (shouldAttack)
            combos.currentKeyAgent = KeyCode.Mouse0;
        else if (shouldUseQ)
            combat.currentKeyAgent = KeyCode.Q;
        else
        {
            combat.currentKeyAgent = KeyCode.None;
            combos.currentKeyAgent = KeyCode.None;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contActions = actionsOut.ContinuousActions;
        ActionSegment<int> discActions = actionsOut.DiscreteActions;
        contActions[0] = Input.GetAxis("Horizontal");
        contActions[1] = Input.GetAxis("Vertical");
        discActions[0] = Input.GetKeyDown(KeyCode.Mouse0) ? 1 : 0;
        discActions[1] = Input.GetKeyDown(KeyCode.Q) ? 1 : 0;

    }


    public override void OnEpisodeBegin()
    {
        if(isFirstTime)
        {
            isFirstTime = false;
            return;
        }
        playerEvents.FireResurrection(this, 0);
        enemyEvents.FireResurrection(this, 0);
        player.transform.position = startPos.position;

    }

    private void OnDestroy()
    {
        playerEvents.DamageDealtListeners -= DealDamage;
        playerEvents.DamageTakenListeners -= TakeDamage;
    }
}
