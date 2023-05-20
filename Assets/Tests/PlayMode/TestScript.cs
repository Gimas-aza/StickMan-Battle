using NUnit.Framework;
using UnityEngine;
using TMPro;

public class TestScript
{
    //[Test]
    //public void WhenStartGameOrNextLevel_AndStartMethodRandomG_ThenCountWallToMinToMax()
    //{
    //    LevelController generationWall = new GameObject().AddComponent<LevelController>();

    //    //generationWall.RandomG();

    //    var countWallInLevel = GameObject.FindGameObjectsWithTag("Wall_Leval");
    //    //Debug.Log(countWallInLevel.Length);
    //    if (countWallInLevel.Length != 0 )
    //    {
    //        Assert.IsTrue(countWallInLevel.Length >= generationWall.Min && countWallInLevel.Length <= generationWall.Max);
    //    }
    //}

    //[Test]
    //public void WhenStartMove_AndClickKeyWASD_ThenMoveVectorEqual1()
    //{
    //    PlayerController playerController = new GameObject().AddComponent<PlayerController>();
    //    playerController.tag = "Pl1";

    //    Vector3 movement = playerController.GetMove();
    //    Assert.AreEqual(Vector3.zero, movement);
    //}

    //[Test]
    //public void WhenStartShootAim_AndClickKeyLF_ThenWeaponShot()
    //{
    //    WeaponController weaponController = new GameObject().AddComponent<WeaponController>();
    //    weaponController.tag = "Pl1";

    //    bool isBullet = weaponController.Bullet != null;
    //    Assert.IsTrue(!isBullet);
    //}

    //[Test]
    //public void WhenDamegeToPlayerOrUseJerk_AndShootAimToPlayer_ThenSetDamegeOrSetJerk()
    //{
    //    ParametersController parametersController = new GameObject().AddComponent<ParametersController>();
    //    parametersController.tag = "Pl1";

    //    Assert.IsTrue(parametersController.Player2.Health != 99);
    //}

    //[Test]
    //public void WhenKillPlayer_AndWriteStatistical_Then1PlayerEqual1KillAnd10Money()
    //{
    //    ParametersController parametersController = new GameObject().AddComponent<ParametersController>();
    //    parametersController.tag = "Pl1";
    //    parametersController.Player1.TextCountKill = new TextMeshProUGUI();
    //    parametersController.Player1.TextCountMoney = new TextMeshProUGUI();

    //    var countKill = System.Convert.ToInt32(parametersController.Player1.TextCountKill.text);
    //    var countMoney = System.Convert.ToInt32(parametersController.Player1.TextCountMoney.text);

    //    Assert.IsTrue(countKill == 0);
    //    Assert.IsTrue(countMoney == 0);
    //}

    //[Test]
    //public void WhenHaveMoney_AndOpenShop_ThenBuyProductItem()
    //{
    //    ParametersController parametersController = new GameObject().AddComponent<ParametersController>();

    //    parametersController.tag = "ParametersPlayer";
    //    parametersController.Player1.TextCountMoney = new TextMeshProUGUI();
    //}
}
