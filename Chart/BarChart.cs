using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Presentacion {
    class BarChart {

        Graphics _g;
        List<String> _textValues;
        List<double> _numbersValues;
        int _position_X;
        int _position_Y;
        int _width;
        int _height;
        Brush[] _brushes;
        Font _font;
        Color _backColor;
        List<Brush> _randomBrushes;

        public BarChart() {
            this._textValues = new List<String>();
            this._numbersValues = new List<double>();
            this._position_X = 0;
            this._position_Y = 0;
            this._width = 0;
            this._height = 0;
            this._backColor = Color.White;
            this._brushes = null;
            this._font = null;
            _randomBrushes = GenerateBurshList(Color.Black, 40);

        }

        // Constructor without font & brushes
        public BarChart(Graphics g, List<String> textValues, List<double> numbersValues, int position_x, int position_y,
            int width, int height, Color backColor) {
            this._g = g;
            this._textValues = textValues;
            this._numbersValues = numbersValues;
            this._position_X = position_x;
            this._position_Y = position_y;
            this._width = width;
            this._height = height;
            this._backColor = backColor;
            this._brushes = null;
            this._font = null;
            _randomBrushes = GenerateBurshList(Color.Black, 40);
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

        public int Width {
            get { return _width; }
            set { _width = value; }
        }

        public int Height {
            get { return _height; }
            set { _height = value; }
        }

        public Color BackColor {
            get { return _backColor; }
            set { _backColor = value; }
        }

        // Print the piechart
        public void PrinBarChart() {

            CleanScreen(_backColor);

            Font fontNumbers = new Font("Times New Roman", 10);
            Font fontText = new Font("Times New Roman", 8);

            double chartXPadding = 0.08;
            double chartYPadding = 0.1;
            double chartWidthPadding = 0.3;
            double chartHeightPadding = 0.2;

            int sectionsNumber = 4;

            int chartPositionX = (int)(_position_X + (_width * chartXPadding));
            int chartPositionY = (int)(_position_Y + (_height * chartYPadding));
            int chartWidth = (int)(_width - (_width * chartWidthPadding));
            int chartHeight = (int)(_height - (_height * chartHeightPadding));
            int sectionSize = chartHeight / sectionsNumber;

            int referenceRectangleWidth = 10;
            int referenceRectangleHeight = 10;
            int space = 10;

            int maximumValue = 4;

            int barHeight;
            int barWidth = 0;
            int totalBarsWidth;
            int startPositionBar = 0;
            double largestValue;

            if (_numbersValues.Count != 0) {

                barWidth = (int)(chartWidth / _numbersValues.Count);

                if (((barWidth * 100) / chartWidth) > 10) {
                    barWidth = 10 * chartWidth / 100;
                }

                totalBarsWidth = barWidth * _numbersValues.Count;
                startPositionBar = (chartWidth - totalBarsWidth) / 2;

                largestValue = getLargestValue(_numbersValues);


                if (largestValue % 10 != 0) {
                    maximumValue = (int)largestValue;
                    while (maximumValue % 10 != 0) {
                        maximumValue++;
                    }
                } else {
                    maximumValue = (int)largestValue + 10;
                }
            }

            this._g.FillRectangle(Brushes.White, new Rectangle(this._position_X, this._position_Y, this._width, this._height));
            this._g.DrawRectangle(Pens.Black, new Rectangle(chartPositionX, chartPositionY, chartWidth, chartHeight));
            this._g.DrawLine(Pens.Black, chartPositionX - space, chartPositionY, chartPositionX, chartPositionY);
            this._g.DrawString($"{maximumValue}", fontNumbers, Brushes.Black, chartPositionX - fontNumbers.Size * 3, chartPositionY - fontNumbers.Size / 2);
            this._g.DrawLine(Pens.Black, chartPositionX - space, chartPositionY + chartHeight, chartPositionX, chartPositionY + chartHeight);
            this._g.DrawString("0", fontNumbers, Brushes.Black, chartPositionX - fontNumbers.Size * 3, chartPositionY + chartHeight - fontNumbers.Size / 2);



            for (int i = 1; i < 4; i++) {
                this._g.DrawLine(Pens.Black, chartPositionX - 10, chartPositionY + (sectionSize * i), chartPositionX + chartWidth, chartPositionY + (sectionSize * i));
                int numberValue = (int)(maximumValue - (maximumValue * (0.25 * i)));
                this._g.DrawString($"{numberValue}", fontNumbers, Brushes.Black, chartPositionX - fontNumbers.Size * 3, chartPositionY + (sectionSize * i) - fontNumbers.Size / 2);
            }

            if (_numbersValues.Count != 0) {

                for (int i = 0; i < _textValues.Count; i++) {

                    this._g.FillRectangle(_randomBrushes[i], new Rectangle(chartPositionX + chartWidth + space,
                        chartPositionY + (15 * i), referenceRectangleWidth, referenceRectangleHeight));

                    String valueToPrint;
                    if (_textValues[i].Length < 30) {
                        valueToPrint = _textValues[i];
                    } else {
                        valueToPrint = _textValues[i].Substring(0, 29) + ".";
                    }

                    this._g.DrawString($"{valueToPrint}", fontText, Brushes.Black,
                        chartPositionX + chartWidth + 10 + 10 + 5, chartPositionY + (15 * i) - 3);
                }

                for (int i = 0; i < _numbersValues.Count; i++) {

                    int oneUnitHeight = chartHeight / maximumValue;

                    if (_numbersValues[i] > 0) {
                        barHeight = (int)(oneUnitHeight * _numbersValues[i]);
                    } else {
                        barHeight = (int)(oneUnitHeight * 0.5);
                    }

                    this._g.FillRectangle(_randomBrushes[i], new Rectangle(startPositionBar + chartPositionX + (barWidth * i),
                        chartPositionY + (chartHeight - barHeight), barWidth, barHeight));

                    this._g.DrawString($"{_numbersValues[i]}", fontNumbers, Brushes.Black, startPositionBar + chartPositionX + (barWidth * i) + (barWidth / 2) - fontNumbers.Size / 2,
                        chartPositionY + (chartHeight - barHeight) - fontNumbers.Size * 2);
                }
            }
        }


        public double getLargestValue(List<double> numericValues) {
            double largest = -2147483648;

            foreach (double d in numericValues) {
                if (d > largest) {
                    largest = d;
                }
            }

            return largest;
        }

        // Calculate the width of the largest String
        public int GetLargestStringValue(List<String> text, List<double> value, int start, int end) {
            int largest_String = 0;

            for (int i = start; i <= end; i++) {
                if (text[i].Length + value[i].ToString().Length + 2 > largest_String) {
                    largest_String = text[i].Length + value[i].ToString().Length + 3;
                }
            }

            return largest_String;

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
        public List<Brush> GenerateBurshList(Color color, int cuantity) {
            List<Brush> brushes = new List<Brush>();
            List<Color> generatedColors = new List<Color>();
            Color currentColor;

            while (generatedColors.Count < cuantity) {

                currentColor = GenerateRandomColor(color);
                if (CheckColors(generatedColors, currentColor)) {
                    brushes.Add(new SolidBrush(currentColor));
                    generatedColors.Add(currentColor);
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
