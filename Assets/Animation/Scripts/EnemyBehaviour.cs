using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class EnemyBehaviour : BehaviourTree<EnemyBehaviour>
{
    public Vector2 moveDirection;
    private GameObject m_model;
    private EnemyController m_controller;
    private Weapon m_weapons;
    private GameObject m_player;
    private Seeker m_seeker;
    private Path m_currentPath;
    private MoveAtPlayerNode m_moveNode;

    public EnemyBehaviour()
    {
        root = new ParralelNode<EnemyBehaviour>();

        var displacementNode = new SequentialNode<EnemyBehaviour>();
        m_moveNode = new MoveAtPlayerNode();
        displacementNode.addNode(new FindPlayerNode());
        displacementNode.addNode(m_moveNode);

        root.addNode(displacementNode);
        root.addNode(new AimAtPlayerNode());
    }

    public void init(GameObject model, EnemyController enemyControler, Weapon weapon, GameObject target, Seeker seeker)
    {
        m_model = model;
        m_controller = enemyControler;
        m_weapons = weapon;
        m_player = target;
        m_seeker = seeker;
        root.init(this);
    }

    public GameObject getModel()
    {
        return m_model;
    }

    public EnemyController getController()
    {
        return m_controller;
    }

    public Weapon getWeapon()
    {
        return m_weapons;
    }

    public Seeker getSeeker()
    {
        return m_seeker;
    }

    public GameObject getPlayer()
    {
        return m_player;
    }

    public void setCurrentPath(Path path)
    {
        m_currentPath = path;
        m_moveNode.setCurrentPath(path);
    }

    public Path getCurrentPath()
    {
        return m_currentPath;
    }
}

public class FindPlayerNode : LeafNode<EnemyBehaviour>
{
    private bool m_isPathComputed;
    private bool m_isComputing;
    private bool m_hasFoundPlayer;

    public FindPlayerNode() : base()
    {
        m_isPathComputed = false;
        m_isComputing = false;
    }

    public override ProcessResult process()
    {
        if (m_isPathComputed) {
            m_isPathComputed = false;
            return m_hasFoundPlayer ? ProcessResult.Success : ProcessResult.Failed;
        }
        else {
            if (!m_isComputing) {
                m_isComputing = true;
                Seeker seeker = m_tree.getSeeker();
                Transform currentPosition = m_tree.getModel().transform;
                Transform playerPosition = m_tree.getPlayer().transform;
                seeker.StartPath(currentPosition.position, playerPosition.position, OnPathComplete);
            }
            return ProcessResult.Running;
        }
    }

    public void OnPathComplete(Path p)
    {
        m_isComputing = false;
        m_isPathComputed = true;
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        m_hasFoundPlayer = !p.error;
        m_tree.setCurrentPath(p);
    }
}

public class MoveAtPlayerNode : LeafNode<EnemyBehaviour>
{

    private int m_currentWaypoint;
    private float xDistanceTreshHold = 2f;
    private float yDistanceTreshHold = 9f;

    private PlateformerPointNode previousNode;
    private PlateformerPointNode nextNode;

    private Path m_currentPath;

    public MoveAtPlayerNode() : base()
    {
        m_currentWaypoint = 0;
    } 

    public override ProcessResult process()
    {
        Vector3 currentPosition = m_tree.getModel().transform.position;
        float distanceToCurrentWayPoint = Vector3.Distance(currentPosition, m_currentPath.vectorPath[m_currentWaypoint]);
        
        //find if we have reach a wayPoint
        for(int i = 0; i < m_currentPath.vectorPath.Count; i++)
        {
            float distanceToWaypoint = Vector3.Distance(currentPosition, m_currentPath.vectorPath[i]);
            float xDistance = Mathf.Abs(currentPosition.x - m_currentPath.vectorPath[i].x);
            float yDistance = Mathf.Abs(currentPosition.y - m_currentPath.vectorPath[i].y);
            if (xDistance < xDistanceTreshHold)
            {
                var currentPlateformerNode = (PlateformerPointNode)m_currentPath.path[i];
                distanceToCurrentWayPoint = distanceToWaypoint;
                previousNode = (PlateformerPointNode)m_currentPath.path[i];
                m_currentWaypoint = i == (m_currentPath.vectorPath.Count - 1) ? i : i+1;
                nextNode = (PlateformerPointNode)m_currentPath.path[m_currentWaypoint];
            }
        }

        bool hasReachEndOfPath = (m_currentWaypoint == m_currentPath.vectorPath.Count - 1) && (distanceToCurrentWayPoint < 0.5f);
        if (hasReachEndOfPath)
        {
            return ProcessResult.Success;
        }
        else
        {
            Vector2 dir = (m_currentPath.vectorPath[m_currentWaypoint] - currentPosition);
            bool shouldJump = false;
            if(previousNode != null && nextNode != null)
            {
                shouldJump = PlateformerPointNode.shouldJump(previousNode, nextNode);
            }
            m_tree.getController().setCurrentDirection(dir);
            m_tree.getController().setShouldJump(shouldJump);
            return ProcessResult.Running;
        }
    }

    public void setCurrentPath(Path p)
    {
        m_currentPath = p;
        var nodes = p.path;
        bool previousNodeFound = false;
        for(int i = 0; i < nodes.Count; i++)
        {
            if(nodes[i] == nextNode)
            {
                previousNodeFound = true;
                m_currentWaypoint = i;
                break;
            }
        }
        if (!previousNodeFound) {
            m_currentWaypoint = 0;
            previousNode = null;
            nextNode = null;
        }
    }
}

public class AimAtPlayerNode : LeafNode<EnemyBehaviour>
{

    public override ProcessResult process()
    {
        var currentPosition = m_tree.getModel().transform.position;
        var playerPosition = m_tree.getPlayer().transform.position;
        var weapon = m_tree.getWeapon();
        var range = weapon.range;
        var shootingDirection = playerPosition - currentPosition;
        var distanceToPlayer = shootingDirection.magnitude;

        if (true) { //distanceToPlayer < range)
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, shootingDirection, range);
            if (hit.collider != null)
            {
                GameObject collider = hit.collider.gameObject;
                if (collider.tag == "Player")
                {
                    shootingDirection = shootingDirection.normalized;
                    weapon.updateOrientation(shootingDirection);
                    weapon.requestShoot(shootingDirection);
                }
            }
        }

        return ProcessResult.Success;
    }
}