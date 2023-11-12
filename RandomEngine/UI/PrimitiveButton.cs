namespace RandomEngine.UI;

public class PrimitiveButton
{
    private SpriteFontBase _font;

    private Vector2 _textPosition;

    private string _text;

    public delegate void ActionCallback(PrimitiveButton button);

    public PrimitiveButton(Rectangle rectangle, string text, SpriteFontBase font, ActionCallback? action)
    {
        _font = font ?? throw new ArgumentNullException(nameof(font));

        Rectangle = rectangle;

        _text = text ?? string.Empty;
        CalcTextPos(_text, _font);

        Action = action;

        BorderWidth = 3;

        ForeColor = Color.FromNonPremultiplied(0, 0, 0, 255);
        BackColor = Color.FromNonPremultiplied(200, 200, 200, 255);
        BorderColor = Color.FromNonPremultiplied(48, 48, 48, 255);

        ForeColorHovered = Color.FromNonPremultiplied(0, 0, 0, 255);
        BackColorHovered = Color.FromNonPremultiplied(200, 200, 230, 255);
        BorderColorHovered = Color.FromNonPremultiplied(48, 48, 64, 255);

        ForeColorPressed = Color.FromNonPremultiplied(0, 0, 0, 255);
        BackColorPressed = Color.FromNonPremultiplied(210, 210, 255, 255);
        BorderColorPressed = Color.FromNonPremultiplied(48, 48, 96, 255);

        ToggledForeColor = Color.FromNonPremultiplied(0, 128, 0, 255);
        ToggledBackColor = Color.FromNonPremultiplied(235, 235, 235, 255);
        ToggledBorderColor = Color.FromNonPremultiplied(48, 48, 48, 255);

        ToggledForeColorHovered = Color.FromNonPremultiplied(0, 128, 0, 255);
        ToggledBackColorHovered = Color.FromNonPremultiplied(235, 235, 235, 255);
        ToggledBorderColorHovered = Color.FromNonPremultiplied(48, 48, 64, 255);

        ToggledForeColorPressed = Color.FromNonPremultiplied(0, 128, 0, 255);
        ToggledBackColorPressed = Color.FromNonPremultiplied(255, 255, 255, 255);
        ToggledBorderColorPressed = Color.FromNonPremultiplied(48, 48, 96, 255);
    }

    public bool Hover { get; protected set; }
    public bool Toggled { get; set; }
    public bool PressedDown { get; protected set; }

    public Color ForeColor { get; set; }
    public Color BackColor { get; set; }
    public Color BorderColor { get; set; }

    public Color ForeColorHovered { get; set; }
    public Color BackColorHovered { get; set; }
    public Color BorderColorHovered { get; set; }

    public Color ForeColorPressed { get; set; }
    public Color BackColorPressed { get; set; }
    public Color BorderColorPressed { get; set; }

    public Color ToggledForeColor { get; set; }
    public Color ToggledBackColor { get; set; }
    public Color ToggledBorderColor { get; set; }

    public Color ToggledForeColorHovered { get; set; }
    public Color ToggledBackColorHovered { get; set; }
    public Color ToggledBorderColorHovered { get; set; }

    public Color ToggledForeColorPressed { get; set; }
    public Color ToggledBackColorPressed { get; set; }
    public Color ToggledBorderColorPressed { get; set; }

    public int BorderWidth { get; set; }

    public string Text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            CalcTextPos(_text, _font);
        }
    }

    public ActionCallback? Action { get; set; }

    public SpriteFontBase Font
    {
        get
        {
            return _font;
        }
        set
        {
            _font = value;
            CalcTextPos(_text, _font);
        }
    }

    public Rectangle Rectangle { get; set; }

    public virtual void Update(Engine engine, GameTime gameTime)
    {
        Hover = Rectangle.Contains(engine.Mouse.PositionScreen);
        PressedDown = Hover && engine.Mouse.LeftDown;

        if (Hover && engine.Mouse.LeftClicked)
            Action?.Invoke(this);
    }

    public virtual void Draw(Engine engine, GameTime gameTime)
    {
        if (Toggled)
        {
            if (PressedDown)
                Draw(engine, ToggledForeColorPressed, ToggledBackColorPressed, ToggledBorderColorPressed);
            else if (Hover)
                Draw(engine, ToggledForeColorHovered, ToggledBackColorHovered, ToggledBorderColorHovered);
            else
                Draw(engine, ToggledForeColor, ToggledBackColor, BorderColor);
        }
        else
        {
            if (PressedDown)
                Draw(engine, ForeColorPressed, BackColorPressed, BorderColorPressed);
            else if (Hover)
                Draw(engine, ForeColorHovered, BackColorHovered, BorderColorHovered);
            else
                Draw(engine, ForeColor, BackColor, BorderColor);
        }
    }

    protected virtual void Draw(Engine engine, Color foreColor, Color backColor, Color borderColor)
    {
        engine.Render.Rectangle(Rectangle, backColor);
        engine.Render.Rectangle(Rectangle, borderColor, BorderWidth);
        engine.Render.Text(_font, Text, _textPosition, foreColor);
    }

    protected virtual void CalcTextPos(string text, SpriteFontBase font)
    {
        var stringSize = font.MeasureString(text);
        _textPosition = new Vector2(Rectangle.X + Rectangle.Width / 2 - stringSize.X / 2, Rectangle.Y + Rectangle.Height / 2 - stringSize.Y / 2);
    }
}