using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Projectile projectile;

    public float updateDamping = 0.15f;

    public GameObject[] payloads;
    static readonly Vector2 size = Vector2.one * 0.1f;
    public void SetProjectile(Projectile projectile)
    {
        this.projectile = projectile;
    }

    private void LateUpdate()
    {
        transform.position += (Vector3)projectile.direction * projectile.speed * Time.deltaTime;
    }
    int i = 0;
    private void FixedUpdate()
    {
        float angle = Vector2.SignedAngle(Vector2.up, projectile.direction);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        var hit = Physics2D.BoxCast(
                                projectile.position,
                                size,
                                angle,
                                projectile.direction,
                                Mathf.Min(projectile.remainingRange, projectile.speed * Time.fixedDeltaTime),
                                LayerMask.GetMask(Projectile.targetableLayer)
                                );
        if (hit.fraction == 0)
            hit.fraction = 1;
        Debug.DrawRay(projectile.position, projectile.direction * Mathf.Min(projectile.remainingRange,
                    projectile.speed * Time.fixedDeltaTime * hit.fraction), Color.red);
        TranverseTo(hit);

        if (projectile.remainingRange <= 0)
            gameObject.SetActive(false);
    }

    void TranverseTo(RaycastHit2D hit)
    {
        Health target;
        if (hit.collider != null)
        {
            hit.collider.TryGetComponent(out target);

            float refraction = hit.collider.bounciness;

            if (target != null)
            {
                target.HitObject(hit.point, hit.normal, projectile);
                refraction = target.armor.GetArmorFactor(hit.point - target.GetRigidBody.position);
            }
            projectile.Ricochet(hit.normal, refraction);

            projectile.position = hit.point;
            transform.position = hit.point;

            foreach(var payload in payloads)
                Instantiate(payload, transform.position, transform.rotation);
        }
        else
        {
            hit.fraction = 1;
            projectile.position += projectile.direction * projectile.speed * Time.fixedDeltaTime;
        }

        projectile.remainingRange -= projectile.speed * Time.fixedDeltaTime * hit.fraction;
    }
}
