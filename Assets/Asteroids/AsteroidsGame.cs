#region usings
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
#endregion usings

public static class AsteroidsGame
{
    public static Vector2 HalfScreenSize = Vector2.zero;
    private static int score = 0;
    public static int Score { get => score; set
        {
            score = value;
            UpdateScore();
        }
    }

    public static LogicalObject ship;
    //UI
    public static LogicalObject UI_Score;
    public static LogicalObject UI_GameOver;
    public static LogicalObject UI_LaserCounter;
    public static LogicalObject UI_CheckBoxPolyMode;
    public static void Start()
    {
        if (HalfScreenSize == Vector2.zero)
        {
            var t = Chache.cam.ViewportToWorldPoint(Vector3.one);
            HalfScreenSize = new Vector2(t.x, t.y);
        }
        LoadAssets();

        ship = Prefabs.Ship();
        ship.GetPart<SpriteDrawer>().PartActive = !Settings.PolyMode;
        ship.GetPart<PolyRenderer>().PartActive = Settings.PolyMode;
        UI();
        AsteroidsGenerator.Start();
    }
    public static void Update()
    {
        AsteroidsGenerator.Update();
    }
    //Assets
    public static void LoadAssets()
    {
        Sprite[] tilemap = Resources.LoadAll<Sprite>("Sprites/Tilemap");
        Chache.Resources.loaded.Add("Asteroid0", tilemap[0]);
        Chache.Resources.loaded.Add("Asteroid1", tilemap[1]);
        Chache.Resources.loaded.Add("Asteroid2", tilemap[2]);
        Chache.Resources.loaded.Add("UFO", tilemap[3]);
        Chache.Resources.loaded.Add("ship", tilemap[4]);
        Chache.Resources.loaded.Add("bullet", tilemap[5]);
    }

    //Methods
    public static void UI()
    {
        GUIStyle style = new GUIStyle();
        style.font = PrefabMaster.Load<Font>("Fonts/Hyperspace Bold");
        style.fontSize = 72;
        style.normal.textColor = Color.white;
        Chache.style = style;

        UI_Score = Prefabs.Score();
        UI_GameOver = Prefabs.GameOver();
        UI_GameOver.isActive = false;
        UI_LaserCounter = Prefabs.LaserCounter();
        UpdateLaserCounter();
        UI_CheckBoxPolyMode = Prefabs.CheckBoxPolyMode();
    }
    public static void ApplyGameOver()//Gameover clicked
    {
        Restart();
    }
    public static void OnGameOver()
    {
        UI_GameOver.isActive = true;
        FastPhysics.TimeScale = 0;
    }
    public static void Restart()
    {
        Debug.Log("Restart game");
        UI_GameOver.isActive = false;
        //stop game
        FastPhysics.TimeScale = 1;
        //reset score
        Score = 0;
        //reset objects on scene
        foreach (var item in Engine.FindObjectsOfType<FastPhysics>())
            if (item.tag != "Ship")
                LogicalObject.Destroy(item.logicobj);
        AsteroidsGenerator.Objects.Clear();
        //reset ship
        var sl = ship.GetPart<ShipLogic>();
        sl.LasersCharges.x = sl.LasersCharges.y;
        UpdateLaserCounter();
        ship.GetPart<FastPhysics>().Velocity = Vector2.zero;
        ship.position = Vector2.zero;
        ship.angle = 0;
        foreach (var item in sl.bullets)
            LogicalObject.Destroy(item);
    }
    public static void UpdateScore()
    {
        UI_Score.GetPart<UIText>().Text = "Score: " + Score;
    }
    public static void UpdateLaserCounter()
    {
        UI_LaserCounter.GetPart<UIText>().Text = "Lasers: " + ship.GetPart<ShipLogic>().LasersCharges.x;
    }
    public static void ChangePolyMode()
    {
        Debug.Log("ChangePolyMode");
        Settings.PolyMode = !Settings.PolyMode;
        UI_CheckBoxPolyMode.GetPart<UIText>().Text = $"PolyMode [{(Settings.PolyMode ? "X" : " ")}]";
        foreach (var item in Engine.FindObjectsOfType<PolyRenderer>())
        {
            item.logicobj.GetPart<SpriteDrawer>().PartActive = !Settings.PolyMode;
            item.PartActive = Settings.PolyMode;
        }
        Chache.cam.clearFlags = Settings.PolyMode ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
    }
}