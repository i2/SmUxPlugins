namespace EditorExtension
{
    partial class DlgMultiItemEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chbLesson = new System.Windows.Forms.CheckBox();
            this.txbLesson = new System.Windows.Forms.TextBox();
            this.txbChapter = new System.Windows.Forms.TextBox();
            this.chbChapter = new System.Windows.Forms.CheckBox();
            this.txbCommand = new System.Windows.Forms.TextBox();
            this.chbCommand = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txbTreeItemName = new System.Windows.Forms.TextBox();
            this.rbtAsValue = new System.Windows.Forms.RadioButton();
            this.rbtAsQuestion = new System.Windows.Forms.RadioButton();
            this.rdbLeaveAsIs = new System.Windows.Forms.RadioButton();
            this.txbTemplate = new System.Windows.Forms.TextBox();
            this.chbTemplate = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbUseRegularExpressions = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txbSearchText = new System.Windows.Forms.TextBox();
            this.txbReplaceText = new System.Windows.Forms.TextBox();
            this.chbReplaceInQuestionTitle = new System.Windows.Forms.CheckBox();
            this.chbReplaceInChapterTitle = new System.Windows.Forms.CheckBox();
            this.chbReplaceInLessonTitle = new System.Windows.Forms.CheckBox();
            this.chbReplaceInAnswer = new System.Windows.Forms.CheckBox();
            this.chbReplaceInQuestion = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.CheckBoxes = true;
            this.treeView1.HotTracking = true;
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(394, 531);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(742, 549);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(661, 549);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPreview.Location = new System.Drawing.Point(12, 549);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 14;
            this.btnPreview.Text = "Podgląd...";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(412, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dla zaznaczonych jednostek ustaw:";
            // 
            // chbLesson
            // 
            this.chbLesson.AutoSize = true;
            this.chbLesson.Location = new System.Drawing.Point(412, 40);
            this.chbLesson.Name = "chbLesson";
            this.chbLesson.Size = new System.Drawing.Size(86, 17);
            this.chbLesson.TabIndex = 2;
            this.chbLesson.Text = "Nazwa &lekcji";
            this.chbLesson.UseVisualStyleBackColor = true;
            this.chbLesson.CheckedChanged += new System.EventHandler(this.chbLesson_CheckedChanged);
            // 
            // txbLesson
            // 
            this.txbLesson.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbLesson.Location = new System.Drawing.Point(412, 63);
            this.txbLesson.Name = "txbLesson";
            this.txbLesson.Size = new System.Drawing.Size(402, 20);
            this.txbLesson.TabIndex = 3;
            // 
            // txbChapter
            // 
            this.txbChapter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbChapter.Location = new System.Drawing.Point(412, 112);
            this.txbChapter.Name = "txbChapter";
            this.txbChapter.Size = new System.Drawing.Size(402, 20);
            this.txbChapter.TabIndex = 5;
            // 
            // chbChapter
            // 
            this.chbChapter.AutoSize = true;
            this.chbChapter.Location = new System.Drawing.Point(412, 89);
            this.chbChapter.Name = "chbChapter";
            this.chbChapter.Size = new System.Drawing.Size(100, 17);
            this.chbChapter.TabIndex = 4;
            this.chbChapter.Text = "Nazwa &rodziału";
            this.chbChapter.UseVisualStyleBackColor = true;
            this.chbChapter.CheckedChanged += new System.EventHandler(this.chbChapter_CheckedChanged);
            // 
            // txbCommand
            // 
            this.txbCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbCommand.Location = new System.Drawing.Point(412, 161);
            this.txbCommand.Name = "txbCommand";
            this.txbCommand.Size = new System.Drawing.Size(402, 20);
            this.txbCommand.TabIndex = 7;
            // 
            // chbCommand
            // 
            this.chbCommand.AutoSize = true;
            this.chbCommand.Location = new System.Drawing.Point(412, 138);
            this.chbCommand.Name = "chbCommand";
            this.chbCommand.Size = new System.Drawing.Size(123, 17);
            this.chbCommand.TabIndex = 6;
            this.chbCommand.Text = "&Polecenie ćwiczenia";
            this.chbCommand.UseVisualStyleBackColor = true;
            this.chbCommand.CheckedChanged += new System.EventHandler(this.chbCommand_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txbTreeItemName);
            this.groupBox1.Controls.Add(this.rbtAsValue);
            this.groupBox1.Controls.Add(this.rbtAsQuestion);
            this.groupBox1.Controls.Add(this.rdbLeaveAsIs);
            this.groupBox1.Location = new System.Drawing.Point(409, 236);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 97);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nazwa jednostki w drzewie jednostek";
            // 
            // txbTreeItemName
            // 
            this.txbTreeItemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTreeItemName.Location = new System.Drawing.Point(122, 65);
            this.txbTreeItemName.Name = "txbTreeItemName";
            this.txbTreeItemName.Size = new System.Drawing.Size(274, 20);
            this.txbTreeItemName.TabIndex = 3;
            // 
            // rbtAsValue
            // 
            this.rbtAsValue.AutoSize = true;
            this.rbtAsValue.Location = new System.Drawing.Point(6, 65);
            this.rbtAsValue.Name = "rbtAsValue";
            this.rbtAsValue.Size = new System.Drawing.Size(110, 17);
            this.rbtAsValue.TabIndex = 2;
            this.rbtAsValue.Text = "Ustaw na &wartość";
            this.rbtAsValue.UseVisualStyleBackColor = true;
            this.rbtAsValue.CheckedChanged += new System.EventHandler(this.rbtAsValue_CheckedChanged);
            // 
            // rbtAsQuestion
            // 
            this.rbtAsQuestion.AutoSize = true;
            this.rbtAsQuestion.Location = new System.Drawing.Point(6, 42);
            this.rbtAsQuestion.Name = "rbtAsQuestion";
            this.rbtAsQuestion.Size = new System.Drawing.Size(161, 17);
            this.rbtAsQuestion.TabIndex = 1;
            this.rbtAsQuestion.Text = "&Ustaw na podstawie  pytania";
            this.rbtAsQuestion.UseVisualStyleBackColor = true;
            this.rbtAsQuestion.CheckedChanged += new System.EventHandler(this.rbtAsQuestion_CheckedChanged);
            // 
            // rdbLeaveAsIs
            // 
            this.rdbLeaveAsIs.AutoSize = true;
            this.rdbLeaveAsIs.Checked = true;
            this.rdbLeaveAsIs.Location = new System.Drawing.Point(6, 19);
            this.rdbLeaveAsIs.Name = "rdbLeaveAsIs";
            this.rdbLeaveAsIs.Size = new System.Drawing.Size(121, 17);
            this.rdbLeaveAsIs.TabIndex = 0;
            this.rdbLeaveAsIs.TabStop = true;
            this.rdbLeaveAsIs.Text = "Pozostaw &bez zmian";
            this.rdbLeaveAsIs.UseVisualStyleBackColor = true;
            this.rdbLeaveAsIs.CheckedChanged += new System.EventHandler(this.rdbLeaveAsIs_CheckedChanged);
            // 
            // txbTemplate
            // 
            this.txbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTemplate.Location = new System.Drawing.Point(409, 210);
            this.txbTemplate.Name = "txbTemplate";
            this.txbTemplate.Size = new System.Drawing.Size(402, 20);
            this.txbTemplate.TabIndex = 9;
            // 
            // chbTemplate
            // 
            this.chbTemplate.AutoSize = true;
            this.chbTemplate.Location = new System.Drawing.Point(409, 187);
            this.chbTemplate.Name = "chbTemplate";
            this.chbTemplate.Size = new System.Drawing.Size(102, 17);
            this.chbTemplate.TabIndex = 8;
            this.chbTemplate.Text = "&Numer szablonu";
            this.chbTemplate.UseVisualStyleBackColor = true;
            this.chbTemplate.CheckedChanged += new System.EventHandler(this.chbTemplate_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbUseRegularExpressions);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txbSearchText);
            this.groupBox2.Controls.Add(this.txbReplaceText);
            this.groupBox2.Controls.Add(this.chbReplaceInQuestionTitle);
            this.groupBox2.Controls.Add(this.chbReplaceInChapterTitle);
            this.groupBox2.Controls.Add(this.chbReplaceInLessonTitle);
            this.groupBox2.Controls.Add(this.chbReplaceInAnswer);
            this.groupBox2.Controls.Add(this.chbReplaceInQuestion);
            this.groupBox2.Location = new System.Drawing.Point(412, 339);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(405, 204);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Zamień w jednostkach";
            // 
            // chbUseRegularExpressions
            // 
            this.chbUseRegularExpressions.AutoSize = true;
            this.chbUseRegularExpressions.Location = new System.Drawing.Point(6, 79);
            this.chbUseRegularExpressions.Name = "chbUseRegularExpressions";
            this.chbUseRegularExpressions.Size = new System.Drawing.Size(146, 17);
            this.chbUseRegularExpressions.TabIndex = 5;
            this.chbUseRegularExpressions.Text = "Użyj wyrażeń regularnych";
            this.chbUseRegularExpressions.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Tekst do podmiany";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Szukany tekst";
            // 
            // txbSearchText
            // 
            this.txbSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbSearchText.Location = new System.Drawing.Point(6, 135);
            this.txbSearchText.Name = "txbSearchText";
            this.txbSearchText.Size = new System.Drawing.Size(393, 20);
            this.txbSearchText.TabIndex = 7;
            // 
            // txbReplaceText
            // 
            this.txbReplaceText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbReplaceText.Location = new System.Drawing.Point(6, 174);
            this.txbReplaceText.Name = "txbReplaceText";
            this.txbReplaceText.Size = new System.Drawing.Size(393, 20);
            this.txbReplaceText.TabIndex = 9;
            // 
            // chbReplaceInQuestionTitle
            // 
            this.chbReplaceInQuestionTitle.AutoSize = true;
            this.chbReplaceInQuestionTitle.Location = new System.Drawing.Point(249, 19);
            this.chbReplaceInQuestionTitle.Name = "chbReplaceInQuestionTitle";
            this.chbReplaceInQuestionTitle.Size = new System.Drawing.Size(137, 17);
            this.chbReplaceInQuestionTitle.TabIndex = 4;
            this.chbReplaceInQuestionTitle.Text = "Zamień w treści pytania";
            this.chbReplaceInQuestionTitle.UseVisualStyleBackColor = true;
            this.chbReplaceInQuestionTitle.CheckedChanged += new System.EventHandler(this.chbReplaceInQuestionTitle_CheckedChanged);
            // 
            // chbReplaceInChapterTitle
            // 
            this.chbReplaceInChapterTitle.AutoSize = true;
            this.chbReplaceInChapterTitle.Location = new System.Drawing.Point(134, 42);
            this.chbReplaceInChapterTitle.Name = "chbReplaceInChapterTitle";
            this.chbReplaceInChapterTitle.Size = new System.Drawing.Size(116, 17);
            this.chbReplaceInChapterTitle.TabIndex = 3;
            this.chbReplaceInChapterTitle.Text = "Zamień w rozdziale";
            this.chbReplaceInChapterTitle.UseVisualStyleBackColor = true;
            this.chbReplaceInChapterTitle.CheckedChanged += new System.EventHandler(this.chbReplaceInChapterTitle_CheckedChanged);
            // 
            // chbReplaceInLessonTitle
            // 
            this.chbReplaceInLessonTitle.AutoSize = true;
            this.chbReplaceInLessonTitle.Location = new System.Drawing.Point(134, 19);
            this.chbReplaceInLessonTitle.Name = "chbReplaceInLessonTitle";
            this.chbReplaceInLessonTitle.Size = new System.Drawing.Size(99, 17);
            this.chbReplaceInLessonTitle.TabIndex = 2;
            this.chbReplaceInLessonTitle.Text = "Zamień w lekcji";
            this.chbReplaceInLessonTitle.UseVisualStyleBackColor = true;
            this.chbReplaceInLessonTitle.CheckedChanged += new System.EventHandler(this.chbReplaceInLessonTitle_CheckedChanged);
            // 
            // chbReplaceInAnswer
            // 
            this.chbReplaceInAnswer.AutoSize = true;
            this.chbReplaceInAnswer.Location = new System.Drawing.Point(6, 42);
            this.chbReplaceInAnswer.Name = "chbReplaceInAnswer";
            this.chbReplaceInAnswer.Size = new System.Drawing.Size(128, 17);
            this.chbReplaceInAnswer.TabIndex = 1;
            this.chbReplaceInAnswer.Text = "Zamień w odpowiedzi";
            this.chbReplaceInAnswer.UseVisualStyleBackColor = true;
            this.chbReplaceInAnswer.CheckedChanged += new System.EventHandler(this.chbReplaceInAnswer_CheckedChanged);
            // 
            // chbReplaceInQuestion
            // 
            this.chbReplaceInQuestion.AutoSize = true;
            this.chbReplaceInQuestion.Location = new System.Drawing.Point(6, 19);
            this.chbReplaceInQuestion.Name = "chbReplaceInQuestion";
            this.chbReplaceInQuestion.Size = new System.Drawing.Size(109, 17);
            this.chbReplaceInQuestion.TabIndex = 0;
            this.chbReplaceInQuestion.Text = "Zamień w pytaniu";
            this.chbReplaceInQuestion.UseVisualStyleBackColor = true;
            this.chbReplaceInQuestion.CheckedChanged += new System.EventHandler(this.chbReplaceInQuestion_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "label2";
            // 
            // DlgMultiItemEdit
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(829, 584);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txbTemplate);
            this.Controls.Add(this.chbTemplate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txbCommand);
            this.Controls.Add(this.chbCommand);
            this.Controls.Add(this.txbChapter);
            this.Controls.Add(this.chbChapter);
            this.Controls.Add(this.txbLesson);
            this.Controls.Add(this.chbLesson);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.treeView1);
            this.Name = "DlgMultiItemEdit";
            this.Text = "Zbiorowa edycja jednostek";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbLesson;
        private System.Windows.Forms.TextBox txbLesson;
        private System.Windows.Forms.TextBox txbChapter;
        private System.Windows.Forms.CheckBox chbChapter;
        private System.Windows.Forms.TextBox txbCommand;
        private System.Windows.Forms.CheckBox chbCommand;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txbTreeItemName;
        private System.Windows.Forms.RadioButton rbtAsValue;
        private System.Windows.Forms.RadioButton rbtAsQuestion;
        private System.Windows.Forms.RadioButton rdbLeaveAsIs;
        private System.Windows.Forms.TextBox txbTemplate;
        private System.Windows.Forms.CheckBox chbTemplate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbReplaceInChapterTitle;
        private System.Windows.Forms.CheckBox chbReplaceInLessonTitle;
        private System.Windows.Forms.CheckBox chbReplaceInAnswer;
        private System.Windows.Forms.CheckBox chbReplaceInQuestion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chbUseRegularExpressions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbSearchText;
        private System.Windows.Forms.TextBox txbReplaceText;
        private System.Windows.Forms.CheckBox chbReplaceInQuestionTitle;

    }
}