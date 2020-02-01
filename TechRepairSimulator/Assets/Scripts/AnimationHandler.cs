using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public NamedAnimators[] animators;
}

[System.Serializable]
public struct NamedAnimators {
    public string Name;
    public GameObject Icon;
}
