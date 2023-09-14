using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Juego
{
    public static class SpriteBatchExtensions
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Texture2D pixelTexture, Vector2 point1, Vector2 point2, Color color, int width = 1) {
            Vector2 direction = point1 - point2;
            float angle = (float)Math.Atan2(direction.Y,direction.X);
            float length = direction.Length();
            spriteBatch.Draw(
                pixelTexture,
                point1,
                null,
                color,
                angle,
                Vector2.Zero,
                new Vector2(length,width),
                SpriteEffects.None,
                0);
        }
    }

    public class Game1 : Game
    {
        /*
         Mas importantes:
        -Sistema de ki, que condicione la cantidad de ataque
        -Limites de la pantalla
        -Los ataques empiezen desde la mitad del jugador
        -Hacer un menu
        -Poder elegir personajes y controles
        -Transformaciones
         Menos imporatantes:
        -Remplazar el modo de disparo
        -Ataques especiales
        -Modificar skills de los jugadores
         */
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Texture2D _pruebasTexture, _pruebasTexture1 , _manzanaTexture, _ataque1,particula,particula1,ni1,ni2,ni3;
        private Vector2 _manzanaPosition, _pruebasPosition, _pruebasPosition1, _ataquePosition1, _pruebasSpeed , ba1v , ba2v ;
        private Song _backgroundMusic;
        private Barra ba1,ba2;
        private float posInicial = 0, posInicial1 = 0, at1=0,at=0;
        private double distInicial1, dif1, co1,ca1,md1 = 0,xd1, marca, cont;
        private float m1, b1, fijo1, fijo2, fijo3, fijo4;
        private List<DisparoNormal> p1 = new List<DisparoNormal>(), p2 = new List<DisparoNormal>();
        private EspecialDisparo t1, t2;

        private bool _manzanaTocada = false, salto = false, _manzanaTocada1 = false, salto1 = false, disparo1 = false, bban = false;
        private bool n1, n2; //bandera para innmovilizar mientras prepara ataque

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteFont = Content.Load<SpriteFont>("fuente");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pruebasTexture = Content.Load<Texture2D>("img/goku (2)");
            _pruebasPosition = new Vector2(100, 100);
            _pruebasTexture1 = Content.Load<Texture2D>("img/goku (3)");
            _pruebasPosition1 = new Vector2(600, 100);
            _ataque1 = Content.Load<Texture2D>("img/bola (1)");
            _ataquePosition1 = new Vector2(0, 0);
            particula = Content.Load<Texture2D>("img/particula");
            particula1 = Content.Load<Texture2D>("img/particula1");
            ni1 = Content.Load<Texture2D>("img/n1");
            ni2 = Content.Load<Texture2D>("img/n2");
            ni3 = Content.Load<Texture2D>("img/n3");

            _pruebasSpeed = new Vector2(3, 3);
            ba1v = new Vector2(10, 30);
            ba2v = new Vector2(480, 30);

            ba1 = new Barra(3000);
            ba2 = new Barra(3000);

            // Carga las textura "manzana" en la variable.
            _manzanaTexture = Content.Load<Texture2D>("img/mifondo");
            // (?)
            _manzanaPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - _manzanaTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - _manzanaTexture.Height / 3-35);

            //_backgroundMusic = Content.Load<Song>("background_music");
            //MediaPlayer.Play(_backgroundMusic);
            //MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            
            Rectangle pruebasRectangle = new Rectangle((int)_pruebasPosition.X, (int)_pruebasPosition.Y, _pruebasTexture.Width, _pruebasTexture.Height);
            Rectangle pruebasRectangle1 = new Rectangle((int)_pruebasPosition1.X, (int)_pruebasPosition1.Y, _pruebasTexture1.Width, _pruebasTexture1.Height);
            // Es la caja de coliciones de "pruebas" y se basa en el tamaño de la imgane.xx
            Rectangle manzanaRectangle = new Rectangle((int)_manzanaPosition.X, (int)_manzanaPosition.Y + (_manzanaTexture.Height / 3) * 2, _manzanaTexture.Width, _manzanaTexture.Height / 3);
            // Es la caja de coliciones de "manzana" y se basa en el tamaño de la imgane.

            // Se utiliza para poder recibir el estado del teclado.
            KeyboardState keyboardState = Keyboard.GetState();


            //primer jugador
            if (keyboardState.IsKeyDown(Keys.Left) && !n1)
                // Se resta la posición actual en "X" por el valor de la velocidad. Si velocidad es 2 se restan 2 posiciones.
                _pruebasPosition1.X -= 5;

            // Cuando se aprieta la tecla "right" se hace la siguiente operación:
            if (keyboardState.IsKeyDown(Keys.Right) && !n1)
                // Se suma la posición actual en "X" por el valor de la velocidad.
                _pruebasPosition1.X += 5;

            // Cuando se aprieta la tecla "up" se hace la siguiente operación:
            if (keyboardState.IsKeyDown(Keys.Up) && !n1)
            {
                _manzanaTocada1 = false;
                salto1 = true;
                _pruebasPosition1.Y -= 5;
                posInicial1 = _pruebasPosition1.Y;
                // Se resta la posición actual en "Y" por el valor de la velocidad.
            }
            if (keyboardState.IsKeyDown(Keys.Down) && !_manzanaTocada1 && !n1)
            // Se suma la posición actual en "Y" por el valor de la velocidad.
            _pruebasPosition1.Y += 5;

            if (keyboardState.IsKeyDown(Keys.A) && !n2)
                // Se resta la posición actual en "X" por el valor de la velocidad. Si velocidad es 2 se restan 2 posiciones.
                _pruebasPosition.X -= 5;

            // Cuando se aprieta la tecla "right" se hace la siguiente operación:
            if (keyboardState.IsKeyDown(Keys.D) && !n2)
                // Se suma la posición actual en "X" por el valor de la velocidad.
                _pruebasPosition.X += 5;

            // Cuando se aprieta la tecla "up" se hace la siguiente operación:
            if (keyboardState.IsKeyDown(Keys.W) && !salto && _manzanaTocada && !n2)
            {
                salto = true;
                posInicial = _pruebasPosition.Y;
                // Se resta la posición actual en "Y" por el valor de la velocidad.
            }
            if (salto)
            {
                _pruebasPosition.Y -= 5;
                if (posInicial - 100 == _pruebasPosition.Y)
                {
                    _manzanaTocada = false;
                    salto = false;
                }
            }


            if (keyboardState.IsKeyDown(Keys.J) && t2 == null)
            {
                at1++;
                n1 = true;
            }
            if (keyboardState.IsKeyUp(Keys.J)){
                if(at1 > 20)
                {
                    EspecialDisparo(true);
                }
                else if (at1 > 0)
                {
                    NuevoDisparo(true);
                }
                at1 = 0;
                n1 = false;
            }
            if (keyboardState.IsKeyDown(Keys.T) && t1 == null)
            {
                at++;
                n2 = true;
            }
            if (keyboardState.IsKeyUp(Keys.T))
            {
                if (at > 50)
                {
                    EspecialDisparo(false);
                }
                else if (at > 0)
                {
                    NuevoDisparo(false);
                }
                at = 0;
                n2 = false;
            }
            AvanzarDisparos(pruebasRectangle,pruebasRectangle1);

            //nuevo

            /*if (salto1)
            {
                _pruebasPosition1.Y -= 5;
                if (posInicial1 - 100 == _pruebasPosition1.Y)
                {
                    _manzanaTocada1 = false;
                    salto1 = false;
                }
            }*/


            // Se ejecuta cuando la variable _manzanaTocada es "false".
            //if (!_manzanaTocada)
            //{
            // Movimiento del sprite "pruebas" controlado por las flechas del teclado

            // Cuando se aprieta la tecla "left" se hace la siguiente operación:
            

            // Cuando se aprieta la tecla "down" se hace la siguiente operación:
            //if (keyboardState.IsKeyDown(Keys.Down) && !_manzanaTocada)
                    // Se suma la posición actual en "Y" por el valor de la velocidad.
                    //_pruebasPosition.Y += 2;

            if (!_manzanaTocada && !salto) _pruebasPosition.Y += 5;
            //if (!_manzanaTocada1 && !salto1) _pruebasPosition1.Y += 5;
                // Verificar colisión con la manzana
                

                if (pruebasRectangle.Intersects(manzanaRectangle))
                {
                    _manzanaTocada = true;
                }
            if (pruebasRectangle1.Intersects(manzanaRectangle))
            {
                _manzanaTocada1 = true;
            }
            //}
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Limpia 20 veces por segundo la imagen.

            _spriteBatch.Begin();
            _spriteBatch.Draw(_manzanaTexture, _manzanaPosition, Color.White);

            _spriteBatch.Draw(_pruebasTexture, _pruebasPosition, Color.White);
            _spriteBatch.Draw(_pruebasTexture1, _pruebasPosition1, Color.White);
            foreach (DisparoNormal d in p1)
            {
                _spriteBatch.Draw(d._ataque1, d._ataquePosition1, Color.White);
            }
            foreach (DisparoNormal d in p2)
            {
                _spriteBatch.Draw(d._ataque1, d._ataquePosition1, Color.White);
            }

            if (t1 != null) {
                for (int i = 0; i < t1.non; i++)
                {
                    if (i > t1.non-200)
                        _spriteBatch.Draw(particula1, t1.particulas[i]._ataquePosition1, Color.White);
                    else
                        _spriteBatch.Draw(particula, t1.particulas[i]._ataquePosition1, Color.White);
                }
                t1.non += 50;
                if (t1.non == t1.particulas.Count)
                {
                    t1 = null;
                }
                n2 = true;
                //_spriteBatch.DrawLine(particula,t1._ataquePosition1,t1._objetivoPosition1,Color.Red);
            }

            if (t2 != null)
            {
                for (int i = 0; i < t2.non; i++)
                {
                    if (i > t2.non - 200)
                        _spriteBatch.Draw(particula1, t2.particulas[i]._ataquePosition1, Color.White);
                    else
                        _spriteBatch.Draw(particula, t2.particulas[i]._ataquePosition1, Color.White);
                }
                t2.non += 50;
                if (t2.non == t2.particulas.Count)
                {
                    t2 = null;
                }
                n1 = true;
                //_spriteBatch.DrawLine(particula, t2._ataquePosition1, t2._objetivoPosition1, Color.Red);
            }

            if(ba1 != null)
            {
                bool a = false, v = false;
                for(var i = 0; i < ba1.vida; i++)
                {
                    ba1v.X += 0.3f;
                    if (i < 1000) _spriteBatch.Draw(ni1, ba1v, Color.White);
                    else if(i < 2000)
                    {
                        if (!a)
                        {
                            ba1v.X = 10.3f;
                            a = true;
                        }
                        _spriteBatch.Draw(ni2, ba1v, Color.White);
                    }
                    else if (i < 3000)
                    {
                        if (!v)
                        {
                            ba1v.X = 10.3f;
                            v = true;
                        }
                        _spriteBatch.Draw(ni3, ba1v, Color.White);
                    }
                }
                ba1v.X = 10;
            }
            if (ba2 != null)
            {
                bool a = false, v = false;
                for (var i = 0; i < ba2.vida; i++)
                {
                    ba2v.X += 0.3f;
                    if (i < 1000) _spriteBatch.Draw(ni1, ba2v, Color.White);
                    else if (i < 2000)
                    {
                        if (!a)
                        {
                            ba2v.X = 480.3f;
                            a = true;
                        }
                        _spriteBatch.Draw(ni2, ba2v, Color.White);
                    }
                    else if (i < 3000)
                    {
                        if (!v)
                        {
                            ba2v.X = 480.3f;
                            v = true;
                        }
                        _spriteBatch.Draw(ni3, ba2v, Color.White);
                    }
                }
                ba2v.X = 480;
            }
            //_spriteBatch.Draw(_pruebasTexture, new Rectangle(800, 800, 64, 64), Color.White);
            //if (!_manzanaTocada)


            //_spriteBatch.DrawString(_spriteFont,"SIUUUUUUUUUUUUUUUUU",new Vector2(0,0),Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void NuevoDisparo(bool xd)
        {
            if (xd)
            {
                p2.Add(new DisparoNormal(_pruebasPosition.X,_pruebasPosition1.X,_pruebasPosition.Y,_pruebasPosition1.Y));
                p2[p2.Count-1]._ataque1 = Content.Load<Texture2D>("img/bola (1)");
            }
            else
            {
                p1.Add(new DisparoNormal(_pruebasPosition1.X, _pruebasPosition.X, _pruebasPosition1.Y, _pruebasPosition.Y));
                p1[p1.Count - 1]._ataque1 = Content.Load<Texture2D>("img/bola (1)");
            }
        }

        void EspecialDisparo(bool xd)
        {
            if (!xd)
            {
                t1 = new EspecialDisparo(_pruebasPosition1.X, _pruebasPosition.X, _pruebasPosition1.Y, _pruebasPosition.Y,particula);
                
            }
            else
            {
                t2 = new EspecialDisparo(_pruebasPosition.X, _pruebasPosition1.X, _pruebasPosition.Y, _pruebasPosition1.Y, particula);
            }
        }

        void AvanzarDisparos(Rectangle ataque, Rectangle objetivo)
        {
            for (int i = p1.Count - 1; i >= 0; i--)
            {
                int atx = p1[i].AvanzarDisparo(new Rectangle((int)p1[i]._ataquePosition1.X, (int)p1[i]._ataquePosition1.Y, (int)p1[i]._ataque1.Width, (int)p1[i]._ataque1.Height), objetivo);
                if(atx == 2 || atx == 3)
                {
                    if(atx == 2)ba2.vida-=10;
                    p1.RemoveAt(i);
                }
            }
            for (int i = p2.Count-1; i >= 0; i--)
            {
                int atx = p2[i].AvanzarDisparo(new Rectangle((int)p2[i]._ataquePosition1.X, (int)p2[i]._ataquePosition1.Y, (int)p2[i]._ataque1.Width, (int)p2[i]._ataque1.Height), ataque);
                if(atx == 2 || atx == 3)
                {
                    if(atx == 2)ba1.vida-=10;
                    p2.RemoveAt(i);
                }
            }
        }
    }
}