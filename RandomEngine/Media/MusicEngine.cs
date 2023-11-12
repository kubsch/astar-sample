using Microsoft.Xna.Framework.Audio;
using System.Media;

namespace RandomEngine.Media;

public class MusicEngine
{
    public float Volume => MediaPlayer.Volume;

    public bool IsMuted
    {
        get
        {
            return MediaPlayer.IsMuted;
        }
        set
        {
            MediaPlayer.IsMuted = value;
        }
    }

    public bool IsRepeasting => MediaPlayer.IsRepeating;
    public MediaState State => MediaPlayer.State;

    public void Mute() => MediaPlayer.IsMuted = true;

    public void Unmute() => MediaPlayer.IsMuted = false;

    public void Loop(Song song)
    {
        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = true;
    }

    public void Play(Song song)
    {
        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = false;
    }

    public void Pause() => MediaPlayer.Pause();

    public void Resume() => MediaPlayer.Resume();

    public void Stop() => MediaPlayer.Stop();
}