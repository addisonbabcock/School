/****************************************************
Project: Lab 02 - Drawing Line Thing
Files: Form1.h, PenSelector.h
Date: 11 Oct 07
Author: Addison Babcock Class: CNT4K
Instructor: Simon Walker Course: CNT453
****************************************************/
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
		//Called when the color is changed
		//Set by Form1
		delVoidVoid ^ colorChanged;

		/***********************************************
		Function: PenSelector(void)
		Description: Initializes a PenSelector dialog
		Parameters: void
		Returns: nothing
		***********************************************/
		PenSelector(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//

			//Populate the domain for the pen size
			for (int i (256); i >= 1; --i)
				UI_Domain_PenSize->Items->Add (i);
			UI_Domain_PenSize->SelectedIndex = 10;

			//Populate the domain for the pen alpha
			for (int i (255); i >= 0; --i)
				UI_Domain_PenAlpha->Items->Add (i);

			//Set up the delegates
			colorChanged = nullptr;
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~PenSelector()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::Label^  UI_Label_CurrentColor;
	private: System::Windows::Forms::Label^  UI_Label_CurrentColourDisplay;
	private: System::Windows::Forms::Label^  UI_Label_PenSize;
	private: System::Windows::Forms::DomainUpDown^  UI_Domain_PenSize;
	private: System::Windows::Forms::Label^  UI_Label_PenAlpha;
	private: System::Windows::Forms::DomainUpDown^  UI_Domain_PenAlpha;
	private: System::Windows::Forms::ColorDialog^  colorPicker;
	protected: 

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
				return this->UI_Label_CurrentColourDisplay->BackColor;
			}
			void set (System::Drawing::Color newColor)
			{
				this->UI_Label_CurrentColourDisplay->BackColor = newColor;
				this->colorChanged ();
			}
		}

		//the actual pen size is being stored in the text property of 
		//UI_Domain_PenSize
		property int p_iPenSize
		{
			int get (void)
			{
				if (this->UI_Domain_PenSize->Text->Length > 0)
					return Convert::ToInt32 (this->UI_Domain_PenSize->Text);
				else
					return 0;
			}
			void set (int iNewPenSize)
			{
				this->UI_Domain_PenSize->Text = iNewPenSize.ToString ();
				this->colorChanged ();
			}
		}

		//the actual pen alpha is being stored in the text property of
		//UI_Domain_PenAlpha
		property int p_iPenAlpha
		{
			int get (void)
			{
				if (this->UI_Domain_PenAlpha->Text->Length > 0)
					return Convert::ToInt32 (this->UI_Domain_PenAlpha->Text);
				else
					return 0;
			}
			void set (int iNewAlpha)
			{
				this->UI_Domain_PenAlpha->Text = iNewAlpha.ToString ();
				this->colorChanged ();
			}
		}
	private: 
		/***********************************************
		Function: UI_Label_CurrentColourDisplay_Click
		Description: Event handler for when the label
		for displaying the color is clicked. Opens a 
		color picker dialog and saves the selected
		color.
		***********************************************/
		System::Void UI_Label_CurrentColourDisplay_Click(System::Object^sender,
			System::EventArgs^  e) 
		{
			//set the default value of the color picker to be the currently
			//selected color, then get and save a new color from the dialog
			colorPicker->Color = p_PenColor;
			colorPicker->ShowDialog ();
			p_PenColor = Color::FromArgb (p_iPenAlpha, colorPicker->Color.R,
								 colorPicker->Color.G, colorPicker->Color.B);
		}
		
		/***********************************************
		Function: UI_Domain_PenAlpha_TextChanged
		Description: Event handler to allow changes in 
		the text of the UI_Domain_PenAlpha control to be
		reflected in p_PenColor.
		***********************************************/
		System::Void UI_Domain_PenAlpha_TextChanged(System::Object^  sender, System::EventArgs^  e) 
		{
			//rebuild the color and save it
			System::Drawing::Color oldColor = this->p_PenColor;
			p_PenColor = Color::FromArgb (p_iPenAlpha, oldColor.R, oldColor.G, oldColor.B);
		}

		/***********************************************
		Function: PenSelector_FormClosing
		Description: Occurs when the user attempts to 
		close the form. This function cancels the close
		and hides the form instead so all the data is 
		still valid.
		***********************************************/
		System::Void PenSelector_FormClosing(System::Object^  sender, System::Windows::Forms::FormClosingEventArgs^  e) 
		{
			//prevent the user from closing the window, just hide it
			if (e->CloseReason == System::Windows::Forms::CloseReason::UserClosing)
			{
				this->Hide ();
				e->Cancel = true;
			}
		}
};
}
