using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Juego
{
    public class DisparoNormal
    {
        public Texture2D _ataque1;
        public Vector2 _ataquePosition1 = new Vector2(0,0);
        public bool disparo1, bban, bban1;
        public double m1,b1, fijo3;

        public DisparoNormal(float px1, float px2, float py1, float py2) {
            _ataquePosition1.Y = py2;
            _ataquePosition1.X = px2;
            m1 = (py1 - py2) / (px1 - px2);
            b1 = py2 - (m1 * px2);
            disparo1 = true;
            bban = false;
            bban1 = false;
            fijo3 = px1;
        }

        public bool AvanzarDisparo(Rectangle ataque, Rectangle objetivo)
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
            else if(_ataquePosition1.X < fijo3 || bban1)
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
            if (ataque.Intersects(objetivo))
            {
                return true;
            }
            return false;
        }
    }

    /*
     if (keyboardState.IsKeyDown(Keys.H) && !disparo1)
            {
                disparo1 = true;
                _ataquePosition1.X = _pruebasPosition1.X;
                _ataquePosition1.Y = _pruebasPosition1.Y;
                ca1 = _ataquePosition1.Y - _pruebasPosition.Y; 
                co1 = _ataquePosition1.X - _pruebasPosition.X;
                distInicial1 = Math.Sqrt(Math.Pow(co1,2) + Math.Pow(ca1, 2)); 
                dif1 = distInicial1 / 120; 
                at1 = 30;
                xd1 = Math.Round((_ataquePosition1.Y + _ataquePosition1.X)/120);
                if (xd1 + xd1 < 0) xd1 = -xd1;
                Debug.Print(xd1.ToString());
                md1 = 0;
            }
            if (disparo1 && false)
            {
                
                md1++;
                if (xd1 == md1)
                {
                    if (at1 > 0)
                    {
                        _ataquePosition1.Y = float.Parse((_pruebasPosition.Y + (ca1 * distInicial1) / (distInicial1 - dif1)).ToString()); 
                        _ataquePosition1.X = float.Parse((_pruebasPosition.X + (co1 * distInicial1) / (distInicial1 - dif1)).ToString());
                        dif1 += dif1;
                        at1--;
                    }
                    else
                    {
                        disparo1 = false;
                    }
                    md1 = 0;
                }
            }
     */
}
