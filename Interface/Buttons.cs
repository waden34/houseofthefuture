using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace HouseOfTheFuture
{
    public partial class Buttons : UserControl
    {
        ButtonType buttonType;
        int angle;
        Image image;
        Font font;
        string caption;
        bool moving;
        ButtonState state;

        public Buttons()
        {
            InitializeComponent();
            state = ButtonState.Normal;
            this.MouseEnter += new EventHandler(Buttons_MouseEnter);
            this.MouseLeave += new EventHandler(Buttons_MouseLeave);
            this.MouseDown += new MouseEventHandler(Buttons_MouseDown);
            this.MouseUp += new MouseEventHandler(Buttons_MouseUp);
            font = new Font("Cooper Black", 12, FontStyle.Regular);
            this.Paint += new PaintEventHandler(Buttons_Paint);
            switch (buttonType)
            {
                case ButtonType.Horizontal:
                    image = Properties.Resources.button;
                    break;
                case ButtonType.Round:
                    image = Properties.Resources.round_button;
                    break;
                case ButtonType.Verticle:
                    image = Properties.Resources.button_tall;
                    break;
                case ButtonType.TriangleDown:
                    image = Properties.Resources.button_triangle_down;
                    break;
                case ButtonType.TriangleLeft:
                    image = Properties.Resources.button_triangle_left;
                    break;
                case ButtonType.TriangleRight:
                    image = Properties.Resources.button_triangle_right;
                    break;
                case ButtonType.TriangleUp:
                    image = Properties.Resources.button_triangle;
                    break;
                default:
                    image = Properties.Resources.button;
                    break;

            }
            if (caption == null)
            {
                caption = " ";
            }
            this.Resize += new EventHandler(Buttons_Resize);
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.Clear(this.BackColor);
            g.DrawImage(image, 0, 0, this.Width, this.Height);
            g.RotateTransform(angle);
            g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);

        }

        void Buttons_Paint(object sender, PaintEventArgs e)
        {
            if (buttonType == ButtonType.Verticle)
            {
                switch (state)
                {
                    case ButtonState.Normal:
                        image = Properties.Resources.button_tall;
                        break;
                    case ButtonState.Click:
                        image = Properties.Resources.button_tall_click;
                        break;
                    case ButtonState.Hover:
                        image = Properties.Resources.button_tall_over;
                        break;
                    default:
                        image = Properties.Resources.button_tall;
                        break;
                }
                
            
            }
            else if (buttonType == ButtonType.Round)
            {
                switch (state)
                {
                    case ButtonState.Normal:
                        image = Properties.Resources.round_button;
                        break;
                    case ButtonState.Click:
                        image = Properties.Resources.round_button_click;
                        break;
                    case ButtonState.Hover:
                        image = Properties.Resources.round_button_over;
                        break;
                    default:
                        image = Properties.Resources.round_button;
                        break;
                }
            }
            else if (buttonType == ButtonType.TriangleDown)
            {
                switch (state)
                {
                    case ButtonState.Normal:
                        image = Properties.Resources.button_triangle_down;
                        break;
                    case ButtonState.Click:
                        image = Properties.Resources.button_triangle_down_click;
                        break;
                    case ButtonState.Hover:
                        image = Properties.Resources.button_triangle_down_over;
                        break;
                    default:
                        image = Properties.Resources.button_triangle_down;
                        break;
                }
            }
            else if (buttonType == ButtonType.TriangleLeft)
            {
                switch (state)
                {
                    case ButtonState.Normal:
                        image = Properties.Resources.button_triangle_left;
                        break;
                    case ButtonState.Click:
                        image = Properties.Resources.button_triangle_left_click;
                        break;
                    case ButtonState.Hover:
                        image = Properties.Resources.button_triangle_left_over;
                        break;
                    default:
                        image = Properties.Resources.button_triangle_left;
                        break;
                }
            }
            else if (buttonType == ButtonType.TriangleRight)
            {
                switch (state)
                {
                    case ButtonState.Normal:
                        image = Properties.Resources.button_triangle_right;
                        break;
                    case ButtonState.Click:
                        image = Properties.Resources.button_triangle_right_click;
                        break;
                    case ButtonState.Hover:
                        image = Properties.Resources.button_triangle_right_over;
                        break;
                    default:
                        image = Properties.Resources.button_triangle_right;
                        break;
                }
            }
            else if (buttonType == ButtonType.TriangleUp)
            {
                switch (state)
                {
                    case ButtonState.Normal:
                        image = Properties.Resources.button_triangle;
                        break;
                    case ButtonState.Click:
                        image = Properties.Resources.button_triangle_click;
                        break;
                    case ButtonState.Hover:
                        image = Properties.Resources.button_triangle_over;
                        break;
                    default:
                        image = Properties.Resources.button_triangle;
                        break;
                }
            }
            else
            {
                switch (state)
                {
                    case ButtonState.Normal:
                        image = Properties.Resources.button;
                        break;
                    case ButtonState.Click:
                        image = Properties.Resources.button_click;
                        break;
                    case ButtonState.Hover:
                        image = Properties.Resources.button_over;
                        break;
                    default:
                        image = Properties.Resources.button;
                        break;
                }
            }
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.Clear(this.BackColor);
            g.DrawImage(image, 0, 0, this.Width, this.Height);
            g.RotateTransform(angle);

            if (g.MeasureString(caption, font).Height > this.Height)
            {
                font = new Font(font.FontFamily, (this.Height / 2));
            }
            else if (g.MeasureString(caption, font).Width > this.Width)
            {
                font = new Font(font.FontFamily, (this.Width / caption.Length));
            }
            else
            {
                if ((this.Height / 2) > this.Width / caption.Length)
                {
                    font = new Font(font.FontFamily, (this.Width / caption.Length));
                }
                else
                {
                    font = new Font(font.FontFamily, (this.Height / 2));
                }
            }
            g.DrawString(caption, font, Brushes.White, ((this.Width - (g.MeasureString(caption, font).Width - (font.Size / 2))) / 2), ((this.Height - g.MeasureString(caption, font).Height) / 2), StringFormat.GenericTypographic);


        }
        public bool isMoving
        {
            set
            {
                moving = value;
            }
        }
        void Buttons_Resize(object sender, EventArgs e)
        {
            Buttons_Paint(null, null);
            
        }

 
      


        void lblCaption_FontChanged(object sender, EventArgs e)
        {
            caption = caption + " ";
            caption = caption.Substring(0, caption.Length - 1);
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.Clear(this.BackColor);
            g.DrawImage(image, 0, 0, this.Width, this.Height);
            g.RotateTransform(angle);
            g.DrawString(caption, font, Brushes.White, (this.Width - (g.MeasureString(caption, font).Width - (font.Size / 2))) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);

        }



        public virtual void Buttons_MouseUp(object sender, MouseEventArgs e)
        {

            state = ButtonState.Hover;
                if (!moving)
                {
                    
                    if (buttonType == ButtonType.Verticle)
                    {
                        image = Properties.Resources.button_tall_over;
                    }
                    else if (buttonType == ButtonType.Round)
                    {
                        image = Properties.Resources.round_button_over;
                    }
                    else if (buttonType == ButtonType.TriangleDown)
                    {
                        image = Properties.Resources.button_triangle_down_over;
                    }
                    else if (buttonType == ButtonType.TriangleLeft)
                    {
                        image = Properties.Resources.button_triangle_left_over;
                    }
                    else if (buttonType == ButtonType.TriangleRight)
                    {
                        image = Properties.Resources.button_triangle_right_over;
                    }
                    else if (buttonType == ButtonType.TriangleUp)
                    {
                        image = Properties.Resources.button_triangle_over;
                    }
                    else
                    {
                        image = Properties.Resources.button_over;
                    }
                    Graphics g = Graphics.FromHwnd(this.Handle);
                    g.Clear(this.BackColor);
                    g.DrawImage(image, 0, 0, this.Width, this.Height);
                    g.RotateTransform(angle);
                    g.DrawString(caption, font, Brushes.LightGray, (this.Width - (g.MeasureString(caption, font).Width - (font.Size / 2))) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
                }
            
        }

        protected virtual void Buttons_MouseDown(object sender, MouseEventArgs e)
        {
            state = ButtonState.Click;
                if (!moving)
                {
                   
                    if (buttonType == ButtonType.Verticle)
                    {
                        image = Properties.Resources.button_tall_click;
                    }
                    else if (buttonType == ButtonType.Round)
                    {
                        image = Properties.Resources.round_button_click;
                    }
                    else if (buttonType == ButtonType.TriangleDown)
                    {
                        image = Properties.Resources.button_triangle_down_click;
                    }
                    else if (buttonType == ButtonType.TriangleLeft)
                    {
                        image = Properties.Resources.button_triangle_left_click;
                    }
                    else if (buttonType == ButtonType.TriangleRight)
                    {
                        image = Properties.Resources.button_triangle_right_click;
                    }
                    else if (buttonType == ButtonType.TriangleUp)
                    {
                        image = Properties.Resources.button_triangle_click;
                    }
                    else
                    {
                        image = Properties.Resources.button_click;
                    }
                    Graphics g = Graphics.FromHwnd(this.Handle);
                    g.Clear(this.BackColor);
                    g.DrawImage(image, 0, 0, this.Width, this.Height);
                    g.RotateTransform(angle);
                    g.DrawString(caption, font, Brushes.DimGray, (this.Width - (g.MeasureString(caption, font).Width - (font.Size / 2))) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
                }
            
        }

        void Buttons_MouseLeave(object sender, EventArgs e)
        {
            state = ButtonState.Normal;
                if (!moving)
                {
                    
                    if (buttonType == ButtonType.Verticle)
                    {
                        image = Properties.Resources.button_tall;
                    }
                    else if (buttonType == ButtonType.Round)
                    {
                        image = Properties.Resources.round_button;
                    }
                    else if (buttonType == ButtonType.TriangleDown)
                    {
                        image = Properties.Resources.button_triangle_down;
                    }
                    else if (buttonType == ButtonType.TriangleLeft)
                    {
                        image = Properties.Resources.button_triangle_left;
                    }
                    else if (buttonType == ButtonType.TriangleRight)
                    {
                        image = Properties.Resources.button_triangle_right;
                    }
                    else if (buttonType == ButtonType.TriangleUp)
                    {
                        image = Properties.Resources.button_triangle;
                    }
                    else
                    {
                        image = Properties.Resources.button;
                    }
                    Graphics g = Graphics.FromHwnd(this.Handle);
                    g.Clear(this.BackColor);
                    g.DrawImage(image, 0, 0, this.Width, this.Height);
                    g.RotateTransform(angle);
                    g.DrawString(caption, font, Brushes.White, (this.Width - (g.MeasureString(caption, font).Width - (font.Size / 2))) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
                }
            
        }

        void Buttons_MouseEnter(object sender, EventArgs e)
        {
            state = ButtonState.Hover;
                if (!moving)
                {
                    
                    if (buttonType == ButtonType.Verticle)
                    {
                        image = Properties.Resources.button_tall_over;
                    }
                    else if (buttonType == ButtonType.Round)
                    {
                        image = Properties.Resources.round_button_over;
                    }
                    else if (buttonType == ButtonType.TriangleDown)
                    {
                        image = Properties.Resources.button_triangle_down_over;
                    }
                    else if (buttonType == ButtonType.TriangleLeft)
                    {
                        image = Properties.Resources.button_triangle_left_over;
                    }
                    else if (buttonType == ButtonType.TriangleRight)
                    {
                        image = Properties.Resources.button_triangle_right_over;
                    }
                    else if (buttonType == ButtonType.TriangleUp)
                    {
                        image = Properties.Resources.button_triangle_over;
                    }
                    else
                    {
                        image = Properties.Resources.button_over;
                    }
                    Graphics g = Graphics.FromHwnd(this.Handle);
                    g.Clear(this.BackColor);
                    g.DrawImage(image, 0, 0, this.Width, this.Height);
                    g.RotateTransform(angle);
                    g.DrawString(caption, font, Brushes.LightGray, (this.Width - (g.MeasureString(caption, font).Width - (font.Size / 2))) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
                }
            
        }
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
                Graphics g = Graphics.FromHwnd(this.Handle);
                g.Clear(this.BackColor);
                g.DrawImage(image, 0, 0, this.Width, this.Height);
                g.RotateTransform(angle);
                g.DrawString(caption, font, Brushes.White, (this.Width - (g.MeasureString(caption, font).Width - (font.Size / 2))) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
            }
        }
        public Font CaptionFont
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
            }
        }
        public ButtonType Type
        {
            get
            {
                return buttonType;
            }
            set
            {
                buttonType = value;
                switch (buttonType)
                {
                    case ButtonType.Verticle:
                        image = Properties.Resources.button_tall;
                        angle = 180;
                        break;
                    case ButtonType.Horizontal:
                        image = Properties.Resources.button;
                        angle = 0;
                        break;
                    case ButtonType.Round:
                        image = Properties.Resources.round_button;
                        angle = 0;
                        break;
                    case ButtonType.TriangleDown:
                        image = Properties.Resources.button_triangle_down;
                        angle = 0;
                        break;
                    case ButtonType.TriangleLeft:
                        image = Properties.Resources.button_triangle_left;
                        angle = 0;
                        break;
                    case ButtonType.TriangleRight:
                        image = Properties.Resources.button_triangle_right;
                        angle = 0;
                        break;
                    case ButtonType.TriangleUp:
                        image = Properties.Resources.button_triangle;
                        angle = 0;
                        break;
                    default:
                        angle = 0;
                        break;
                }
                Graphics g = Graphics.FromHwnd(this.Handle);
                g.Clear(this.BackColor);
                g.DrawImage(image, 0, 0, this.Width, this.Height);
                g.RotateTransform(angle);
                g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
            }
        }
        public enum ButtonType
        {
            Verticle, 
            Horizontal, 
            Round, 
            TriangleUp,
            TriangleRight,
            TriangleDown,
            TriangleLeft,
            Square
        };
        enum ButtonState
        {
            Normal,
            Hover,
            Click
        }
        
    }
}
