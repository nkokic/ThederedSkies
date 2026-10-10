using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField]
    TurretDriver turretDriver;
    [SerializeField]
    Transform barrel;

    DistanceJoint2D distanceJoint;
    Rigidbody2D target;
    Rigidbody2D rb;
    Vector2 error;

    [SerializeReference, SubclassPicker]
    IRopeRenderer ropeRenderer;
    [SerializeField]
    LineRenderer lineRenderer;

    bool isActivated => target != null;

    [SerializeField]
    float maxDistance = 10;
    [SerializeField]
    float spoolingForce = 0;

    [SerializeField]
    float spoolingSpeed = 1, spoolingDamp = 1;

    [SerializeField]
    float strenght = 100;
    readonly static float minDistance = 1f;

    public float MaxDistance 
    { 
        get => distanceJoint.distance;
        set
        {
            distanceJoint.distance = value;
        }
    }

    private void Awake()
    {
        ConstructDistanceJoint();
    }
    void ConstructDistanceJoint()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        distanceJoint = rb.gameObject.AddComponent<DistanceJoint2D>();

        distanceJoint.autoConfigureDistance = false;
        distanceJoint.enableCollision = true;
        distanceJoint.maxDistanceOnly = true;
        distanceJoint.breakAction = JointBreakAction2D.CallbackOnly;
        distanceJoint.breakForce = strenght;
        Deactivate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
            Toggle();

        if (isActivated)
        {
            turretDriver.LookAt(target.transform.TransformPoint(distanceJoint.connectedAnchor));
            distanceJoint.anchor = rb.transform.InverseTransformPoint(barrel.position);
            ropeRenderer.RenderRope(barrel.position,
                                    target.transform.TransformPoint(distanceJoint.connectedAnchor),
                                    lineRenderer
                                    );
        }
        else
            turretDriver.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void FixedUpdate()
    {
        if (isActivated)
        {
            error = rb.transform.TransformPoint(distanceJoint.anchor) - target.transform.TransformPoint(distanceJoint.connectedAnchor);
            if (Input.mouseScrollDelta.y > 0)
                SpoolDown();
            else if (Input.mouseScrollDelta.y < 0)
                spoolingForce = spoolingSpeed;

            if(spoolingForce > 0)
                SpoolUp();

            spoolingForce -= spoolingDamp * Time.fixedDeltaTime;
            spoolingForce = Mathf.Clamp(spoolingForce, 0, float.PositiveInfinity);
        }
    }
    public void Toggle()
    {
        if (isActivated)
            Deactivate();
        else
            Activate();
    }

    public void SpoolDown()
    {
        distanceJoint.distance += spoolingSpeed * Time.deltaTime;
        distanceJoint.distance = Mathf.Clamp(distanceJoint.distance, minDistance, maxDistance);
    }

    void SpoolUp()
    {
        float massRatio = MathExpanded.GetRatioWithInfinites(rb.mass, target.mass + rb.mass);

        if (target.isKinematic)
            massRatio = 0;

        rb.AddForceAtPosition(spoolingForce * rb.mass * -error.normalized * (1 - massRatio), 
                              rb.transform.TransformPoint(distanceJoint.anchor));
        target.AddForceAtPosition(spoolingForce * target.mass * error.normalized * massRatio,
                                  target.transform.TransformPoint(distanceJoint.connectedAnchor));
        if (error.magnitude < distanceJoint.distance)
            distanceJoint.distance = error.magnitude;
    }

    void Activate()
    {
        var hit = Physics2D.Raycast(barrel.position,
                                    transform.up,
                                    maxDistance,
                                    LayerMask.GetMask(Projectile.targetableLayer)
                                    );

        if(hit.rigidbody != null)
        {
            target = hit.rigidbody;
            distanceJoint.connectedBody = target;
            distanceJoint.connectedAnchor = hit.transform.InverseTransformPoint(hit.point);
            distanceJoint.distance = (hit.point - (Vector2)barrel.position).magnitude;
            distanceJoint.enabled = true;

            lineRenderer.enabled = true;
        }
    }

    void Deactivate()
    {
        lineRenderer.enabled = false;

        spoolingForce = 0;
        distanceJoint.enabled = false;
        distanceJoint.connectedBody = null;
        target = null;
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        Deactivate();
    }
}
