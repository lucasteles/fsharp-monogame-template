namespace MyGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Game1() as this =
    inherit Game()

    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<_>

    let mutable logo: Texture2D = Unchecked.defaultof<_>
    let mutable rotation = 0f
    let mutable scale = 0f

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
        logo <- this.Content.Load<_>("logo")

    override this.Update gameTime =
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)) then
            this.Exit()

        rotation <- (rotation + 0.01f)

        if (scale < 2f)
        then scale <- scale + 0.05f

        base.Update gameTime

    override this.Draw gameTime =
        this.GraphicsDevice.Clear Color.LightGray

        let logoCenter = Vector2(float32 logo.Bounds.Width, float32 logo.Bounds.Height) / 2f
        let position =
            Vector2(float32 this.Window.ClientBounds.Width / 2f,
                    float32 this.Window.ClientBounds.Height / 2f)

        spriteBatch.Begin()
        spriteBatch.Draw(logo, position, logo.Bounds, Color.White, rotation, logoCenter, scale, SpriteEffects.None, 0f)
        spriteBatch.End()

        base.Draw(gameTime)
