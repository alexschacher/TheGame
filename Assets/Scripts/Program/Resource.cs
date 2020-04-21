using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Resource loads Prefabs, Models, and Materials at the start of the game and holds references to them.

public class Resource
{
    private static bool showDebug = true;
    private static bool hasLoaded = false;

    public static Dictionary<string, GameObject> pfb;
    public static Dictionary<string, Material> mat;
    public static Dictionary<string, Mesh> mesh;
    public static Dictionary<string, Dictionary<EntityAnimator.Anim, Sprite[]>> charAnim;

    public static void LoadResources()
    {
        if (hasLoaded) { return; }
        hasLoaded = true;

        LoadPrefabs();
        LoadMaterials();
        LoadModels();
        LoadCharAnims();
    }

    private static void LoadPrefabs()
    {
        GameObject[] prefabList = Resources.LoadAll<GameObject>("Prefabs");
        pfb = new Dictionary<string, GameObject>(prefabList.Length);

        foreach (GameObject prefab in prefabList)
        {
            pfb.Add(prefab.name, prefab);
            Log("Prefab loaded: " + prefab.name);
        }
    }

    private static void LoadMaterials()
    {
        Material[] materialList = Resources.LoadAll<Material>("Materials");
        mat = new Dictionary<string, Material>(materialList.Length);

        foreach (Material material in materialList)
        {
            mat.Add(material.name, material);
            Log("Material loaded: " + material.name);
        }
    }

    private static void LoadModels()
    {
        Mesh[] meshList = Resources.LoadAll<Mesh>("Models");
        mesh = new Dictionary<string, Mesh>(meshList.Length);

        foreach (Mesh m in meshList)
        {
            mesh.Add(m.name, m);
            Log("Mesh loaded: " + m.name);
        }
    }

    private static void LoadCharAnims()
    {
        Texture2D[] texList = Resources.LoadAll<Texture2D>("CharAnims");
        charAnim = new Dictionary<string, Dictionary<EntityAnimator.Anim, Sprite[]>>(texList.Length);

        foreach (Texture2D tex in texList)
        {
            Dictionary<EntityAnimator.Anim, Sprite[]> charAnimation = new Dictionary<EntityAnimator.Anim, Sprite[]>();

            // Create sprites
            Sprite walk1 = Sprite.Create(tex, new Rect(00, 48, 16, 16), new Vector2(0.5f, 0), 12);
            Sprite walk2 = Sprite.Create(tex, new Rect(16, 48, 16, 16), new Vector2(0.5f, 0), 12);
            Sprite walk3 = Sprite.Create(tex, new Rect(32, 48, 16, 16), new Vector2(0.5f, 0), 12);
            Sprite walk4 = Sprite.Create(tex, new Rect(48, 48, 16, 16), new Vector2(0.5f, 0), 12);
            Sprite stand = Sprite.Create(tex, new Rect(00, 32, 16, 16), new Vector2(0.5f, 0), 12);

            // Organize sprites into animations
            charAnimation.Add(EntityAnimator.Anim.Walk, new Sprite[4] { walk1, walk2, walk3, walk4 });
            charAnimation.Add(EntityAnimator.Anim.Stand, new Sprite[1] { stand });
            charAnimation.Add(EntityAnimator.Anim.Hurt, new Sprite[2] { walk3, walk1 });
            charAnimation.Add(EntityAnimator.Anim.Attack, new Sprite[1] { walk3 });

            // Add charAnim to the dictionary of charAnims
            charAnim.Add(tex.name, charAnimation);
            Log("CharAnim loaded: " + tex.name);
        }
    }

    private static void Log(string msg)
    {
        if (showDebug)
        {
            Debug.Log(msg);
        }
    }
}