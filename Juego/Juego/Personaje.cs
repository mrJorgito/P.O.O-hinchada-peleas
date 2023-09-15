using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juego
{
    internal class Personaje
    {
        public Texture2D _textura, _ataque, _ataqueEspecial;
        public Vector2 pos;
        public int stamina = 3000,vida = 3000;

        public Personaje(Texture2D _textura, Texture2D _ataque, Texture2D _ataqueEspecial, Vector2 pos) {
            this._textura = _textura;
            this._ataque = _ataque;
            this._ataqueEspecial = _ataqueEspecial;
            this.pos = pos;
        }
        
        public virtual void Arriba()
        {
            pos.Y -= 5;
        }
        public virtual void Abajo()
        {
            pos.Y += 5;
        }
        public virtual void Izquierda()
        {
            pos.X -= 5;
        }
        public virtual void Derecha()
        {
            pos.X += 5;
        }
        public virtual void AtaqueNormal()
        {

        }
        public virtual void AtaqueEspecial()
        {

        }
    }
}
