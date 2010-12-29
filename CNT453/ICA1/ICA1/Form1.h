#pragma once


namespace ICA1 {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Summary for Form1
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class Form1 : public System::Windows::Forms::Form
	{
	public:
		Form1(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~Form1()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::TrackBar^  trackBar;
	private: System::Windows::Forms::TextBox^  inchesTextBox;

	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::TextBox^  mmTextBox;

	private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::Label^  label3;
	private: System::Windows::Forms::Label^  label4;

	protected: 

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->trackBar = (gcnew System::Windows::Forms::TrackBar());
			this->inchesTextBox = (gcnew System::Windows::Forms::TextBox());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->mmTextBox = (gcnew System::Windows::Forms::TextBox());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->label3 = (gcnew System::Windows::Forms::Label());
			this->label4 = (gcnew System::Windows::Forms::Label());
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->trackBar))->BeginInit();
			this->SuspendLayout();
			// 
			// trackBar
			// 
			this->trackBar->Location = System::Drawing::Point(12, 29);
			this->trackBar->Maximum = 50;
			this->trackBar->Minimum = 1;
			this->trackBar->Name = L"trackBar";
			this->trackBar->Size = System::Drawing::Size(610, 45);
			this->trackBar->TabIndex = 0;
			this->trackBar->TickStyle = System::Windows::Forms::TickStyle::TopLeft;
			this->trackBar->Value = 1;
			this->trackBar->Scroll += gcnew System::EventHandler(this, &Form1::trackBar_Scroll);
			// 
			// inchesTextBox
			// 
			this->inchesTextBox->BackColor = System::Drawing::Color::FromArgb(static_cast<System::Int32>(static_cast<System::Byte>(128)), static_cast<System::Int32>(static_cast<System::Byte>(255)), 
				static_cast<System::Int32>(static_cast<System::Byte>(255)));
			this->inchesTextBox->Cursor = System::Windows::Forms::Cursors::Default;
			this->inchesTextBox->ForeColor = System::Drawing::Color::Black;
			this->inchesTextBox->Location = System::Drawing::Point(202, 80);
			this->inchesTextBox->Name = L"inchesTextBox";
			this->inchesTextBox->ReadOnly = true;
			this->inchesTextBox->Size = System::Drawing::Size(91, 20);
			this->inchesTextBox->TabIndex = 1;
			this->inchesTextBox->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Location = System::Drawing::Point(212, 13);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(210, 13);
			this->label1->TabIndex = 2;
			this->label1->Text = L"Each tick on the slider is 1/32nd of an inch";
			// 
			// mmTextBox
			// 
			this->mmTextBox->BackColor = System::Drawing::Color::FromArgb(static_cast<System::Int32>(static_cast<System::Byte>(128)), static_cast<System::Int32>(static_cast<System::Byte>(255)), 
				static_cast<System::Int32>(static_cast<System::Byte>(255)));
			this->mmTextBox->Location = System::Drawing::Point(386, 80);
			this->mmTextBox->Name = L"mmTextBox";
			this->mmTextBox->ReadOnly = true;
			this->mmTextBox->Size = System::Drawing::Size(91, 20);
			this->mmTextBox->TabIndex = 3;
			this->mmTextBox->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			this->mmTextBox->TextChanged += gcnew System::EventHandler(this, &Form1::mmTextBox_TextChanged);
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->Location = System::Drawing::Point(157, 83);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(39, 13);
			this->label2->TabIndex = 4;
			this->label2->Text = L"Inches";
			// 
			// label3
			// 
			this->label3->AutoSize = true;
			this->label3->Location = System::Drawing::Point(357, 83);
			this->label3->Name = L"label3";
			this->label3->Size = System::Drawing::Size(23, 13);
			this->label3->TabIndex = 5;
			this->label3->Text = L"mm";
			// 
			// label4
			// 
			this->label4->AutoSize = true;
			this->label4->Location = System::Drawing::Point(531, 83);
			this->label4->Name = L"label4";
			this->label4->Size = System::Drawing::Size(91, 13);
			this->label4->TabIndex = 6;
			this->label4->Text = L"Addison Babcock";
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(634, 113);
			this->Controls->Add(this->label4);
			this->Controls->Add(this->label3);
			this->Controls->Add(this->label2);
			this->Controls->Add(this->mmTextBox);
			this->Controls->Add(this->label1);
			this->Controls->Add(this->inchesTextBox);
			this->Controls->Add(this->trackBar);
			this->Name = L"Form1";
			this->Text = L"ICA #1 - Tiny Distance Con-Ver-Tor";
			this->Load += gcnew System::EventHandler(this, &Form1::Form1_Load);
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->trackBar))->EndInit();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: 
		void Convert (double dNotches)
		{
			this->inchesTextBox->Text = (dNotches / 32).ToString ("0.00'\"'");
			this->mmTextBox->Text = (dNotches / 32 * 25.4).ToString ("#0.00");
		}
		System::Void Form1_Load(System::Object^  sender, System::EventArgs^  e) 
		{
			Convert (1.0);
		}
		System::Void trackBar_Scroll(System::Object^  sender, System::EventArgs^  e) 
		{
			Convert (this->trackBar->Value);
		}
private: System::Void mmTextBox_TextChanged(System::Object^  sender, System::EventArgs^  e) {
		 }
};
}

