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
    public float DefencePower; // 방어력
    public int Level; // 플레이어 레벨
    public float CurrentEXP; //현재 경험치
    public float EXP; //Max Exp
    public float Credit; // 플레이어 자금
    public float PosSetValue; // 포션세팅 퍼센트
    public float Healing; // 포션 회복량
    public float Postion1Count;// 보유 포션 개수
    public float Postion2Count;
    public float Postion3Count;
    public float HPIncrease;// 훈련 HP 증가량
    public float AttackIncrease;
    public float DefenceIncrease;
    public float HPPrice; // HP 훈련 가격
    public float AttackPrice;
    public float DefencePrice;
    public float Pos1Price; // 포션 가격
    public float Pos2Price;
    public float Pos3Price;
    public float Buff1Price; // 버프 가격


    public Text Name;
    public Text HPText;
    public Text AttackText;
    public Text DefenceText;
    public Slider PlayerHPUI;
    public Slider PostionSetting;
    public Text PostionPersent;
    public Text Pos1CountText;
    public Text Pos2CountText;
    public Text Pos3CountText;
    public Animator Anim;
    public Text LevelUI;
    public Slider PlayerEXPUI;
    public Text CreditText;
    public Text HPIncreaseText;
    public Text AttackIncreaseText;
    public Text DefenceIncreaseText;
    public Text HPPriceText;
    public Text AttackPriceText;
    public Text DefencePriceText;
    public Text Pos1PriceText;
    public Text Pos2PriceText;
    public Text Pos3PriceText;
    public AudioSource LvUPSound; // 레벨업 사운드
    public AudioSource AttackSound; // 공격 사운드
    
    
    void Start()
    {
        PlayerSet();
        
        Enemy = GameObject.FindGameObjectWithTag("enemy");
        PostionSetting.onValueChanged.AddListener(PostionSet);
        PostionPersent.text = PosSetValue + "%";
        Pos1PriceText.text = "" + Pos1Price;
        Pos2PriceText.text = "" + Pos2Price;
        Pos3PriceText.text = "" + Pos3Price;
    }

    void Update()
    {
        CheckPlayerEXP();
        CheckPlayerHP();
        LevelUp();
        UpdateText();

        if (CurrentHP / HP *100 < PostionSetting.value)
        {
            UsePostion();
        }

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Enemy.GetComponent<Skeleton>().Player = null;
            Anim.SetInteger("AnimDead", 1);
        }

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

    public void UpdateText()
    {
        LevelUI.text = "LV : " + Level;
        CreditText.text = "" + Credit;
        HPText.text = "HP             " + CurrentHP + "/" + HP;
        AttackText.text = "Attack              " + AttackPower;
        DefenceText.text = "Defence             " + DefencePower;
        Pos1CountText.text = "" + Postion1Count;
        Pos2CountText.text = "" + Postion2Count;
        Pos3CountText.text = "" + Postion3Count;
        HPIncreaseText.text = "+" + HPIncrease;
        AttackIncreaseText.text = "+" + AttackIncrease;
        DefenceIncreaseText.text = "+" + DefenceIncrease;
        HPPriceText.text = "" + HPPrice;
        AttackPriceText.text = "" + AttackPrice;
        DefencePriceText.text = "" + DefencePrice;
    }

    public void PlayerSet() // 초기 설정
    {
        Name.text = PlayerName;
        HP = 100f;
        CurrentHP = HP;
        AttackPower = 10f;
        DefencePower = 1f;
        EXP = 100f; // 0레벨 플레이어 최대 경험치
        CurrentEXP = 0;
        Credit = 0;
        PosSetValue = 30;
        Postion1Count = 5;
        Postion2Count = 0;
        Postion3Count = 0;
        SelectPos1();
        HPIncrease = 10f;
        AttackIncrease = 1f;
        DefenceIncrease = 1f;
        HPPrice = 10f;
        AttackPrice = 10f;
        DefencePrice = 10f;
        Pos1Price = 5f;
        Pos2Price = 10f;
        Pos3Price = 15f;
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
        AttackSound.Play();
    }

    public void LevelUp()
    {
        if(CurrentEXP >= EXP)
        {
            CurrentEXP -= EXP;
            Level++;
            AttackPower += 3f;
            EXP += 100f;
            LvUPSound.Play();
        }
    }

    public void PostionSet(float value)
    {
        PostionPersent.text = PostionSetting.value.ToString() + "%";
    }

    public void SelectPos1()
    {
        GameObject.Find("postion1").GetComponent<Image>().color = Color.yellow;
        GameObject.Find("postion2").GetComponent<Image>().color = Color.green;
        GameObject.Find("postion3").GetComponent<Image>().color = Color.green;
        Healing = 20f;
    }
    public void SelectPos2()
    {
        GameObject.Find("postion1").GetComponent<Image>().color = Color.green;
        GameObject.Find("postion2").GetComponent<Image>().color = Color.yellow;
        GameObject.Find("postion3").GetComponent<Image>().color = Color.green;
        Healing = 50f;
    }
    public void SelectPos3()
    {
        GameObject.Find("postion1").GetComponent<Image>().color = Color.green;
        GameObject.Find("postion2").GetComponent<Image>().color = Color.green;
        GameObject.Find("postion3").GetComponent<Image>().color = Color.yellow;
        Healing = 100f;
    }

    public void UsePostion()
    {
        
        if (Healing == 20f)
        {
            if (Postion1Count > 0)
            {
                CurrentHP += Healing;
                Postion1Count--;
            }
        }
        else if (Healing == 50f)
        {
            if (Postion2Count > 0)
            {
                CurrentHP += Healing;
                Postion2Count--;
            }
        }
        else if (Healing == 100f)
        {
            if (Postion3Count > 0)
            {
                CurrentHP += Healing;
                Postion3Count--;
            }
        }
            
    }

    public void HPTraining()
    {
        if(Credit >= HPPrice)
        {
            HP += HPIncrease;
            CurrentHP += HPIncrease;
            Credit -= HPPrice;
            HPIncrease += 10f;
            HPPrice += 2f;
        }
    }
    public void AttackTraining()
    {
        if (Credit >= AttackPrice)
        {
            AttackPower += AttackIncrease;
            Credit -= AttackPrice;
            AttackIncrease += 1f;
            AttackPrice += 2f;
        }
    }
    public void DefenceTraining()
    {
        if (Credit >= DefencePrice)
        {
            DefencePower += DefenceIncrease;
            Credit -= DefencePrice;
            DefenceIncrease += 1f;
            DefencePrice += 2f;
        }
    }
    
    public void BuyPostion1()
    {
        if(Credit >= Pos1Price)
        {
            Credit -= Pos1Price;
            Postion1Count++;
        }
    }
    public void BuyPostion2()
    {
        if (Credit >= Pos2Price)
        {
            Credit -= Pos2Price;
            Postion2Count++;
        }
    }
    public void BuyPostion3()
    {
        if (Credit >= Pos3Price)
        {
            Credit -= Pos3Price;
            Postion3Count++;
        }
    }
}
