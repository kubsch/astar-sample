using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace RandomGame;

public static class Assets
{
    public static Textures Textures { get; private set; } = null!;
    public static Songs Songs { get; private set; } = null!;
    public static Fonts Fonts { get; private set; } = null!;
    public static Sounds Sounds { get; private set; } = null!;

    public static void Load(ContentManager content)
    {
        Textures = new(content);
        Songs = new(content);
        Fonts = new();
        Sounds = new(content);
    }
}

public class Sounds
{
    public Sounds(ContentManager content)
    {
        SoundEffect load(string assetName)
        {
            return content.Load<SoundEffect>(assetName);
        }
    }
}

public class Fonts
{
    private FontSystem _fontSystem = new();

    public Fonts()
    {
        _fontSystem.AddFont(File.ReadAllBytes(@"Content/neuro.otf"));

        Neuro10 = _fontSystem.GetFont(10);
        Neuro12 = _fontSystem.GetFont(12);
        Neuro18 = _fontSystem.GetFont(18);
        Neuro20 = _fontSystem.GetFont(20);
        Neuro22 = _fontSystem.GetFont(22);
        Neuro32 = _fontSystem.GetFont(32);
        Neuro64 = _fontSystem.GetFont(64);
        Neuro128 = _fontSystem.GetFont(128);
    }

    public DynamicSpriteFont Neuro10 { get; }
    public DynamicSpriteFont Neuro12 { get; }
    public DynamicSpriteFont Neuro18 { get; }
    public DynamicSpriteFont Neuro20 { get; }
    public DynamicSpriteFont Neuro22 { get; }
    public DynamicSpriteFont Neuro32 { get; }
    public DynamicSpriteFont Neuro64 { get; }
    public DynamicSpriteFont Neuro128 { get; }

    public DynamicSpriteFont GetFont(int size)
    {
        return _fontSystem.GetFont(size);
    }
}

public class Songs
{
    public Songs(ContentManager content)
    {
        Song load(string assetName)
        {
            return content.Load<Song>(assetName);
        }

        Scary = load("scary");
        HeroOfThe80s = load("a-hero-of-the-80s");
        LadyOfThe80s = load("lady-of-the-80s");
    }

    public Song Scary { get; set; }
    public Song HeroOfThe80s { get; set; }
    public Song LadyOfThe80s { get; set; }
}

public class Textures
{
    public Textures(ContentManager content)
    {
        Texture2D load(string assetName)
        {
            return content.Load<Texture2D>(assetName);
        }

        Grass = load("grass");
        Tree = load("tree");
        White = load("white");
        CyborgIdle = load("cyborg_idle");
        CyborgRun = load("cyborg_run");
    }

    public Texture2D Grass { get; }
    public Texture2D Tree { get; }
    public Texture2D White { get; }
    public Texture2D CyborgIdle { get; }
    public Texture2D CyborgRun { get; }
}