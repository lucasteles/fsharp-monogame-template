open System
open MyGame

[<EntryPoint; STAThread>]
let main args =
    use game = new Game1()
    game.Run()
    0
