#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;


namespace ICA6 {

	/// <summary>
	/// Summary for Int
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class Int : public System::Windows::Forms::Form
	{
	public:
		Int(void)
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
		~Int()
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
			// Int
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(792, 566);
			this->MaximumSize = System::Drawing::Size(800, 600);
			this->MinimumSize = System::Drawing::Size(800, 600);
			this->Name = L"Int";
			this->Text = L"Intersections";
			this->ResumeLayout(false);

		}
#pragma endregion
	public:
		void ShowIntersection (Drawing::Drawing2D::GraphicsPath ^ hShapes, 
							   Drawing2D::GraphicsPath ^ hMouse)
		{
			Drawing::Region ^ hShapesRegion = gcnew Drawing::Region (hShapes);
			Drawing::Region ^ hMouseRegion = gcnew Drawing::Region (hMouse);
			Drawing::Region ^ hIntersect = gcnew Drawing::Region;

			hIntersect = hMouseRegion->Clone ();
			hIntersect->Intersect (hShapesRegion);

			Graphics ^ hGR = this->CreateGraphics ();
			hGR->FillRegion (gcnew SolidBrush (Color::Green), hIntersect);
		}
	};
}
