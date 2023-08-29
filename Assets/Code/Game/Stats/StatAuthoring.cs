using System.Collections;
using UnityEngine;

public abstract class StatAuthoring : MonoBehaviour
{
    [SerializeField]
    protected StatOperation m_Option = StatOperation.Overwrite;
    [SerializeField]
    protected float m_Value;
}
