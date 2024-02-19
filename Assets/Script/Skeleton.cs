using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class Skeleton : MonoBehaviour
{
    public GameObject Player;
    public string MonsterName; // Skeleton
    public float SkeletonCurrentHP; // ���̷��� ����ü��
    public float SkeletonHP; // ���̷��� �� ü��
    public float SkeletonAttackPower; // ���̷��� ���ݷ�
    public float StageLV;
    public float StageGoal; //�������� Ŭ��� ���� ��ǥ ����
    public float StageMonster;// �������� Ŭ��� ���� ���� ����
    public float RespawnTime; // ������ ��Ÿ��


    public Slider SkeletonHPUI;
    public Text StageNum;
    public Text MonCount;
    public AudioSource StageUpSound;
    public Animator Anim;

    private void Start()
    {
        SkeletonSet();
        SetStage();
        Player = GameObject.FindGameObjectWithTag("player");
    }

    private void Update()
    {
        CheckSkeletonHP();

        StageNum.text = "Stage " + StageLV;
        MonCount.text = StageMonster + "/" + StageGoal;

        if(SkeletonCurrentHP <= 0)
        {
            Player.GetComponent<Player>().Enemy = null;
            Anim.SetInteger("AnimDead", 1);
        }

        if (StageMonster > StageGoal)
        {
            StageClear();
        }

        if (Player != null)
        {
            if (Player.GetComponent<Player>().nowPlayer.CurrentHP > 0)
            {
                Anim.SetInteger("AnimState", 1);
            }
        }
        else if (Player == null)
        {
            Anim.SetInteger("AnimState", 2);
        }


    }


    public void StageClear()
    {
        StageLV++;
        SkeletonHP += 50f;
        SkeletonAttackPower += 10f;
        StageMonster = 0;
        StageUpSound.Play();
    }

    public void SetStage()
    {
        StageMonster = 1f;
        StageLV = 1;
        StageGoal = 5;
    }
    public void SkeletonSet()
    {
        SkeletonHP = 50f;
        SkeletonCurrentHP = SkeletonHP;
        RespawnTime = 2f;
    }

    public void CheckSkeletonHP()
    {
        if (SkeletonHPUI != null)
            SkeletonHPUI.value = SkeletonCurrentHP / SkeletonHP * 100f;
    }

    public void AttackPlayer()
    {
        float damage;
        damage = SkeletonAttackPower - Player.GetComponent<Player>().nowPlayer.DefencePower;
        if(damage <= 0) // �޴� �������� �����ų� 0�̸� ������ 1�� ����
        {
            damage = 1;
        }
        Player.GetComponent<Player>().nowPlayer.CurrentHP -= damage;
    }

  
    public void Death()
    {
        SkeletonHPUI.gameObject.SetActive(false);
        Player.GetComponent<Player>().nowPlayer.CurrentEXP += 20;
        Player.GetComponent<Player>().nowPlayer.Credit += Random.Range(8, 12);
        
    }

    public void DisalbeObject()
    {
        this.gameObject.SetActive(false);
        Invoke("Respawn", RespawnTime); //2�� �� ������Ʈ ��Ȱ
    }
    public void Respawn()
    {
        StageMonster++;
        this.gameObject.SetActive(true);
        SkeletonCurrentHP = SkeletonHP;
        SkeletonHPUI.gameObject.SetActive(true);
        Player.GetComponent<Player>().Enemy = this.gameObject;
    }
}

