using System;

namespace Particles.Drawing
{
    public interface IDrawing : IDisposable
    {
        ParticleSettings Settings { get; set; }
        void Draw(Particle particle);
        void Init();
    }
}
