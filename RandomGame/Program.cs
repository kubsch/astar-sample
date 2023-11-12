var engine = new Engine();

engine.AddGameState(new GameStateIntro(engine));
engine.AddGameState(new GameStateMenu(engine));
engine.AddGameState(new GameStateWorld(engine));

engine.Run("intro");