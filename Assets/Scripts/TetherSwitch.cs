using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSwitch : MonoBehaviour
{
    public Tether tether;

    private TetherSwitchToggle tetherSwitchToggle;

    public List<GameObject> trajectoryTargets;

    public float radius;

    public float angle;

    private Animator animator;

    private void Awake()
    {
        tether = GetTether();
        tetherSwitchToggle = GetComponentInChildren<TetherSwitchToggle>();
        animator = GetComponentInChildren<Animator>();
    }

    public Vector2 HomeOnTarget(Rigidbody2D projectile)
    {
        foreach (GameObject targetObject in trajectoryTargets)
        {
            Vector2 target = new Vector2(targetObject.transform.position.x, targetObject.transform.position.y);
            Vector2 differenceVector = target - projectile.position;
            if (Vector2.Angle(differenceVector, projectile.velocity) < 8)
            {
                return projectile.velocity.magnitude * differenceVector.normalized;
            }
        }
        return projectile.velocity;
    }

    public Tether GetTether()
    {
        return GetComponentInParent<Tether>();
    }

    public bool IsActivated()
    {
        return tetherSwitchToggle.isActivated;
    }

    private void OnDrawGizmosSelected()
    {
        Tether tether = GetTether();
        if (tether != null)
        {
            Vector2 displacementVector = new Vector2(transform.position.x - tether.transform.position.x,
                transform.position.y - tether.transform.position.y);
            Vector2 tempVector = Vector2.Perpendicular(displacementVector).normalized;
            Vector3 perpVector = new Vector3(tempVector.x, tempVector.y, 0);
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(transform.position + perpVector * 20, transform.position - perpVector * 20);
            Gizmos.DrawWireSphere(tether.transform.position, radius);
        }
    }

    public void TriggerSwitchAnimation()
    {
        animator.SetTrigger("tetherActionPerformed");
    }

}
