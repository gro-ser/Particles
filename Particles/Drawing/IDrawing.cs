using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particles.Drawing
{
    public interface IDrawing:IDisposable
    {
        ParticleSettings Settings { get; set; }
        void Draw(Particle particle);
        void Init();
    }
}
