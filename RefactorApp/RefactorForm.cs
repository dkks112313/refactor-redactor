using RefactoringApp;
using RefactoringChange;
using RefactoringTool;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace WindowsFormsApp6
{
    public partial class RefactorForm : Form
    {
        private TextBox inputCode;
        private TextBox outputCode;
        private ComboBox refactorSelector;
        private Panel paramsPanel;
        private Button runButton;
        private Button exitButton;
        private Label historyLabel;
        private Button backButton;
        private Button forwardButton;

        private List<RefactoringMethods> refactorings;

        private List<string> history = new List<string>();
        private int currentIndex = -1;

        public RefactorForm()
        {
            this.Text = "Refactoring Tool";
            this.Width = 1024;
            this.Height = 768;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            inputCode = new TextBox
            {
                Multiline = true,
                Width = 350,
                Height = 700,
                Left = 290,
                Top = 10,
                ScrollBars = ScrollBars.Vertical
            };

            outputCode = new TextBox
            {
                Multiline = true,
                Width = 350,
                Height = 700,
                Left = 650,
                Top = 10,
                ScrollBars = ScrollBars.Vertical
            };

            refactorSelector = new ComboBox
            {
                Left = 20,
                Top = 30,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };

            refactorSelector.SelectedIndexChanged += RefactorChanged;

            paramsPanel = new Panel
            {
                Left = 20,
                Top = 150,
                Width = 250,
                Height = 150,
                BorderStyle = BorderStyle.FixedSingle,
            };

            runButton = new Button
            {
                Text = "Run",
                Left = 150,
                Top = 330,
                Width = 120,
                Height = 40
            };

            runButton.Click += RunClicked;

            exitButton = new Button
            {
                Text = "Exit",
                Left = 20,
                Top = 330,
                Width = 50,
                Height = 40
            };

            exitButton.Click += ExitClicked;

            historyLabel = new Label
            {
                Text = "History",
                Left = 20,
                Top = 380,
                Width = 150,
                Height = 20
            };

            backButton = new Button
            {
                Text = "<-",
                Left = 20,
                Top = 410,
                Width = 50,
                Height = 40
            };

            forwardButton = new Button
            {
                Text = "->",
                Left = 80,
                Top = 410,
                Width = 50,
                Height = 40
            };

            backButton.Click += (s, e) => GoBack();
            forwardButton.Click += (s, e) => GoForward();

            this.Controls.Add(inputCode);
            this.Controls.Add(outputCode);
            this.Controls.Add(runButton);
            this.Controls.Add(exitButton);
            this.Controls.Add(refactorSelector);
            this.Controls.Add(paramsPanel);
            this.Controls.Add(historyLabel);
            this.Controls.Add(forwardButton);
            this.Controls.Add(backButton);

            refactorings = new List<RefactoringMethods>
            {
                new RefactorRenameMethodController(),
                new RefactorRenameVariableController(),
                new RefactorRemoveParameterController(),
                new MagicNumberRefactoringController()
            };

            refactorSelector.DataSource = refactorings;
            refactorSelector.DisplayMember = "Name";
        }

        private void RefactorChanged(object sender, EventArgs e)
        {
            paramsPanel.Controls.Clear();

            var selected = (RefactoringMethods)refactorSelector.SelectedItem;
            var parameters = selected.GetParameters();

            int top = 10;

            foreach (var param in parameters)
            {
                Label label = new Label
                {
                    Text = param.Name,
                    Left = 10,
                    Top = top,
                };

                TextBox textbox = new TextBox
                {
                    Left = 10,
                    Top = top + 20,
                    Width = 160,
                    Tag = param.Value
                };

                paramsPanel.Controls.Add(label);
                paramsPanel.Controls.Add(textbox);

                top += 60;
            }
        }

        private void RunClicked(object sender, EventArgs e)
        {
            var selected = (RefactoringMethods)refactorSelector.SelectedItem;
            var parameters = new Dictionary<string, string>();

            foreach (Control c in paramsPanel.Controls)
            {
                if (c is TextBox tb && tb.Tag != null)
                {
                    parameters[tb.Tag.ToString()] = tb.Text;
                }
            }

            string results = selected.Execute(inputCode.Text, parameters);
            outputCode.Text = results;

            SaveToHistory(inputCode.Text);
        }

        private void ExitClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Метод для збереження поточного коду в історії.
        /// </summary>
        /// <param name="code">Код для збереження.</param>
        private void SaveToHistory(string code)
        {
            // Якщо список порожній, додати код і встановить поточний індекс на 0.
            if (history.Count == 0)
            {
                history.Add(code);
                currentIndex = 0;
                return;
            }

            // Якщо поточний код збігається з останнім у історії, не додавати його знову.
            if (history[currentIndex] == code)
                return;

            // Якщо не досягнуто кінця історії, видалити всі записи, що йдуть за поточним індексом
            if (currentIndex < history.Count - 1)
            {
                history.RemoveRange(currentIndex + 1, history.Count - currentIndex - 1);
            }

            // Додати новий код до історії та оновити поточний індекс.
            history.Add(code);
            currentIndex = history.Count - 1;
        }

        /// <summary>
        /// Метод для переходу назад в історії.
        /// </summary>
        private void GoBack()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                inputCode.Text = history[currentIndex];

                outputCode.Text = "";
            }
        }

        /// <summary>
        /// Метод для переходу вперед в історії.
        /// </summary>
        private void GoForward()
        {
            if (currentIndex < history.Count - 1)
            {
                currentIndex++;
                inputCode.Text = history[currentIndex];

                outputCode.Text = "";
            }
        }
    }
}