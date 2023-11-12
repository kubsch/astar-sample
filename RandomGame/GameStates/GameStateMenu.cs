using RandomEngine.UI;
using System.Reflection;

namespace RandomGame.GameStates;

public class GameStateMenu : IGameState
{
    private readonly Engine _engine;
    private readonly List<PrimitiveButton> _buttonsMain = new();
    private readonly List<PrimitiveButton> _buttonsOptions = new();
    private MenuScreen _currentMenuScreen;
    private Point _screenCenter;
    private GameTime _gameTime;

    private enum MenuScreen
    {
        Main,
        Options
    }

    public GameStateMenu(Engine engine)
    {
        _engine = engine;
    }

    public string Name => "menu";

    public void Initialize()
    {
        _screenCenter = new Point(_engine.Graphics.PreferredBackBufferWidth / 2, _engine.Graphics.PreferredBackBufferHeight / 2);

        _currentMenuScreen = MenuScreen.Main;

        AddButton(_buttonsMain, "Start game", ButtonStartClick);
        AddButton(_buttonsMain, "Options", ButtonOptionsClick);
        AddButton(_buttonsMain, "Quit", ButtonExitClick);

        AddToggleButton(_buttonsOptions, "Music", MusicOnClick);
        AddToggleButton(_buttonsOptions, "Fullscreen", WindowsClick);
        AddButton(_buttonsOptions, "Back to main menu", BackToMainClick);
    }

