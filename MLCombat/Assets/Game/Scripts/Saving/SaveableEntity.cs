﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MiddleAges.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        public string uniqueIdentifier = "";
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject) || string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                    saveable.RestoreState(stateDict[typeString]);
            }
        }

        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate) || globalLookup[candidate] == this) return true;

            if (globalLookup[candidate] == null || globalLookup[candidate].GetUniqueIdentifier() != candidate) // invalid lookup
            {
                globalLookup.Remove(candidate);
                return true;
            }
            return false;
        }
    }
}