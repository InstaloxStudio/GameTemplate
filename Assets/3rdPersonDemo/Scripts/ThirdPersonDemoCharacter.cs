using UnityEngine;

public class ThirdPersonDemoCharacter : RigidBodyCharacter
{
    [Header("Ball Properties")]
    public bool hasBall;
    public GameObject ballPrefab;
    public GameBall gameBall;

    [Header("Throw Properties")]
    public float throwForce = 10f;
    public float throwHeight = 1f;
    public float throwMaxHeight = 1000f;
    public float throwDistance = 1f;

    [Header("Charge Properties")]
    public float throwChargeTime = 0f;
    public float throwChargeMax = 2f;
    public float maxThrowForce = 1000f;
    public float currentThrowForce = 0f;

    [Header("Parabola Properties")]
    public float parabolaScale = 0.2f; // Change this value to your liking
    private LineRenderer lr;
    public float maxTimeOfFlight = 3f; // Adjust as needed


    public override void Initialize()
    {
        base.Initialize();
        lr = GetComponent<LineRenderer>();

        this.GetPlayerController().LockMouse();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetPlayerController().ToggleMouseLock();
        }

        //do a charge throw if we have the ball
        if (hasBall && Input.GetMouseButton(0))
        {
            lockRotation = true;
            throwChargeTime = Mathf.PingPong(Time.time, throwChargeMax);
            //throwChargeTime += Time.deltaTime;
            currentThrowForce = Mathf.Lerp(0, maxThrowForce, throwChargeTime / throwChargeMax);
            throwHeight = Mathf.Lerp(0, throwMaxHeight, throwChargeTime / throwChargeMax);
            var calculateDirection = transform.forward * currentThrowForce + transform.up * throwHeight;
            DrawParabola(currentThrowForce, throwHeight);

        }

        if (!hasBall && Input.GetMouseButtonDown(0))
        {
            SpawnBall();
        }

        if (hasBall && Input.GetMouseButtonUp(0))
        {

            lockRotation = false;
            lr.positionCount = 0;
            ThrowChargeBall(currentThrowForce);
            currentThrowForce = 0f;
            throwChargeTime = 0f;
            throwHeight = 1f;
        }

    }

    public void ThrowChargeBall(float charge)
    {
        gameBall.transform.parent = null;
        gameBall.rb.isKinematic = false;
        gameBall.rb.useGravity = true;
        gameBall.rb.velocity = Vector3.zero;
        gameBall.rb.angularVelocity = Vector3.zero;
        gameBall.rb.AddForce(transform.forward * charge + transform.up * throwHeight);
        gameBall.rb.AddTorque(transform.right * throwDistance);
        hasBall = false;
    }
    public void ThrowBall()
    {
        gameBall.transform.parent = null;
        gameBall.rb.isKinematic = false;
        gameBall.rb.useGravity = true;
        gameBall.rb.velocity = Vector3.zero;
        gameBall.rb.angularVelocity = Vector3.zero;
        gameBall.rb.AddForce(transform.forward * throwForce + transform.up * throwHeight);
        gameBall.rb.AddTorque(transform.right * throwDistance);
        hasBall = false;
    }

    public void PickUpBall(GameBall ball)
    {
        if (hasBall)
            return;

        gameBall = ball;
        gameBall.transform.parent = transform;
        gameBall.transform.localPosition = new Vector3(0, 1, 2);
        gameBall.rb.isKinematic = true;
        gameBall.rb.useGravity = false;
        hasBall = true;
    }

    public void DrawParabola(float charge, float height)
    {
        var line = lr;
        line.positionCount = 100;
        var points = new Vector3[100];

        Vector3 start = gameBall.transform.position;
        Vector3 direction = transform.forward * charge * parabolaScale;

        for (int i = 0; i < 100; i++)
        {
            var t = i / 100f * maxTimeOfFlight; // Scale t to represent seconds up to maxTimeOfFlight
            var x = direction * t;
            var y = height * parabolaScale * t - 0.5f * -Physics.gravity.y * t * t;
            points[i] = start + x + y * transform.up;
        }
        line.SetPositions(points);
    }


    public void SpawnBall()
    {
        var prefab = Instantiate(ballPrefab, transform.position + transform.forward * 2f, Quaternion.identity);
        var ball = prefab.GetComponent<GameBall>();
        PickUpBall(ball);
    }
    public Vector3 CalculateParabola(Vector3 start, Vector3 direction, float height, float gravity, float t)
    {
        float parabola = -gravity * t * t + height * t; // Parabolic equation
        return start + t * direction + parabola * Vector3.up;
    }


    public Vector3 CalculateParabolaPoint(float t, Vector3 initialPosition, Vector3 initialVelocity)
    {
        float tTotal = 2 * initialVelocity.y / -Physics.gravity.y;
        t *= tTotal; // Scale t to the total time of flight

        Vector3 position = initialPosition;
        position += initialVelocity * t; // Add the initial velocity multiplied by time
        position += 0.5f * Physics.gravity * t * t; // Add the effect of gravity

        return position;
        //usage
        // Vector3 initialPosition = transform.position;
        //Vector3 initialVelocity = transform.forward * currentThrowForce + transform.up * throwHeight;

        //float t = 0.5f; // For the peak of the parabola
        // Vector3 peakPoint = CalculateParabolaPoint(t, initialPosition, initialVelocity);

    }


}