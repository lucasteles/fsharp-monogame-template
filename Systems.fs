module Systems

open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open Components
open Events

// put all your system config here
let configureWorld (world: Container) =

    // load content
    world.On(fun (LoadContent game) -> world.Create().With(Logo.create game) |> ignore)
    |> ignore

    // start logo system
    world.On(
        fun (Start game) struct (eid: Eid, _: Logo) ->
            let entity = world.Get eid
            entity.Add(Transform.center game)
            eid
        |> Join.update2
        |> Join.over world
    )
    |> ignore

    // update logo system
    world.On<Update>(
        fun e struct (tr: Transform, logo: Logo) -> Logo.update logo tr (single e.DeltaTime.TotalSeconds)
        |> Join.update2
        |> Join.over world
    )
    |> ignore

    // drawlogo system
    world.On<Draw>(
        fun e struct (tr: Transform, logo: Logo) -> Logo.draw e.SpriteBatch logo tr
        |> Join.iter2
        |> Join.over world
    )
    |> ignore

    // quit game system
    world.On<Update>(fun e ->
        if
            GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        then
            e.Game.Exit())
    |> ignore

    world
