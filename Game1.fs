namespace MyGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Game1 () as this =
    inherit Game()

    let graphics = new GraphicsDeviceManager(this)
    let mutable spriteBatch = Unchecked.defaultof<_>
//    let mutable playerSpriteSheet = Unchecked.defaultof<Texture2D>

    do
        this.Content.RootDirectory <- "Content"
        this.IsMouseVisible <- true

    override this.Initialize() =
        base.Initialize()
    override this.LoadContent() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)

    override this.Update gameTime =
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        then this.Exit()
        base.Update gameTime

    override this.Draw (gameTime) =
        this.GraphicsDevice.Clear Color.CornflowerBlue
        base.Draw(gameTime)




