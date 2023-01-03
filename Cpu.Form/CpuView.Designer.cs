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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.FileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flagGroup = new System.Windows.Forms.GroupBox();
            this.negativeFlag = new System.Windows.Forms.CheckBox();
            this.overflowFlag = new System.Windows.Forms.CheckBox();
            this.breakFlag = new System.Windows.Forms.CheckBox();
            this.decimalFlag = new System.Windows.Forms.CheckBox();
            this.interruptFlag = new System.Windows.Forms.CheckBox();
            this.zeroFlag = new System.Windows.Forms.CheckBox();
            this.carryFlag = new System.Windows.Forms.CheckBox();
            this.registerGroup = new System.Windows.Forms.GroupBox();
            this.yRegisterInput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.xRegisterInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.accumulatorInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.stackPointerInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.programCounterInput = new System.Windows.Forms.TextBox();
            this.programCounterLabel = new System.Windows.Forms.Label();
            this.stateGroup = new System.Windows.Forms.GroupBox();
            this.triggerInterruptButton = new System.Windows.Forms.Button();
            this.softwareInterruptFlag = new System.Windows.Forms.CheckBox();
            this.hardwareInterruptFlag = new System.Windows.Forms.CheckBox();
            this.opcodeInput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cyclesInput = new System.Windows.Forms.TextBox();
            this.cyclesLabel = new System.Windows.Forms.Label();
            this.executionGroup = new System.Windows.Forms.GroupBox();
            this.programText = new System.Windows.Forms.TextBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.executionContent = new System.Windows.Forms.TextBox();
            this.instructionButton = new System.Windows.Forms.Button();
            this.clockButton = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.flagGroup.SuspendLayout();
            this.registerGroup.SuspendLayout();
            this.stateGroup.SuspendLayout();
            this.executionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // FileItem
            // 
            this.FileItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveStateToolStripMenuItem,
            this.loadStateToolStripMenuItem});
            this.FileItem.Name = "FileItem";
            this.FileItem.Size = new System.Drawing.Size(37, 20);
            this.FileItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveStateToolStripMenuItem
            // 
            this.saveStateToolStripMenuItem.Enabled = false;
            this.saveStateToolStripMenuItem.Name = "saveStateToolStripMenuItem";
            this.saveStateToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.saveStateToolStripMenuItem.Text = "Save State...";
            this.saveStateToolStripMenuItem.Click += new System.EventHandler(this.SaveStateToolStripMenuItem_Click);
            // 
            // loadStateToolStripMenuItem
            // 
            this.loadStateToolStripMenuItem.Name = "loadStateToolStripMenuItem";
            this.loadStateToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.loadStateToolStripMenuItem.Text = "Load State...";
            this.loadStateToolStripMenuItem.Click += new System.EventHandler(this.LoadStateToolStripMenuItem_Click);
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
            this.flagGroup.Location = new System.Drawing.Point(659, 27);
            this.flagGroup.Name = "flagGroup";
            this.flagGroup.Size = new System.Drawing.Size(129, 201);
            this.flagGroup.TabIndex = 1;
            this.flagGroup.TabStop = false;
            this.flagGroup.Text = "Flags";
            // 
            // negativeFlag
            // 
            this.negativeFlag.AutoSize = true;
            this.negativeFlag.Enabled = false;
            this.negativeFlag.Location = new System.Drawing.Point(6, 172);
            this.negativeFlag.Name = "negativeFlag";
            this.negativeFlag.Size = new System.Drawing.Size(73, 19);
            this.negativeFlag.TabIndex = 6;
            this.negativeFlag.Text = "Negative";
            this.negativeFlag.UseVisualStyleBackColor = true;
            // 
            // overflowFlag
            // 
            this.overflowFlag.AutoSize = true;
            this.overflowFlag.Enabled = false;
            this.overflowFlag.Location = new System.Drawing.Point(6, 147);
            this.overflowFlag.Name = "overflowFlag";
            this.overflowFlag.Size = new System.Drawing.Size(74, 19);
            this.overflowFlag.TabIndex = 5;
            this.overflowFlag.Text = "Overflow";
            this.overflowFlag.UseVisualStyleBackColor = true;
            // 
            // breakFlag
            // 
            this.breakFlag.AutoSize = true;
            this.breakFlag.Enabled = false;
            this.breakFlag.Location = new System.Drawing.Point(6, 122);
            this.breakFlag.Name = "breakFlag";
            this.breakFlag.Size = new System.Drawing.Size(113, 19);
            this.breakFlag.TabIndex = 4;
            this.breakFlag.Text = "Break command";
            this.breakFlag.UseVisualStyleBackColor = true;
            // 
            // decimalFlag
            // 
            this.decimalFlag.AutoSize = true;
            this.decimalFlag.Enabled = false;
            this.decimalFlag.Location = new System.Drawing.Point(6, 97);
            this.decimalFlag.Name = "decimalFlag";
            this.decimalFlag.Size = new System.Drawing.Size(103, 19);
            this.decimalFlag.TabIndex = 3;
            this.decimalFlag.Text = "Decimal mode";
            this.decimalFlag.UseVisualStyleBackColor = true;
            // 
            // interruptFlag
            // 
            this.interruptFlag.AutoSize = true;
            this.interruptFlag.Enabled = false;
            this.interruptFlag.Location = new System.Drawing.Point(6, 72);
            this.interruptFlag.Name = "interruptFlag";
            this.interruptFlag.Size = new System.Drawing.Size(119, 19);
            this.interruptFlag.TabIndex = 2;
            this.interruptFlag.Text = "Interrupt disabled";
            this.interruptFlag.UseVisualStyleBackColor = true;
            // 
            // zeroFlag
            // 
            this.zeroFlag.AutoSize = true;
            this.zeroFlag.Enabled = false;
            this.zeroFlag.Location = new System.Drawing.Point(6, 47);
            this.zeroFlag.Name = "zeroFlag";
            this.zeroFlag.Size = new System.Drawing.Size(50, 19);
            this.zeroFlag.TabIndex = 1;
            this.zeroFlag.Text = "Zero";
            this.zeroFlag.UseVisualStyleBackColor = true;
            // 
            // carryFlag
            // 
            this.carryFlag.AutoSize = true;
            this.carryFlag.Enabled = false;
            this.carryFlag.Location = new System.Drawing.Point(6, 22);
            this.carryFlag.Name = "carryFlag";
            this.carryFlag.Size = new System.Drawing.Size(54, 19);
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
            this.registerGroup.Location = new System.Drawing.Point(511, 234);
            this.registerGroup.Name = "registerGroup";
            this.registerGroup.Size = new System.Drawing.Size(277, 170);
            this.registerGroup.TabIndex = 2;
            this.registerGroup.TabStop = false;
            this.registerGroup.Text = "Registers";
            // 
            // yRegisterInput
            // 
            this.yRegisterInput.Location = new System.Drawing.Point(114, 132);
            this.yRegisterInput.Name = "yRegisterInput";
            this.yRegisterInput.ReadOnly = true;
            this.yRegisterInput.Size = new System.Drawing.Size(153, 23);
            this.yRegisterInput.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Y Register:";
            // 
            // xRegisterInput
            // 
            this.xRegisterInput.Location = new System.Drawing.Point(114, 103);
            this.xRegisterInput.Name = "xRegisterInput";
            this.xRegisterInput.ReadOnly = true;
            this.xRegisterInput.Size = new System.Drawing.Size(153, 23);
            this.xRegisterInput.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "X Register:";
            // 
            // accumulatorInput
            // 
            this.accumulatorInput.Location = new System.Drawing.Point(114, 74);
            this.accumulatorInput.Name = "accumulatorInput";
            this.accumulatorInput.ReadOnly = true;
            this.accumulatorInput.Size = new System.Drawing.Size(153, 23);
            this.accumulatorInput.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Accumulator:";
            // 
            // stackPointerInput
            // 
            this.stackPointerInput.Location = new System.Drawing.Point(114, 45);
            this.stackPointerInput.Name = "stackPointerInput";
            this.stackPointerInput.ReadOnly = true;
            this.stackPointerInput.Size = new System.Drawing.Size(153, 23);
            this.stackPointerInput.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Stack Pointer:";
            // 
            // programCounterInput
            // 
            this.programCounterInput.Location = new System.Drawing.Point(114, 16);
            this.programCounterInput.Name = "programCounterInput";
            this.programCounterInput.ReadOnly = true;
            this.programCounterInput.Size = new System.Drawing.Size(153, 23);
            this.programCounterInput.TabIndex = 1;
            // 
            // programCounterLabel
            // 
            this.programCounterLabel.AutoSize = true;
            this.programCounterLabel.Location = new System.Drawing.Point(6, 19);
            this.programCounterLabel.Name = "programCounterLabel";
            this.programCounterLabel.Size = new System.Drawing.Size(102, 15);
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
            this.stateGroup.Location = new System.Drawing.Point(511, 27);
            this.stateGroup.Name = "stateGroup";
            this.stateGroup.Size = new System.Drawing.Size(142, 201);
            this.stateGroup.TabIndex = 3;
            this.stateGroup.TabStop = false;
            this.stateGroup.Text = "State";
            // 
            // triggerInterruptButton
            // 
            this.triggerInterruptButton.Location = new System.Drawing.Point(6, 122);
            this.triggerInterruptButton.Name = "triggerInterruptButton";
            this.triggerInterruptButton.Size = new System.Drawing.Size(130, 23);
            this.triggerInterruptButton.TabIndex = 6;
            this.triggerInterruptButton.Text = "Trigger HW Interrupt";
            this.triggerInterruptButton.UseVisualStyleBackColor = true;
            this.triggerInterruptButton.Click += new System.EventHandler(this.TriggerInterruptButton_Click);
            // 
            // softwareInterruptFlag
            // 
            this.softwareInterruptFlag.AutoSize = true;
            this.softwareInterruptFlag.Enabled = false;
            this.softwareInterruptFlag.Location = new System.Drawing.Point(6, 97);
            this.softwareInterruptFlag.Name = "softwareInterruptFlag";
            this.softwareInterruptFlag.Size = new System.Drawing.Size(121, 19);
            this.softwareInterruptFlag.TabIndex = 5;
            this.softwareInterruptFlag.Text = "Software Interrupt";
            this.softwareInterruptFlag.UseVisualStyleBackColor = true;
            // 
            // hardwareInterruptFlag
            // 
            this.hardwareInterruptFlag.AutoSize = true;
            this.hardwareInterruptFlag.Enabled = false;
            this.hardwareInterruptFlag.Location = new System.Drawing.Point(6, 72);
            this.hardwareInterruptFlag.Name = "hardwareInterruptFlag";
            this.hardwareInterruptFlag.Size = new System.Drawing.Size(126, 19);
            this.hardwareInterruptFlag.TabIndex = 4;
            this.hardwareInterruptFlag.Text = "Hardware Interrupt";
            this.hardwareInterruptFlag.UseVisualStyleBackColor = true;
            // 
            // opcodeInput
            // 
            this.opcodeInput.Location = new System.Drawing.Point(76, 45);
            this.opcodeInput.Name = "opcodeInput";
            this.opcodeInput.ReadOnly = true;
            this.opcodeInput.Size = new System.Drawing.Size(60, 23);
            this.opcodeInput.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Opcode:";
            // 
            // cyclesInput
            // 
            this.cyclesInput.Location = new System.Drawing.Point(76, 16);
            this.cyclesInput.Name = "cyclesInput";
            this.cyclesInput.ReadOnly = true;
            this.cyclesInput.Size = new System.Drawing.Size(60, 23);
            this.cyclesInput.TabIndex = 1;
            // 
            // cyclesLabel
            // 
            this.cyclesLabel.AutoSize = true;
            this.cyclesLabel.Location = new System.Drawing.Point(6, 19);
            this.cyclesLabel.Name = "cyclesLabel";
            this.cyclesLabel.Size = new System.Drawing.Size(64, 15);
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
            this.executionGroup.Location = new System.Drawing.Point(12, 27);
            this.executionGroup.Name = "executionGroup";
            this.executionGroup.Size = new System.Drawing.Size(493, 377);
            this.executionGroup.TabIndex = 4;
            this.executionGroup.TabStop = false;
            this.executionGroup.Text = "Execution";
            // 
            // programText
            // 
            this.programText.Location = new System.Drawing.Point(6, 51);
            this.programText.Multiline = true;
            this.programText.Name = "programText";
            this.programText.Size = new System.Drawing.Size(96, 311);
            this.programText.TabIndex = 4;
            // 
            // resetButton
            // 
            this.resetButton.Enabled = false;
            this.resetButton.Location = new System.Drawing.Point(412, 22);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // executionContent
            // 
            this.executionContent.Location = new System.Drawing.Point(108, 51);
            this.executionContent.Multiline = true;
            this.executionContent.Name = "executionContent";
            this.executionContent.Size = new System.Drawing.Size(379, 311);
            this.executionContent.TabIndex = 2;
            // 
            // instructionButton
            // 
            this.instructionButton.Enabled = false;
            this.instructionButton.Location = new System.Drawing.Point(108, 22);
            this.instructionButton.Name = "instructionButton";
            this.instructionButton.Size = new System.Drawing.Size(120, 23);
            this.instructionButton.TabIndex = 1;
            this.instructionButton.Text = "Cycle instruction";
            this.instructionButton.UseVisualStyleBackColor = true;
            this.instructionButton.Click += new System.EventHandler(this.InstructionButton_Click);
            // 
            // clockButton
            // 
            this.clockButton.Enabled = false;
            this.clockButton.Location = new System.Drawing.Point(6, 22);
            this.clockButton.Name = "clockButton";
            this.clockButton.Size = new System.Drawing.Size(96, 23);
            this.clockButton.TabIndex = 0;
            this.clockButton.Text = "Cycle clock";
            this.clockButton.UseVisualStyleBackColor = true;
            this.clockButton.Click += new System.EventHandler(this.ClockButton_Click);
            // 
            // CpuView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 422);
            this.Controls.Add(this.executionGroup);
            this.Controls.Add(this.stateGroup);
            this.Controls.Add(this.registerGroup);
            this.Controls.Add(this.flagGroup);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximumSize = new System.Drawing.Size(816, 461);
            this.MinimumSize = new System.Drawing.Size(816, 461);
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