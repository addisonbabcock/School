/***********************************************************
Project: Lab 03 - Drawing Line Thing w/ Sockets
Files: Form1.h, Form1.cpp, PenSelector.h, PenSelector.cpp,
	GetIPDialog.h, GetIPDialog.cpp, StdAfx.h
Date: 19 Oct 07
Author: Addison Babcock Class: CNT4K
Instructor: Simon Walker Course: CNT453
***********************************************************/
#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;

namespace LAB2 {

	/// <summary>
	/// Summary for PenSelector
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	
	public delegate void delVoidVoid (void);

	public ref class PenSelector : public System::Windows::Forms::Form
	{
	public:
		//delegate pointing at Form1::_ColorChanged ()
		delVoidVoid ^ colorChanged;
		
		PenSelector(void);

	protected:
		~PenSelector();

	private: 
		System::Windows::Forms::Label^  UI_Label_CurrentColor;
		System::Windows::Forms::Label^  UI_Label_CurrentColourDisplay;
		System::Windows::Forms::Label^  UI_Label_PenSize;
		System::Windows::Forms::DomainUpDown^  UI_Domain_PenSize;
		System::Windows::Forms::Label^  UI_Label_PenAlpha;
		System::Windows::Forms::DomainUpDown^  UI_Domain_PenAlpha;
		System::Windows::Forms::ColorDialog^  colorPicker;
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->UI_Label_CurrentColor = (gcnew System::Windows::Forms::Label());
			this->UI_Label_CurrentColourDisplay = (gcnew System::Windows::Forms::Label());
			this->UI_Label_PenSize = (gcnew System::Windows::Forms::Label());
			this->UI_Domain_PenSize = (gcnew System::Windows::Forms::DomainUpDown());
			this->UI_Label_PenAlpha = (gcnew System::Windows::Forms::Label());
			this->UI_Domain_PenAlpha = (gcnew System::Windows::Forms::DomainUpDown());
			this->colorPicker = (gcnew System::Windows::Forms::ColorDialog());
			this->SuspendLayout();
			// 
			// UI_Label_CurrentColor
			// 
			this->UI_Label_CurrentColor->AutoSize = true;
			this->UI_Label_CurrentColor->Location = System::Drawing::Point(12, 9);
			this->UI_Label_CurrentColor->Name = L"UI_Label_CurrentColor";
			this->UI_Label_CurrentColor->Size = System::Drawing::Size(71, 13);
			this->UI_Label_CurrentColor->TabIndex = 0;
			this->UI_Label_CurrentColor->Text = L"Current Color:";
			// 
			// UI_Label_CurrentColourDisplay
			// 
			this->UI_Label_CurrentColourDisplay->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Bottom) 
				| System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->UI_Label_CurrentColourDisplay->BackColor = System::Drawing::Color::White;
			this->UI_Label_CurrentColourDisplay->Location = System::Drawing::Point(12, 22);
			this->UI_Label_CurrentColourDisplay->Name = L"UI_Label_CurrentColourDisplay";
			this->UI_Label_CurrentColourDisplay->Size = System::Drawing::Size(268, 223);
			this->UI_Label_CurrentColourDisplay->TabIndex = 1;
			this->UI_Label_CurrentColourDisplay->Click += gcnew System::EventHandler(this, &PenSelector::UI_Label_CurrentColourDisplay_Click);
			// 
			// UI_Label_PenSize
			// 
			this->UI_Label_PenSize->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left));
			this->UI_Label_PenSize->AutoSize = true;
			this->UI_Label_PenSize->Location = System::Drawing::Point(12, 245);
			this->UI_Label_PenSize->Name = L"UI_Label_PenSize";
			this->UI_Label_PenSize->Size = System::Drawing::Size(81, 13);
			this->UI_Label_PenSize->TabIndex = 2;
			this->UI_Label_PenSize->Text = L"Pen Thickness:";
			// 
			// UI_Domain_PenSize
			// 
			this->UI_Domain_PenSize->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->UI_Domain_PenSize->Location = System::Drawing::Point(12, 261);
			this->UI_Domain_PenSize->Name = L"UI_Domain_PenSize";
			this->UI_Domain_PenSize->Size = System::Drawing::Size(267, 20);
			this->UI_Domain_PenSize->TabIndex = 3;
			this->UI_Domain_PenSize->Text = L"1";
			// 
			// UI_Label_PenAlpha
			// 
			this->UI_Label_PenAlpha->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left));
			this->UI_Label_PenAlpha->AutoSize = true;
			this->UI_Label_PenAlpha->Location = System::Drawing::Point(12, 284);
			this->UI_Label_PenAlpha->Name = L"UI_Label_PenAlpha";
			this->UI_Label_PenAlpha->Size = System::Drawing::Size(59, 13);
			this->UI_Label_PenAlpha->TabIndex = 4;
			this->UI_Label_PenAlpha->Text = L"Pen Alpha:";
			// 
			// UI_Domain_PenAlpha
			// 
			this->UI_Domain_PenAlpha->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->UI_Domain_PenAlpha->Location = System::Drawing::Point(12, 300);
			this->UI_Domain_PenAlpha->Name = L"UI_Domain_PenAlpha";
			this->UI_Domain_PenAlpha->Size = System::Drawing::Size(267, 20);
			this->UI_Domain_PenAlpha->TabIndex = 5;
			this->UI_Domain_PenAlpha->Text = L"0";
			this->UI_Domain_PenAlpha->TextChanged += gcnew System::EventHandler(this, &PenSelector::UI_Domain_PenAlpha_TextChanged);
			// 
			// colorPicker
			// 
			this->colorPicker->AnyColor = true;
			this->colorPicker->FullOpen = true;
			this->colorPicker->ShowHelp = true;
			// 
			// PenSelector
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(292, 332);
			this->Controls->Add(this->UI_Domain_PenAlpha);
			this->Controls->Add(this->UI_Label_PenAlpha);
			this->Controls->Add(this->UI_Domain_PenSize);
			this->Controls->Add(this->UI_Label_PenSize);
			this->Controls->Add(this->UI_Label_CurrentColourDisplay);
			this->Controls->Add(this->UI_Label_CurrentColor);
			this->Name = L"PenSelector";
			this->Text = L"PenSelector";
			this->FormClosing += gcnew System::Windows::Forms::FormClosingEventHandler(this, &PenSelector::PenSelector_FormClosing);
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	public:
		//the actual pen color is being stored in the background color of
		//UI_Label_CurrentColourDisplay
		property System::Drawing::Color p_PenColor
		{
			System::Drawing::Color get (void)
			{
				return UI_Label_CurrentColourDisplay->BackColor;
			}
			void set (System::Drawing::Color newColor)
			{
				UI_Label_CurrentColourDisplay->BackColor = newColor;
				colorPicker->Color = newColor;
				if (colorChanged)
					this->colorChanged ();
			}
		}

		//the actual pen size is being stored in the text property of 
		//UI_Domain_PenSize
		property int p_iPenSize
		{
			int get (void)
			{
				if (UI_Domain_PenSize->Text->Length > 0)
					return Convert::ToInt32 (UI_Domain_PenSize->Text);
				else
					return 0;
			}
			void set (int iNewPenSize)
			{
				UI_Domain_PenSize->Text = iNewPenSize.ToString ();
				colorChanged ();
			}
		}

		//the actual pen alpha is being stored in the text property of
		//UI_Domain_PenAlpha
		property int p_iPenAlpha
		{
			int get (void)
			{
				if (UI_Domain_PenAlpha->Text->Length > 0)
					return Convert::ToInt32 (UI_Domain_PenAlpha->Text);
				else
					return 0;
			}
			void set (int iNewAlpha)
			{
				UI_Domain_PenAlpha->Text = iNewAlpha.ToString ();
				colorChanged ();
			}
		}
	private: 
		System::Void UI_Label_CurrentColourDisplay_Click (
			System::Object^  sender, System::EventArgs^  e);
		System::Void UI_Domain_PenAlpha_TextChanged (
			System::Object^  sender, System::EventArgs^  e);
		System::Void PenSelector_FormClosing (System::Object^  sender, 
			System::Windows::Forms::FormClosingEventArgs^  e);
	};
}
