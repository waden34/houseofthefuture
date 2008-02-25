using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Lighting_Interface
{
    public partial class Buttons : UserControl
    {
        Orient orient;
        int angle;
        Image image;
        Font font;
        string caption;
        bool moving;
        public Buttons()
        {
            InitializeComponent();
            this.MouseEnter += new EventHandler(Buttons_MouseEnter);
            this.MouseLeave += new EventHandler(Buttons_MouseLeave);
            this.MouseDown += new MouseEventHandler(Buttons_MouseDown);
            this.MouseUp += new MouseEventHandler(Buttons_MouseUp);
            font = new Font("Cooper Black", 12, FontStyle.Regular);
            image = Properties.Resources.button;
            this.Resize += new EventHandler(Buttons_Resize);
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.Clear(this.BackColor);
            g.DrawImage(image, 0, 0, this.Width, this.Height);
            g.RotateTransform(angle);
            g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);

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

            Bitmap bmp;
            if (orient == Orient.Tall)
            {
                bmp = new Bitmap(image, this.Height, this.Width);
                image = bmp;
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else
            {
                bmp = new Bitmap(image, this.Width, this.Height);
                image = bmp;
            }
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.Clear(this.BackColor);
            g.DrawImage(image, 0, 0, this.Width, this.Height);
            g.RotateTransform(angle);
            g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);


        }

 
      


        void lblCaption_FontChanged(object sender, EventArgs e)
        {
            caption = caption + " ";
            caption = caption.Substring(0, caption.Length - 1);
            Graphics g = Graphics.FromHwnd(this.Handle);
            g.Clear(this.BackColor);
            g.DrawImage(image, 0, 0, this.Width, this.Height);
            g.RotateTransform(angle);
            g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);

        }



        public virtual void Buttons_MouseUp(object sender, MouseEventArgs e)
        {
            if (!moving)
            {
                image = Properties.Resources.button_over;
                if (orient == Orient.Tall)
                {
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if (orient == Orient.Round)
                {
                    image = Properties.Resources.round_button_over;
                }
                Graphics g = Graphics.FromHwnd(this.Handle);
                g.Clear(this.BackColor);
                g.DrawImage(image, 0, 0, this.Width, this.Height);
                g.RotateTransform(angle);
                g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
            }
        }

        protected virtual void Buttons_MouseDown(object sender, MouseEventArgs e)
        {
            if (!moving)
            {
                image = Properties.Resources.button_click;
                if (orient == Orient.Tall)
                {
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if (orient == Orient.Round)
                {
                    image = Properties.Resources.round_button_click;
                }
                Graphics g = Graphics.FromHwnd(this.Handle);
                g.Clear(this.BackColor);
                g.DrawImage(image, 0, 0, this.Width, this.Height);
                g.RotateTransform(angle);
                g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
            }
        }

        void Buttons_MouseLeave(object sender, EventArgs e)
        {
            if (!moving)
            {
                image = Properties.Resources.button;
                if (orient == Orient.Tall)
                {
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if (orient == Orient.Round)
                {
                    image = Properties.Resources.round_button;
                }
                Graphics g = Graphics.FromHwnd(this.Handle);
                g.Clear(this.BackColor);
                g.DrawImage(image, 0, 0, this.Width, this.Height);
                g.RotateTransform(angle);
                g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
            }
        }

        void Buttons_MouseEnter(object sender, EventArgs e)
        {
            if (!moving)
            {
                image = Properties.Resources.button_over;
                if (orient == Orient.Tall)
                {
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                else if (orient == Orient.Round)
                {
                    image = Properties.Resources.round_button_over;
                }
                Graphics g = Graphics.FromHwnd(this.Handle);
                g.Clear(this.BackColor);
                g.DrawImage(image, 0, 0, this.Width, this.Height);
                g.RotateTransform(angle);
                g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
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
                g.DrawString(caption, font, Brushes.White, (this.Width - g.MeasureString(caption, font).Width) / 2, (this.Height - g.MeasureString(caption, font).Height) / 2, StringFormat.GenericTypographic);
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
        public Orient Orientation
        {
            get
            {
                return orient;
            }
            set
            {
                orient = value;
                switch (orient)
                {
                    case Orient.Tall:
                        image = Properties.Resources.button;
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        int w = this.Width;
                        int h = this.Height;
                        this.Width = h;
                        this.Height = w;
                        angle = 90;
                        break;
                    case Orient.Long:
                        image = Properties.Resources.button;
                        angle = 0;
                        break;
                    case Orient.Round:
                        image = Properties.Resources.round_button;
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
        public enum Orient
        {
            Tall, Long, Round, Triangle
        };
        
    }
}
