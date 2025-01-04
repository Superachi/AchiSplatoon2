using System.Collections.Generic;

namespace AchiSplatoon2.Content.AsepriteAnimationData.Models
{
    public class AnimationFrames
    {
        public List<AnimationFrame> Frames { get; set; } = new();
    }

    public class AnimationFrame
    {
        public string Filename { get; set; } = "";
        public int Duration { get; set; }
    }
}
