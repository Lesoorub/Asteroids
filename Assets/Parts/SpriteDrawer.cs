using UnityEngine;

public class SpriteDrawer : Part
{
    Sprite source;
    Mesh mesh;
    Material mat;
    public SpriteDrawer(Sprite source)
    {
        this.source = source;
    }
    public override void Start()
    {
        mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f), new Vector3(-0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f), new Vector3( 0.5f, -0.5f)
        };
        mesh.uv = new Vector2[] {
            new Vector2(0, 0), new Vector2(0, 1),
            new Vector2(1, 1), new Vector2(1, 0)
        };
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        //mesh.RecalculateNormals();
        //mesh.RecalculateBounds();
        mat = new Material(Chache.spritematerial);
        mat.mainTexture = source.texture;
        mat.SetVector("_SpriteRect", new Vector4(
            source.textureRect.x, source.textureRect.y,
            source.textureRect.width, source.textureRect.height));
    }
    public override void Update()
    {
        mat.SetPass(0);
        Graphics.DrawMeshNow(mesh, Matrix4x4.TRS(position, rotation, scale));
    }
}