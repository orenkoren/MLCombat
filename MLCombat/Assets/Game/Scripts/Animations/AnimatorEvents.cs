using MiddleAges.Events;
using System;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    public GameEvents events;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        //events.DeathListeners += SetTrigger;
        //events.ResurrectionListeners += Resurrect;
        events.AbilityDequeueListeners += SetBool;
        events.AbilityQueuedListeners += SetTrigger;
        events.GroundedListeners += SetBool;

    }

    private void Resurrect(object sender, int e)
    {
        animator.SetTrigger("Resurrect");
        animator.SetBool("Death", false);
    }

    private void SetTrigger(object sender, AbilityEventArgs abilityArgs)
    {
        animator.SetTrigger(abilityArgs.AnimationParameters.AnimationParameterName);
    }

    private void ResetTrigger(object sender, AbilityEventArgs animationArgs)
    {
        animator.ResetTrigger(animationArgs.AnimationParameters.AnimationParameterName);
    }

    private void SetBool(object sender, AbilityEventArgs animationArgs)
    {
        animator.SetBool(animationArgs.AnimationParameters.AnimationParameterName, animationArgs.AnimationParameters.BoolValue);
    }

    private void SetBool(object sender, AnimationEventArgs animationArgs)
    {
        animator.SetBool(animationArgs.AnimationParameterName, animationArgs.BoolValue);
    }

    private void OnDestroy()
    {
        //events.DeathListeners -= SetTrigger;
        events.AbilityDequeueListeners -= SetBool;
        events.AbilityQueuedListeners -= SetTrigger;
        events.GroundedListeners -= SetBool;
        //events.ResurrectionListeners -= Resurrect;
    }
}
