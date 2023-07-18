namespace Components

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

[<Struct>]
type Transform =
    { Rotation: Angle
      Scale: single
      Position: Vector2 }

[<Struct>]
type Logo = { Texture: Texture2D; Speed: single }

module Transform =
    let center (game: Game) =
        { Position = Vector2(single game.Window.ClientBounds.Width / 2f, single game.Window.ClientBounds.Height / 2f)
          Rotation = 0f<rad>
          Scale = 0f }

module Logo =
    let create (game: Game) =
        { Texture = game.Content.Load("logo")
          Speed = 100f }

    let update logo logoTransform (deltaTime: single) =
        let moveVector = Keyboard.GetState() |> Vector2.movement
        let moveOffset = moveVector * logo.Speed * deltaTime

        let { Scale = scale
              Rotation = rot
              Position = pos } =
            logoTransform

        { logoTransform with
            Rotation = rot + 0.01f<rad>
            Scale = if scale < 2f then scale + 0.04f else scale
            Position = pos + moveOffset }

    let draw (spriteBatch: SpriteBatch) transform logo =
        let logoCenter =
            Vector2(single logo.Texture.Bounds.Width, single logo.Texture.Bounds.Height)
            / 2f

        spriteBatch.Draw(
            logo.Texture,
            transform.Position,
            logo.Texture.Bounds,
            Color.White,
            single transform.Rotation,
            logoCenter,
            transform.Scale,
            SpriteEffects.None,
            0f
        )
