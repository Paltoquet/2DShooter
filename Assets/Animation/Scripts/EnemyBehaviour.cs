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
    private FindPlayerNode m_findNode;

    private bool m_hasSight;

    public EnemyBehaviour()
    {
        root = new ParralelNode<EnemyBehaviour>();

        var displacementNode = new SequentialNode<EnemyBehaviour>();
        m_findNode = new FindPlayerNode();
        m_moveNode = new MoveAtPlayerNode();
        //displacementNode.addNode(m_findNode);
        displacementNode.addNode(m_moveNode);

        root.addNode(m_findNode);
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

    public bool hasSight()
    {
        return m_hasSight;
    }

    public void setHasSight(bool hasSight)
    {
        m_hasSight = hasSight;
    }
}

public class FindPlayerNode : LeafNode<EnemyBehaviour>
{
    private bool m_isPathComputed;
    private bool m_hasFoundPlayer;
    private bool m_isSearching;
    private float m_waitingPeriod;


    public FindPlayerNode() : base()
    {
        m_isPathComputed = false;
        m_hasFoundPlayer = false;
        m_isSearching = false;
        m_waitingPeriod = 0.5f;
    }

    private void startSearch()
    {

    }

    private IEnumerator search()
    {
        while (m_isSearching)
        {
            Seeker seeker = m_tree.getSeeker();
            Transform currentPosition = m_tree.getModel().transform;
            Transform playerPosition = m_tree.getPlayer().transform;
            seeker.StartPath(currentPosition.position, playerPosition.position, OnPathComplete);
            yield return new WaitForSeconds(m_waitingPeriod);
        }
        yield break;
    }

    public override ProcessResult process()
    {
        if (!m_isSearching)
        {
            m_isSearching = true;
            m_tree.StartCoroutine(this.search());
        }
        return ProcessResult.Running;
    }

    /*public override ProcessResult process()
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
    }*/

    public void OnPathComplete(Path p)
    {
        m_isPathComputed = true;
        //Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        m_hasFoundPlayer = !p.error;
        m_tree.setCurrentPath(p);
    }
}

public class MoveAtPlayerNode : LeafNode<EnemyBehaviour>
{

    private int m_currentWaypoint;
    private bool m_running;
    private float xDistanceTreshHold = 2f;
    private float yDistanceTreshHold = 9f;
    private float m_shortDistance;
    private float m_longDistance;
    private int m_historySize = 50;

    //private PlateformerPointNode previousNode;
    private PlateformerPointNode nextNode;
    private Stack<PlateformerPointNode> m_previousNodes;
    private PlateformerPointNode m_previousNode;
    private PlateformerPointNode m_nextNode;

    private Path m_currentPath;

    public MoveAtPlayerNode() : base()
    {
        m_currentWaypoint = 0;
        m_running = false;
        m_shortDistance = 0.25f;
        m_longDistance = 0.9f;
        m_previousNodes = new Stack<PlateformerPointNode>();
    } 

    public override ProcessResult process()
    {
        if (m_currentPath != null)
        {
            Vector3 currentPosition = m_tree.getModel().transform.position;
            bool hasReachEndOfPath = false;
            computeBehaviour();

            if (m_running) {
                m_nextNode = m_previousNodes.Peek();
                Vector3 currentWayPointPosition = (Vector3)m_nextNode.position;
                bool inside = isInside(currentPosition, currentWayPointPosition);
                if (inside) {
                    m_previousNode = m_previousNodes.Pop();
                    if (m_previousNodes.Count > 0)
                    {
                        m_nextNode = m_previousNodes.Peek();    
                    }
                }
            }
            else
            {
                float distanceToCurrentWayPoint = Vector3.Distance(currentPosition, m_currentPath.vectorPath[m_currentWaypoint]);
                //find if we have reach a wayPoint
                for (int i = 0; i < m_currentPath.vectorPath.Count; i++)
                {
                    float distanceToWaypoint = Vector3.Distance(currentPosition, m_currentPath.vectorPath[i]);
                    bool inside = isInside(currentPosition, m_currentPath.vectorPath[i]);
                    if (inside)
                    {
                        if (i < m_currentPath.path.Count)
                        {
                            var currentPlateformerNode = (PlateformerPointNode)m_currentPath.path[i];
                            distanceToCurrentWayPoint = distanceToWaypoint;
                            m_currentWaypoint = i == (m_currentPath.path.Count - 1) ? i : i + 1;
                            m_previousNode = (PlateformerPointNode)m_currentPath.path[i];
                            m_previousNodes.Push(m_previousNode);
                        }
                    }
                }

                m_nextNode = (PlateformerPointNode)m_currentPath.path[m_currentWaypoint];
                hasReachEndOfPath = (m_currentWaypoint == m_currentPath.vectorPath.Count - 1) && (distanceToCurrentWayPoint < 0.5f);
            }

            Move();
            return ProcessResult.Running;
        }
        return ProcessResult.Failed;
    }

    private void computeBehaviour()
    {
        Vector3 currentPosition = m_tree.getModel().transform.position;
        float distanceToPlayer = Vector3.Distance(currentPosition, m_tree.getPlayer().transform.position);
        float currentRange = m_tree.getWeapon().getCurrentWeapon().range;
        var fleeDistance = currentRange * m_shortDistance;
        var returnDistance = currentRange * m_longDistance;

        if (!m_running && distanceToPlayer < fleeDistance)
        {
            m_running = true;
        }

        if (m_running && distanceToPlayer > returnDistance || m_previousNodes.Count == 0 || !m_tree.hasSight())
        {
            m_running = false;
        }
    }

    private void Move()
    {
        Vector3 currentPosition = m_tree.getModel().transform.position; ;
        Vector2 dir = (Vector3)m_nextNode.position - currentPosition;
        bool shouldJump = false;
        if (m_previousNode != null && m_nextNode != null)
        {
            shouldJump = PlateformerPointNode.shouldJump(m_previousNode, m_nextNode);
        }
        m_tree.getController().setCurrentDirection(dir);
        m_tree.getController().setShouldJump(shouldJump);
    }

    private bool isInside(Vector3 currentPosition, Vector3 target)
    {
        float distanceToWaypoint = Vector3.Distance(currentPosition, target);
        float xDistance = Mathf.Abs(currentPosition.x - target.x);
        float yDistance = Mathf.Abs(currentPosition.y - target.y);
        bool isInside = xDistance < xDistanceTreshHold;
        return isInside;
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
            bool hasSight = false;
            if (hit.collider != null)
            {
                GameObject collider = hit.collider.gameObject;
                if (collider.tag == "Player")
                {
                    shootingDirection = shootingDirection.normalized;
                    weapon.updateOrientation(shootingDirection);
                    weapon.requestShoot(shootingDirection);
                    hasSight = true;
                }
            }

            m_tree.setHasSight(hasSight);
        }

        return ProcessResult.Success;
    }
}