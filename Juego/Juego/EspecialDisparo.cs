using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Juego
{
    internal class EspecialDisparo
    {
        public Texture2D _ataque1;
        public Vector2 _ataquePosition1 = new Vector2(0, 0),_objetivoPosition1 = new Vector2(0,0);
        public bool disparo1, bban, bban1;
        public double m1, b1, fijo3, cont = 0, non = 200;
        public List<Particula> particulas = new List<Particula>();

        public EspecialDisparo(float px1, float px2, float py1, float py2, Texture2D texture)
        {
            _ataquePosition1.Y = py2;
            _ataquePosition1.X = px2;
            m1 = (py1 - py2) / (px1 - px2);
            b1 = py2 - (m1 * px2);
            disparo1 = true;
            bban = false;
            bban1 = false;
            fijo3 = px1;
            _objetivoPosition1.Y = py2;
            _objetivoPosition1.X = px2;
            _ataquePosition1.X = float.Parse(((_ataquePosition1.Y - b1) / m1).ToString());
            float restador1 = _ataquePosition1.Y; //????
            float restador2 = _ataquePosition1.X; //????
            /*double temp;
            if (m1 >= 0 && m1 < 0.5)
            {
                temp = 6400*(0.5-m1);
            }
            else if (m1 < 0 && m1 > -0.5)
            {
                temp = 6400 * (-0.5-m1);
            }
            else
            {
                temp = 1600;
            }
            for (float i = 0; i < temp; i++)*/
            for (float i = 0; i < 6400; i++)
            {
                particulas.Add(new Particula(new Vector2(restador2,restador1)));
                particulas.Add(new Particula(new Vector2(restador2-1,restador1-1)));
                particulas.Add(new Particula(new Vector2(restador2+1,restador1+1)));
                particulas.Add(new Particula(new Vector2(restador2 - 2, restador1 - 2)));
                particulas.Add(new Particula(new Vector2(restador2 + 2, restador1 + 2)));
                if ((_objetivoPosition1.X >= fijo3 || bban) && !bban1)
                {
                    if (m1 >= 0) restador1 -= 0.03f;
                    else if (m1 < 0) restador1 += 0.03f;
                    restador2 = float.Parse(((restador1 - b1) / m1).ToString());
                }
                else if (_objetivoPosition1.X < fijo3 || bban1)
                {
                    if (m1 >= 0) restador1 += 0.03f;
                    else if (m1 < 0) restador1 -= 0.03f;
                    restador2 = float.Parse(((restador1 - b1) / m1).ToString());
                }
            }
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
