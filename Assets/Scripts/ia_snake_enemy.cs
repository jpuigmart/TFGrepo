using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.TextCore.Text;

public class ia_snake_enemy : TreeAI
{
    protected override Node SetupTree()
    {

        Node root = new Selector(new List<Node> { new Patrol(transform) });

        return root;
    }
}
