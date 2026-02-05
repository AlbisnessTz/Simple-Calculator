namespace Simple_Calculator
{
    public partial class Form1 : Form
    {
        // calculator state
        private double accumulator = 0;
        private string pendingOp = string.Empty;
        private bool isNewEntry = true;

        public Form1()
        {
            InitializeComponent();
            // initialize UI texts
            button1.Text = "7";
            button2.Text = "8";
            button3.Text = "9";
            button13.Text = "/";

            button4.Text = "4";
            button5.Text = "5";
            button6.Text = "6";
            button14.Text = "*";

            button7.Text = "1";
            button8.Text = "2";
            button9.Text = "3";
            button15.Text = "+";

            button11.Text = "0";
            button10.Text = ".";
            button12.Text = "=";
            button16.Text = "AC"; // all clear
            button17.Text = "DEL"; // delete last
            button18.Text = "-"; // minus operator

            textBox1.Text = "0";

            // wire handlers (button3 and button12 are already wired in designer)
            button1.Click += Digit_Click;
            button2.Click += Digit_Click;
            // button3 uses designer handler button3_Click
            button4.Click += Digit_Click;
            button5.Click += Digit_Click;
            button6.Click += Digit_Click;
            button7.Click += Digit_Click;
            button8.Click += Digit_Click;
            button9.Click += Digit_Click;
            button11.Click += Digit_Click;

            button13.Click += Operator_Click;
            button14.Click += Operator_Click;
            button15.Click += Operator_Click;

            button18.Click += Operator_Click;

            button16.Click += AllClear_Click;
            button17.Click += Delete_Click;

            button10.Click += Decimal_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // button3 is mapped to digit 9
            OnDigitPressed("9");

        }

        private void button12_Click(object sender, EventArgs e)
        {
            // equals
            OnEqualsPressed();
        }

        private void Digit_Click(object? sender, EventArgs e)
        {
            if (sender is Button b)
            {
                OnDigitPressed(b.Text);
            }
        }

        private void Operator_Click(object? sender, EventArgs e)
        {
            if (sender is Button b)
            {
                OnOperatorPressed(b.Text);
            }
        }

        private void Decimal_Click(object? sender, EventArgs e)
        {
            if (isNewEntry)
            {
                textBox1.Text = "0.";
                isNewEntry = false;
            }
            else if (!textBox1.Text.Contains('.'))
            {
                textBox1.Text += ".";
            }
        }

        private void AllClear_Click(object? sender, EventArgs e)
        {
            textBox1.Text = "0";
            accumulator = 0;
            pendingOp = string.Empty;
            isNewEntry = true;
        }

        private void Delete_Click(object? sender, EventArgs e)
        {
            if (isNewEntry || string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = "0";
                isNewEntry = true;
                return;
            }

            if (textBox1.Text.Length <= 1)
            {
                textBox1.Text = "0";
                isNewEntry = true;
                return;
            }

            textBox1.Text = textBox1.Text[..^1];
        }

        private void OnDigitPressed(string digit)
        {
            if (isNewEntry || textBox1.Text == "0")
            {
                textBox1.Text = digit;
                isNewEntry = false;
            }
            else
            {
                textBox1.Text += digit;
            }
        }

        private void OnOperatorPressed(string op)
        {
            if (double.TryParse(textBox1.Text, out double current))
            {
                if (!string.IsNullOrEmpty(pendingOp))
                {
                    accumulator = Calculate(accumulator, current, pendingOp);
                    textBox1.Text = accumulator.ToString();
                }
                else
                {
                    accumulator = current;
                }
            }
            pendingOp = op;
            isNewEntry = true;
        }

        private void OnEqualsPressed()
        {
            if (!double.TryParse(textBox1.Text, out double current))
                return;

            double result = current;
            if (!string.IsNullOrEmpty(pendingOp))
            {
                result = Calculate(accumulator, current, pendingOp);
                textBox1.Text = result.ToString();
                pendingOp = string.Empty;
                accumulator = result;
                isNewEntry = true;
            }
        }

        private double Calculate(double a, double b, string op)
        {
            return op switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b == 0 ? double.NaN : a / b,
                _ => b,
            };
        }
    }
}
