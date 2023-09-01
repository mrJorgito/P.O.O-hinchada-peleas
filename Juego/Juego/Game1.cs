using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Juego
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _pruebasTexture, _pruebasTexture1 , _manzanaTexture, _ataque1;
        private Vector2 _manzanaPosition, _pruebasPosition, _pruebasPosition1, _ataquePosition1, _pruebasSpeed = new Vector2(3, 3);
        private Song _backgroundMusic;
        private float posInicial = 0, posInicial1 = 0, at1;
        private double distInicial1, dif1, co1,ca1,md1 = 0,xd1;

        private bool _manzanaTocada = false, salto = false, _manzanaTocada1 = false, salto1 = false, disparo1 = false;

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
            // Se utiliza para poder recibir el estado del teclado.
            KeyboardState keyboardState = Keyboard.GetState();


            //primer jugador
            if (keyboardState.IsKeyDown(Keys.Left))
                // Se resta la posición actual en "X" por el valor de la velocidad. Si velocidad es 2 se restan 2 posiciones.
                _pruebasPosition1.X -= 5;

            // Cuando se aprieta la tecla "right" se hace la siguiente operación:
            if (keyboardState.IsKeyDown(Keys.Right))
                // Se suma la posición actual en "X" por el valor de la velocidad.
                _pruebasPosition1.X += 5;

            // Cuando se aprieta la tecla "up" se hace la siguiente operación:
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                _manzanaTocada1 = false;
                salto1 = true;
                _pruebasPosition1.Y -= 5;
                posInicial1 = _pruebasPosition1.Y;
                // Se resta la posición actual en "Y" por el valor de la velocidad.
            }
            if (keyboardState.IsKeyDown(Keys.Down) && !_manzanaTocada1)
            // Se suma la posición actual en "Y" por el valor de la velocidad.
            _pruebasPosition1.Y += 5;

            if (keyboardState.IsKeyDown(Keys.J) && !disparo1)
            {
                disparo1 = true;
                _ataquePosition1.X = _pruebasPosition1.X;
                _ataquePosition1.Y = _pruebasPosition1.Y;
                /*
                m1 = (_pruebasPosition.Y-_pruebasPosition1.Y)/(_pruebasPosition.X-_pruebasPosition1.X);
                b1 = _pruebasPosition1.Y - (m1*_pruebasPosition1.X);*/
                ca1 = _ataquePosition1.Y - _pruebasPosition.Y;
                co1 = _ataquePosition1.X - _pruebasPosition.X;
                distInicial1 = Math.Sqrt(Math.Pow(co1,2) + Math.Pow(ca1, 2));
                dif1 = distInicial1 / 30;
                at1 = 30;
                xd1 = Math.Round(_ataquePosition1.Y / _ataquePosition1.X);
            }
            if (disparo1)
            {/*
                if (m1 > 0) _ataquePosition1.Y -= ;
                else if (m1 < 0) _ataquePosition1.Y += ;
                _ataquePosition1.X = (_ataquePosition1.Y - b1)/m1;*/
                
                md1++;
                if (xd1 == md1)
                {
                    if (at1 > 0)
                    {
                        _ataquePosition1.Y = float.Parse((_pruebasPosition.Y + (ca1 * distInicial1) / (distInicial1 - dif1)).ToString());
                        _ataquePosition1.X = float.Parse((_pruebasPosition.X + (co1 * distInicial1) / (distInicial1 - dif1)).ToString());
                        at1++;
                    }
                    else
                    {
                        disparo1 = false;
                    }
                    md1 = 0;
                }
            }
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
            if (keyboardState.IsKeyDown(Keys.A))
                    // Se resta la posición actual en "X" por el valor de la velocidad. Si velocidad es 2 se restan 2 posiciones.
                    _pruebasPosition.X -= 5;

                // Cuando se aprieta la tecla "right" se hace la siguiente operación:
                if (keyboardState.IsKeyDown(Keys.D))
                    // Se suma la posición actual en "X" por el valor de la velocidad.
                    _pruebasPosition.X += 5;

            // Cuando se aprieta la tecla "up" se hace la siguiente operación:
            if (keyboardState.IsKeyDown(Keys.W) && !salto && _manzanaTocada)
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
                Rectangle pruebasRectangle = new Rectangle((int)_pruebasPosition.X, (int)_pruebasPosition.Y, _pruebasTexture.Width, _pruebasTexture.Height);
                Rectangle pruebasRectangle1 = new Rectangle((int)_pruebasPosition1.X, (int)_pruebasPosition1.Y, _pruebasTexture1.Width, _pruebasTexture1.Height);
                // Es la caja de coliciones de "pruebas" y se basa en el tamaño de la imgane.
                Rectangle manzanaRectangle = new Rectangle((int)_manzanaPosition.X, (int)_manzanaPosition.Y + (_manzanaTexture.Height / 3)*2, _manzanaTexture.Width, _manzanaTexture.Height/3);
                Rectangle ataqueRectangle1 = new Rectangle((int)_ataquePosition1.X, (int)_ataquePosition1.Y + (_ataque1.Height / 3)*2, _ataque1.Width, _ataque1.Height/3);
                // Es la caja de coliciones de "manzana" y se basa en el tamaño de la imgane.

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
            if(disparo1)
                _spriteBatch.Draw(_ataque1, _ataquePosition1, Color.White);
            //_spriteBatch.Draw(_pruebasTexture, new Rectangle(800, 800, 64, 64), Color.White);
            //if (!_manzanaTocada)
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}