    public void Update(GameTime gameTime)
    {
        _gameTime = gameTime;

        UpdateMenu(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        DrawMenu(gameTime);
    }

    public void WakeUp(GameTime gameTime, IGameState? oldGameState)
    {
        _engine.Music.Loop(Assets.Songs.LadyOfThe80s);
        _engine.IsMouseVisible = true;
    }

    private void UpdateMenu(GameTime gameTime)
    {
        switch (_currentMenuScreen)
        {
            case MenuScreen.Main:
                for (var i = 0; i < _buttonsMain.Count; i++)
                    _buttonsMain[i].Update(_engine, gameTime);
                break;

            case MenuScreen.Options:
                for (var i = 0; i < _buttonsOptions.Count; i++)
                    _buttonsOptions[i].Update(_engine, gameTime);

                if (_engine.Keyboard.IsKeyReleased(Keys.Escape))
                    _currentMenuScreen = MenuScreen.Main;
                break;

            default:
                break;
        }
    }

    private void DrawMenu(GameTime gameTime)
    {
        _engine.Render.Begin();

        RenderTitle(gameTime);

        switch (_currentMenuScreen)
        {
            case MenuScreen.Main:
                foreach (var button in _buttonsMain)
                    button.Draw(_engine, gameTime);
                break;

            case MenuScreen.Options:
                foreach (var button in _buttonsOptions)
                    button.Draw(_engine, gameTime);
                break;

            default:
                break;
        }

        RenderFooter();

        _engine.Render.End();
    }

    private void WindowsClick(PrimitiveButton button)
    {
        button.Toggled = !button.Toggled;
        _engine.Graphics.IsFullScreen = !_engine.Graphics.IsFullScreen;
        _engine.Graphics.ApplyChanges();
    }

    private void RenderFooter()
    {
        var footerFont = Assets.Fonts.GetFont(16);
        var footerText = $"Version {Assembly.GetExecutingAssembly().GetName().Version}";
        var footerSize = footerFont.MeasureString(footerText);
        _engine.Render.Text(footerFont, footerText, new Vector2(_screenCenter.X - footerSize.X / 2, _engine.Graphics.PreferredBackBufferHeight - footerSize.Y - 10), Color.White);
    }

    private void RenderTitle(GameTime gameTime)
    {
        var title = "PATHFINDING DEMO";
        var titleSize = Assets.Fonts.Neuro128.MeasureString(title);
        var titleX = _screenCenter.X - titleSize.X / 2;
        var titleY = 100;

        var offset = 12;
        var color = Color.FromNonPremultiplied(0, 255, 0, 255);

        // LOL lazy
        for (int i = 1; i < 6; i += 1)
        {
            offset = 6 - i;
            color = Color.FromNonPremultiplied(0, (96 - (offset * 4)), 0, 255);

            _engine.Render.Text(Assets.Fonts.Neuro128, title, new Vector2(titleX - offset, titleY), color);
            _engine.Render.Text(Assets.Fonts.Neuro128, title, new Vector2(titleX, titleY - offset), color);
            _engine.Render.Text(Assets.Fonts.Neuro128, title, new Vector2(titleX - offset, titleY - offset), color);
            _engine.Render.Text(Assets.Fonts.Neuro128, title, new Vector2(titleX + offset, titleY), color);
            _engine.Render.Text(Assets.Fonts.Neuro128, title, new Vector2(titleX, titleY + offset), color);
            _engine.Render.Text(Assets.Fonts.Neuro128, title, new Vector2(titleX + offset, titleY + offset), color);
            _engine.Render.Text(Assets.Fonts.Neuro128, title, new Vector2(titleX - offset, titleY + offset), color);
            _engine.Render.Text(Assets.Fonts.Neuro128, title, new Vector2(titleX + offset, titleY - offset), color);
        }

        _engine.Render.Text(Assets.Fonts.Neuro128, title, new Vector2(titleX, titleY), Color.White);
    }

    private void ButtonStartClick(PrimitiveButton button)
    {
        _engine.ToGameState(_gameTime, _engine.GetGameState("world"));

        _buttonsMain[0].Text = "Return to game";
    }

    private void ButtonOptionsClick(PrimitiveButton button)
    {
        _currentMenuScreen = MenuScreen.Options;
    }

    private void BackToMainClick(PrimitiveButton button)
    {
        _currentMenuScreen = MenuScreen.Main;
    }

    private void ButtonExitClick(PrimitiveButton button)
    {
        _engine.Exit();
    }

    private void MusicOnClick(PrimitiveButton button)
    {
        button.Toggled = !button.Toggled;
        _engine.Music.IsMuted = !button.Toggled;
    }

    private void AddButton(List<PrimitiveButton> buttons, string text, PrimitiveButton.ActionCallback? action = null)
    {
        var buttonWidth = (int)Assets.Fonts.Neuro64.MeasureString(text).X;
        var buttonHeight = 125;
        var buttonX = _screenCenter.X - buttonWidth / 2;
        var buttonY = 400;
        var buttonOffset = 32;
        var rectangle = new Rectangle(buttonX, buttonY + buttonOffset + buttons.Count * (buttonHeight + buttonOffset), buttonWidth, buttonHeight);

        var button = new PrimitiveButton(
            rectangle,
            text,
            Assets.Fonts.Neuro64,
            action);

        button.ForeColor = Color.FromNonPremultiplied(200, 200, 200, 255);
        button.BackColor = Color.FromNonPremultiplied(0, 0, 0, 255);
        button.BorderColor = Color.FromNonPremultiplied(0, 0, 0, 255);

        button.ForeColorHovered = Color.FromNonPremultiplied(230, 255, 230, 255);
        button.BackColorHovered = Color.FromNonPremultiplied(0, 0, 0, 255);
        button.BorderColorHovered = Color.FromNonPremultiplied(0, 0, 0, 255);

        button.ForeColorPressed = Color.FromNonPremultiplied(200, 255, 200, 255);
        button.BackColorPressed = Color.FromNonPremultiplied(0, 0, 0, 255);
        button.BorderColorPressed = Color.FromNonPremultiplied(0, 0, 0, 255);

        buttons.Add(button);
    }

    private void AddToggleButton(List<PrimitiveButton> buttons, string text, PrimitiveButton.ActionCallback? action = null)
    {
        var buttonWidth = (int)Assets.Fonts.Neuro64.MeasureString(text).X;
        var buttonHeight = 125;
        var buttonX = _screenCenter.X - buttonWidth / 2;
        var buttonY = 400;
        var buttonOffset = 32;
        var rectangle = new Rectangle(buttonX, buttonY + buttonOffset + buttons.Count * (buttonHeight + buttonOffset), buttonWidth, buttonHeight);

        var button = new PrimitiveButton(
            rectangle,
            text,
            Assets.Fonts.Neuro64,
            action);

        button.Toggled = true;

        button.ForeColor = Color.FromNonPremultiplied(255, 100, 100, 255);
        button.BackColor = Color.FromNonPremultiplied(0, 0, 0, 255);
        button.BorderColor = Color.FromNonPremultiplied(0, 0, 0, 255);

        button.ForeColorHovered = Color.FromNonPremultiplied(255, 150, 150, 255);
        button.BackColorHovered = Color.FromNonPremultiplied(0, 0, 0, 255);
        button.BorderColorHovered = Color.FromNonPremultiplied(0, 0, 0, 255);

        button.ForeColorPressed = Color.FromNonPremultiplied(255, 170, 170, 255);
        button.BackColorPressed = Color.FromNonPremultiplied(0, 0, 0, 255);
        button.BorderColorPressed = Color.FromNonPremultiplied(0, 0, 0, 255);

        button.ToggledForeColor = Color.FromNonPremultiplied(100, 255, 100, 255);
        button.ToggledBackColor = Color.FromNonPremultiplied(0, 0, 0, 255);
        button.ToggledBorderColor = Color.FromNonPremultiplied(0, 0, 0, 255);

        button.ToggledForeColorHovered = Color.FromNonPremultiplied(150, 255, 150, 255);
        button.ToggledBackColorHovered = Color.FromNonPremultiplied(0, 0, 0, 255);
        button.ToggledBorderColorHovered = Color.FromNonPremultiplied(0, 0, 0, 255);

        button.ToggledForeColorPressed = Color.FromNonPremultiplied(170, 255, 170, 255);
        button.ToggledBackColorPressed = Color.FromNonPremultiplied(0, 0, 0, 255);
        button.ToggledBorderColorPressed = Color.FromNonPremultiplied(0, 0, 0, 255);

        buttons.Add(button);
    }
}