open System
open MyGame

[<EntryPoint; STAThread>]
let main args =
    use game = Game1()
    game.Run()
    0
