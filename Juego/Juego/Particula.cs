using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juego
{
    internal class Particula
    {
        public Texture2D _ataque1;
        public Vector2 _ataquePosition1;
        public bool activo;

        public Particula(Vector2 b)
        {
            _ataquePosition1 = b;
        }
    }
}
