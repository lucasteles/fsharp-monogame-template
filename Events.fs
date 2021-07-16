module Events

open System
open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

// Events
[<Struct>] type Start = Game of Game
                             member _.Data(Game game) = game

[<Struct>] type Update = { DeltaTime: TimeSpan; Game: Game }
[<Struct>] type Draw = { Time: TimeSpan; SpriteBatch: SpriteBatch}



