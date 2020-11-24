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

public class Launcher : MonoBehaviour
{
    void Start() =>
        Engine.Start(this, new AsteroidsGame());

    void Update() =>
        Engine.Update();
    void OnGUI() =>
        Engine.OnGUI();

    void OnEnable() =>
        Camera.onPostRender += Engine.MyPostRenderer;
    void OnDisable() =>
        Camera.onPostRender -= Engine.MyPostRenderer;
}