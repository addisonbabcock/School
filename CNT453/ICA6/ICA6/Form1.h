#pragma once
#include "Int.h"

namespace ICA6 {

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
			_hShapes = gcnew System::Drawing::Drawing2D::GraphicsPath;

			for (int x (0); x < 800; x += 20)
			{
				for (int y (0); y < 600; y += 40)
				{
					_hShapes->AddRectangle (Rectangle (x, y, 10, 10));
					_hShapes->AddEllipse (x, y + 20, 10, 10);
				}
			}
			_intDlg = gcnew Int;
			_intDlg->Show ();
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
			this->Text = L"Shapes and mouses - ICA6 - Addison Babcock";
			this->Paint += gcnew System::Windows::Forms::PaintEventHandler(this, &Form1::Form1_Paint);
			this->MouseMove += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::Form1_MouseMove);
			this->ResumeLayout(false);

		}
#pragma endregion
	private:
		System::Drawing::Drawing2D::GraphicsPath ^ _hShapes;
		Int ^ _intDlg;
		System::Void Form1_Paint(System::Object^  sender, System::Windows::Forms::PaintEventArgs^  e) 
		{
			Drawing::Region ^ hShapesRegion = gcnew Drawing::Region (_hShapes);

			e->Graphics->FillRegion (gcnew SolidBrush (Color::Gray), hShapesRegion);
		}
		System::Void Form1_MouseMove(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e) 
		{
			Graphics ^ hGR = this->CreateGraphics ();
			SolidBrush ^ hBR = gcnew SolidBrush (Color::Blue);

			hGR->FillEllipse (hBR, e->X - 20, e->Y - 20, 40, 40);

			Drawing2D::GraphicsPath ^ hMousePath = gcnew Drawing2D::GraphicsPath;
			hMousePath->AddEllipse (e->X - 20, e->Y - 20, 40, 40);
			_intDlg->ShowIntersection (_hShapes, hMousePath);
		}
	};
}

