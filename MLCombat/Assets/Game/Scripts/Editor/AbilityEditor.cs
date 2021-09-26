#if UNITY_EDITOR

using MiddleAges.Database;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MiddleAges.CustomEditors
{

    [CustomEditor(typeof(Ability))]
    public class AbilityEditor : Editor
    {
        AbilityData ability;
        AbilityEditorType editorType;

        [SerializeField]
        private bool isInActionBar;

        public override void OnInspectorGUI()
        {
            ability = ((Ability)target).AbilityInfo;
            SetName();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            editorType = (AbilityEditorType)EditorGUILayout.EnumPopup("User", selected: editorType);
            EditorGUILayout.Space();
            if (editorType == AbilityEditorType.Player)
                InitPlayerFields();
            else
                InitAIFields();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            InitCommonFields();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            InitConditionFields();
            InitRelatedAbilities();

            if(GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

        }

        private void SetName()
        {
            ability.Name = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(target.GetInstanceID()));
            EditorGUILayout.LabelField("Name", ability.Name);
        }

        private void InitCommonFields()
        {
            HandleAbilityType();
            EditorGUILayout.Space();
            ability.Cooldown = EditorGUILayout.FloatField("Cooldown", ability.Cooldown);
            HandleChannelFields();
            ability.DamageText = EditorGUILayout.TextField("Floating Text", ability.DamageText);
            ability.VerticalForce = EditorGUILayout.FloatField("Vertical Force", ability.VerticalForce);
            ability.KnockbackTime = EditorGUILayout.FloatField("Knockback Time", ability.KnockbackTime);
            if (ability.KnockbackTime > 0)
                ability.KnockbackPower = EditorGUILayout.FloatField("KnockbackPower", ability.KnockbackPower);

        }

        private void HandleAbilityType()
        {
            ability.Type = (AbilityType)EditorGUILayout.EnumPopup("Ability Type", selected: ability.Type);
            EditorGUILayout.Space();
            switch (ability.Type)
            {
                case AbilityType.Melee:
                    InitMeleeFields();
                    break;
                case AbilityType.Heal:
                    InitHealFields();
                    break;
                case AbilityType.Block:
                    InitBlockFields();
                    break;
                case AbilityType.Spell:
                    InitSpellFields();
                    break;
            }
        }

        private void InitSpellFields()
        {
            InitDamageFields();
            ability.Range = EditorGUILayout.FloatField("Range", ability.Range);

        }

        private void InitMeleeFields()
        {
            InitDamageFields();
            ability.Range = EditorGUILayout.FloatField("Range", ability.Range);

        }

        private void InitBlockFields()
        {
        }

        private void InitHealFields()
        {
            ability.BaseDamage = EditorGUILayout.IntField("Base Damage", ability.BaseDamage);
        }

        private void InitDamageFields()
        {
            ability.BaseDamage = EditorGUILayout.IntField("Base Damage", ability.BaseDamage);
            ability.HitRadius = EditorGUILayout.FloatField("Hit Radius", ability.HitRadius);
            ability.HitRadiusArcDegrees = EditorGUILayout.FloatField("Hit Arc", ability.HitRadiusArcDegrees);
            EditorGUILayout.Space();
            ability.ApplySlow = EditorGUILayout.IntField("Apply Slow", ability.ApplySlow);
            if (ability.ApplySlow > 0)
                ability.SlowDuration = EditorGUILayout.FloatField("Slow Duration", ability.SlowDuration);
            ability.Dot = EditorGUILayout.IntField("Dot", ability.Dot);
            if (ability.Dot > 0)
            {
                ability.DotInterval = EditorGUILayout.FloatField("Interval", ability.DotInterval);
                ability.TickAmount = EditorGUILayout.FloatField("Tick Amount", ability.TickAmount);
            }
            ability.IsStun = EditorGUILayout.Toggle("Stun", ability.IsStun);
            if (ability.IsStun)
                ability.StunDuration = EditorGUILayout.FloatField("Stun Duration", ability.StunDuration);
        }

        private void HandleChannelFields()
        {
            ability.IsChanneled = EditorGUILayout.Toggle("Channeled", ability.IsChanneled);
            if (ability.IsChanneled)
            {
                ability.DamageReduction = EditorGUILayout.IntField("Damage Reduction", ability.DamageReduction);
                ability.SelfSlow = EditorGUILayout.IntField("Self Slow", ability.SelfSlow);
            }
        }

        private void InitConditionFields()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.Label("Ability Conditions");

            for (int i = 0; i < ability.Conditions.Count; i++)
            {
                ability.Conditions[i] = (AbilityConditionType)EditorGUILayout.EnumPopup(ability.Conditions[i]);
            }
            AddRemoveButtonGroup(out bool addButton, out bool removeButton);
            if (addButton)
                ability.Conditions.Add((AbilityConditionType)EditorGUILayout.EnumPopup(AbilityConditionType.WhileChanneling));
            if (removeButton)
                ability.Conditions.RemoveAt(ability.Conditions.Count - 1);
        }

        private void AddRemoveButtonGroup(out bool addButton, out bool removeButton)
        {
            GUILayout.BeginHorizontal();
            addButton = GUILayout.Button("Add");
            removeButton = GUILayout.Button("Remove Last");
            GUILayout.EndHorizontal();
        }

        private void InitRelatedAbilities()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.Label("Related Abilities");
            for (int i = 0; i < ability.RelatedAbilities.Count; i++)
            {
                ability.RelatedAbilities[i] =  AddRelatedAbilityGUI(ability.RelatedAbilities[i]);
            }
            AddRemoveButtonGroup(out bool addButton, out bool removeButton);
            if(addButton)
            {
                ability.RelatedAbilities.Add(AddRelatedAbilityGUI(new RelatedAbility()));
            }
            if (removeButton)
            {
                ability.RelatedAbilities.RemoveAt(ability.RelatedAbilities.Count - 1);
            }

        }

        private RelatedAbility AddRelatedAbilityGUI(RelatedAbility relatedAbility)
        {
            relatedAbility.Ability = (Ability)EditorGUILayout.ObjectField(
                    label: "Ability", relatedAbility.Ability, typeof(Ability), true);
            relatedAbility.RestoresCooldown = EditorGUILayout.FloatField("Restore Cooldown",
                    relatedAbility.RestoresCooldown);
            return relatedAbility;
        }

        private void InitPlayerFields()
        {
            isInActionBar = EditorGUILayout.Toggle("Is In Action Bar", isInActionBar);
            if (isInActionBar)
            {
                ability.Icon = (Sprite)EditorGUILayout.ObjectField(label: "Icon", ability.Icon, ability.Icon.GetType(), true);
                ability.Keybind = (KeyCode)EditorGUILayout.EnumPopup("Keybind", selected: ability.Keybind);
                ability.Tooltip = EditorGUILayout.TextField("Tooltip", ability.Tooltip);
                ability.ActionSlot = EditorGUILayout.IntField("Action Slot", ability.ActionSlot);
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Resource Usage");
            ability.RetCost = EditorGUILayout.FloatField("Ret Cost", ability.RetCost);
            ability.RetGain = EditorGUILayout.FloatField("Ret Gain", ability.RetGain);
            ability.SancCost = EditorGUILayout.FloatField("Sanc Cost", ability.SancCost);
            ability.SancGain = EditorGUILayout.FloatField("Sanc Gain", ability.SancGain);


        }

        private void InitAIFields()
        {
        }
    }
}

enum AbilityEditorType
{
    Player,
    AI
}

#endif