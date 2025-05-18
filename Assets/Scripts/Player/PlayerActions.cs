using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerActions : MonoBehaviour
{
    [Header("Input Variables")]
    [SerializeField] private InputActionMap playerInputMap;
    [SerializeField] private InputActionAsset playerInputAsset;
    [Header("Input References")]
    [SerializeField] private InputActionReference moveX;
    [SerializeField] private InputActionReference moveY;
    [SerializeField] private InputActionReference dash;
    [SerializeField] private InputActionReference attack;
    [SerializeField] private InputActionReference fireBall;
    /*[SerializeField] private InputActionReference dashUp;
    [SerializeField] private InputActionReference dashDown;
    [SerializeField] private InputActionReference dashRight;
    [SerializeField] private InputActionReference dashLeft;*/
    private InputAction moveXAction, moveYAction, dashAction, attackAction, fireBallAction/*,
        dashActionUp, dashActionDown, dashActionRight, dashActionLeft*/;
    [Header("Input Values")]
    [SerializeField] public float moveXfloat;
    [SerializeField] public float moveYfloat;
    [SerializeField] public float attackfloat;

    private Rigidbody rb;
    public PlayerLife playerLife;
    private Invisible invisible;
    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float desiredMoveSpeed;
    [SerializeField] private float lastDesiredMoveSpeed;
    [SerializeField] private MovementState lastState;
    public MovementState state;
    [SerializeField] private bool keepMomentum;

    [Header("Dash Variables")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashSpeedChangeFactor;
    [SerializeField] private float dashTimer;
    [SerializeField] private bool dashing;
    [SerializeField] private float dashDuration;
    [SerializeField] private bool resetVel;

    [Header("Materials")]
    [SerializeField] private Renderer meshRenderer;    
    [SerializeField] public Material materialYellow;
    [SerializeField] public Material materialBlue;
    [SerializeField] public Material materialInvisible;
    [SerializeField] public Material InitialMaterial;

    [Header("Attack Variables")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shootPoint;
    [SerializeField] private bool attacking;
    [SerializeField] private bool haveBlaster;
    [SerializeField] private float delayAttack;
    [SerializeField] private float baseDelayAttack;
    [SerializeField] private float blasterDelayAttack;
    [SerializeField] public GunType gunType;
    private float lastAttackTime;

    [Header("Missiles Variables")]
    [SerializeField] private GameObject fireBallAttack;
    [SerializeField] private float missiles;
    [SerializeField] private float maxMissiles;
    private TMP_Text missilesTMP;

    [Header("Damage Variables")]
    public float baseShootDmg;
    public float blasterShootDmg;
    public float missileExplosionDmg;
    public float electricBubbleDmg;

    public enum MovementState { moving, dashing}
    public enum GunType { baseShoot, blasterShoot}
    void Start()
    {
        dashTimer = 2f;
        rb = gameObject.GetComponent<Rigidbody>();
        meshRenderer = gameObject.GetComponent<Renderer>();
        playerLife = gameObject.GetComponent<PlayerLife>();
        invisible = gameObject.GetComponent<Invisible>();

        missilesTMP = GameObject.Find("RocketTMP").GetComponent<TMP_Text>();
        shootPoint = transform.Find("ShootPoint").gameObject;
        playerInputMap = playerInputAsset.FindActionMap("PlayerActions");
        moveXAction = moveX.ToInputAction();
        moveYAction = moveY.ToInputAction();
        dashAction = dash.ToInputAction();
        /*dashActionUp = dashUp.ToInputAction();
        dashActionDown = dashDown.ToInputAction();
        dashActionRight = dashRight.ToInputAction();
        dashActionLeft = dashLeft.ToInputAction();*/
        dashAction.performed += DashingPerformed;
        /*dashActionUp.performed += DashingPerformed;
        dashActionDown.performed += DashingPerformed;
        dashActionRight.performed += DashingPerformed;
        dashActionLeft.performed += DashingPerformed;*/
        attackAction = attack.ToInputAction();
        fireBallAction = fireBall.ToInputAction();
        fireBallAction.performed += SpawnFireBall;
        missilesTMP.text = $"Missiles: {+missiles} / {maxMissiles}";

    }

    void Update()
    {
        GetInput();
        DashingTimer();
        SpeedLimit();
        State();
        LimitMissiles();
    }
    private void FixedUpdate()
    {
        Movement();
        Attack();
        AttackType();
    }
    private void State()
    {
        if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
            meshRenderer.sharedMaterial = materialYellow;

        }
        else
        {
            state = MovementState.moving;
            desiredMoveSpeed = normalSpeed;
            if (playerLife.haveBubble)
            {
                if (playerLife.state == PlayerLife.State.Base)
                {
                    if (invisible.isInvisible)
                    {
                        meshRenderer.sharedMaterial = materialInvisible;
                    }
                    else
                    {
                        meshRenderer.sharedMaterial = materialBlue;
                    }
                }
            }
            else
            {
                if (playerLife.state == PlayerLife.State.Base)
                {
                    if (invisible.isInvisible)
                    {
                        meshRenderer.sharedMaterial = materialInvisible;
                    }
                    else
                    {
                        meshRenderer.sharedMaterial = InitialMaterial;
                    }
                }
                else if (playerLife.state == PlayerLife.State.Critic)
                {
                    if (playerLife.timerHit <= 0.3)
                    {
                        haveBlaster = false;
                    }
                    if (playerLife.timerCritic <= 1)
                    {
                        meshRenderer.sharedMaterial = playerLife.materialRed;
                    }
                    if (playerLife.timerCritic >= 3)
                    {
                        meshRenderer.sharedMaterial = playerLife.materialGreen;
                    }
                }
            }           

        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing)
        {
            keepMomentum = true;
        }

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }
    private void Movement()
    {
        if (state == MovementState.dashing) return;

        Vector3 moveDirection = transform.up * moveYfloat + transform.right * moveXfloat;
        //rb.AddRelativeForce(new Vector3(moveXfloat * moveSpeed, moveYfloat * moveSpeed, 0), ForceMode.Impulse);
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        
        //rb.AddForceAtPosition(new Vector3(moveXfloat * moveSpeed, moveYfloat * moveSpeed, 0) ,
        //new Vector3(transform.position.x, transform.position.y, transform.position.z), ForceMode.Impulse);

        
    }
    private void SpeedLimit()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0f);
        if (flatVel.magnitude >= moveSpeed)
        {
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitVel.x, limitVel.y, rb.linearVelocity.z);
            //rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
    }
    private float speedChangeFactor;
    
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }
    private void DashingPerformed(InputAction.CallbackContext context)
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0f);

        if (dashTimer >= 2f/*&& flatVel.magnitude >= 3f*/)
        {
            dashing = true;
            playerLife.canGetHit = false;
            //rb.AddForce(new Vector3(0, moveYfloat * dashForce, 0), ForceMode.Impulse);
            Vector3 direction = GetDirection(transform);
            Vector3 forceToApply = direction * dashForce;
            delayedForceToApply = forceToApply;
            Invoke(nameof(DelayedDashForce), 0.005f);
            Invoke(nameof(ResetDash), dashDuration);
            dashTimer = 0;

        }
    }
    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if (resetVel)
        {
            rb.linearVelocity = Vector3.zero;
        }
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }
    private void DashingTimer()
    {
        dashTimer += Time.deltaTime;
        
        if(dashTimer >= 0.5f && playerLife.canGetHit == false)
        {
            playerLife.canGetHit = true;
        }
    }

    private void ResetDash()
    {
        dashing = false;
    }
    private Vector3 GetDirection(Transform forwardT)
    {
        Vector3 direction = new Vector3();
        direction = forwardT.up * moveYfloat + forwardT.right * moveXfloat;

        if (moveXfloat == 0 && moveYfloat == 0)
        {
            direction = forwardT.up;
        }

        return direction.normalized;
    }

    private void Attack()
    {
        if(attackfloat > 0 && !dashing)
        {
            attacking = true;
            switch (gunType)
            {
                case GunType.baseShoot:
                    if (Time.time - lastAttackTime > delayAttack)
                    {
                        if (attacking)
                        {
                            StartCoroutine(ShootBullet());

                        }
                        lastAttackTime = Time.time;
                        //Invoke(nameof(SpawnBullet), 0.9f);
                    }
                    break;
                case GunType.blasterShoot:
                    if (Time.time - lastAttackTime > delayAttack)
                    {
                        if (attacking)
                        {
                            StartCoroutine(ShootBullet());

                        }
                        lastAttackTime = Time.time;
                        //Invoke(nameof(SpawnBullet), 0.9f);
                    }
                    break;
            }
            
        }
        else
        {
            attacking = false;
        }
    }
    private void AttackType()
    {
        if (haveBlaster)
        {
            gunType = GunType.blasterShoot;
            delayAttack = blasterDelayAttack;
        }
        else if(!haveBlaster || playerLife.state == PlayerLife.State.Critic)
        {
            gunType = GunType.baseShoot;
            delayAttack = baseDelayAttack;
        }
    }

    private void SpawnBullet()
    {
        Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation);

    }
    private IEnumerator ShootBullet()
    {
        
            for(int i = 0; i < 1; i++)
            {
                SpawnBullet();
                yield return new WaitForSeconds(1f);
            }
        
    }

    private void SpawnFireBall(InputAction.CallbackContext context)
    {
        if(missiles > 0)
        {
            Instantiate(fireBallAttack, shootPoint.transform.position, fireBallAttack.transform.rotation);
            missiles -= 1;
            missilesTMP.text = $"Missiles: {+missiles} / {maxMissiles}";
        }
    }
    
    private void LimitMissiles()
    {
        if(missiles > maxMissiles)
        {
            missiles = maxMissiles;
            missilesTMP.text = $"Missiles: {+missiles} / {maxMissiles}";
        }
        if(missiles < 0)
        {
            missiles = 0;
            missilesTMP.text = $"Missiles: {+missiles} / {maxMissiles}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blaster"))
        {
            haveBlaster = true;
            Money.score += 150 * Money.multiplier;
            //Destroy(other.gameObject);
        }
        if (other.CompareTag("FireBall"))
        {
            missiles += 1;
            missilesTMP.text = $"Missiles: {+missiles} / 3";
            Money.score += 150 * Money.multiplier;
            //Destroy(other.gameObject);
        }
    }
    private void GetInput()
    {
        moveXfloat = moveXAction.ReadValue<float>();
        moveYfloat = moveYAction.ReadValue<float>();
        attackfloat = attackAction.ReadValue<float>();
    }
    private void OnEnable()
    {
        moveXAction = moveX.ToInputAction();
        moveXAction.Enable();

        moveYAction = moveY.ToInputAction();
        moveYAction.Enable();

        dashAction = dash.ToInputAction();
        dashAction.Enable();

        attackAction = attack.ToInputAction();
        attackAction.Enable();

        fireBallAction = fireBall.ToInputAction();
        fireBallAction.Enable();
        /*dashActionUp = dashUp.ToInputAction();
        dashActionUp.Enable();

        dashActionDown = dashDown.ToInputAction();
        dashActionDown.Enable();

        dashActionRight = dashRight.ToInputAction();
        dashActionRight.Enable();

        dashActionLeft = dashLeft.ToInputAction();
        dashActionLeft.Enable();*/
    }

    private void OnDisable()
    {
        moveXAction.Disable();
        moveYAction.Disable();
        dashAction.Disable();
        attackAction.Disable();
        fireBallAction.Disable();
        /*dashActionUp.Disable();
        dashActionDown.Disable();
        dashActionRight.Disable();
        dashActionLeft.Disable();*/
    }
}
