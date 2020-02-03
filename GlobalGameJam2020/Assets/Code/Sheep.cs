using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float maxSpeed;
    [SerializeField] private Animator anim;

    [SerializeField]
    private int attackAmount;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float nextAttack;
    [SerializeField, Range(0, 3)]
    private float attackRange;
    [SerializeField, Range(0, 3)]
    private int attackTime;
    [SerializeField]
    LayerMask repairMask;

    [SerializeField] private PathFinding path;
    [SerializeField] private int currentPathIndex;
    [SerializeField] private List<Vector3> pathVectorList;
    [SerializeField] private Transform target;
    [SerializeField] private AudioSource audio;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("speed", 0.0f);
        pathVectorList = new List<Vector3>();
        audio = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        Cake.OnNewCake += FindCake;
    }
    private void OnDisable()
    {
        Cake.OnNewCake -= FindCake;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetTargetPosition(new Vector3(-6.5f, 1, 0));
        GetRepairableTarget();
    }

    private void GetRepairableTarget()
    {
        int i = UnityEngine.Random.Range(0, GameManager.Instance.Repairables.Count);
        target = GameManager.Instance.Repairables[i].transform;
    }

    private void AttackRepairable()
    {
        Collider2D repairable = Physics2D.OverlapCircle(transform.localPosition, attackRange, repairMask);
        if (repairable == null) return;
        Debug.Log($"Got {repairable.name} to repair");

        if (CanAttack())
        {
            var repairObject = repairable.GetComponent<Repairable>();
            repairObject.Hurt(attackAmount);
            nextAttack = Time.time + attackSpeed;
            audio.Play();
            anim.SetTrigger("chomp");
            StartCoroutine(ChillOut());
        }


    }
    private IEnumerator ChillOut()
    {
        yield return new WaitForSeconds(attackTime);
        GetRepairableTarget();
    }

    private bool CanAttack()
    {
        return Time.time >= nextAttack;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        if (target == null)
            GetRepairableTarget();
    }

    private void HandleMovement()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > attackRange)
            {

                Vector3 moveDir = (target.position - transform.position).normalized;
                //Debug.Log($"Move Dir Sheep Distance {Vector3.Distance(transform.position, target.position)}");
                transform.position = transform.position + moveDir * maxSpeed * Time.deltaTime;

                var dir = target.position - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                anim.SetFloat("speed", maxSpeed);
            }
            else
            {
                StopMoveing();
                AttackRepairable();
            }
        }
    }

    private void HandleMovementPathFinding()
    {
        if (pathVectorList != null && pathVectorList.Count > 0)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * maxSpeed * Time.deltaTime;
                anim.SetFloat("speed", maxSpeed);
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoveing();
                }
            }
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;

        pathVectorList = PathFinding.Instance.FindPath(transform.position, targetPosition);
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    private void FindRepairable()
    {

    }
    private void FindCake(Cake cake)
    {
        target = cake.transform;
        //SetTargetPosition(cake.transform.position);
    }

    private void StopMoveing()
    {
        //target = null;
        pathVectorList = null;
        anim.SetFloat("speed", 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Sheep hit something {collision.name}");
        if (collision.gameObject.tag == "Player")
        {

            var playerRB = collision.GetComponent<Rigidbody2D>();
            playerRB.AddForce(-playerRB.velocity * 100);
        }
    }
}
