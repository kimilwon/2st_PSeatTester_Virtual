using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace UserImageButton
{
    /// <summary>
    /// 비스타 버튼
    /// </summary>
    [DefaultEvent("Click")]
    public class ImageButton : UserControl
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Enumeration
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 이미지 버튼 스타일 - ImageButtonStyle

        /// <summary>
        /// 이미지 버튼 스타일
        /// </summary>
        public enum ImageButtonStyle
        {
            /// <summary>
            /// DEFAULT
            /// </summary>
            Default,

            /// <summary>
            /// FLAT
            /// </summary>
            Flat
        };

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region 이미지 버튼 상태 - ImageButtonState

        /// <summary>
        /// 이미지 버튼 상태
        /// </summary>
        private enum ImageButtonState { None, Hover, Pressed };

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 컨테이너
        /// </summary>
        private Container container = null;

        /// <summary>
        /// 버튼 스타일
        /// </summary>
        private ImageButtonStyle buttonStyle = ImageButtonStyle.Default;

        /// <summary>
        /// 버튼 상태
        /// </summary>
        private ImageButtonState buttonState = ImageButtonState.None;

        /// <summary>
        /// 코너 반지름
        /// </summary>
        private int cornerRadius = 8;

        /// <summary>
        /// 배경 이미지
        /// </summary>
        private Image backImage;

        /// <summary>
        /// 이미지 크기
        /// </summary>
        private Size imageSize = new Size(24, 24);

        /// <summary>
        /// 이미지 정렬
        /// </summary>
        private ContentAlignment imageAlign = ContentAlignment.MiddleLeft;

        /// <summary>
        /// 이미지 텍스트 정렬
        /// </summary>
        private TextImageRelation textimageAlign = TextImageRelation.Overlay;

        /// <summary>
        /// 이미지
        /// </summary>
        private Image image;

        /// <summary>
        /// 기본 색상
        /// </summary>
        private Color baseColor = Color.Black;

        /// <summary>
        /// 버튼 색상
        /// </summary>
        private Color buttonColor = Color.Black;

        /// <summary>
        /// 하이라이트 색상
        /// </summary>
        private Color highlightColor = Color.White;

        /// <summary>
        /// GLOW 투명도
        /// </summary>
        private int glowAlpha = 0;

        /// <summary>
        /// GLOW 색상
        /// </summary>
        private Color glowColor = Color.FromArgb(141, 189, 255);

        /// <summary>
        /// 전경색
        /// </summary>
        private Color foreColor = Color.White;

        /// <summary>
        /// 텍스트 정렬
        /// </summary>
        private ContentAlignment textAlign = ContentAlignment.MiddleCenter;

        /// <summary>
        /// 버튼 텍스트
        /// </summary>
        private string buttonText;

        /// <summary>
        /// 대화 상자 결과
        /// </summary>
        private DialogResult dialogResult = DialogResult.None;

        /// <summary>
        /// FADE IN 타이머
        /// </summary>
        private Timer fadeInTimer = null;

        /// <summary>
        /// FADE OUT 타이머
        /// </summary>
        private Timer fadeOutTimer = null;

        /// <summary>
        /// 키로 호출 여부
        /// </summary>
        private bool calledByKey = false;

        /// <summary>
        /// 클릭 여부
        /// </summary>
        private bool isClicked = false;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 버튼 스타일 - ButtonStyle

        /// <summary>
        /// 버튼 스타일
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(typeof(ImageButtonStyle), "Default")]
        [Description("마우스가 클라이언트 영역 밖에 있는 동안 버튼 배경을 그릴지 여부를 설정합니다.")]
        public ImageButtonStyle ButtonStyle
        {
            get
            {
                return this.buttonStyle;
            }
            set
            {
                this.buttonStyle = value;

                Invalidate();
            }
        }

        #endregion
        #region 코너 반지름 - CornerRadius

        /// <summary>
        /// 코너 반지름
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(8)]
        [Description("버튼 모서리의 반지름입니다. 이 값이 클수록 모퉁이가 부드럽습니다. " +
                     "이 속성은 컨트롤 높이의 절반보다 커서는 안됩니다.")]
        public int CornerRadius
        {
            get
            {
                return this.cornerRadius;
            }
            set
            {
                this.cornerRadius = value;

                Invalidate();
            }
        }

        #endregion
        #region 배경 이미지 - BackImage

        /// <summary>
        /// 배경 이미지
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(null)]
        [Description("버튼의 배경 이미지로, 이 이미지는 버튼의 기본 색상 위에 그려집니다.")]
        public Image BackImage
        {
            get
            {
                return this.backImage;
            }
            set
            {
                this.backImage = value;

                Invalidate();
            }
        }

        #endregion
        #region 이미지 크기 - ImageSize

        /// <summary>
        /// 이미지 크기
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(typeof(Size), "24, 24")]
        [Description("버튼에 표시할 이미지의 크기입니다. 이 속성의 기본값은 24x24입니다.")]
        public Size ImageSize
        {
            get
            {
                return this.imageSize;
            }
            set
            {
                this.imageSize = value;

                Invalidate();
            }
        }

        #endregion
        #region 이미지 정렬 - ImageAlign

        /// <summary>
        /// 이미지 정렬
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
        [Description("버튼에 관련한 이미지의 정렬 방법입니다.")]
        public ContentAlignment ImageAlign
        {
            get
            {
                return this.imageAlign;
            }
            set
            {
                this.imageAlign = value;

                Invalidate();
            }
        }

        #endregion

        #region 텍스트 이미지 정렬 - ImageTextRelation

        /// <summary>
        /// 이미지 정렬
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(typeof(TextImageRelation), "Overlay")]
        [Description("버튼에 관련한 이미지와 텍스트의 정렬 방법입니다.")]
        public TextImageRelation ImageTextRelation
        {
            get
            {
                return this.textimageAlign;
            }
            set
            {
                this.textimageAlign = value;

                Invalidate();
            }
        }

        #endregion

        #region 이미지 - Image

        /// <summary>
        /// 이미지
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(null)]
        [Description("이미지의 정렬 단추에 표시된 이미지는 단추와 관련하여 텍스트가 모호한 경우 " +
                     "사용자가 기능을 식별하는 데 도움이 됩니다.")]
        public Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;

                Invalidate();
            }
        }

        #endregion
        #region 기본 색상 - BaseColor

        [Category("ImageButton")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("버튼의 나머지 부분이 그려지는 바탕 색상입니다. " +
                     "글래스 효과를 얻으려면 이 속성을 Transparent로 설정하십시오.")]
        public Color BaseColor
        {
            get
            {
                return this.baseColor;
            }
            set
            {
                this.baseColor = value;

                Invalidate();
            }
        }

        #endregion
        #region 버튼 색상 - ButtonColor

        /// <summary>
        /// 버튼 색상
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("기본 색상 위에 그려지는 버튼의 아래쪽 색상입니다.")]
        public Color ButtonColor
        {
            get
            {
                return this.buttonColor;
            }
            set
            {
                this.buttonColor = value;

                Invalidate();
            }
        }

        #endregion
        #region 하이라이트 색상 - HighlightColor

        /// <summary>
        /// 하이라이트 색상
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(typeof(Color), "White")]
        [Description("버튼 맨 위의 강조 표시 색상입니다.")]
        public Color HighlightColor
        {
            get
            {
                return this.highlightColor;
            }
            set
            {
                this.highlightColor = value;

                Invalidate();
            }
        }

        #endregion
        #region GLOW 색상 - GlowColor

        /// <summary>
        /// GLOW 색상
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(typeof(Color), "141,189,255")]
        [Description("마우스가 클라이언트 영역 안에 있을 때 버튼이 빛나는 색입니다.")]
        public Color GlowColor
        {
            get
            {
                return this.glowColor;
            }
            set
            {
                this.glowColor = value;

                Invalidate();
            }
        }

        #endregion
        #region 전경색 - ForeColor

        /// <summary>
        /// 전경색
        /// </summary>
        [Category("ImageButton")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        [Description("텍스트가 그려지는 색 입니다.")]
        public override Color ForeColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                this.foreColor = value;

                Invalidate();
            }
        }

        #endregion
        #region 텍스트 정렬 - TextAlign

        /// <summary>
        /// 텍스트 정렬
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
        [Description("컨트롤에 표시되는 버튼 텍스트의 정렬 방법입니다.")]
        public ContentAlignment TextAlign
        {
            get
            {
                return this.textAlign;
            }
            set
            {
                this.textAlign = value;

                Invalidate();
            }
        }

        #endregion
        #region 버튼 텍스트 - ButtonText

        /// <summary>
        /// 버튼 텍스트
        /// </summary>
        [Category("ImageButton")]
        [Description("버튼에 표시되는 텍스트입니다.")]
        public String ButtonText
        {
            get
            {
                return this.buttonText;
            }
            set
            {
                this.buttonText = value;

                Invalidate();
            }
        }

        #endregion
        #region 대화 상자 결과 - DialogResult

        /// <summary>
        /// 대화 상자 결과
        /// </summary>
        [Category("ImageButton")]
        [DefaultValue(typeof(DialogResult), "DialogResult.None")]
        public DialogResult DialogResult
        {
            get
            {
                return this.dialogResult;
            }
            set
            {
                this.dialogResult = value;

                Invalidate();
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - ImageButton()

        /// <summary>
        /// 생성자
        /// </summary>
        public ImageButton()
        {
            Name = "ImageButton";
            Size = new Size(100, 32);
            BackColor = Color.Transparent;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            this.fadeInTimer = new Timer();

            this.fadeInTimer.Interval = 30;

            this.fadeOutTimer = new Timer();

            this.fadeOutTimer.Interval = 30;

            Resize += UserControl_Resize;
            Paint += UserControl_Paint;
            KeyUp += UserControl_KeyUp;
            KeyDown += UserControl_KeyDown;
            MouseEnter += UserControl_MouseEnter;
            MouseDown += UserControl_MouseDown;
            MouseUp += UserControl_MouseUp;
            MouseLeave += UserControl_MouseLeave;
            GotFocus += UserControl_MouseEnter;
            LostFocus += UserControl_MouseLeave;
            this.fadeInTimer.Tick += fadeInTimer_Tick;
            this.fadeOutTimer.Tick += fadeOutTimer_Tick;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Protected

        #region 리소스 해제하기 - Dispose(disposing)

        /// <summary>
        /// 리소스 해제하기
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.container != null)
                {
                    this.container.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Private
        //////////////////////////////////////////////////////////////////////////////// Event

        #region 사용자 컨트롤 크기 변경시 처리하기 - UserControl_Resize(sender, e)

        /// <summary>
        /// 사용자 컨트롤 크기 변경시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_Resize(object sender, EventArgs e)
        {
            Rectangle rectangle = ClientRectangle;

            rectangle.X -= 1;
            rectangle.Y -= 1;

            rectangle.Width += 2;
            rectangle.Height += 2;

            using (GraphicsPath graphicsPath = GetRoundRectangleGraphicsPath(rectangle, CornerRadius, CornerRadius, CornerRadius, CornerRadius))
            {
                Region = new Region(graphicsPath);
            }
        }

        #endregion
        #region 사용자 컨트롤 페인트시 처리하기 - UserControl_Paint(sender, e)

        /// <summary>
        /// 사용자 컨트롤 페인트시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            DrawBackground(e.Graphics);
            DrawHighlight(e.Graphics);
            DrawImage(e.Graphics);
            DrawText(e.Graphics);
            DrawGlow(e.Graphics);
            DrawOuterBorder(e.Graphics);
            DrawInnerBorder(e.Graphics);
        }

        #endregion
        #region 사용자 컨트롤 키 UP시 처리하기 - UserControl_KeyUp(sender, e)

        /// <summary>
        /// 사용자 컨트롤 키 UP시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!this.isClicked)
                {
                    return;
                }

                MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0);

                this.calledByKey = true;

                UserControl_MouseUp(sender, mouseEventArgs);
            }
        }

        #endregion
        #region 사용자 컨트롤 키 DOWN시 처리하기 - UserControl_KeyDown(sender, e)

        /// <summary>
        /// 사용자 컨트롤 키 DOWN시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0);

                UserControl_MouseDown(sender, mouseEventArgs);
            }
        }

        #endregion
        #region 사용자 컨트롤 마우스 진입시 처리하기 - UserControl_MouseEnter(sender, e)

        /// <summary>
        /// 사용자 컨트롤 마우스 진입시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_MouseEnter(object sender, EventArgs e)
        {
            this.buttonState = ImageButtonState.Hover;

            this.fadeOutTimer.Stop();

            this.fadeInTimer.Start();
        }

        #endregion
        #region 사용자 컨트롤 마우스 이탈시 처리하기 - UserControl_MouseLeave(sender, e)

        /// <summary>
        /// 사용자 컨트롤 마우스 이탈시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_MouseLeave(object sender, EventArgs e)
        {
            this.buttonState = ImageButtonState.None;

            if (this.buttonStyle == ImageButtonStyle.Flat)
            {
                this.glowAlpha = 0;
            }

            this.fadeInTimer.Stop();

            this.fadeOutTimer.Start();

            this.isClicked = false;
        }

        #endregion
        #region 사용자 컨트롤 마우스 DOWN시 처리하기 - UserControl_MouseDown(sender, e)

        /// <summary>
        /// 사용자 컨트롤 마우스 DOWN시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.isClicked)
                {
                    return;
                }

                this.isClicked = true;

                this.buttonState = ImageButtonState.Pressed;

                if (this.buttonStyle != ImageButtonStyle.Flat)
                {
                    this.glowAlpha = 255;
                }

                this.fadeInTimer.Stop();

                this.fadeOutTimer.Stop();

                Invalidate();
            }
        }

        #endregion
        #region 사용자 컨트롤 마우스 UP시 처리하기 - UserControl_MouseUp(sender, e)

        /// <summary>
        /// 사용자 컨트롤 마우스 UP시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void UserControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.buttonState = ImageButtonState.Hover;

                this.fadeInTimer.Stop();

                this.fadeOutTimer.Stop();

                Invalidate();

                if (this.calledByKey == true)
                {
                    OnClick(EventArgs.Empty);

                    this.calledByKey = false;
                }

                this.isClicked = false;
            }
        }

        #endregion

        #region FADE IN 타이머 틱 처리하기 - fadeInTimer_Tick(sender, e)

        /// <summary>
        /// FADE IN 타이머 틱 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void fadeInTimer_Tick(object sender, EventArgs e)
        {
            if (this.buttonStyle == ImageButtonStyle.Flat)
            {
                this.glowAlpha = 0;
            }

            if (this.glowAlpha + 30 >= 255)
            {
                this.glowAlpha = 255;

                this.fadeInTimer.Stop();
            }
            else
            {
                this.glowAlpha += 30;
            }

            Invalidate();
        }

        #endregion
        #region FADE OUT 타이머 틱 처리하기 - fadeOutTimer_Tick(sender, e)

        /// <summary>
        /// FADE OUT 타이머 틱 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void fadeOutTimer_Tick(object sender, EventArgs e)
        {
            if (this.buttonStyle == ImageButtonStyle.Flat)
            {
                this.glowAlpha = 0;
            }

            if (this.glowAlpha - 30 <= 0)
            {
                this.glowAlpha = 0;

                this.fadeOutTimer.Stop();
            }
            else
            {
                this.glowAlpha -= 30;
            }

            this.Invalidate();
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////// Function

        #region 둥근 사각형 그래픽스 패스 구하기 - GetRoundRectangleGraphicsPath(rectangle, r1, r2, r3, r4)

        /// <summary>
        /// 둥근 사각형 그래픽스 패스 구하기
        /// </summary>
        /// <param name="rectangle">사각형</param>
        /// <param name="r1">R1</param>
        /// <param name="r2">R2</param>
        /// <param name="r3">R3</param>
        /// <param name="r4">R4</param>
        /// <returns>둥근 사각형 그래픽스 패스</returns>
        private GraphicsPath GetRoundRectangleGraphicsPath(RectangleF rectangle, float r1, float r2, float r3, float r4)
        {
            float x = rectangle.X;
            float y = rectangle.Y;
            float width = rectangle.Width;
            float height = rectangle.Height;

            GraphicsPath graphicsPath = new GraphicsPath();

            graphicsPath.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
            graphicsPath.AddLine(x + r1, y, x + width - r2, y);
            graphicsPath.AddBezier(x + width - r2, y, x + width, y, x + width, y + r2, x + width, y + r2);
            graphicsPath.AddLine(x + width, y + r2, x + width, y + height - r3);
            graphicsPath.AddBezier(x + width, y + height - r3, x + width, y + height, x + width - r3, y + height, x + width - r3, y + height);
            graphicsPath.AddLine(x + width - r3, y + height, x + r4, y + height);
            graphicsPath.AddBezier(x + r4, y + height, x, y + height, x, y + height - r4, x, y + height - r4);
            graphicsPath.AddLine(x, y + height - r4, x, y + r1);

            return graphicsPath;
        }

        #endregion
        #region 문자열 포맷 구하기 - GetStringFormat(textAlignment)

        /// <summary>
        /// 문자열 포맷 구하기
        /// </summary>
        /// <param name="textAlignment">텍스트 정렬</param>
        /// <returns>문자열 포맷</returns>
        private StringFormat GetStringFormat(ContentAlignment textAlignment)
        {
            StringFormat stringFormat = new StringFormat();

            switch (textAlignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:

                    stringFormat.LineAlignment = StringAlignment.Near;

                    break;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:

                    stringFormat.LineAlignment = StringAlignment.Center;

                    break;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:

                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
            }

            switch (textAlignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:

                    stringFormat.Alignment = StringAlignment.Near;

                    break;

                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:

                    stringFormat.Alignment = StringAlignment.Center;

                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:

                    stringFormat.Alignment = StringAlignment.Far;

                    break;
            }

            return stringFormat;
        }

        #endregion
        #region 클리핑 영역 설정하기 - SetClippingArea(graphics)

        /// <summary>
        /// 클리핑 영역 설정하기
        /// </summary>
        /// <param name="graphics">그래픽스</param>
        private void SetClippingArea(Graphics graphics)
        {
            Rectangle rectangle = ClientRectangle;

            rectangle.X++;
            rectangle.Y++;

            rectangle.Width -= 3;
            rectangle.Height -= 3;

            using (GraphicsPath graphicsPath = GetRoundRectangleGraphicsPath(rectangle, CornerRadius, CornerRadius, CornerRadius, CornerRadius))
            {
                graphics.SetClip(graphicsPath);
            }
        }

        #endregion

        #region 외부 테두리 그리기 - DrawOuterBorder(graphics)

        /// <summary>
        /// 외부 테두리 그리기
        /// </summary>
        /// <param name="graphics">그래픽스</param>
        private void DrawOuterBorder(Graphics graphics)
        {
            if (this.buttonStyle == ImageButtonStyle.Flat && this.buttonState == ImageButtonState.None)
            {
                return;
            }

            Rectangle rectangle = ClientRectangle;

            rectangle.Width -= 1;
            rectangle.Height -= 1;

            using (GraphicsPath graphicsPath = GetRoundRectangleGraphicsPath(rectangle, CornerRadius, CornerRadius, CornerRadius, CornerRadius))
            {
                using (Pen pen = new Pen(this.buttonColor))
                {
                    graphics.DrawPath(pen, graphicsPath);
                }
            }
        }

        #endregion
        #region 내부 테두리 그리기 - DrawInnerBorder(graphics)

        /// <summary>
        /// 내부 테두리 그리기
        /// </summary>
        /// <param name="graphics">그래픽스</param>
        private void DrawInnerBorder(Graphics graphics)
        {
            if (this.buttonStyle == ImageButtonStyle.Flat && this.buttonState == ImageButtonState.None)
            {
                return;
            }

            Rectangle rectangle = ClientRectangle;

            rectangle.X++;
            rectangle.Y++;

            rectangle.Width -= 3;
            rectangle.Height -= 3;

            using (GraphicsPath graphicsPath = GetRoundRectangleGraphicsPath(rectangle, CornerRadius, CornerRadius, CornerRadius, CornerRadius))
            {
                using (Pen pen = new Pen(this.highlightColor))
                {
                    graphics.DrawPath(pen, graphicsPath);
                }
            }
        }

        #endregion
        #region 배경 그리기 - DrawBackground(graphics)

        /// <summary>
        /// 배경 그리기
        /// </summary>
        /// <param name="graphics">그래픽스</param>
        private void DrawBackground(Graphics graphics)
        {
            if (this.buttonStyle == ImageButtonStyle.Flat && this.buttonState == ImageButtonState.None)
            {
                return;
            }

            int alpha = (this.buttonState == ImageButtonState.Pressed) ? 204 : 127;

            Rectangle rectangle = ClientRectangle;

            rectangle.Width--;
            rectangle.Height--;

            using (GraphicsPath graphicsPath = GetRoundRectangleGraphicsPath(rectangle, CornerRadius, CornerRadius, CornerRadius, CornerRadius))
            {
                using (SolidBrush solidBrush = new SolidBrush(this.baseColor))
                {
                    graphics.FillPath(solidBrush, graphicsPath);
                }

                SetClippingArea(graphics);

                if (this.backImage != null)
                {
                    graphics.DrawImage(this.backImage, ClientRectangle);
                }

                graphics.ResetClip();

                using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(alpha, this.ButtonColor)))
                {
                    graphics.FillPath(solidBrush, graphicsPath);
                }
            }
        }

        #endregion
        #region 하이라이트 그리기 - DrawHighlight(graphics)

        /// <summary>
        /// 하이라이트 그리기
        /// </summary>
        /// <param name="graphics">그래픽스</param>
        private void DrawHighlight(Graphics graphics)
        {
            if (this.buttonStyle == ImageButtonStyle.Flat && this.buttonState == ImageButtonState.None)
            {
                return;
            }

            int alpha = (this.buttonState == ImageButtonState.Pressed) ? 60 : 150;

            Rectangle rectangle = new Rectangle(0, 0, Width, Height / 2);

            GraphicsPath graphicsPath = GetRoundRectangleGraphicsPath(rectangle, CornerRadius, CornerRadius, 0, 0);

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush
            (
                graphicsPath.GetBounds(),
                Color.FromArgb(alpha,
                this.HighlightColor),
                Color.FromArgb(alpha / 3, this.HighlightColor),
                LinearGradientMode.Vertical
            );

            graphics.FillPath(linearGradientBrush, graphicsPath);

            graphicsPath.Dispose();
            linearGradientBrush.Dispose();
        }

        #endregion
        #region GLOW 그리기 - DrawGlow(graphics)

        /// <summary>
        /// GLOW 그리기
        /// </summary>
        /// <param name="graphics">그래픽스</param>
        private void DrawGlow(Graphics graphics)
        {
            if (this.buttonState == ImageButtonState.Pressed || this.buttonState == ImageButtonState.None)
            {
                return;
            }

            SetClippingArea(graphics);

            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddEllipse(-5, this.Height / 2 - 10, Width + 11, Height + 11);
                //graphicsPath.AddEllipse(-5, this.Height / 2 - 10, Width + 21, Height + 11);

                using (PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath))
                {
                    pathGradientBrush.CenterColor = Color.FromArgb(this.glowAlpha, this.glowColor);
                    pathGradientBrush.SurroundColors = new Color[] { Color.FromArgb(0, this.glowColor) };

                    graphics.FillPath(pathGradientBrush, graphicsPath);
                }
            }

            graphics.ResetClip();
        }

        #endregion
        #region 텍스트 그리기 - DrawText(graphics)

        /// <summary>
        /// 텍스트 그리기
        /// </summary>
        /// <param name="graphics">그래픽스</param>
        private void DrawText(Graphics graphics)
        {
            StringFormat stringFormat = GetStringFormat(this.textAlign);

            Rectangle rectangle;

            switch (this.TextAlign)
            {

                case ContentAlignment.MiddleCenter:
                    if (this.image == null)
                        rectangle = new Rectangle(8, 8, Width - 17, Height - 17);
                    else rectangle = new Rectangle(8, 8, Width + 15, Height - 17);
                    break;
                case ContentAlignment.BottomCenter:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomRight:
                default:
                    rectangle = new Rectangle(8, 8, Width - 17, Height - 17);
                    break;
            }

            graphics.DrawString(this.buttonText, Font, new SolidBrush(this.foreColor), rectangle, stringFormat);
            //float FWidth = (this.Font.SizeInPoints / 72) * graphics.DpiX;
            //graphics.DrawString(FWidth.ToString(), Font, new SolidBrush(this.foreColor), rectangle, stringFormat);
        }

        #endregion
        #region 이미지 그리기 - DrawImage(graphics)

        /// <summary>
        /// 이미지 그리기
        /// </summary>
        /// <param name="graphics">그래픽스</param>
        private void DrawImage(Graphics graphics)
        {
            if (this.image == null)
            {
                return;
            }
            
            Rectangle rectangle = new Rectangle(8, 8, this.imageSize.Width, this.imageSize.Height);

            float FWidth = (this.Font.SizeInPoints / 72) * graphics.DpiX;
            int FHeight = this.Font.Height;
            //실제 버튼 문자열 사이즈를 계산한다.
            SizeF stringSize = graphics.MeasureString(this.ButtonText, this.Font);
            int X;

            switch (this.ImageAlign)
            {
                case ContentAlignment.TopCenter:
                    rectangle = new Rectangle(Width / 2 - this.imageSize.Width / 2, 8, this.imageSize.Width, this.imageSize.Height);
                    break;
                case ContentAlignment.TopRight:
                    rectangle = new Rectangle(Width - 8 - this.imageSize.Width, 8, this.imageSize.Width, this.imageSize.Height);
                    break;
                case ContentAlignment.MiddleLeft:
                    rectangle = new Rectangle(8, Height / 2 - this.imageSize.Height / 2, this.imageSize.Width, this.imageSize.Height);
                    break;
                case ContentAlignment.MiddleCenter:
                    
                    X = (int)((Width / 2) - ((stringSize.Width / 2) + (this.imageSize.Width / 2)));
                    if (X < 8) X = 8;
                    rectangle = new Rectangle(X, (Height / 2) - (this.imageSize.Height / 2), this.imageSize.Width, this.imageSize.Height);
                    //rectangle = new Rectangle(50, Height / 2 - this.imageSize.Height / 2, this.imageSize.Width, this.imageSize.Height);
                    break;
                case ContentAlignment.MiddleRight:
                    rectangle = new Rectangle(Width - 8 - this.imageSize.Width, Height / 2 - this.imageSize.Height / 2, this.imageSize.Width, this.imageSize.Height);
                    break;
                case ContentAlignment.BottomLeft:
                    rectangle = new Rectangle(8, Height - 8 - this.imageSize.Height, this.imageSize.Width, this.imageSize.Height);
                    break;
                case ContentAlignment.BottomCenter:
                    rectangle = new Rectangle(Width / 2 - this.imageSize.Width / 2, Height - 8 - this.imageSize.Height, this.imageSize.Width, this.imageSize.Height);
                    break;
                case ContentAlignment.BottomRight:
                    rectangle = new Rectangle(Width - 8 - this.imageSize.Width, Height - 8 - this.imageSize.Height, this.imageSize.Width, this.imageSize.Height);
                    break;
            }

            graphics.DrawImage(this.image, rectangle);
        }

        #endregion
    }
}