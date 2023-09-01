using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFPController
{
    [SerializeField] public bool IsGrounded { get; }
}
