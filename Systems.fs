module Systems

open Garnet.Composition
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open Components
open Events

let configureWorld (world: Container) =
    [
      // load content
      world.On(fun (LoadContent game) -> world.Create().With(Logo.create game) |> ignore)

      // start logo system
      world.On
      <| fun (Start game) ->
          for logo in world.Query<Eid, Logo>() do
              let entity = world.Get logo.Value1
              entity.Add(Transform.center game)

      // update logo system
      world.On<Update>
      <| fun e ->
          for r in world.Query<Transform, Logo>() do
              let tr = &r.Value1
              let logo = r.Value2
              tr <- Logo.update logo tr e.TotalSeconds

      // draw logo system
      world.On<Draw>
      <| fun e ->
          for r in world.Query<Transform, Logo>() do
              Logo.draw e.SpriteBatch r.Value1 r.Value2

      // quit game system
      world.On<Update>(fun e ->
          if
              GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed
              || Keyboard.GetState().IsKeyDown(Keys.Escape)
          then
              e.Game.Exit()) ]
