module GameLogic

open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Events

// components
[<Struct>] type Transform = { Rotation: float32; Scale: float32; Position: Vector2 }
[<Struct>] type FSharpLogo = { Texture: Texture2D;  Speed: float32 }

let (|KeyDown|_|) k (state: KeyboardState) = if state.IsKeyDown k then Some() else None

let movementVector = function
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
    {
        Texture = game.Content.Load<_>("logo")
        Speed = 100f
    }

let startPosition (game: Game) =
    {
      Position = Vector2(float32 game.Window.ClientBounds.Width / 2f,
                         float32 game.Window.ClientBounds.Height / 2f)
      Rotation = 0f
      Scale = 0f
    }

let updateLogo logo logoTransform deltaTime =

    let moveVector = Keyboard.GetState() |> movementVector
    let moveOffset = moveVector * logo.Speed * deltaTime

    let { Scale=scale; Rotation=rot; Position=pos } = logoTransform

    let newTransform = {
        logoTransform with
            Rotation = rot + 0.01f
            Scale = if (scale < 2f) then scale + 0.04f else scale
            Position = pos + moveOffset
    }

    newTransform


let drawLogo (spriteBatch: SpriteBatch) logo transform =
    let logoCenter = Vector2(float32 logo.Texture.Bounds.Width, float32 logo.Texture.Bounds.Height) / 2f
    spriteBatch.Draw(
        logo.Texture,
        transform.Position,
        logo.Texture.Bounds,
        Color.White,
        transform.Rotation,
        logoCenter,
        transform.Scale,
        SpriteEffects.None, 0f)

let configureWorld (world: Container) =

    // start logo system
    world.On<Start>(
        fun (Game game) ->
            world.Create()
                .With(createLogo game)
                .With(startPosition game)
            |> ignore) |> ignore

    // update logo system
    world.On<Update>(
        fun e struct(tr: Transform, logo: FSharpLogo) ->
            printfn "p: %A" tr.Position
            updateLogo logo tr (float32 e.DeltaTime.TotalSeconds)

        |> Join.update2
        |> Join.over world) |> ignore

    // drawlogo system
    world.On<Draw>(
        fun e struct(tr: Transform, logo: FSharpLogo) ->
            drawLogo e.SpriteBatch logo tr

        |> Join.iter2
        |> Join.over world) |> ignore

    // quit game syste
    world.On<Update>(
        fun e ->
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)) then
                e.Game.Exit()
        ) |> ignore

    world
