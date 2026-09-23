using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinCalc;

public class CalculatorForm : Form
{
    private TextBox display;
    private ListBox history;
    private double firstOperand = 0;
    private string currentOp = "";
    private bool newInput = true;

    public CalculatorForm()
    {
        // --- Настройки окна ---
        Text = "Калькулятор";
        Size = new Size(520, 520);
        MinimumSize = new Size(520, 520);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10);

        // --- Экран (display) ---
        display = new TextBox
        {
            Location = new Point(15, 15),
            Size = new Size(300, 50),
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            TextAlign = HorizontalAlignment.Right,
            ReadOnly = true,
            Text = "0",
            BackColor = Color.WhiteSmoke
        };
        Controls.Add(display);

        // --- Панель истории ---
        var historyLabel = new Label
        {
            Text = "История",
            Location = new Point(335, 15),
            Size = new Size(150, 25),
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };
        Controls.Add(historyLabel);

        history = new ListBox
        {
            Location = new Point(335, 45),
            Size = new Size(155, 415),
            Font = new Font("Consolas", 10),
            IntegralHeight = false
        };
        Controls.Add(history);

        // Кнопка очистки истории
        var clearHistoryBtn = new Button
        {
            Text = "Очистить историю",
            Location = new Point(335, 465),
            Size = new Size(155, 28)
        };
        clearHistoryBtn.Click += (s, e) => history.Items.Clear();
        Controls.Add(clearHistoryBtn);

        // --- Кнопки калькулятора ---
        string[,] buttons = new string[,]
        {
            { "C",  "±",  "√",  "/" },
            { "7",  "8",  "9",  "*" },
            { "4",  "5",  "6",  "-" },
            { "1",  "2",  "3",  "+" },
            { "0",  ".",  "=",  "←" }
        };

        int btnW = 68, btnH = 68, gap = 8, startX = 15, startY = 80;

        for (int row = 0; row < buttons.GetLength(0); row++)
        {
            for (int col = 0; col < buttons.GetLength(1); col++)
            {
                string text = buttons[row, col];
                var btn = new Button
                {
                    Text = text,
                    Location = new Point(startX + col * (btnW + gap),
                                         startY + row * (btnH + gap)),
                    Size = new Size(btnW, btnH),
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };

                // Подсветка операторов
                if ("+-*/=".Contains(text))
                {
                    btn.BackColor = Color.LightSteelBlue;
                }
                else if (text == "C" || text == "←")
                {
                    btn.BackColor = Color.MistyRose;
                }

                btn.Click += Button_Click;
                Controls.Add(btn);
            }
        }
    }

    private void Button_Click(object? sender, EventArgs e)
    {
        string cmd = ((Button)sender!).Text;

        if (char.IsDigit(cmd[0]) || cmd == ".")
        {
            AppendDigit(cmd);
        }
        else switch (cmd)
        {
            case "C":  Clear(); break;
            case "←":  Backspace(); break;
            case "±":  ToggleSign(); break;
            case "√":  SquareRoot(); break;
            case "+": case "-": case "*": case "/":
                SetOperator(cmd); break;
            case "=":  Calculate(); break;
        }
    }

    private void AppendDigit(string d)
    {
        if (newInput)
        {
            display.Text = d == "." ? "0." : d;
            newInput = false;
        }
        else
        {
            if (d == "." && display.Text.Contains(".")) return;
            if (display.Text == "0" && d != ".") display.Text = d;
            else display.Text += d;
        }
    }

    private void Clear()
    {
        display.Text = "0";
        firstOperand = 0;
        currentOp = "";
        newInput = true;
    }

    private void Backspace()
    {
        if (newInput || display.Text.Length <= 1)
            display.Text = "0";
        else
            display.Text = display.Text[..^1];
    }

    private void ToggleSign()
    {
        if (display.Text.StartsWith("-"))
            display.Text = display.Text[1..];
        else if (display.Text != "0")
            display.Text = "-" + display.Text;
    }

    private void SquareRoot()
    {
        if (!double.TryParse(display.Text, out double x)) return;
        if (x < 0)
        {
            MessageBox.Show("Нельзя извлечь корень из отрицательного числа",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        double result = Math.Sqrt(x);
        history.Items.Add($"√({x}) = {result}");
        display.Text = result.ToString();
        newInput = true;
    }

    private void SetOperator(string op)
    {
        if (!double.TryParse(display.Text, out double x)) return;
        firstOperand = x;
        currentOp = op;
        newInput = true;
    }

    private void Calculate()
    {
        if (currentOp == "") return;
        if (!double.TryParse(display.Text, out double second)) return;

        double result = 0;
        try
        {
            switch (currentOp)
            {
                case "+": result = firstOperand + second; break;
                case "-": result = firstOperand - second; break;
                case "*": result = firstOperand * second; break;
                case "/":
                    if (second == 0)
                        throw new DivideByZeroException("Деление на ноль невозможно");
                    result = firstOperand / second;
                    break;
            }

            string record = $"{firstOperand} {currentOp} {second} = {result}";
            history.Items.Add(record);

            display.Text = result.ToString();
            firstOperand = result;
            currentOp = "";
            newInput = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Clear();
        }
    }
}