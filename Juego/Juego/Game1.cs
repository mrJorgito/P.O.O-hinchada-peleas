using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Diagnostics;

namespace Juego
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _pruebasTexture, _pruebasTexture1 , _manzanaTexture, _ataque1,particula,particula1;
        private Vector2 _manzanaPosition, _pruebasPosition, _pruebasPosition1, _ataquePosition1, _pruebasSpeed = new Vector2(3, 3);
        private Song _backgroundMusic;
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pruebasTexture = Content.Load<Texture2D>("img/goku (2)");
            _pruebasPosition = new Vector2(100, 100);
            _pruebasTexture1 = Content.Load<Texture2D>("img/goku (3)");
            _pruebasPosition1 = new Vector2(600, 100);
            _ataque1 = Content.Load<Texture2D>("img/bola (1)");
            _ataquePosition1 = new Vector2(0, 0);
            particula = Content.Load<Texture2D>("img/particula");
            particula1 = Content.Load<Texture2D>("img/particula1");


            // Carga las textura "manzana" en la variable.
            _manzanaTexture = Content.Load<Texture2D>("img/fondo (3)");
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

            
            if (keyboardState.IsKeyDown(Keys.J))
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
            if (keyboardState.IsKeyDown(Keys.T))
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
                if(posInicial-100 == _pruebasPosition.Y)
                {
                    _manzanaTocada = false;
                    salto = false;
                }
            }

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
                    if (i < 200)
                        _spriteBatch.Draw(particula1, t1.particulas[i]._ataquePosition1, Color.White);
                    else
                        _spriteBatch.Draw(particula, t1.particulas[i]._ataquePosition1, Color.White);
                }
                t1.non += 50;
                if (t1.non == 4800)
                {
                    t1 = null;
                }
                n2 = true;
            }

            if (t2 != null)
            {
                Debug.Print(t2.particulas.Count.ToString());
                for (int i = 0; i < t2.non; i++)
                {
                    if (i < 200)
                        _spriteBatch.Draw(particula1, t2.particulas[i]._ataquePosition1, Color.White);
                    else
                        _spriteBatch.Draw(particula, t2.particulas[i]._ataquePosition1, Color.White);
                }
                t2.non += 50;
                if (t2.non == 4800)
                {
                    t2 = null;
                }
                n1 = true;
            }
            //_spriteBatch.Draw(_pruebasTexture, new Rectangle(800, 800, 64, 64), Color.White);
            //if (!_manzanaTocada)
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
                if (p1[i].AvanzarDisparo(new Rectangle((int)p1[i]._ataquePosition1.X, (int)p1[i]._ataquePosition1.Y, (int)p1[i]._ataque1.Width, (int)p1[i]._ataque1.Height) ,objetivo)) p1.RemoveAt(i);
            }
            for (int i = p2.Count-1; i >= 0; i--)
            {
                if (p2[i].AvanzarDisparo(new Rectangle((int)p2[i]._ataquePosition1.X, (int)p2[i]._ataquePosition1.Y, (int)p2[i]._ataque1.Width, (int)p2[i]._ataque1.Height), ataque)) p2.RemoveAt(i);
            }
        }
    }
}