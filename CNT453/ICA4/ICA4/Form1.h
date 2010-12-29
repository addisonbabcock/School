#pragma once
#include "support.h"


namespace ICA4 {

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
			selecter = gcnew support;
			selecter->m_delColourChanged = gcnew _dtagColourCallback (this, &Form1::ColourChangedInSelecter);

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
			this->SuspendLayout();
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->BackColor = System::Drawing::SystemColors::Control;
			this->ClientSize = System::Drawing::Size(292, 266);
			this->Name = L"Form1";
			this->Text = L"ICA #4 - Colour This! - Addison Babcock";
			this->Click += gcnew System::EventHandler(this, &Form1::Form1_Click);
			this->ResumeLayout(false);

		}
#pragma endregion

	private: 
		support ^ selecter;
		System::Void Form1_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			selecter->SetTrackers (this->BackColor, this->Opacity * 100);
			selecter->Show ();
		}

	public:
		void ColourChangedInSelecter (System::Drawing::Color newColour, int iOpacity)
		{
			this->BackColor = newColour;
			this->Opacity = iOpacity / 100.0;
		}
	};
}

