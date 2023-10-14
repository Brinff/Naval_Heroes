using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetRaycaster
{
    bool isOverrideTargetRaycasts { get; set; }

    void AddTargetRaycast(GameObject gameObject);
    void RemoveTargetRaycast(GameObject gameObject);
}
