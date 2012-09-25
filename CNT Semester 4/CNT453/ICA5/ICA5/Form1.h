#pragma once


namespace ICA5 {

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
			this->ClientSize = System::Drawing::Size(792, 566);
			this->MaximumSize = System::Drawing::Size(800, 600);
			this->MinimumSize = System::Drawing::Size(800, 600);
			this->Name = L"Form1";
			this->Text = L"ICA5 - Addison Babcock";
			this->Paint += gcnew System::Windows::Forms::PaintEventHandler(this, &Form1::Form1_Paint);
			this->ResumeLayout(false);

		}
#pragma endregion
	private: 
		System::Void Form1_Paint(System::Object^  sender, System::Windows::Forms::PaintEventArgs^  e) 
		{
			//clear the graphics
			e->Graphics->Clear (Color::Aqua);

			//prepare the pens
			Pen ^ hBlackPen = gcnew Pen (Color::Black);
			Pen ^ hBluePen = gcnew Pen (Color::Blue);
			Pen ^ hRedPen = gcnew Pen (Color::Red);
			Pen ^ hGreenPen = gcnew Pen (Color::Green);

			//prepare the brushes
			System::Drawing::Drawing2D::LinearGradientBrush ^ hLGB = 
				gcnew System::Drawing::Drawing2D::LinearGradientBrush 
				(PointF (665, 450), PointF (765, 550), Color::Yellow, Color::Brown);
			//TextureBrush ^ hTB = gcnew TextureBrush (Image::FromFile ("o_rly.jpg"),
			//	System::Drawing::Drawing2D::WrapMode::Clamp, 
			//	System::Drawing::Rectangle (0, 0, 438, 400));
			SolidBrush ^ hSB = gcnew SolidBrush (Color::Black);

			//prepare the fonts
			System::Drawing::Font ^ hName = gcnew System::Drawing::Font ("Comic Sans MS", 20);
			System::Drawing::Font ^ hClass = gcnew System::Drawing::Font ("Times New Roman", 14);
			System::Drawing::Font ^ hTitle = gcnew System::Drawing::Font ("Courier New", 16);

			//Put some text on the cert.
			e->Graphics->DrawString ("Addison Babcock", hName, hSB, 275, 50);
			e->Graphics->DrawString ("CNT 4K", hClass, hSB, 350, 150);
			e->Graphics->DrawString ("GDI+ Expert", hTitle, hSB, 313, 175);

			//Display the owl
			//e->Graphics->FillRectangle (hTB, 200, 200, 438, 400);
			e->Graphics->DrawImage (Image::FromFile ("o_rly.jpg"), 200.0f, 200.0f);

			//Display the seal
			e->Graphics->FillEllipse (hLGB, 665, 450, 100, 100);

			//Display the bezier curve under my name
			e->Graphics->DrawBezier (hBluePen, 275, 75, 275, 100, 525, 100, 525, 75);

			//Build the polygon
			cli::array <System::Drawing::Point, 1> ^ poly = 
				gcnew cli::array <System::Drawing::Point, 1> (4);
			poly [0] = System::Drawing::Point (10, 10);
			poly [1] = System::Drawing::Point (75, 10);
			poly [2] = System::Drawing::Point (100, 100);
			poly [3] = System::Drawing::Point (25, 100);
			
			//Display the star
			e->Graphics->DrawPolygon (hRedPen, poly);

			//Display a pie
			e->Graphics->DrawPie (hGreenPen, 0, 100, 100, 100, 0, 90);

			//Build the border
			cli::array <System::Drawing::Rectangle, 1> ^ border = 
				gcnew cli::array <System::Drawing::Rectangle, 1> (4);
			border [0] = System::Drawing::Rectangle (0, 0, 800, 10);
			border [1] = System::Drawing::Rectangle (0, 0, 10, 600);
			border [2] = System::Drawing::Rectangle (0, 590, 800, 10);
			border [3] = System::Drawing::Rectangle (790, 0, 10, 600);

			//Display a rectangle around the whole screen
			e->Graphics->FillRectangles (hLGB, border);
		}
	};
}

