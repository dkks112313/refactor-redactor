using RefactoringApp;
using RefactoringApp;
using RefactoringChange;
using RefactoringTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //backButton.Click += (s, e) => GoBack();
            //forwardButton.Click += (s, e) => GoForward();

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
                new MagicNumberRefactoringController(),
                new AdapterCondolidateConditionalController(),
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

            foreach(Control c in paramsPanel.Controls)
            {
                if(c is TextBox tb && tb.Tag != null)
                {
                    parameters[tb.Tag.ToString()] = tb.Text;
                }
            }

            string results = selected.Execute(inputCode.Text, parameters);
            outputCode.Text = results;
        }

        private void ExitClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void HistorySelected(object sender, EventArgs e)
        {
            
        }
    }
}
