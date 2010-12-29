#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;


namespace ICA3 {

	/// <summary>
	/// Summary for select
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class select : public System::Windows::Forms::Form
	{
	public:
		select(void)
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
		~select()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::Button^  button1;
	private: System::Windows::Forms::Button^  button2;
	private: System::Windows::Forms::Label^  fontLabel;
	private: System::Windows::Forms::Label^  colorLabel;

	private: System::Windows::Forms::Button^  okButton;


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
			this->button1 = (gcnew System::Windows::Forms::Button());
			this->button2 = (gcnew System::Windows::Forms::Button());
			this->fontLabel = (gcnew System::Windows::Forms::Label());
			this->colorLabel = (gcnew System::Windows::Forms::Label());
			this->okButton = (gcnew System::Windows::Forms::Button());
			this->SuspendLayout();
			// 
			// button1
			// 
			this->button1->Location = System::Drawing::Point(13, 13);
			this->button1->Name = L"button1";
			this->button1->Size = System::Drawing::Size(114, 23);
			this->button1->TabIndex = 0;
			this->button1->Text = L"Select Font";
			this->button1->UseVisualStyleBackColor = true;
			this->button1->Click += gcnew System::EventHandler(this, &select::button1_Click);
			// 
			// button2
			// 
			this->button2->Location = System::Drawing::Point(13, 43);
			this->button2->Name = L"button2";
			this->button2->Size = System::Drawing::Size(114, 23);
			this->button2->TabIndex = 1;
			this->button2->Text = L"Select Color";
			this->button2->UseVisualStyleBackColor = true;
			this->button2->Click += gcnew System::EventHandler(this, &select::button2_Click);
			// 
			// fontLabel
			// 
			this->fontLabel->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->fontLabel->BorderStyle = System::Windows::Forms::BorderStyle::FixedSingle;
			this->fontLabel->Location = System::Drawing::Point(133, 13);
			this->fontLabel->Name = L"fontLabel";
			this->fontLabel->Size = System::Drawing::Size(502, 23);
			this->fontLabel->TabIndex = 2;
			this->fontLabel->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// colorLabel
			// 
			this->colorLabel->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->colorLabel->BorderStyle = System::Windows::Forms::BorderStyle::FixedSingle;
			this->colorLabel->ImageAlign = System::Drawing::ContentAlignment::TopRight;
			this->colorLabel->Location = System::Drawing::Point(133, 43);
			this->colorLabel->Name = L"colorLabel";
			this->colorLabel->Size = System::Drawing::Size(502, 23);
			this->colorLabel->TabIndex = 3;
			this->colorLabel->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// okButton
			// 
			this->okButton->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->okButton->Location = System::Drawing::Point(286, 69);
			this->okButton->Name = L"okButton";
			this->okButton->Size = System::Drawing::Size(75, 23);
			this->okButton->TabIndex = 4;
			this->okButton->Text = L"OK";
			this->okButton->UseVisualStyleBackColor = true;
			this->okButton->Click += gcnew System::EventHandler(this, &select::button3_Click);
			// 
			// select
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(647, 103);
			this->Controls->Add(this->okButton);
			this->Controls->Add(this->colorLabel);
			this->Controls->Add(this->fontLabel);
			this->Controls->Add(this->button2);
			this->Controls->Add(this->button1);
			this->MaximizeBox = false;
			this->MinimizeBox = false;
			this->Name = L"select";
			this->Text = L"Select Font and Color";
			this->TopMost = true;
			this->Load += gcnew System::EventHandler(this, &select::select_Load);
			this->ResumeLayout(false);

		}
#pragma endregion
	private: 
		System::Void select_Load(System::Object^  sender, System::EventArgs^  e) 
		{
			fontLabel->Text = _font->ToString ();
			colorLabel->Text = _color.ToString ();
		}
		System::Void button3_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			this->DialogResult = System::Windows::Forms::DialogResult::OK;
		}
		System::Void button1_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			System::Windows::Forms::FontDialog ^ fontDialog = gcnew System::Windows::Forms::FontDialog;
			fontDialog->ShowColor = false;
			fontDialog->Font = _font;
			if (fontDialog->ShowDialog () == System::Windows::Forms::DialogResult::OK)
			{
				_font = fontDialog->Font;
			}
		}
		System::Void button2_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			System::Windows::Forms::ColorDialog ^ colorDialog = gcnew System::Windows::Forms::ColorDialog;
			colorDialog->FullOpen = true;
			colorDialog->Color = _color;
			if (colorDialog->ShowDialog () == System::Windows::Forms::DialogResult::OK)
			{
				_color = colorDialog->Color;
			}
		}
	public:
		property System::Drawing::Font ^ _font
		{
			System::Drawing::Font ^ get (void)
			{
				return fontLabel->Font;
			}
			void set (System::Drawing::Font ^ font)
			{
				fontLabel->Text = font->ToString ();
				fontLabel->Font = font;
			}
		}
		property System::Drawing::Color  _color
		{
			System::Drawing::Color get (void)
			{
				return colorLabel->ForeColor;
			}
			void set (System::Drawing::Color color)
			{
				colorLabel->Text = color.ToString ();
				colorLabel->ForeColor = color;
			}
		}
	};
}
