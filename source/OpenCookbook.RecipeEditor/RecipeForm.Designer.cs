namespace OpenCookbook.RecipeEditor
{
  partial class RecipeForm
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            titleTextbox = new TextBox();
            serializeButton = new Button();
            resultTextbox = new TextBox();
            titleLabel = new Label();
            sourceLabel = new Label();
            sourceTextbox = new TextBox();
            servingsInput = new NumericUpDown();
            servingsLabel = new Label();
            prepTimeLabel = new Label();
            prepTimeInput = new NumericUpDown();
            cookingTimeLabel = new Label();
            cookingTimeInput = new NumericUpDown();
            ingredientsTextbox = new TextBox();
            ingredientsLabel = new Label();
            directionsLabel = new Label();
            directionsTextbox = new TextBox();
            tagsLabel = new Label();
            tagsTextbox = new TextBox();
            notesLabel = new Label();
            notesTextbox = new TextBox();
            slugLabel = new Label();
            slugTextbox = new TextBox();
            loadButton = new Button();
            loadRecipeDialog = new OpenFileDialog();
            clearButton = new Button();
            ((System.ComponentModel.ISupportInitialize)servingsInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)prepTimeInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cookingTimeInput).BeginInit();
            SuspendLayout();
            // 
            // titleTextbox
            // 
            titleTextbox.Location = new Point(135, 12);
            titleTextbox.Name = "titleTextbox";
            titleTextbox.Size = new Size(441, 23);
            titleTextbox.TabIndex = 0;
            // 
            // serializeButton
            // 
            serializeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            serializeButton.Location = new Point(501, 418);
            serializeButton.Name = "serializeButton";
            serializeButton.Size = new Size(75, 23);
            serializeButton.TabIndex = 1;
            serializeButton.Text = "Serialize";
            serializeButton.UseVisualStyleBackColor = true;
            serializeButton.Click += SerializeButton_Click;
            // 
            // resultTextbox
            // 
            resultTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            resultTextbox.Location = new Point(580, 41);
            resultTextbox.Multiline = true;
            resultTextbox.Name = "resultTextbox";
            resultTextbox.Size = new Size(582, 371);
            resultTextbox.TabIndex = 2;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(12, 15);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(29, 15);
            titleLabel.TabIndex = 3;
            titleLabel.Text = "Title";
            // 
            // sourceLabel
            // 
            sourceLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            sourceLabel.AutoSize = true;
            sourceLabel.Location = new Point(12, 73);
            sourceLabel.Name = "sourceLabel";
            sourceLabel.Size = new Size(43, 15);
            sourceLabel.TabIndex = 5;
            sourceLabel.Text = "Source";
            // 
            // sourceTextbox
            // 
            sourceTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            sourceTextbox.Location = new Point(135, 70);
            sourceTextbox.Name = "sourceTextbox";
            sourceTextbox.Size = new Size(441, 23);
            sourceTextbox.TabIndex = 4;
            // 
            // servingsInput
            // 
            servingsInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            servingsInput.Location = new Point(135, 99);
            servingsInput.Name = "servingsInput";
            servingsInput.Size = new Size(441, 23);
            servingsInput.TabIndex = 6;
            // 
            // servingsLabel
            // 
            servingsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            servingsLabel.AutoSize = true;
            servingsLabel.Location = new Point(12, 101);
            servingsLabel.Name = "servingsLabel";
            servingsLabel.Size = new Size(51, 15);
            servingsLabel.TabIndex = 7;
            servingsLabel.Text = "Servings";
            // 
            // prepTimeLabel
            // 
            prepTimeLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            prepTimeLabel.AutoSize = true;
            prepTimeLabel.Location = new Point(12, 130);
            prepTimeLabel.Name = "prepTimeLabel";
            prepTimeLabel.Size = new Size(60, 15);
            prepTimeLabel.TabIndex = 9;
            prepTimeLabel.Text = "Prep Time";
            // 
            // prepTimeInput
            // 
            prepTimeInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            prepTimeInput.Location = new Point(135, 128);
            prepTimeInput.Name = "prepTimeInput";
            prepTimeInput.Size = new Size(441, 23);
            prepTimeInput.TabIndex = 8;
            // 
            // cookingTimeLabel
            // 
            cookingTimeLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cookingTimeLabel.AutoSize = true;
            cookingTimeLabel.Location = new Point(12, 159);
            cookingTimeLabel.Name = "cookingTimeLabel";
            cookingTimeLabel.Size = new Size(81, 15);
            cookingTimeLabel.TabIndex = 11;
            cookingTimeLabel.Text = "Cooking Time";
            // 
            // cookingTimeInput
            // 
            cookingTimeInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cookingTimeInput.Location = new Point(135, 157);
            cookingTimeInput.Name = "cookingTimeInput";
            cookingTimeInput.Size = new Size(441, 23);
            cookingTimeInput.TabIndex = 10;
            // 
            // ingredientsTextbox
            // 
            ingredientsTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ingredientsTextbox.Location = new Point(135, 186);
            ingredientsTextbox.Multiline = true;
            ingredientsTextbox.Name = "ingredientsTextbox";
            ingredientsTextbox.ScrollBars = ScrollBars.Both;
            ingredientsTextbox.Size = new Size(441, 52);
            ingredientsTextbox.TabIndex = 12;
            // 
            // ingredientsLabel
            // 
            ingredientsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ingredientsLabel.AutoSize = true;
            ingredientsLabel.Location = new Point(12, 189);
            ingredientsLabel.Name = "ingredientsLabel";
            ingredientsLabel.Size = new Size(66, 15);
            ingredientsLabel.TabIndex = 13;
            ingredientsLabel.Text = "Ingredients";
            // 
            // directionsLabel
            // 
            directionsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            directionsLabel.AutoSize = true;
            directionsLabel.Location = new Point(12, 247);
            directionsLabel.Name = "directionsLabel";
            directionsLabel.Size = new Size(60, 15);
            directionsLabel.TabIndex = 15;
            directionsLabel.Text = "Directions";
            // 
            // directionsTextbox
            // 
            directionsTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            directionsTextbox.Location = new Point(135, 244);
            directionsTextbox.Multiline = true;
            directionsTextbox.Name = "directionsTextbox";
            directionsTextbox.ScrollBars = ScrollBars.Both;
            directionsTextbox.Size = new Size(441, 52);
            directionsTextbox.TabIndex = 14;
            // 
            // tagsLabel
            // 
            tagsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tagsLabel.AutoSize = true;
            tagsLabel.Location = new Point(12, 305);
            tagsLabel.Name = "tagsLabel";
            tagsLabel.Size = new Size(30, 15);
            tagsLabel.TabIndex = 17;
            tagsLabel.Text = "Tags";
            // 
            // tagsTextbox
            // 
            tagsTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tagsTextbox.Location = new Point(135, 302);
            tagsTextbox.Multiline = true;
            tagsTextbox.Name = "tagsTextbox";
            tagsTextbox.ScrollBars = ScrollBars.Both;
            tagsTextbox.Size = new Size(441, 52);
            tagsTextbox.TabIndex = 16;
            // 
            // notesLabel
            // 
            notesLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            notesLabel.AutoSize = true;
            notesLabel.Location = new Point(12, 363);
            notesLabel.Name = "notesLabel";
            notesLabel.Size = new Size(38, 15);
            notesLabel.TabIndex = 19;
            notesLabel.Text = "Notes";
            // 
            // notesTextbox
            // 
            notesTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            notesTextbox.Location = new Point(135, 360);
            notesTextbox.Multiline = true;
            notesTextbox.Name = "notesTextbox";
            notesTextbox.ScrollBars = ScrollBars.Both;
            notesTextbox.Size = new Size(441, 52);
            notesTextbox.TabIndex = 18;
            // 
            // slugLabel
            // 
            slugLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            slugLabel.AutoSize = true;
            slugLabel.Location = new Point(12, 44);
            slugLabel.Name = "slugLabel";
            slugLabel.Size = new Size(30, 15);
            slugLabel.TabIndex = 21;
            slugLabel.Text = "Slug";
            // 
            // slugTextbox
            // 
            slugTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            slugTextbox.Location = new Point(135, 41);
            slugTextbox.Name = "slugTextbox";
            slugTextbox.Size = new Size(441, 23);
            slugTextbox.TabIndex = 20;
            // 
            // loadButton
            // 
            loadButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            loadButton.Location = new Point(1087, 11);
            loadButton.Name = "loadButton";
            loadButton.Size = new Size(75, 23);
            loadButton.TabIndex = 22;
            loadButton.Text = "Load";
            loadButton.UseVisualStyleBackColor = true;
            loadButton.Click += LoadButton_Click;
            // 
            // loadRecipeDialog
            // 
            loadRecipeDialog.FileName = "recipe.yaml";
            loadRecipeDialog.Filter = "Yaml files|*.yaml";
            // 
            // clearButton
            // 
            clearButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            clearButton.Location = new Point(1006, 11);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(75, 23);
            clearButton.TabIndex = 23;
            clearButton.Text = "Clear";
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += ClearButton_Click;
            // 
            // RecipeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1174, 453);
            Controls.Add(clearButton);
            Controls.Add(loadButton);
            Controls.Add(slugLabel);
            Controls.Add(slugTextbox);
            Controls.Add(notesLabel);
            Controls.Add(notesTextbox);
            Controls.Add(tagsLabel);
            Controls.Add(tagsTextbox);
            Controls.Add(directionsLabel);
            Controls.Add(directionsTextbox);
            Controls.Add(ingredientsLabel);
            Controls.Add(ingredientsTextbox);
            Controls.Add(cookingTimeLabel);
            Controls.Add(cookingTimeInput);
            Controls.Add(prepTimeLabel);
            Controls.Add(prepTimeInput);
            Controls.Add(servingsLabel);
            Controls.Add(servingsInput);
            Controls.Add(sourceLabel);
            Controls.Add(sourceTextbox);
            Controls.Add(titleLabel);
            Controls.Add(resultTextbox);
            Controls.Add(serializeButton);
            Controls.Add(titleTextbox);
            Name = "RecipeForm";
            Text = "Recipe";
            ((System.ComponentModel.ISupportInitialize)servingsInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)prepTimeInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)cookingTimeInput).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox titleTextbox;
        private Button serializeButton;
        private TextBox resultTextbox;
        private Label titleLabel;
        private Label sourceLabel;
        private TextBox sourceTextbox;
        private NumericUpDown servingsInput;
        private Label servingsLabel;
        private Label prepTimeLabel;
        private NumericUpDown prepTimeInput;
        private Label cookingTimeLabel;
        private NumericUpDown cookingTimeInput;
        private TextBox ingredientsTextbox;
        private Label ingredientsLabel;
        private Label directionsLabel;
        private TextBox directionsTextbox;
        private Label tagsLabel;
        private TextBox tagsTextbox;
        private Label notesLabel;
        private TextBox notesTextbox;
        private Label slugLabel;
        private TextBox slugTextbox;
        private Button loadButton;
        private OpenFileDialog loadRecipeDialog;
        private Button clearButton;
    }
}
