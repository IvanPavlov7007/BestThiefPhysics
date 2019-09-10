using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HumanoidController : FollowingObject
{
    [SerializeField]
    private float maxDistanceToVehicle, minDistanceToVehicle, torqueKoefficient;
    [SerializeField]
    private LayerMask vehiclesLayerMask, groundMask;
    private Animator animator;
    private Rigidbody rb;
    private CharacterState currentState;

    public event SeatingInVehicleHandler onSeat;
    public event DisembarkingHandler onDisembark;

    private int horzFloat = Animator.StringToHash("Horz");
    private int vertFloat = Animator.StringToHash("Vert");
    private int shiftFloat = Animator.StringToHash("Shift");
    private float forwardMovingAxis;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        SetState(typeof(OnFeetState), null);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(shiftFloat, Input.GetAxis("Shift"), 0.2f, Time.deltaTime);
        animator.SetFloat(horzFloat, Input.GetAxis("Horizontal"), 0.2f, Time.deltaTime);
        animator.SetFloat(vertFloat, Input.GetAxis("Vertical"), 0.2f, Time.deltaTime);
        currentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void SetState(System.Type stateType, object parm)
    {
        if (stateType == typeof(OnFeetState))
        {
            currentState = new OnFeetState(rb, animator, transform, this, maxDistanceToVehicle, minDistanceToVehicle, torqueKoefficient, vehiclesLayerMask, groundMask);
            (currentState as OnFeetState).onSeat += onSeat;
        }
        else if (stateType == typeof(SeatingState))
        {
            currentState = new SeatingState(rb, animator, transform, this, parm as Vehicle);
            (currentState as SeatingState).onDisembark += onDisembark;
        }
    }

    private Coroutine stateCoroutine;
    public void StartStateCoroutine(StateCoroutine coroutine)
    {
        if (stateCoroutine != null)
            StopCoroutine(stateCoroutine);
        stateCoroutine = StartCoroutine(coroutine());
    }
}

public delegate void SeatingInVehicleHandler(Vehicle vehicle);
public delegate void DisembarkingHandler();
public delegate IEnumerator StateCoroutine();
public abstract class CharacterState
{
    protected HumanoidController humanoidController;
    protected Rigidbody rb;
    protected Animator animator;
    protected Transform transform;
    public CharacterState(Rigidbody rb, Animator anim, Transform transform, HumanoidController humanoidController)
    {
        this.rb = rb;
        this.animator = anim;
        this.transform = transform;
        this.humanoidController = humanoidController;
    }

    private CharacterState() { }
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
}

public class OnFeetState : CharacterState
{
    private float maxDistanceToVehicle, minDistanceToVehicle, torqueKoefficient;
    private int vehiclesLayerMask,groundMask;
    private int goToVehicleTrigger = Animator.StringToHash("GoToVehicle");
    private int sittingBool = Animator.StringToHash("SittingInVehicle");
    private int disntaceFloat = Animator.StringToHash("DistanceToTheGround");

    public event SeatingInVehicleHandler onSeat;

    public OnFeetState(Rigidbody rb, Animator anim, Transform transform, HumanoidController humanoidController, float maxDistanceToVehicle, float minDistanceToVehicle, float torqueKoefficient, int vehiclesLayerMask, int groundMask) : base(rb, anim, transform,humanoidController)
    {
        this.maxDistanceToVehicle = maxDistanceToVehicle;
        this.minDistanceToVehicle = minDistanceToVehicle;
        this.torqueKoefficient = torqueKoefficient;
        this.vehiclesLayerMask = vehiclesLayerMask;
        this.groundMask = groundMask;

        transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right,Vector3.up), Vector3.up);
        animator.SetBool(sittingBool, false);
        humanoidController.GetComponent<Collider>().enabled = true;
    }

    public override void OnUpdate()
    {
        if (Input.GetButtonDown("SeatInAVehicle"))
            startSeatingIntoAVehicle();
        RaycastHit hit;
        Physics.Raycast(new Ray(transform.position,Vector3.down),  out hit, 2000f, groundMask);
        animator.SetFloat(disntaceFloat, hit.distance);
    }
    public override void OnFixedUpdate()
    {
        //rb.AddTorque(Vector3.Cross(transform.up, Vector3.up) * torqueKoefficient);
    }

    public void startSeatingIntoAVehicle()
    {
        var colliders = Physics.OverlapSphere(transform.position,maxDistanceToVehicle,vehiclesLayerMask);
        if (colliders.Length > 0)
        {
            var vehicle = theNearestCollider(colliders).gameObject.GetComponent<Vehicle>();
            humanoidController.StartCoroutine(seatIntoAVehicleCoroutine(vehicle));
        }
    }

    private IEnumerator seatIntoAVehicleCoroutine(Vehicle vehicle)
    {
        transform.LookAt(MyTools.yPlaneVector(vehicle.transform.position,transform.position.y));
        animator.SetTrigger(goToVehicleTrigger);
        yield return new WaitUntil(
            () => { return (transform.position - vehicle.transform.position).magnitude < minDistanceToVehicle; }
            );
        animator.ResetTrigger(goToVehicleTrigger);
        onSeat?.Invoke(vehicle);
        humanoidController.SetState(typeof(SeatingState),vehicle);
        yield break;
    }

    Collider theNearestCollider(Collider[] colliders)
    {
        float smallestDist = Vector3.Distance(colliders[0].transform.position, this.transform.position);
        Collider nearest = colliders[0];
        for (int i = 1; i < colliders.Length; i++)
        {
            float dist = Vector3.Distance(colliders[i].transform.position, this.transform.position);
            if (dist < smallestDist)
            {
                smallestDist = dist;
                nearest = colliders[i];
            }
        }
        return nearest;
    }
}

public class SeatingState : CharacterState
{
    private Vehicle vehicle;

    private int sittingBool = Animator.StringToHash("SittingInVehicle");

    public event DisembarkingHandler onDisembark;
    public SeatingState(Rigidbody rb, Animator anim, Transform transform, HumanoidController humanoidController,Vehicle vehicle) : base(rb, anim, transform, humanoidController)
    {
        this.vehicle = vehicle;
        animator.SetBool(sittingBool, true);
        animator.Update(0f);
        humanoidController.GetComponent<CapsuleCollider>().enabled = false;

    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        transform.position = vehicle.SeatPlace.position;
        transform.rotation = vehicle.transform.rotation;
        if (Input.GetButtonDown("SeatInAVehicle"))
            getOutOfTheVehicle();
    }

    public void getOutOfTheVehicle()
    {
        onDisembark?.Invoke();
        humanoidController.SetState(typeof(OnFeetState),null);
    }
}