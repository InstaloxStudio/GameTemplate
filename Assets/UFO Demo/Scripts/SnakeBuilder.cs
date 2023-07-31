using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBuilder : MonoBehaviour
{
    public GameObject segmentPrefab;
    public int numberOfSegments = 10;
    public float mass = 1f;
    public float drag = 0f;
    public float angularDrag = 0.05f;
    public float springStrength = 10f;
    public float springDamper = 1f;
    public float minLimit = -45f;
    public float maxLimit = 45f;

    void Start()
    {
        // The parent object for the previous segment in the chain
        Rigidbody parentBody = this.GetComponent<Rigidbody>();

        for (int i = 0; i < numberOfSegments; i++)
        {
            GameObject segment = Instantiate(segmentPrefab, transform);
            segment.transform.localPosition = new Vector3(0, (-i * segment.transform.localScale.x), 0);


            Rigidbody segmentRb = segment.GetComponent<Rigidbody>();
            segmentRb.mass = mass;
            segmentRb.drag = drag;
            segmentRb.angularDrag = angularDrag;

            HingeJoint hinge = segment.AddComponent<HingeJoint>();
            hinge.connectedBody = parentBody;

            JointSpring spring = new JointSpring();
            spring.spring = springStrength;
            spring.damper = springDamper;

            hinge.spring = spring;
            hinge.useSpring = true;

            JointLimits limits = new JointLimits();
            limits.min = minLimit;
            limits.max = maxLimit;

            hinge.limits = limits;
            hinge.useLimits = true;

            parentBody = segmentRb;
        }
    }
}
