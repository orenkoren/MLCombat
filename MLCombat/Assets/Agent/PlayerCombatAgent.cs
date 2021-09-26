using MiddleAges.Combat;
using MiddleAges.Events;
using MiddleAges.Motion;
using MiddleAges.Resources;
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
    private float playerHp;
    private float enemyHp;

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
        playerEvents.DamageTakenListeners += PlayerTookDamage;
        enemyEvents.DamageTakenListeners += EnemyTookDamage;
        playerHp = player.GetComponentInChildren<HealthPoints>().GetMaxResourcePoints();
        enemyHp = enemy.GetComponentInChildren<HealthPoints>().GetMaxResourcePoints();
        enemyEvents.StunStateListeners += EnemyStunned;
    }

    private void EnemyStunned(object sender, bool isStunned)
    {
        if (isStunned)
            AddReward(0.4f);

    }

    private void PlayerTookDamage(object sender, DamageTakenEventArgs e)
    {
        float amount = -e.DamageAmount / playerHp;
        print("reduced" + amount);
        AddReward(amount);
    }

    private void EnemyTookDamage(object sender, DamageTakenEventArgs e)
    {
        float amount = e.DamageAmount / enemyHp;
        print("added" + amount);
        AddReward(amount);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //TODO: why is this messing up the decision requester?
        sensor.AddObservation(enemy.transform.localPosition - player.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        bool shouldAttack = actions.DiscreteActions[0] == 0;
        bool shouldUseQ = actions.DiscreteActions[0] == 1;
        bool shouldUseE = actions.DiscreteActions[0] == 2;
        bool shouldUseR = actions.DiscreteActions[0] == 3;
        bool shouldUseF = actions.DiscreteActions[0] == 4;
        bool shouldUseC = actions.DiscreteActions[0] == 5;
        bool shouldUseV = actions.DiscreteActions[0] == 6;
        bool shouldBlock = actions.DiscreteActions[0] == 7;
        bool shouldDodge = actions.DiscreteActions[0] == 8;
        bool shouldDoNothing = actions.DiscreteActions[0] == 9;

        // apply ThirdPersonMovement logic
        if (shouldDoNothing)
        {
            movement.agentMoveX = moveX;
            movement.agentMoveZ = moveZ;
        }

        //apply combat logic
        if (shouldAttack)
            combos.currentKeyAgent = KeyCode.Mouse0;
        else if (shouldUseQ)
            combat.currentKeyAgent = KeyCode.Q;
        else if (shouldUseE)
            combat.currentKeyAgent = KeyCode.E;
        else if (shouldUseR)
            combat.currentKeyAgent = KeyCode.R;
        else if (shouldUseF)
            combat.currentKeyAgent = KeyCode.F;
        else if (shouldUseC)
            combat.currentKeyAgent = KeyCode.C;
        else if (shouldUseV)
            combat.currentKeyAgent = KeyCode.V;
        else if (shouldBlock)
            combat.currentKeyAgent = KeyCode.Mouse1;
        else if (shouldDodge)
            combat.currentKeyAgent = KeyCode.LeftShift;
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
        discActions[0] = Input.GetKeyDown(KeyCode.Mouse0) ? 0 : 9;
        discActions[0] = Input.GetKeyDown(KeyCode.Q) ? 1 : 9;
        discActions[0] = Input.GetKeyDown(KeyCode.E) ? 2 : 9;
        discActions[0] = Input.GetKeyDown(KeyCode.R) ? 3 : 9;
        discActions[0] = Input.GetKeyDown(KeyCode.F) ? 4 : 9;
        discActions[0] = Input.GetKeyDown(KeyCode.C) ? 5 : 9;
        discActions[0] = Input.GetKeyDown(KeyCode.V) ? 6 : 9;
        discActions[0] = Input.GetKeyDown(KeyCode.Mouse1) ? 7 : 9;
        discActions[0] = Input.GetKeyDown(KeyCode.LeftShift) ? 8 : 9;



    }


    public override void OnEpisodeBegin()
    {
        if (isFirstTime)
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
        playerEvents.DamageTakenListeners -= PlayerTookDamage;
        enemyEvents.DamageTakenListeners -= EnemyTookDamage;
    }
}