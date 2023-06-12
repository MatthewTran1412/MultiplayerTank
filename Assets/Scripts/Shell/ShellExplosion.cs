using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,m_ExplosionRadius,m_TankMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if(!targetRigidbody)
                continue;
            targetRigidbody.AddExplosionForce(m_ExplosionForce,transform.position,m_ExplosionRadius);
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
            Item item=targetRigidbody.GetComponent<Item>();
            if(!targetHealth&&!item)
                continue;
            float damage=CalculateDamage(targetRigidbody.position);
            if(targetHealth)
                targetHealth.TakeDamage(damage);
            if(item)
                item.TakeDamage(damage);
        }
        m_ExplosionParticles.transform.parent=null;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
        Destroy(m_ExplosionParticles.gameObject,m_ExplosionParticles.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition-transform.position;
        float expplosionDistance=explosionToTarget.magnitude;
        float relativeDistance=(m_ExplosionRadius-expplosionDistance)/m_ExplosionRadius;
        float damage = relativeDistance*m_MaxDamage;
        damage=Mathf.Max(0f,damage);
        return damage;
    }
}