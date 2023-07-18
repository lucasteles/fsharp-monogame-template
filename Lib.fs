[<AutoOpen>]
module Lib

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

let (|KeyDown|_|) k (state: KeyboardState) =
    if state.IsKeyDown k then Some() else None

module Vector2 =
    let up = Vector2(0.f, -1.f)
    let down = Vector2(0.f, 1.f)
    let left = Vector2(-1.f, 0.f)
    let right = Vector2(1.f, 0.f)

    let movement =
        function
        | KeyDown Keys.W & KeyDown Keys.A -> up + left
        | KeyDown Keys.W & KeyDown Keys.D -> up + right
        | KeyDown Keys.S & KeyDown Keys.A -> down + left
        | KeyDown Keys.S & KeyDown Keys.D -> down + right
        | KeyDown Keys.W -> up
        | KeyDown Keys.S -> down
        | KeyDown Keys.A -> left
        | KeyDown Keys.D -> right
        | _ -> Vector2.Zero
