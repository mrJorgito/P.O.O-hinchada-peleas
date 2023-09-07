using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juego
{
    internal class EspecialDisparo
    {
        public Texture2D _ataque1;
        public Vector2 _ataquePosition1 = new Vector2(0, 0);
        public bool disparo1, bban, bban1;
        public double m1, b1, fijo3;

        public EspecialDisparo(float px1, float px2, float py1, float py2)
        {
            _ataquePosition1.Y = py2;
            _ataquePosition1.X = px2;
            m1 = (py1 - py2) / (px1 - px2);
            b1 = py2 - (m1 * px2);
            disparo1 = true;
            bban = false;
            bban1 = false;
            fijo3 = px1;
        }

        public bool AvanzarDisparo()
        {
            if ((_ataquePosition1.X >= fijo3 || bban) && !bban1)
            {
                if (m1 > 0) _ataquePosition1.Y -= 5;
                else if (m1 < 0) _ataquePosition1.Y += 5;
                _ataquePosition1.X = float.Parse(((_ataquePosition1.Y - b1) / m1).ToString());
                if (_ataquePosition1.X < 0)
                {
                    return true;
                }
                bban = true;
            }
            else if (_ataquePosition1.X < fijo3 || bban1)
            {
                if (m1 > 0) _ataquePosition1.Y += 5;
                else if (m1 < 0) _ataquePosition1.Y -= 5;
                _ataquePosition1.X = float.Parse(((_ataquePosition1.Y - b1) / m1).ToString());
                if (_ataquePosition1.X < 0)
                {
                    return true;
                }
                bban1 = true;
            }
            return false;
        }
    }
}
