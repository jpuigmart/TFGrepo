using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using TMPro;
using UnityEditor;

public class Patrol : Node
{
    private Transform _transform;
    private snake_enemy snake;
    private float moveInput;
    private Vector3 velocity;
 
    public Patrol (Transform transform)
    {
        _transform = transform;


    }
    public override NodeState Evaluate()
    {
        snake = _transform.GetComponent<snake_enemy>();
        velocity = new Vector3(moveInput * snake.speed, 0, 0);
        _transform.Translate(velocity);
        if (snake.isImpact)
        {
            moveInput *= -1;
        }  
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, _transform.position.z);
        return NodeState.RUNNING;
    }
}
