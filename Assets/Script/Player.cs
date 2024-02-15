using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject Enemy;
    public string PlayerName; // 플레이어 닉네임
    public float CurrentHP; //현재 체력
    public float HP; //Max HP
    public float AttackPower; //공격력
    public int Level; // 플레이어 레벨
    public float CurrentEXP; //현재 경험치
    public float EXP; //Max Exp

    public Text Name;
    public Slider PlayerHPUI;
    public Animator Anim;
    public Text LevelUI;
    public Slider PlayerEXPUI;
    
    void Start()
    {
        PlayerSet();
        Enemy = GameObject.FindGameObjectWithTag("enemy");
        
    }

    void Update()
    {
        CheckPlayerEXP();
        CheckPlayerHP();
        LevelUp();

        LevelUI.text = "LV : " + Level;

        if (Enemy != null)
        {
            if (Enemy.GetComponent<Skeleton>().SkeletonCurrentHP > 0)
            {
                Anim.SetInteger("AnimState", 1);
            }
        }
        else if (Enemy == null)
        {
            Anim.SetInteger("AnimState", 2);
        }
        
    }

    public void PlayerSet() // 초기 설정
    {
        Name.text = PlayerName; 
        HP = 100f;
        CurrentHP = HP;
        EXP = 100f; // 0레벨 플레이어 최대 경험치
        CurrentEXP = 0;
    }

    public void CheckPlayerHP()
    {
        if(PlayerHPUI != null)
            PlayerHPUI.value = CurrentHP / HP * 100f;
    }

    public void CheckPlayerEXP()
    {
        if(PlayerEXPUI != null)
        {
            PlayerEXPUI.value = CurrentEXP / EXP * 100f;
        }
    }

    public void AttackEnemy()
    {
        Enemy.GetComponent<Skeleton>().SkeletonCurrentHP -= AttackPower;
    }

    public void LevelUp()
    {
        if(CurrentEXP >= EXP)
        {
            CurrentEXP -= EXP;
            Level++;
            AttackPower += 3f;
            EXP += 100f;
        }
    }
}
