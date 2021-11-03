using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion {
    class PieChart {

        Graphics _g;
        List<String> _textValues;
        List<double> _numbersValues;
        int _position_X;
        int _position_Y;
        int _pieSize;
        Size _chartSize;
        Brush[] _brushes;
        List<Brush> _randomBrushes;
        Font _font;
        Color _backColor;
        List<Color> _generatedColors;

        public PieChart() {
            this._textValues = new List<String>();
            this._numbersValues = new List<double>();
            this._position_X = 0;
            this._position_Y = 0;
            this._pieSize = 0;
            this._chartSize = new Size();
            this._backColor = Color.White;
            this._brushes = null;
            this._font = null; 
            this._generatedColors = new List<Color>();
            this._randomBrushes = GenerateBurshList(Color.Black);
            

        }

        // Constructor without font & brushes
        public PieChart(Graphics g, List<String> textValues, List<double> numbersValues, int position_x, 
            int position_y, Size chartSize, Color backColor) {
            this._g = g;
            this._textValues = textValues;
            this._numbersValues = numbersValues;
            this._position_X = position_x;
            this._position_Y = position_y;
            this._pieSize = 0;
            this._chartSize = chartSize;
            this._backColor = backColor;
            this._brushes = null;
            this._font = null;
            this._generatedColors = new List<Color>();
            this._randomBrushes = GenerateBurshList(Color.Black);
            
        }

        public Graphics Graph {
            get { return _g; }
            set { _g = value; }
        }

        public List<String> TextValues {
            get { return _textValues; }
            set { _textValues = value; }
        }

        public List<double> NumbersValues {
            get { return _numbersValues; }
            set { _numbersValues = value; }
        }

        public int Position_X {
            get { return _position_X; }
            set { _position_X = value; }
        }

        public int Position_Y {
            get { return _position_Y; }
            set { _position_Y = value; }
        }

        public Size ChartSize {
            get { return _chartSize; }
            set { _chartSize = value; }
        }

        public Color BackColor {
            get { return _backColor; }
            set { _backColor = value; }
        }


        // Print the piechart
        public void PrintPieChart() {

            CleanScreen(_backColor);

            // If user do not provide font, use the default
            if (this._font == null) {
                this._font = new Font("Times New Roman", 10);
            }
            
            if (_chartSize.Width < _chartSize.Height) {
                _pieSize = (int)(_chartSize.Width - (_chartSize.Width * 0.2));
            } else {
                _pieSize = (int)(_chartSize.Height - (_chartSize.Height * 0.2));
            }

            int freeSpaceX = (_chartSize.Width - _pieSize) / 4;
            int freeSpaceY = (_chartSize.Height - _pieSize) / 2;

            this._g.FillRectangle(Brushes.White, _position_X, _position_Y, _chartSize.Width, _chartSize.Height);

            int auxPositionX =_position_X + freeSpaceX;
            int auxPositionY =_position_Y + freeSpaceY;

            int spaceCounter = 0;
            double pie_next_section = 0;
            double percentage;
            double pie_value = 0;
            double total_sum = 0;
            int total_value = 0;

            int space_Horizontal_Rectangle = 20;
            int space_Vertical_Rectangle = 15;
            int space_Horizontal_Text = 45;
            int space_Vertical_Text = 15;

            int rectangle_Size = 10;
            int textColumnSpace = 7;

            int inner_Size = (int)(this._pieSize * 0.425);
            int inner_Space = (this._pieSize - inner_Size) / 2;
            int inner_Position_X = (int)(auxPositionX + inner_Space);
            int inner_Position_Y = (int)(auxPositionY + inner_Space);

            // If user do not provide brushes, use the default
            if (this._brushes == null) {
                this._brushes = GetDefaultBrushes();
            }

            Pen pen = new Pen(Color.Black, 1);


            // The area used to display chart
            Rectangle pieArea = new Rectangle(auxPositionX, auxPositionY, this._pieSize, this._pieSize);

            // Total value of numeric values in List
            foreach (int value in this._numbersValues) {
                total_value += value;
            }

            if (_numbersValues.Count != 0) {
                for (int i = 0; i < this._numbersValues.Count; i++) {

                    if (total_value != 0) {
                        percentage = ((this._numbersValues[i] * 100) / total_value);
                        pie_value = (double)Math.Round(((360 * percentage) / 100));
                        total_sum += pie_value;

                        if (i == this._numbersValues.Count - 1 && (pie_next_section + pie_value) < 360) {
                            pie_value = (pie_value + (360 - (pie_next_section + pie_value)));
                            this._g.FillPie(_randomBrushes[i], pieArea, (int)pie_next_section, (int)pie_value);
                        } else {
                            this._g.FillPie(_randomBrushes[i], pieArea, (int)pie_next_section, (int)pie_value);
                        }


                    } else {
                        this._g.FillPie(Brushes.LightGray, pieArea, 0, 360);
                    }


                    if ((space_Vertical_Rectangle * spaceCounter) > this._pieSize) {
                        spaceCounter = 0;
                        space_Horizontal_Rectangle = space_Horizontal_Rectangle + (25 * textColumnSpace);
                        space_Horizontal_Text = space_Horizontal_Text + (25 * textColumnSpace);
                    }

                    this._g.FillRectangle(_randomBrushes[i], new Rectangle(auxPositionX + this._pieSize + space_Horizontal_Rectangle,
                        auxPositionY + (space_Vertical_Rectangle * spaceCounter), rectangle_Size, rectangle_Size));

                    String valueToPrint;
                    if (_textValues[i].Length < 30) {
                        valueToPrint = _textValues[i];
                    } else {
                        valueToPrint = _textValues[i].Substring(0, 29) + ".";
                    }

                    this._g.DrawString($"{valueToPrint}: {this._numbersValues[i]}", this._font, Brushes.Black,
                        auxPositionX + this._pieSize + space_Horizontal_Text, auxPositionY + (space_Vertical_Text * spaceCounter) - _font.Size / 2);

                    pie_next_section += pie_value;
                    spaceCounter++;

                }
            } else {
                this._g.FillPie(Brushes.LightGray, pieArea, 0, 360);
            }

            // Inner circle
            this._g.FillPie(Brushes.White, new Rectangle(inner_Position_X, inner_Position_Y, inner_Size, inner_Size), 0, 360);
            this._g.DrawEllipse(pen, new Rectangle(inner_Position_X, inner_Position_Y, inner_Size, inner_Size));
            this._g.DrawEllipse(pen, new Rectangle(auxPositionX, auxPositionY, this._pieSize, this._pieSize));

        }

        // Create default brushes for slices
        public Brush[] GetDefaultBrushes() {
            Brush[] default_brushes = { Brushes.Red, Brushes.Green, Brushes.Orange, Brushes.LightSkyBlue, Brushes.Fuchsia,
                Brushes.Gold, Brushes.MidnightBlue, Brushes.PowderBlue, Brushes.Firebrick, Brushes.MediumTurquoise,
                Brushes.Coral, Brushes.LightCyan, Brushes.HotPink, Brushes.PaleGoldenrod, Brushes.Indigo, Brushes.Aqua,
                Brushes.Brown, Brushes.DarkSeaGreen, Brushes.LightSalmon, Brushes.CornflowerBlue };

            return default_brushes;
        }

        // Create list of brushes with random color
        public List<Brush> GenerateBurshList(Color color) {
            List<Brush> brushes = new List<Brush>();
            //_generatedColors = new List<Color>();
            Color currentColor;

            while (_generatedColors.Count < 40) {
                
                currentColor = GenerateRandomColor(color);
                if (CheckColors(_generatedColors, currentColor)) {
                    brushes.Add(new SolidBrush(currentColor));
                    _generatedColors.Add(currentColor);
                }
            }

            return brushes;
        }

        // Generate random color for brushes
        public Color GenerateRandomColor(Color mix) {
            Random random = new Random();
            Color generatedColor;

            int red = random.Next(0, 256);
            int green = random.Next(0, 256);
            int blue = random.Next(0, 256);

            if (mix != Color.Black) {
                red = (red + mix.R) / 2;
                green = (green + mix.G) / 2;
                blue = (red + mix.B) / 2;
            }

            generatedColor = Color.FromArgb(red, green, blue);

            return generatedColor;
        }

        // Check if the list have similiar color
        public bool CheckColors(List<Color> colors, Color color) {

            int difference;

            foreach (Color c in colors) {
                //https://www.compuphase.com/cmetric.htm
                long rmean = ((long)c.R + (long)color.R) / 2;
                long r = (long)c.R - (long)color.R;
                long g = (long)c.G - (long)color.G;
                long b = (long)c.B - (long)color.B;
                difference = (int)Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));
                
                if (difference < 120) {
                    return false;
                }
            }

            return true;
        }

        // Clear form screen
        public void CleanScreen(Color color) {
            this._g.Clear(color);
        }

    }
}
