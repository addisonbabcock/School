#pragma once


namespace ICA2 {

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
	private: System::Windows::Forms::Label^  historyLabel;
	protected: 
	private: System::Windows::Forms::ListBox^  history;
	private: System::Windows::Forms::WebBrowser^  browser;
	private: System::Windows::Forms::Label^  urlLabel;
	private: System::Windows::Forms::TextBox^  url;



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
			this->historyLabel = (gcnew System::Windows::Forms::Label());
			this->history = (gcnew System::Windows::Forms::ListBox());
			this->browser = (gcnew System::Windows::Forms::WebBrowser());
			this->urlLabel = (gcnew System::Windows::Forms::Label());
			this->url = (gcnew System::Windows::Forms::TextBox());
			this->SuspendLayout();
			// 
			// historyLabel
			// 
			this->historyLabel->AutoSize = true;
			this->historyLabel->Location = System::Drawing::Point(13, 9);
			this->historyLabel->Name = L"historyLabel";
			this->historyLabel->Size = System::Drawing::Size(39, 13);
			this->historyLabel->TabIndex = 0;
			this->historyLabel->Text = L"History";
			// 
			// history
			// 
			this->history->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Bottom) 
				| System::Windows::Forms::AnchorStyles::Left));
			this->history->FormattingEnabled = true;
			this->history->Location = System::Drawing::Point(16, 34);
			this->history->Name = L"history";
			this->history->Size = System::Drawing::Size(152, 589);
			this->history->TabIndex = 1;
			this->history->Click += gcnew System::EventHandler(this, &Form1::history_Click);
			// 
			// browser
			// 
			this->browser->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Bottom) 
				| System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->browser->Location = System::Drawing::Point(174, 32);
			this->browser->MinimumSize = System::Drawing::Size(20, 20);
			this->browser->Name = L"browser";
			this->browser->Size = System::Drawing::Size(475, 591);
			this->browser->TabIndex = 2;
			this->browser->DocumentCompleted += gcnew System::Windows::Forms::WebBrowserDocumentCompletedEventHandler(this, &Form1::browser_DocumentCompleted);
			// 
			// urlLabel
			// 
			this->urlLabel->AutoSize = true;
			this->urlLabel->Location = System::Drawing::Point(171, 9);
			this->urlLabel->Name = L"urlLabel";
			this->urlLabel->Size = System::Drawing::Size(29, 13);
			this->urlLabel->TabIndex = 3;
			this->urlLabel->Text = L"URL";
			// 
			// url
			// 
			this->url->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->url->Location = System::Drawing::Point(206, 6);
			this->url->Name = L"url";
			this->url->Size = System::Drawing::Size(443, 20);
			this->url->TabIndex = 4;
			this->url->PreviewKeyDown += gcnew System::Windows::Forms::PreviewKeyDownEventHandler(this, &Form1::url_PreviewKeyDown);
			this->url->TextChanged += gcnew System::EventHandler(this, &Form1::url_TextChanged);
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(661, 629);
			this->Controls->Add(this->url);
			this->Controls->Add(this->urlLabel);
			this->Controls->Add(this->browser);
			this->Controls->Add(this->history);
			this->Controls->Add(this->historyLabel);
			this->Name = L"Form1";
			this->Text = L"Minibrowser - Addison Babcock";
			this->Load += gcnew System::EventHandler(this, &Form1::Form1_Load);
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: 
		System::Void url_TextChanged(System::Object^  sender, System::EventArgs^  e) 
		{
		}
		System::Void browser_DocumentCompleted(System::Object^  sender, System::Windows::Forms::WebBrowserDocumentCompletedEventArgs^  e) 
		{
			this->url->Text = this->browser->Url->ToString ();
			if (!this->history->Items->Contains (this->url->Text))
			{
				this->history->Items->Add (this->url->Text);
			}
		}
		System::Void Form1_Load(System::Object^  sender, System::EventArgs^  e) 
		{
			this->browser->Navigate ("www.google.ca");
			this->url->Text = "www.google.ca";
		}
		System::Void url_PreviewKeyDown(System::Object^  sender, System::Windows::Forms::PreviewKeyDownEventArgs^  e) 
		{
			if (e->KeyCode == Windows::Forms::Keys::Enter)
				this->browser->Navigate (this->url->Text);
		}
		System::Void history_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			this->browser->Navigate (this->history->SelectedItem->ToString ());
		}
		System::Void backButton_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			//
		}
};
}

