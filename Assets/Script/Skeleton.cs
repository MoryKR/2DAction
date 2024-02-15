using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : MonoBehaviour
{
    public GameObject Player;
    public string MonsterName; // Skeleton
    public float SkeletonCurrentHP; // ½ºÄÌ·¹Åæ ÇöÀçÃ¼·Â
    public float SkeletonHP; // ½ºÄÌ·¹Åæ ÃÑ Ã¼·Â
    public float SkeletonAttackPower; // ½ºÄÌ·¹Åæ °ø°Ý·Â
    public float StageLV;
    public float StageMonster;

    public Text SkeletonName;
    public Slider SkeletonHPUI;
    public Text StageNum;
    public Text MonCount;

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
        MonCount.text = StageMonster + "/ 10";

        if(SkeletonCurrentHP <= 0)
        {
            Anim.SetInteger("AnimState", 3);
            Player.GetComponent<Player>().Enemy = null;
            
        }

        if(StageMonster > 10)
        {
            StageClear();
        }


    }

    public void StageClear()
    {
        StageLV++;
        SkeletonHP += 20f;
        SkeletonAttackPower += 0f;
        StageMonster = 0;
    }

    public void SetStage()
    {
        StageMonster = 1f;
        StageLV = 1;
    }
    public void SkeletonSet(/*float amount*/)
    {
        SkeletonName.text = MonsterName;
        SkeletonHP = 50f;
        SkeletonCurrentHP = SkeletonHP;
    }

    public void CheckSkeletonHP()
    {
        if (SkeletonHPUI != null)
            SkeletonHPUI.value = SkeletonCurrentHP / SkeletonHP * 100f;
    }

    public void Death()
    {
        SkeletonName.gameObject.SetActive(false);
        SkeletonHPUI.gameObject.SetActive(false);
        Player.GetComponent<Player>().CurrentEXP += 20;
        Player.GetComponent<Player>().Credit += Random.Range(8, 12);
        StageMonster++;
    }

    public void DisalbeObject()
    {
        this.gameObject.SetActive(false);
        Invoke("Respawn", 2); //2ÃÊ ÈÄ ¿ÀºêÁ§Æ® ºÎÈ°
    }
    public void Respawn()
    {
        this.gameObject.SetActive(true);
        SkeletonCurrentHP = SkeletonHP;
        SkeletonName.gameObject.SetActive(true);
        SkeletonHPUI.gameObject.SetActive(true);
        Player.GetComponent<Player>().Enemy = this.gameObject;
    }
}

