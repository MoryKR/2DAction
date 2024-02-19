using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerData
{
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
}


public class Player : MonoBehaviour
{
    
    public PlayerData nowPlayer = new PlayerData();
    string path;
    string filename;
    

    public GameObject Enemy;
    /*public string PlayerName; // 플레이어 닉네임
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
    public float Buff1Price; // 버프 가격*/


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
        PostionPersent.text = nowPlayer.PosSetValue + "%";
        Pos1PriceText.text = "" + nowPlayer.Pos1Price;
        Pos2PriceText.text = "" + nowPlayer.Pos2Price;
        Pos3PriceText.text = "" + nowPlayer.Pos3Price;
    }

    void Update()
    {
        CheckPlayerEXP();
        CheckPlayerHP();
        LevelUp();
        UpdateText();

        if (nowPlayer.CurrentHP / nowPlayer.HP *100 < PostionSetting.value)
        {
            UsePostion();
        }

        if (nowPlayer.CurrentHP <= 0)
        {
            nowPlayer.CurrentHP = 0;
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
        LevelUI.text = "LV : " + nowPlayer.Level;
        CreditText.text = "" + nowPlayer.Credit;
        HPText.text = "HP             " + nowPlayer.CurrentHP + "/" + nowPlayer.HP;
        AttackText.text = "Attack              " + nowPlayer.AttackPower;
        DefenceText.text = "Defence             " + nowPlayer.DefencePower;
        Pos1CountText.text = "" + nowPlayer.Postion1Count;
        Pos2CountText.text = "" + nowPlayer.Postion2Count;
        Pos3CountText.text = "" + nowPlayer.Postion3Count;
        HPIncreaseText.text = "+" + nowPlayer.HPIncrease;
        AttackIncreaseText.text = "+" + nowPlayer.AttackIncrease;
        DefenceIncreaseText.text = "+" + nowPlayer.DefenceIncrease;
        HPPriceText.text = "" + nowPlayer.HPPrice;
        AttackPriceText.text = "" + nowPlayer.AttackPrice;
        DefencePriceText.text = "" + nowPlayer.DefencePrice;
    }

    public void PlayerSet() // 초기 설정
    {
        Name.text = nowPlayer.PlayerName;
        nowPlayer.HP = 100f;
        nowPlayer.CurrentHP = nowPlayer.HP;
        nowPlayer.AttackPower = 10f;
        nowPlayer.DefencePower = 1f;
        nowPlayer.EXP = 100f; // 0레벨 플레이어 최대 경험치
        nowPlayer.CurrentEXP = 0;
        nowPlayer.Credit = 0;
        nowPlayer.PosSetValue = 30;
        nowPlayer.Postion1Count = 5;
        nowPlayer.Postion2Count = 0;
        nowPlayer.Postion3Count = 0;
        SelectPos1();
        nowPlayer.HPIncrease = 10f;
        nowPlayer.AttackIncrease = 1f;
        nowPlayer.DefenceIncrease = 1f;
        nowPlayer.HPPrice = 10f;
        nowPlayer.AttackPrice = 10f;
        nowPlayer.DefencePrice = 10f;
        nowPlayer.Pos1Price = 5f;
        nowPlayer.Pos2Price = 10f;
        nowPlayer.Pos3Price = 15f;
    }

    public void CheckPlayerHP()
    {
        if(PlayerHPUI != null)
            PlayerHPUI.value = nowPlayer.CurrentHP / nowPlayer.HP * 100f;
    }

    public void CheckPlayerEXP()
    {
        if(PlayerEXPUI != null)
        {
            PlayerEXPUI.value = nowPlayer.CurrentEXP / nowPlayer.EXP * 100f;
        }
    }

    public void AttackEnemy()
    {
        Enemy.GetComponent<Skeleton>().SkeletonCurrentHP -= nowPlayer.AttackPower;
        AttackSound.Play();
    }

    public void LevelUp()
    {
        if(nowPlayer.CurrentEXP >= nowPlayer.EXP)
        {
            nowPlayer.CurrentEXP -= nowPlayer.EXP;
            nowPlayer.Level++;
            nowPlayer.AttackPower += 3f;
            nowPlayer.EXP += 100f;
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
        nowPlayer.Healing = 20f;
    }
    public void SelectPos2()
    {
        GameObject.Find("postion1").GetComponent<Image>().color = Color.green;
        GameObject.Find("postion2").GetComponent<Image>().color = Color.yellow;
        GameObject.Find("postion3").GetComponent<Image>().color = Color.green;
        nowPlayer.Healing = 50f;
    }
    public void SelectPos3()
    {
        GameObject.Find("postion1").GetComponent<Image>().color = Color.green;
        GameObject.Find("postion2").GetComponent<Image>().color = Color.green;
        GameObject.Find("postion3").GetComponent<Image>().color = Color.yellow;
        nowPlayer.Healing = 100f;
    }

    public void UsePostion()
    {
        
        if (nowPlayer.Healing == 20f)
        {
            if (nowPlayer.Postion1Count > 0)
            {
                nowPlayer.CurrentHP += nowPlayer.Healing;
                nowPlayer.Postion1Count--;
            }
        }
        else if (nowPlayer.Healing == 50f)
        {
            if (nowPlayer.Postion2Count > 0)
            {
                nowPlayer.CurrentHP += nowPlayer.Healing;
                nowPlayer.Postion2Count--;
            }
        }
        else if (nowPlayer.Healing == 100f)
        {
            if (nowPlayer.Postion3Count > 0)
            {
                nowPlayer.CurrentHP += nowPlayer.Healing;
                nowPlayer.Postion3Count--;
            }
        }
            
    }

    public void HPTraining()
    {
        if(nowPlayer.Credit >= nowPlayer.HPPrice)
        {
            nowPlayer.HP += nowPlayer.HPIncrease;
            nowPlayer.CurrentHP += nowPlayer.HPIncrease;
            nowPlayer.Credit -= nowPlayer.HPPrice;
            nowPlayer.HPIncrease += 10f;
            nowPlayer.HPPrice += 2f;
        }
    }
    public void AttackTraining()
    {
        if (nowPlayer.Credit >= nowPlayer.AttackPrice)
        {
            nowPlayer.AttackPower += nowPlayer.AttackIncrease;
            nowPlayer.Credit -= nowPlayer.AttackPrice;
            nowPlayer.AttackIncrease += 1f;
            nowPlayer.AttackPrice += 2f;
        }
    }
    public void DefenceTraining()
    {
        if (nowPlayer.Credit >= nowPlayer.DefencePrice)
        {
            nowPlayer.DefencePower += nowPlayer.DefenceIncrease;
            nowPlayer.Credit -= nowPlayer.DefencePrice;
            nowPlayer.DefenceIncrease += 1f;
            nowPlayer.DefencePrice += 2f;
        }
    }
    
    public void BuyPostion1()
    {
        if(nowPlayer.Credit >= nowPlayer.Pos1Price)
        {
            nowPlayer.Credit -= nowPlayer.Pos1Price;
            nowPlayer.Postion1Count++;
        }
    }
    public void BuyPostion2()
    {
        if (nowPlayer.Credit >= nowPlayer.Pos2Price)
        {
            nowPlayer.Credit -= nowPlayer.Pos2Price;
            nowPlayer.Postion2Count++;
        }
    }
    public void BuyPostion3()
    {
        if (nowPlayer.Credit >= nowPlayer.Pos3Price)
        {
            nowPlayer.Credit -= nowPlayer.Pos3Price;
            nowPlayer.Postion3Count++;
        }
    }
}
