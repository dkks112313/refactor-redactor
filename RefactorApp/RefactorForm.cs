using RefactoringApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RefactoringApp;

namespace WindowsFormsApp6
{
    public partial class RefactorForm : Form
    {
        private TextBox inputCode;
        private TextBox outputCode;
        private ComboBox refactorSelector;
        private Panel paramsPanel;
        private Button runButton;

        private List<RefactoringMethods> refactorings;

        public RefactorForm()
        {
            this.Text = "Refactoring Tool";
            this.Width = 900;
            this.Height = 600;

            //Блок ввода кода
            inputCode = new TextBox
            {
                Multiline = true,
                Width = 600,
                Height = 250,
                Left = 10,
                Top = 10,
                ScrollBars = ScrollBars.Vertical
            };

            outputCode = new TextBox
            {
                Multiline = true,
                Width = 600,
                Height = 250,
                Left = 10,
                Top = 270,
                ScrollBars = ScrollBars.Vertical
            };

            //Блок вібора метода рефакторинга
            refactorSelector = new ComboBox
            {
                Left = 650,
                Top = 10,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };

            refactorSelector.SelectedIndexChanged += RefactorChanged;

            //місце для параметрів, необхідних для методів рефакторингу
            paramsPanel = new Panel
            {
                Left = 650,
                Top = 270,
                Width = 200,
                Height = 200,
                BorderStyle = BorderStyle.FixedSingle,
            };

            runButton = new Button
            {
                Text = "Run",
                Left = 750,
                Top = 490,
                Width = 120,
                Height = 40
            };

            this.Controls.Add(inputCode);
            this.Controls.Add(outputCode);
            this.Controls.Add(runButton);
            this.Controls.Add(refactorSelector);
            this.Controls.Add(paramsPanel);

            refactorings = new List<RefactoringMethods>
            {
                new RefactorRenameMethodController()
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
    }
}
