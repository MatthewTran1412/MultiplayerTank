using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              
    public float m_CurrentHealth=1;
    private void Explosion() {
        gameObject.name="";
        Collider[] colliders = Physics.OverlapSphere(transform.position,m_ExplosionRadius,m_TankMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if(!targetRigidbody)
                continue;
            targetRigidbody.AddExplosionForce(m_ExplosionForce,transform.position,m_ExplosionRadius);
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
            Item item = targetRigidbody.GetComponent<Item>();
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
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Player" && gameObject.name=="Medkit(Clone)")
        {
            float heal=other.gameObject.GetComponent<TankHealth>().m_CurrentHealth/100>0.5f?20:40;
            other.gameObject.GetComponent<TankHealth>().m_CurrentHealth+=heal;
            Destroy(gameObject);
        }
    }
    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition-transform.position;
        float expplosionDistance=explosionToTarget.magnitude;
        float relativeDistance=(m_ExplosionRadius-expplosionDistance)/m_ExplosionRadius;
        float damage = relativeDistance*m_MaxDamage;
        damage=Mathf.Max(0f,damage);
        return damage;
    }
    public void TakeDamage(float amount)
    {
        m_CurrentHealth-=amount;
        if(m_CurrentHealth<=0 && gameObject.name=="barrel(Clone)")
            Explosion();
    }
}
