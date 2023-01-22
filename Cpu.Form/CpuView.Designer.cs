namespace Cpu.Forms
{
    partial class CpuView
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
            this.menuStrip = new MenuStrip();
            this.FileItem = new ToolStripMenuItem();
            this.openToolStripMenuItem = new ToolStripMenuItem();
            this.saveStateToolStripMenuItem = new ToolStripMenuItem();
            this.loadStateToolStripMenuItem = new ToolStripMenuItem();
            this.flagGroup = new GroupBox();
            this.negativeFlag = new CheckBox();
            this.overflowFlag = new CheckBox();
            this.breakFlag = new CheckBox();
            this.decimalFlag = new CheckBox();
            this.interruptFlag = new CheckBox();
            this.zeroFlag = new CheckBox();
            this.carryFlag = new CheckBox();
            this.registerGroup = new GroupBox();
            this.yRegisterInput = new TextBox();
            this.label4 = new Label();
            this.xRegisterInput = new TextBox();
            this.label3 = new Label();
            this.accumulatorInput = new TextBox();
            this.label2 = new Label();
            this.stackPointerInput = new TextBox();
            this.label1 = new Label();
            this.programCounterInput = new TextBox();
            this.programCounterLabel = new Label();
            this.stateGroup = new GroupBox();
            this.triggerInterruptButton = new Button();
            this.softwareInterruptFlag = new CheckBox();
            this.hardwareInterruptFlag = new CheckBox();
            this.opcodeInput = new TextBox();
            this.label5 = new Label();
            this.cyclesInput = new TextBox();
            this.cyclesLabel = new Label();
            this.executionGroup = new GroupBox();
            this.programText = new TextBox();
            this.resetButton = new Button();
            this.executionContent = new TextBox();
            this.instructionButton = new Button();
            this.clockButton = new Button();
            this.menuStrip.SuspendLayout();
            this.flagGroup.SuspendLayout();
            this.registerGroup.SuspendLayout();
            this.stateGroup.SuspendLayout();
            this.executionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new ToolStripItem[] { this.FileItem });
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(622, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // FileItem
            // 
            this.FileItem.DropDownItems.AddRange(new ToolStripItem[] { this.openToolStripMenuItem, this.saveStateToolStripMenuItem, this.loadStateToolStripMenuItem });
            this.FileItem.Name = "FileItem";
            this.FileItem.Size = new Size(37, 20);
            this.FileItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new Size(138, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += this.OpenToolStripMenuItem_Click;
            // 
            // saveStateToolStripMenuItem
            // 
            this.saveStateToolStripMenuItem.Enabled = false;
            this.saveStateToolStripMenuItem.Name = "saveStateToolStripMenuItem";
            this.saveStateToolStripMenuItem.Size = new Size(138, 22);
            this.saveStateToolStripMenuItem.Text = "Save State...";
            this.saveStateToolStripMenuItem.Click += this.SaveStateToolStripMenuItem_Click;
            // 
            // loadStateToolStripMenuItem
            // 
            this.loadStateToolStripMenuItem.Name = "loadStateToolStripMenuItem";
            this.loadStateToolStripMenuItem.Size = new Size(138, 22);
            this.loadStateToolStripMenuItem.Text = "Load State...";
            this.loadStateToolStripMenuItem.Click += this.LoadStateToolStripMenuItem_Click;
            // 
            // flagGroup
            // 
            this.flagGroup.Controls.Add(this.negativeFlag);
            this.flagGroup.Controls.Add(this.overflowFlag);
            this.flagGroup.Controls.Add(this.breakFlag);
            this.flagGroup.Controls.Add(this.decimalFlag);
            this.flagGroup.Controls.Add(this.interruptFlag);
            this.flagGroup.Controls.Add(this.zeroFlag);
            this.flagGroup.Controls.Add(this.carryFlag);
            this.flagGroup.Location = new Point(484, 27);
            this.flagGroup.Name = "flagGroup";
            this.flagGroup.Size = new Size(129, 201);
            this.flagGroup.TabIndex = 1;
            this.flagGroup.TabStop = false;
            this.flagGroup.Text = "Flags";
            // 
            // negativeFlag
            // 
            this.negativeFlag.AutoSize = true;
            this.negativeFlag.Enabled = false;
            this.negativeFlag.Location = new Point(6, 172);
            this.negativeFlag.Name = "negativeFlag";
            this.negativeFlag.Size = new Size(73, 19);
            this.negativeFlag.TabIndex = 6;
            this.negativeFlag.Text = "Negative";
            this.negativeFlag.UseVisualStyleBackColor = true;
            // 
            // overflowFlag
            // 
            this.overflowFlag.AutoSize = true;
            this.overflowFlag.Enabled = false;
            this.overflowFlag.Location = new Point(6, 147);
            this.overflowFlag.Name = "overflowFlag";
            this.overflowFlag.Size = new Size(74, 19);
            this.overflowFlag.TabIndex = 5;
            this.overflowFlag.Text = "Overflow";
            this.overflowFlag.UseVisualStyleBackColor = true;
            // 
            // breakFlag
            // 
            this.breakFlag.AutoSize = true;
            this.breakFlag.Enabled = false;
            this.breakFlag.Location = new Point(6, 122);
            this.breakFlag.Name = "breakFlag";
            this.breakFlag.Size = new Size(113, 19);
            this.breakFlag.TabIndex = 4;
            this.breakFlag.Text = "Break command";
            this.breakFlag.UseVisualStyleBackColor = true;
            // 
            // decimalFlag
            // 
            this.decimalFlag.AutoSize = true;
            this.decimalFlag.Enabled = false;
            this.decimalFlag.Location = new Point(6, 97);
            this.decimalFlag.Name = "decimalFlag";
            this.decimalFlag.Size = new Size(103, 19);
            this.decimalFlag.TabIndex = 3;
            this.decimalFlag.Text = "Decimal mode";
            this.decimalFlag.UseVisualStyleBackColor = true;
            // 
            // interruptFlag
            // 
            this.interruptFlag.AutoSize = true;
            this.interruptFlag.Enabled = false;
            this.interruptFlag.Location = new Point(6, 72);
            this.interruptFlag.Name = "interruptFlag";
            this.interruptFlag.Size = new Size(119, 19);
            this.interruptFlag.TabIndex = 2;
            this.interruptFlag.Text = "Interrupt disabled";
            this.interruptFlag.UseVisualStyleBackColor = true;
            // 
            // zeroFlag
            // 
            this.zeroFlag.AutoSize = true;
            this.zeroFlag.Enabled = false;
            this.zeroFlag.Location = new Point(6, 47);
            this.zeroFlag.Name = "zeroFlag";
            this.zeroFlag.Size = new Size(50, 19);
            this.zeroFlag.TabIndex = 1;
            this.zeroFlag.Text = "Zero";
            this.zeroFlag.UseVisualStyleBackColor = true;
            // 
            // carryFlag
            // 
            this.carryFlag.AutoSize = true;
            this.carryFlag.Enabled = false;
            this.carryFlag.Location = new Point(6, 22);
            this.carryFlag.Name = "carryFlag";
            this.carryFlag.Size = new Size(54, 19);
            this.carryFlag.TabIndex = 0;
            this.carryFlag.Text = "Carry";
            this.carryFlag.UseVisualStyleBackColor = true;
            // 
            // registerGroup
            // 
            this.registerGroup.Controls.Add(this.yRegisterInput);
            this.registerGroup.Controls.Add(this.label4);
            this.registerGroup.Controls.Add(this.xRegisterInput);
            this.registerGroup.Controls.Add(this.label3);
            this.registerGroup.Controls.Add(this.accumulatorInput);
            this.registerGroup.Controls.Add(this.label2);
            this.registerGroup.Controls.Add(this.stackPointerInput);
            this.registerGroup.Controls.Add(this.label1);
            this.registerGroup.Controls.Add(this.programCounterInput);
            this.registerGroup.Controls.Add(this.programCounterLabel);
            this.registerGroup.Location = new Point(336, 234);
            this.registerGroup.Name = "registerGroup";
            this.registerGroup.Size = new Size(277, 170);
            this.registerGroup.TabIndex = 2;
            this.registerGroup.TabStop = false;
            this.registerGroup.Text = "Registers";
            // 
            // yRegisterInput
            // 
            this.yRegisterInput.Location = new Point(114, 132);
            this.yRegisterInput.Name = "yRegisterInput";
            this.yRegisterInput.ReadOnly = true;
            this.yRegisterInput.Size = new Size(153, 23);
            this.yRegisterInput.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(6, 135);
            this.label4.Name = "label4";
            this.label4.Size = new Size(62, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Y Register:";
            // 
            // xRegisterInput
            // 
            this.xRegisterInput.Location = new Point(114, 103);
            this.xRegisterInput.Name = "xRegisterInput";
            this.xRegisterInput.ReadOnly = true;
            this.xRegisterInput.Size = new Size(153, 23);
            this.xRegisterInput.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(6, 106);
            this.label3.Name = "label3";
            this.label3.Size = new Size(62, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "X Register:";
            // 
            // accumulatorInput
            // 
            this.accumulatorInput.Location = new Point(114, 74);
            this.accumulatorInput.Name = "accumulatorInput";
            this.accumulatorInput.ReadOnly = true;
            this.accumulatorInput.Size = new Size(153, 23);
            this.accumulatorInput.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new Size(79, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Accumulator:";
            // 
            // stackPointerInput
            // 
            this.stackPointerInput.Location = new Point(114, 45);
            this.stackPointerInput.Name = "stackPointerInput";
            this.stackPointerInput.ReadOnly = true;
            this.stackPointerInput.Size = new Size(153, 23);
            this.stackPointerInput.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new Size(79, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Stack Pointer:";
            // 
            // programCounterInput
            // 
            this.programCounterInput.Location = new Point(114, 16);
            this.programCounterInput.Name = "programCounterInput";
            this.programCounterInput.ReadOnly = true;
            this.programCounterInput.Size = new Size(153, 23);
            this.programCounterInput.TabIndex = 1;
            // 
            // programCounterLabel
            // 
            this.programCounterLabel.AutoSize = true;
            this.programCounterLabel.Location = new Point(6, 19);
            this.programCounterLabel.Name = "programCounterLabel";
            this.programCounterLabel.Size = new Size(102, 15);
            this.programCounterLabel.TabIndex = 0;
            this.programCounterLabel.Text = "Program Counter:";
            // 
            // stateGroup
            // 
            this.stateGroup.Controls.Add(this.triggerInterruptButton);
            this.stateGroup.Controls.Add(this.softwareInterruptFlag);
            this.stateGroup.Controls.Add(this.hardwareInterruptFlag);
            this.stateGroup.Controls.Add(this.opcodeInput);
            this.stateGroup.Controls.Add(this.label5);
            this.stateGroup.Controls.Add(this.cyclesInput);
            this.stateGroup.Controls.Add(this.cyclesLabel);
            this.stateGroup.Location = new Point(336, 27);
            this.stateGroup.Name = "stateGroup";
            this.stateGroup.Size = new Size(142, 201);
            this.stateGroup.TabIndex = 3;
            this.stateGroup.TabStop = false;
            this.stateGroup.Text = "State";
            // 
            // triggerInterruptButton
            // 
            this.triggerInterruptButton.Location = new Point(6, 122);
            this.triggerInterruptButton.Name = "triggerInterruptButton";
            this.triggerInterruptButton.Size = new Size(130, 23);
            this.triggerInterruptButton.TabIndex = 6;
            this.triggerInterruptButton.Text = "Trigger HW Interrupt";
            this.triggerInterruptButton.UseVisualStyleBackColor = true;
            this.triggerInterruptButton.Click += this.TriggerInterruptButton_Click;
            // 
            // softwareInterruptFlag
            // 
            this.softwareInterruptFlag.AutoSize = true;
            this.softwareInterruptFlag.Enabled = false;
            this.softwareInterruptFlag.Location = new Point(6, 97);
            this.softwareInterruptFlag.Name = "softwareInterruptFlag";
            this.softwareInterruptFlag.Size = new Size(121, 19);
            this.softwareInterruptFlag.TabIndex = 5;
            this.softwareInterruptFlag.Text = "Software Interrupt";
            this.softwareInterruptFlag.UseVisualStyleBackColor = true;
            // 
            // hardwareInterruptFlag
            // 
            this.hardwareInterruptFlag.AutoSize = true;
            this.hardwareInterruptFlag.Enabled = false;
            this.hardwareInterruptFlag.Location = new Point(6, 72);
            this.hardwareInterruptFlag.Name = "hardwareInterruptFlag";
            this.hardwareInterruptFlag.Size = new Size(126, 19);
            this.hardwareInterruptFlag.TabIndex = 4;
            this.hardwareInterruptFlag.Text = "Hardware Interrupt";
            this.hardwareInterruptFlag.UseVisualStyleBackColor = true;
            // 
            // opcodeInput
            // 
            this.opcodeInput.Location = new Point(76, 45);
            this.opcodeInput.Name = "opcodeInput";
            this.opcodeInput.ReadOnly = true;
            this.opcodeInput.Size = new Size(60, 23);
            this.opcodeInput.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new Size(52, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Opcode:";
            // 
            // cyclesInput
            // 
            this.cyclesInput.Location = new Point(76, 16);
            this.cyclesInput.Name = "cyclesInput";
            this.cyclesInput.ReadOnly = true;
            this.cyclesInput.Size = new Size(60, 23);
            this.cyclesInput.TabIndex = 1;
            // 
            // cyclesLabel
            // 
            this.cyclesLabel.AutoSize = true;
            this.cyclesLabel.Location = new Point(6, 19);
            this.cyclesLabel.Name = "cyclesLabel";
            this.cyclesLabel.Size = new Size(64, 15);
            this.cyclesLabel.TabIndex = 0;
            this.cyclesLabel.Text = "Cycles left:";
            // 
            // executionGroup
            // 
            this.executionGroup.Controls.Add(this.programText);
            this.executionGroup.Controls.Add(this.resetButton);
            this.executionGroup.Controls.Add(this.executionContent);
            this.executionGroup.Controls.Add(this.instructionButton);
            this.executionGroup.Controls.Add(this.clockButton);
            this.executionGroup.Location = new Point(12, 27);
            this.executionGroup.Name = "executionGroup";
            this.executionGroup.Size = new Size(318, 377);
            this.executionGroup.TabIndex = 4;
            this.executionGroup.TabStop = false;
            this.executionGroup.Text = "Execution";
            // 
            // programText
            // 
            this.programText.Location = new Point(6, 51);
            this.programText.Multiline = true;
            this.programText.Name = "programText";
            this.programText.ScrollBars = ScrollBars.Vertical;
            this.programText.Size = new Size(96, 311);
            this.programText.TabIndex = 4;
            // 
            // resetButton
            // 
            this.resetButton.Enabled = false;
            this.resetButton.Location = new Point(234, 22);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new Size(75, 23);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += this.ResetButton_Click;
            // 
            // executionContent
            // 
            this.executionContent.Location = new Point(108, 51);
            this.executionContent.Multiline = true;
            this.executionContent.Name = "executionContent";
            this.executionContent.ScrollBars = ScrollBars.Vertical;
            this.executionContent.Size = new Size(201, 311);
            this.executionContent.TabIndex = 2;
            // 
            // instructionButton
            // 
            this.instructionButton.Enabled = false;
            this.instructionButton.Location = new Point(108, 22);
            this.instructionButton.Name = "instructionButton";
            this.instructionButton.Size = new Size(120, 23);
            this.instructionButton.TabIndex = 1;
            this.instructionButton.Text = "Cycle instruction";
            this.instructionButton.UseVisualStyleBackColor = true;
            this.instructionButton.Click += this.InstructionButton_Click;
            // 
            // clockButton
            // 
            this.clockButton.Enabled = false;
            this.clockButton.Location = new Point(6, 22);
            this.clockButton.Name = "clockButton";
            this.clockButton.Size = new Size(96, 23);
            this.clockButton.TabIndex = 0;
            this.clockButton.Text = "Cycle clock";
            this.clockButton.UseVisualStyleBackColor = true;
            this.clockButton.Click += this.ClockButton_Click;
            // 
            // CpuView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(622, 413);
            this.Controls.Add(this.executionGroup);
            this.Controls.Add(this.stateGroup);
            this.Controls.Add(this.registerGroup);
            this.Controls.Add(this.flagGroup);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximumSize = new Size(638, 452);
            this.MinimumSize = new Size(638, 452);
            this.Name = "CpuView";
            this.Text = "6502 Emulator";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.flagGroup.ResumeLayout(false);
            this.flagGroup.PerformLayout();
            this.registerGroup.ResumeLayout(false);
            this.registerGroup.PerformLayout();
            this.stateGroup.ResumeLayout(false);
            this.stateGroup.PerformLayout();
            this.executionGroup.ResumeLayout(false);
            this.executionGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem FileItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private GroupBox flagGroup;
        private CheckBox carryFlag;
        private CheckBox zeroFlag;
        private CheckBox decimalFlag;
        private CheckBox interruptFlag;
        private CheckBox negativeFlag;
        private CheckBox overflowFlag;
        private CheckBox breakFlag;
        private GroupBox registerGroup;
        private Label programCounterLabel;
        private TextBox programCounterInput;
        private TextBox yRegisterInput;
        private Label label4;
        private TextBox xRegisterInput;
        private Label label3;
        private TextBox accumulatorInput;
        private Label label2;
        private TextBox stackPointerInput;
        private Label label1;
        private GroupBox stateGroup;
        private Label cyclesLabel;
        private TextBox cyclesInput;
        private TextBox opcodeInput;
        private Label label5;
        private CheckBox softwareInterruptFlag;
        private CheckBox hardwareInterruptFlag;
        private GroupBox executionGroup;
        private Button instructionButton;
        private Button clockButton;
        private TextBox executionContent;
        private Button resetButton;
        private ToolStripMenuItem saveStateToolStripMenuItem;
        private ToolStripMenuItem loadStateToolStripMenuItem;
        private Button triggerInterruptButton;
        private TextBox programText;
    }
}