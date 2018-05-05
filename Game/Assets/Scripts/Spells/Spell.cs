using System;
using UnityEngine;

public class Spell {

    private float speed;
    private float kockback;
    private float stunTime;
    private float coolDownDuration;
    private float damage;

    private GameObject prefab;
    private String effectName;

    public Spell(float speed, float kockback, float stunTime, float coolDownDuration, float damage, GameObject prefab, String effectName)
    {
        this.speed = speed;
        this.kockback = kockback;
        this.stunTime = stunTime;
        this.coolDownDuration = coolDownDuration;
        this.damage = damage;
        this.prefab = prefab;
        this.effectName = effectName;
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public float Kockback
    {
        get
        {
            return kockback;
        }
        set
        {
            kockback = value;
        }
    }

    public float StunTime
    {
        get
        {
            return stunTime;
        }
        set
        {
            stunTime = value;
        }
    }

    public float CoolDownDuration
    {
        get
        {
            return coolDownDuration;
        }
        set
        {
            coolDownDuration = value;
        }
    }

    public float Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public GameObject Prefab
    {
        get
        {
            return prefab;
        }
        set
        {
            prefab = value;
        }
    }

    public String EffectName
    {
        get
        {
            return effectName;
        }
        set
        {
            effectName = value;
        }
    }
}
