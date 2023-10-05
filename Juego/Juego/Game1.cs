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
        -Terminar el menu
        -Incluir las texturas de los personajes
        -Transformaciones
        -Sistema de golpes a corta distancia
        -Acercamiento de la camara cuando los jugadores esten cerca
        -Choque de kamehamehas (o como chota se escriba)
         Menos imporatantes:
        -Remplazar el modo de disparo
        -Ataques especiales
        -Modificar skills de los jugadores
         */
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Texture2D play, titulo;
        private Vector2 playVector, tituloVector; //variables del menu
        private Texture2D cuadro1,cuadro2,cuadro3,cuadro4,s1,s2;
        private Vector2 c1Vector,c2Vector,c3Vector,c4Vector,s1Vector,s2Vector; //variables carga
        private Texture2D _pruebasTexture, _pruebasTexture1 , _manzanaTexture, _ataque1,particula,particula1,ni1,ni2,ni3,ni4,ni5,ni6;
        private Vector2 _manzanaPosition, _pruebasPosition, _pruebasPosition1, _ataquePosition1, _pruebasSpeed , ba1v , ba2v,ba3v,ba4v ;
        private Song _backgroundMusic;
        private Barra ba1,ba2,ba3,ba4;
        private int posPantalla = 1;
        private float posInicial = 0, posInicial1 = 0, at1=0,at=0;
        private double distInicial1, dif1, co1,ca1,md1 = 0,xd1, marca, cont;
        private float m1, b1, fijo1, fijo2, fijo3, fijo4;
        private List<DisparoNormal> p1 = new List<DisparoNormal>(), p2 = new List<DisparoNormal>();
        private EspecialDisparo t1, t2;
        private List<List<Texture2D>> texturas;
        private List<List<bool>> skills;

        private float zoom = 1.0f;
        private Vector2 cam = Vector2.Zero;
        private Matrix originalMatrix;

        private bool _manzanaTocada = false, salto = false, _manzanaTocada1 = false, salto1 = false, disparo1 = false, bban = false;
        private bool n1, n2; //bandera para innmovilizar mientras prepara ataque

        private bool u1, u2, u3, u4, k1, k2;
        private int select1, select2, ax1 = 10,ax2 = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            play = Content.Load<Texture2D>("img/Play");
            playVector = new Vector2(GraphicsDevice.Viewport.Width / 2 - play.Width / 2, 340);
            titulo = Content.Load<Texture2D>("img/Titulo");
            tituloVector = new Vector2(GraphicsDevice.Viewport.Width / 2 - titulo.Width / 2, 50); //sprites y vectores de menu
            cuadro1 = Content.Load<Texture2D>("img/defecto (1)");
            c1Vector = new Vector2(100, GraphicsDevice.Viewport.Height / 2 - cuadro1.Height / 2);
            cuadro2 = Content.Load<Texture2D>("img/defecto (1)");
            c2Vector = new Vector2(250, GraphicsDevice.Viewport.Height / 2 - cuadro1.Height / 2);
            cuadro3 = Content.Load<Texture2D>("img/defecto (1)");
            c3Vector = new Vector2(400, GraphicsDevice.Viewport.Height / 2 - cuadro1.Height / 2);
            cuadro4 = Content.Load<Texture2D>("img/defecto (1)");
            c4Vector = new Vector2(550, GraphicsDevice.Viewport.Height / 2 - cuadro1.Height / 2);
            s1 = Content.Load<Texture2D>("img/s1 (1)");
            s1Vector = new Vector2(125, 367);
            s2 = Content.Load<Texture2D>("img/s2 (1)");
            s2Vector = new Vector2(575, 180); // sprites y vectores de carga
            _spriteFont = Content.Load<SpriteFont>("fuente");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pruebasTexture = Content.Load<Texture2D>("img/goku (6)");

            _pruebasPosition = new Vector2(100, 520);
            _pruebasTexture1 = Content.Load<Texture2D>("img/goku (7)");
            _pruebasPosition1 = new Vector2(600, 520);
            _ataque1 = Content.Load<Texture2D>("img/bola (1)");
            _ataquePosition1 = new Vector2(0, 0);
            particula = Content.Load<Texture2D>("img/particula");
            particula1 = Content.Load<Texture2D>("img/particula1");

            texturas = new List<List<Texture2D>>() {
                new List<Texture2D> {
                Content.Load<Texture2D>("img/goku (6)"),
                Content.Load<Texture2D>("img/goku (7)"),
                Content.Load<Texture2D>("img/bola (1)"),
                Content.Load<Texture2D>("img/particula"),
                Content.Load<Texture2D>("img/particula1")
                },
                new List<Texture2D> {
                Content.Load<Texture2D>("img/goku (6)"),
                Content.Load<Texture2D>("img/goku (7)"),
                Content.Load<Texture2D>("img/bola (1)"),
                Content.Load<Texture2D>("img/particula"),
                Content.Load<Texture2D>("img/particula1")
                },
                new List<Texture2D> {
                Content.Load<Texture2D>("img/goku (6)"),
                Content.Load<Texture2D>("img/goku (7)"),
                Content.Load<Texture2D>("img/bola (1)"),
                Content.Load<Texture2D>("img/particula"),
                Content.Load<Texture2D>("img/particula1")
                },
                new List<Texture2D> {
                Content.Load<Texture2D>("img/goku (6)"),
                Content.Load<Texture2D>("img/goku (7)"),
                Content.Load<Texture2D>("img/bola (1)"),
                Content.Load<Texture2D>("img/particula"),
                Content.Load<Texture2D>("img/particula1")
                }
            };
            skills = new List<List<bool>>()
            {
                new List<bool>(){true,false},
                new List<bool>(){false,false},
                new List<bool>(){true,true},
                new List<bool>(){false,true}
            }; // texturas y habilidades de los personajes


            ni1 = Content.Load<Texture2D>("img/n1");
            ni2 = Content.Load<Texture2D>("img/n2");
            ni3 = Content.Load<Texture2D>("img/n3");
            ni4 = Content.Load<Texture2D>("img/n4");
            ni5 = Content.Load<Texture2D>("img/n5");
            ni6 = Content.Load<Texture2D>("img/n6");

            _pruebasSpeed = new Vector2(3, 3);
            ba1v = new Vector2(10, 30);
            ba2v = new Vector2(480, 30);
            ba3v = new Vector2(10, 62);
            ba4v = new Vector2(480, 62);

            ba1 = new Barra(3000);
            ba2 = new Barra(3000);
            ba3 = new Barra(3000);
            ba4 = new Barra(3000);

            // Carga las textura "manzana" en la variable.
            _manzanaTexture = Content.Load<Texture2D>("img/mifondo");
            // (?)
            _manzanaPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - _manzanaTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - _manzanaTexture.Height / 3+50);

            //_backgroundMusic = Content.Load<Song>("background_music");
            //MediaPlayer.Play(_backgroundMusic);
            //MediaPlayer.IsRepeating = true;
            select1 = -1;
            select2 = -1;

            //originalMatrix = _spriteBatch.GraphicsDevice.ge
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.N))
            {
                zoom += 0.1f;
            }
            if (keyboardState.IsKeyDown(Keys.M))
            {
                zoom -= 0.1f;
            }
            cam = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            originalMatrix = Matrix.CreateTranslation(new Vector3(-cam, 0.0f)) * Matrix.CreateScale(zoom) * Matrix.CreateTranslation(new Vector3(cam, 0.0f));

            if (posPantalla == 1)
            {
                if (keyboardState.IsKeyDown(Keys.I) || keyboardState.IsKeyDown(Keys.O))
                    posPantalla = 2;
            }
            else if (posPantalla == 2)
            {
                if (!k1)
                {
                    if (keyboardState.IsKeyDown(Keys.A) && !u1)
                    {
                        u1 = true;
                        if (s1Vector.X == 125)
                            s1Vector.X = 575;
                        else
                            s1Vector.X -= 150;
                    }
                    else if (keyboardState.IsKeyUp(Keys.A))
                    {
                        u1 = false;
                    }
                    if (keyboardState.IsKeyDown(Keys.D) && !u2)
                    {
                        u2 = true;
                        if (s1Vector.X == 575)
                            s1Vector.X = 125;
                        else
                            s1Vector.X += 150;
                    }
                    else if (keyboardState.IsKeyUp(Keys.D))
                    {
                        u2 = false;
                    }
                }
                if (!k2)
                {
                    if (keyboardState.IsKeyDown(Keys.Left) && !u3)
                    {
                        u3 = true;
                        if (s2Vector.X == 125)
                            s2Vector.X = 575;
                        else
                            s2Vector.X -= 150;
                    }
                    else if (keyboardState.IsKeyUp(Keys.Left))
                    {
                        u3 = false;
                    }
                    if (keyboardState.IsKeyDown(Keys.Right) && !u4)
                    {
                        u4 = true;
                        if (s2Vector.X == 575)
                            s2Vector.X = 125;
                        else
                            s2Vector.X += 150;
                    }
                    else if (keyboardState.IsKeyUp(Keys.Right))
                    {
                        u4 = false;
                    }
                }
                if(keyboardState.IsKeyDown(Keys.C))
                {
                    if (s1Vector.X == 125) select1 = 0;
                    else if (s1Vector.X == 275) select1 = 1;
                    else if (s1Vector.X == 425) select1 = 2;
                    else select1 = 3;
                    k1 = true;
                }
                if (keyboardState.IsKeyDown(Keys.K))
                {
                    if (s2Vector.X == 125) select2 = 0;
                    else if (s2Vector.X == 275) select2 = 1;
                    else if (s2Vector.X == 425) select2 = 2;
                    else select2 = 3;
                    k2 = true;
                }
                if (select1 != -1 && select2 != -1)
                {
                    posPantalla = 3;
                }
            }
            else if (posPantalla == 3)
            {
                Rectangle pruebasRectangle = new Rectangle((int)_pruebasPosition.X, (int)_pruebasPosition.Y, _pruebasTexture.Width, _pruebasTexture.Height);
                Rectangle pruebasRectangle1 = new Rectangle((int)_pruebasPosition1.X, (int)_pruebasPosition1.Y, _pruebasTexture1.Width, _pruebasTexture1.Height);
                Rectangle manzanaRectangle = new Rectangle((int)_manzanaPosition.X, (int)_manzanaPosition.Y + (_manzanaTexture.Height / 3) * 2, _manzanaTexture.Width, _manzanaTexture.Height / 3);

                

                //primer jugador
                if (keyboardState.IsKeyDown(Keys.Left) && _pruebasPosition1.X >= 5 && !n1 && !keyboardState.IsKeyDown(Keys.L)) _pruebasPosition1.X -= 5;

                if (keyboardState.IsKeyDown(Keys.Right) && !n1 && _pruebasPosition1.X + _ataque1.Width <= 795 && !keyboardState.IsKeyDown(Keys.L)) _pruebasPosition1.X += 5;

                if (keyboardState.IsKeyDown(Keys.Up) && !salto1 && _manzanaTocada1 && !n1 && !skills[select2][0] && !keyboardState.IsKeyDown(Keys.L))
                {
                    salto1 = true;
                    posInicial1 = _pruebasPosition1.Y;
                }
                else if (keyboardState.IsKeyDown(Keys.Up) && !n1 && _pruebasPosition1.Y >= 105 && skills[select2][0] && !keyboardState.IsKeyDown(Keys.L))
                {
                    _manzanaTocada1 = false;
                    _pruebasPosition1.Y -= 5;
                }
                if (salto1)
                {
                    _pruebasPosition1.Y -= 5;
                    if (posInicial1 - 100 == _pruebasPosition1.Y)
                    {
                        _manzanaTocada1 = false;
                        salto1 = false;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Down) && !_manzanaTocada1 && !n1 && skills[select2][0] && !keyboardState.IsKeyDown(Keys.L)) _pruebasPosition1.Y += 5;

                if (keyboardState.IsKeyDown(Keys.A) && !n2 && _pruebasPosition.X >= 5 && !keyboardState.IsKeyDown(Keys.V)) _pruebasPosition.X -= 5;

                if (keyboardState.IsKeyDown(Keys.D) && !n2 && _pruebasPosition.X + _ataque1.Width <= 795 && !keyboardState.IsKeyDown(Keys.V)) _pruebasPosition.X += 5;

                if (keyboardState.IsKeyDown(Keys.W) && !salto && _manzanaTocada && !n2 && !skills[select1][0] && !keyboardState.IsKeyDown(Keys.V))
                {
                    salto = true;
                    posInicial = _pruebasPosition.Y;
                }
                else if(keyboardState.IsKeyDown(Keys.W) && !n2 && _pruebasPosition.Y >= 105 && skills[select1][0] && !keyboardState.IsKeyDown(Keys.V))
                {
                    _manzanaTocada = false;
                    _pruebasPosition.Y -= 5;
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
                if (keyboardState.IsKeyDown(Keys.S) && !_manzanaTocada && !n2 && skills[select1][0] && !keyboardState.IsKeyDown(Keys.V)) _pruebasPosition.Y += 5;


                if (keyboardState.IsKeyDown(Keys.K) && t2 == null && ba4.vida >= 10 && !keyboardState.IsKeyDown(Keys.L))
                {
                    ba4.vida -= 10;
                    at1++;
                    n1 = true;
                }
                if (keyboardState.IsKeyUp(Keys.K))
                {
                    if (at1 > 20)
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
                if (keyboardState.IsKeyDown(Keys.L) && ba4.vida <= 2990 && !keyboardState.IsKeyDown(Keys.K))
                {
                    ba4.vida += 5;
                }
                if (keyboardState.IsKeyDown(Keys.C) && t1 == null && ba3.vida >= 10 && !keyboardState.IsKeyDown(Keys.V))
                {
                    ba3.vida -= 10;
                    at++;
                    n2 = true;
                }
                if (keyboardState.IsKeyUp(Keys.C))
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
                if (keyboardState.IsKeyDown(Keys.V) && ba3.vida <= 2990 && !keyboardState.IsKeyDown(Keys.C))
                {
                    ba3.vida += 5;
                }
                AvanzarDisparos(pruebasRectangle, pruebasRectangle1);

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

                if (!_manzanaTocada && !salto && !skills[select1][0]) _pruebasPosition.Y += 5;
                if (!_manzanaTocada1 && !salto1 && !skills[select2][0]) _pruebasPosition1.Y += 5;
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
                if (t1 != null) {
                    for (int i = (int)t1.non-200; i < (int)t1.non; i++)
                    {
                        Rectangle v = new Rectangle(
                            (int)t1.particulas[i]._ataquePosition1.X,
                            (int)t1.particulas[i]._ataquePosition1.Y,
                            texturas[select1][4].Width,
                            texturas[select1][4].Height);
                        if (v.Intersects(pruebasRectangle1))
                        {
                            t1 = null;
                            ba2.vida -= 500;
                            break;
                        }
                    }
                }
                if (t2 != null)
                {
                    for (int i = (int)t1.non - 200; i < (int)t1.non; i++)
                    {
                        Rectangle v = new Rectangle(
                            (int)t2.particulas[i]._ataquePosition1.X,
                            (int)t2.particulas[i]._ataquePosition1.Y,
                            texturas[select2][4].Width,
                            texturas[select2][4].Height);
                        if (v.Intersects(pruebasRectangle))
                        {
                            t2 = null;
                            ba1.vida -= 500;
                            break;
                        }
                    }
                }
                if (ba1.vida == 0 || ba2.vida == 0)
                {
                    Exit();
                }
                //}
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Limpia 20 veces por segundo la imagen.
            _spriteBatch.Begin(transformMatrix:originalMatrix);

            if (posPantalla == 1)
            {
                _spriteBatch.Draw(titulo, tituloVector, Color.White);
                _spriteBatch.Draw(play, playVector, Color.White);
            }
            else if(posPantalla == 2)
            {
                _spriteBatch.Draw(cuadro1, c1Vector, Color.White);
                _spriteBatch.Draw(cuadro2, c2Vector, Color.White);
                _spriteBatch.Draw(cuadro3, c3Vector, Color.White);
                _spriteBatch.Draw(cuadro4, c4Vector, Color.White);
                _spriteBatch.Draw(s1, s1Vector, Color.White);
                _spriteBatch.Draw(s2, s2Vector, Color.White);
            }
            else if (posPantalla == 3)
            {
                _spriteBatch.Draw(_manzanaTexture, _manzanaPosition, Color.White);

                if (_pruebasPosition.X < _pruebasPosition1.X)
                {
                    _spriteBatch.Draw(texturas[select1][0], _pruebasPosition, Color.White);
                    _spriteBatch.Draw(texturas[select2][1], _pruebasPosition1, Color.White);
                    ax1 = 10;
                    ax2 = 0;
                }
                else
                {
                    _spriteBatch.Draw(texturas[select1][1], _pruebasPosition, Color.White);
                    _spriteBatch.Draw(texturas[select2][0], _pruebasPosition1, Color.White);
                    ax1 = 0;
                    ax2 = 10;
                }
                foreach (DisparoNormal d in p1)
                {
                    _spriteBatch.Draw(d._ataque1, d._ataquePosition1, Color.White);
                }
                foreach (DisparoNormal d in p2)
                {
                    _spriteBatch.Draw(d._ataque1, d._ataquePosition1, Color.White);
                }

                if (t1 != null)
                {
                    for (int i = 0; i < t1.non; i++)
                    {
                        if (i > t1.non - 200)
                            _spriteBatch.Draw(texturas[select1][4], t1.particulas[i]._ataquePosition1, Color.White);
                        else
                            _spriteBatch.Draw(texturas[select1][3], t1.particulas[i]._ataquePosition1, Color.White);
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
                            _spriteBatch.Draw(texturas[select2][4], t2.particulas[i]._ataquePosition1, Color.White);
                        else
                            _spriteBatch.Draw(texturas[select2][3], t2.particulas[i]._ataquePosition1, Color.White);
                    }
                    t2.non += 50;
                    if (t2.non == t2.particulas.Count)
                    {
                        t2 = null;
                    }
                    n1 = true;
                    //_spriteBatch.DrawLine(particula, t2._ataquePosition1, t2._objetivoPosition1, Color.Red);
                }

                if (ba1 != null)
                {
                    bool a = false, v = false;
                    for (var i = 0; i < ba1.vida; i++)
                    {
                        ba1v.X += 0.3f;
                        if (i < 1000) _spriteBatch.Draw(ni1, ba1v, Color.White);
                        else if (i < 2000)
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
                if (ba3 != null)
                {
                    bool a = false, v = false;
                    for (var i = 0; i < ba3.vida; i++)
                    {
                        ba3v.X += 0.3f;
                        if (i < 1000) _spriteBatch.Draw(ni4, ba3v, Color.White);
                        else if (i < 2000)
                        {
                            if (!a)
                            {
                                ba3v.X = 10.3f;
                                a = true;
                            }
                            _spriteBatch.Draw(ni5, ba3v, Color.White);
                        }
                        else if (i < 3000)
                        {
                            if (!v)
                            {
                                ba3v.X = 10.3f;
                                v = true;
                            }
                            _spriteBatch.Draw(ni6, ba3v, Color.White);
                        }
                    }
                    ba3v.X = 10;
                }
                if (ba4 != null)
                {
                    bool a = false, v = false;
                    for (var i = 0; i < ba4.vida; i++)
                    {
                        ba4v.X += 0.3f;
                        if (i < 1000) _spriteBatch.Draw(ni4, ba4v, Color.White);
                        else if (i < 2000)
                        {
                            if (!a)
                            {
                                ba4v.X = 480.3f;
                                a = true;
                            }
                            _spriteBatch.Draw(ni5, ba4v, Color.White);
                        }
                        else if (i < 3000)
                        {
                            if (!v)
                            {
                                ba4v.X = 480.3f;
                                v = true;
                            }
                            _spriteBatch.Draw(ni6, ba4v, Color.White);
                        }
                    }
                    ba4v.X = 480;
                }
                //_spriteBatch.Draw(_pruebasTexture, new Rectangle(800, 800, 64, 64), Color.White);
                //if (!_manzanaTocada)


                //_spriteBatch.DrawString(_spriteFont,"SIUUUUUUUUUUUUUUUUU",new Vector2(0,0),Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void NuevoDisparo(bool xd)
        {
            if (xd)
            {
                p2.Add(new DisparoNormal(_pruebasPosition.X, _pruebasPosition1.X + ax2,_pruebasPosition.Y,_pruebasPosition1.Y+13));
                p2[p2.Count-1]._ataque1 = Content.Load<Texture2D>("img/bola (1)");
            }
            else
            {
                p1.Add(new DisparoNormal(_pruebasPosition1.X, _pruebasPosition.X + ax1, _pruebasPosition1.Y, _pruebasPosition.Y+13));
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
                    if(atx == 2)ba2.vida-=50;
                    p1.RemoveAt(i);
                }
            }
            for (int i = p2.Count-1; i >= 0; i--)
            {
                int atx = p2[i].AvanzarDisparo(new Rectangle((int)p2[i]._ataquePosition1.X, (int)p2[i]._ataquePosition1.Y, (int)p2[i]._ataque1.Width, (int)p2[i]._ataque1.Height), ataque);
                if(atx == 2 || atx == 3)
                {
                    if(atx == 2)ba1.vida-=50;
                    p2.RemoveAt(i);
                }
            }
        }
    }
}