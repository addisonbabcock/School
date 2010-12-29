#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;


namespace ICA4 {

	public delegate void _dtagColourCallback (System::Drawing::Color, int);

	/// <summary>
	/// Summary for support
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class support : public System::Windows::Forms::Form
	{
	public:
		support(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
			this->m_delColourChanged = nullptr;
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~support()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::GroupBox^  groupBox1;
	protected: 
	private: System::Windows::Forms::GroupBox^  groupBox2;
	private: System::Windows::Forms::TrackBar^  blueTracker;

	private: System::Windows::Forms::TrackBar^  greenTracker;

	private: System::Windows::Forms::TrackBar^  redTracker;
	private: System::Windows::Forms::TrackBar^  opacityTracker;


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
			this->groupBox1 = (gcnew System::Windows::Forms::GroupBox());
			this->blueTracker = (gcnew System::Windows::Forms::TrackBar());
			this->greenTracker = (gcnew System::Windows::Forms::TrackBar());
			this->redTracker = (gcnew System::Windows::Forms::TrackBar());
			this->groupBox2 = (gcnew System::Windows::Forms::GroupBox());
			this->opacityTracker = (gcnew System::Windows::Forms::TrackBar());
			this->groupBox1->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->blueTracker))->BeginInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->greenTracker))->BeginInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->redTracker))->BeginInit();
			this->groupBox2->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->opacityTracker))->BeginInit();
			this->SuspendLayout();
			// 
			// groupBox1
			// 
			this->groupBox1->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->groupBox1->Controls->Add(this->blueTracker);
			this->groupBox1->Controls->Add(this->greenTracker);
			this->groupBox1->Controls->Add(this->redTracker);
			this->groupBox1->Location = System::Drawing::Point(12, 13);
			this->groupBox1->Name = L"groupBox1";
			this->groupBox1->Size = System::Drawing::Size(689, 174);
			this->groupBox1->TabIndex = 0;
			this->groupBox1->TabStop = false;
			this->groupBox1->Text = L"Colour";
			// 
			// blueTracker
			// 
			this->blueTracker->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->blueTracker->BackColor = System::Drawing::Color::Blue;
			this->blueTracker->Location = System::Drawing::Point(7, 123);
			this->blueTracker->Maximum = 255;
			this->blueTracker->Name = L"blueTracker";
			this->blueTracker->Size = System::Drawing::Size(676, 45);
			this->blueTracker->TabIndex = 2;
			this->blueTracker->TickFrequency = 8;
			this->blueTracker->Scroll += gcnew System::EventHandler(this, &support::blueTracker_Scroll);
			// 
			// greenTracker
			// 
			this->greenTracker->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->greenTracker->BackColor = System::Drawing::Color::Lime;
			this->greenTracker->Location = System::Drawing::Point(7, 72);
			this->greenTracker->Maximum = 255;
			this->greenTracker->Name = L"greenTracker";
			this->greenTracker->Size = System::Drawing::Size(676, 45);
			this->greenTracker->TabIndex = 1;
			this->greenTracker->TickFrequency = 8;
			this->greenTracker->Scroll += gcnew System::EventHandler(this, &support::greenTracker_Scroll);
			// 
			// redTracker
			// 
			this->redTracker->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->redTracker->BackColor = System::Drawing::Color::Red;
			this->redTracker->Location = System::Drawing::Point(7, 21);
			this->redTracker->Maximum = 255;
			this->redTracker->Name = L"redTracker";
			this->redTracker->Size = System::Drawing::Size(676, 45);
			this->redTracker->TabIndex = 0;
			this->redTracker->TickFrequency = 8;
			this->redTracker->Scroll += gcnew System::EventHandler(this, &support::redTracker_Scroll);
			// 
			// groupBox2
			// 
			this->groupBox2->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->groupBox2->Controls->Add(this->opacityTracker);
			this->groupBox2->Location = System::Drawing::Point(12, 193);
			this->groupBox2->Name = L"groupBox2";
			this->groupBox2->Size = System::Drawing::Size(689, 75);
			this->groupBox2->TabIndex = 1;
			this->groupBox2->TabStop = false;
			this->groupBox2->Text = L"Opacity";
			// 
			// opacityTracker
			// 
			this->opacityTracker->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->opacityTracker->BackColor = System::Drawing::Color::Silver;
			this->opacityTracker->Location = System::Drawing::Point(7, 24);
			this->opacityTracker->Maximum = 100;
			this->opacityTracker->Name = L"opacityTracker";
			this->opacityTracker->Size = System::Drawing::Size(676, 45);
			this->opacityTracker->TabIndex = 0;
			this->opacityTracker->TickFrequency = 10;
			this->opacityTracker->Scroll += gcnew System::EventHandler(this, &support::opacityTracker_Scroll);
			// 
			// support
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(713, 277);
			this->Controls->Add(this->groupBox2);
			this->Controls->Add(this->groupBox1);
			this->MinimumSize = System::Drawing::Size(721, 311);
			this->Name = L"support";
			this->Text = L"Select Colour/Opacity";
			this->FormClosing += gcnew System::Windows::Forms::FormClosingEventHandler(this, &support::support_FormClosing);
			this->groupBox1->ResumeLayout(false);
			this->groupBox1->PerformLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->blueTracker))->EndInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->greenTracker))->EndInit();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->redTracker))->EndInit();
			this->groupBox2->ResumeLayout(false);
			this->groupBox2->PerformLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->opacityTracker))->EndInit();
			this->ResumeLayout(false);

		}
#pragma endregion
	private: 
		void ChangeColour (void)
		{
			System::Drawing::Color newColour;

			newColour = System::Drawing::Color::FromArgb (redTracker->Value,
								greenTracker->Value, blueTracker->Value);

			this->m_delColourChanged (newColour, opacityTracker->Value);
		}
		System::Void redTracker_Scroll(System::Object^  sender, System::EventArgs^  e) 
		{
			ChangeColour ();
		}
		System::Void greenTracker_Scroll(System::Object^  sender, System::EventArgs^  e) 
		{
			ChangeColour ();
		}
		System::Void blueTracker_Scroll(System::Object^  sender, System::EventArgs^  e) 
		{
			ChangeColour ();
		}
		System::Void opacityTracker_Scroll(System::Object^  sender, System::EventArgs^  e) 
		{
			ChangeColour ();
		}
		System::Void support_FormClosing(System::Object^  sender, System::Windows::Forms::FormClosingEventArgs^  e) 
		{
			if (e->CloseReason == Windows::Forms::CloseReason::UserClosing)
			{
				this->Hide ();
				e->Cancel = true;
			}
		}

	public:
		_dtagColourCallback ^ m_delColourChanged;

		void SetTrackers (System::Drawing::Color currentColour, int iOpacity)
		{
			redTracker->Value = currentColour.R;
			blueTracker->Value = currentColour.B;
			greenTracker->Value = currentColour.G;
			opacityTracker->Value = iOpacity;
		}
	};
}
