﻿namespace MyGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Transform =
    { Rotation: single
      Scale: single
      Position: Vector2 }

type FSharpLogo =
    { Texture: Texture2D
      Transform: Transform
      Speed: single }

module Transform =
    let create pos rot scale =
        { Position = pos
          Scale = scale
          Rotation = rot }

module GameLogic =

    let (|KeyDown|_|) k (state: KeyboardState) =
        if state.IsKeyDown k then Some() else None

    let movementVector =
        function
        | KeyDown Keys.W & KeyDown Keys.A -> Vector2(-1.f, -1.f)
        | KeyDown Keys.W & KeyDown Keys.D -> Vector2(1.f, -1.f)
        | KeyDown Keys.S & KeyDown Keys.A -> Vector2(-1.f, 1.f)
        | KeyDown Keys.S & KeyDown Keys.D -> Vector2(1.f, 1.f)
        | KeyDown Keys.W -> Vector2(0.f, -1.f)
        | KeyDown Keys.S -> Vector2(0.f, 1.f)
        | KeyDown Keys.A -> Vector2(-1.f, 0.f)
        | KeyDown Keys.D -> Vector2(1.f, -0.f)
        | _ -> Vector2.Zero

    let createLogo (game: Game) =
        let texture = game.Content.Load<_>("logo")

        let position =
            Vector2(single game.Window.ClientBounds.Width / 2f, single game.Window.ClientBounds.Height / 2f)

        { Texture = texture
          Transform = Transform.create position 0f 1f
          Speed = 100f }

    let updateLogo logo (time: GameTime) =

        let moveVector = Keyboard.GetState() |> movementVector

        let moveOffset =
            moveVector * logo.Speed * (float32 time.ElapsedGameTime.TotalSeconds)

        let { Scale = scale
              Rotation = rot
              Position = pos } = logo.Transform

        let newTransform =
            { logo.Transform with
                Rotation = rot + 0.01f
                Scale = if (scale < 2f) then scale + 0.05f else scale
                Position = pos + moveOffset }

        { logo with Transform = newTransform }

    let drawLogo (spriteBatch: SpriteBatch) logo =
        let logoCenter =
            Vector2(float32 logo.Texture.Bounds.Width, float32 logo.Texture.Bounds.Height)
            / 2f

        spriteBatch.Draw(
            logo.Texture,
            logo.Transform.Position,
            logo.Texture.Bounds,
            Color.White,
            logo.Transform.Rotation,
            logoCenter,
            logo.Transform.Scale,
            SpriteEffects.None,
            0f
        )

open GameLogic

type Game1() as this =
    inherit Game()

    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<_>
    let mutable logo: FSharpLogo = Unchecked.defaultof<_>

    do
        this.Content.RootDirectory <- "Content"
        this.IsMouseVisible <- true

    override this.Initialize() =
        graphics.PreferredBackBufferWidth <- 1024
        graphics.PreferredBackBufferHeight <- 768
        graphics.ApplyChanges()
        base.Initialize()

    override this.LoadContent() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        logo <- createLogo this

    override this.Update gameTime =
        if
            GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed
             || Keyboard.GetState().IsKeyDown(Keys.Escape)
        then
            this.Exit()

        logo <- updateLogo logo gameTime

        base.Update gameTime

    override this.Draw gameTime =
        this.GraphicsDevice.Clear Color.LightGray
        spriteBatch.Begin()
        drawLogo spriteBatch logo
        spriteBatch.End()

        base.Draw(gameTime)
