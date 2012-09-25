/****************************************************
Project: Lab 02 - Drawing Line Thing
Files: Form1.h, PenSelector.h
Date: 11 Oct 07
Author: Addison Babcock Class: CNT4K
Instructor: Simon Walker Course: CNT453
****************************************************/
#pragma once
#include "PenSelector.h"

namespace LAB2 {

	//SLineSeg describes a saved line
	public value struct SLineSeg
	{
	public:
		System::Drawing::Color m_penColor; //Color of the line
		System::Drawing::Point m_startingPoint; //Where the line starts
		System::Drawing::Point m_endingPoint; //Where the line ends
		int m_iThickness; //how thick the line is
	};

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::Net;
	using namespace System::Net::Sockets;
	using namespace System::Collections::Generic;

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
	private:
		PenSelector ^ penSelector; //the dialog for pen settings

		//the list containing line info
		LinkedList <SLineSeg> ^ lines;

		//when a line was last added, was the mouse down?
		bool m_bMouseDown;

	public:
		/***********************************************
		Function: Form1 ()
		Description: Constructor for the Form1 Dialog.
		Allocates and inits support dialogs as well.
		***********************************************/
		Form1(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//

			//create the lines list
			lines = gcnew LinkedList <SLineSeg>;
			lines->Clear ();

			//the mouse is not being held down
			m_bMouseDown = false;

			//create and init the PenSelector dialog
			penSelector = gcnew PenSelector;
			penSelector->colorChanged = gcnew delVoidVoid (this, &Form1::ColorChanged);
			penSelector->p_iPenAlpha = 255;
			penSelector->p_iPenSize = 25;
			penSelector->p_PenColor = Color::Black;
			penSelector->Show ();
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
	private: System::Windows::Forms::StatusStrip^  UI_StatusStrip;
	protected: 
	private: System::Windows::Forms::ToolStripStatusLabel^  UI_StatusStrip_PenSize;
	private: System::Windows::Forms::ToolStripStatusLabel^  UI_StatusStrip_SegmentCount;
	private: System::Windows::Forms::ToolStripStatusLabel^  UI_StatusStrip_MouseCoords;
	private: System::Windows::Forms::ToolStripDropDownButton^  UI_StatusStrip_ActionsButton;
	private: System::Windows::Forms::ToolStripMenuItem^  UI_StatusStrip_ActionButton_Reset;
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
			System::ComponentModel::ComponentResourceManager^  resources = (gcnew System::ComponentModel::ComponentResourceManager(Form1::typeid));
			this->UI_StatusStrip = (gcnew System::Windows::Forms::StatusStrip());
			this->UI_StatusStrip_PenSize = (gcnew System::Windows::Forms::ToolStripStatusLabel());
			this->UI_StatusStrip_SegmentCount = (gcnew System::Windows::Forms::ToolStripStatusLabel());
			this->UI_StatusStrip_MouseCoords = (gcnew System::Windows::Forms::ToolStripStatusLabel());
			this->UI_StatusStrip_ActionsButton = (gcnew System::Windows::Forms::ToolStripDropDownButton());
			this->UI_StatusStrip_ActionButton_Reset = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->UI_StatusStrip->SuspendLayout();
			this->SuspendLayout();
			// 
			// UI_StatusStrip
			// 
			this->UI_StatusStrip->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(4) {this->UI_StatusStrip_PenSize, 
				this->UI_StatusStrip_SegmentCount, this->UI_StatusStrip_MouseCoords, this->UI_StatusStrip_ActionsButton});
			this->UI_StatusStrip->LayoutStyle = System::Windows::Forms::ToolStripLayoutStyle::Flow;
			this->UI_StatusStrip->Location = System::Drawing::Point(0, 378);
			this->UI_StatusStrip->Name = L"UI_StatusStrip";
			this->UI_StatusStrip->Size = System::Drawing::Size(425, 22);
			this->UI_StatusStrip->TabIndex = 0;
			this->UI_StatusStrip->Text = L"statusStrip1";
			// 
			// UI_StatusStrip_PenSize
			// 
			this->UI_StatusStrip_PenSize->BorderSides = static_cast<System::Windows::Forms::ToolStripStatusLabelBorderSides>((((System::Windows::Forms::ToolStripStatusLabelBorderSides::Left | System::Windows::Forms::ToolStripStatusLabelBorderSides::Top) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Right) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Bottom));
			this->UI_StatusStrip_PenSize->BorderStyle = System::Windows::Forms::Border3DStyle::SunkenInner;
			this->UI_StatusStrip_PenSize->Name = L"UI_StatusStrip_PenSize";
			this->UI_StatusStrip_PenSize->Size = System::Drawing::Size(29, 17);
			this->UI_StatusStrip_PenSize->Text = L"Pen";
			this->UI_StatusStrip_PenSize->Click += gcnew System::EventHandler(this, &Form1::UI_StatusStrip_PenSize_Click);
			// 
			// UI_StatusStrip_SegmentCount
			// 
			this->UI_StatusStrip_SegmentCount->BorderSides = static_cast<System::Windows::Forms::ToolStripStatusLabelBorderSides>((((System::Windows::Forms::ToolStripStatusLabelBorderSides::Left | System::Windows::Forms::ToolStripStatusLabelBorderSides::Top) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Right) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Bottom));
			this->UI_StatusStrip_SegmentCount->BorderStyle = System::Windows::Forms::Border3DStyle::SunkenInner;
			this->UI_StatusStrip_SegmentCount->Name = L"UI_StatusStrip_SegmentCount";
			this->UI_StatusStrip_SegmentCount->Size = System::Drawing::Size(85, 17);
			this->UI_StatusStrip_SegmentCount->Text = L"Segment Count";
			// 
			// UI_StatusStrip_MouseCoords
			// 
			this->UI_StatusStrip_MouseCoords->BorderSides = static_cast<System::Windows::Forms::ToolStripStatusLabelBorderSides>((((System::Windows::Forms::ToolStripStatusLabelBorderSides::Left | System::Windows::Forms::ToolStripStatusLabelBorderSides::Top) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Right) 
				| System::Windows::Forms::ToolStripStatusLabelBorderSides::Bottom));
			this->UI_StatusStrip_MouseCoords->BorderStyle = System::Windows::Forms::Border3DStyle::SunkenInner;
			this->UI_StatusStrip_MouseCoords->Name = L"UI_StatusStrip_MouseCoords";
			this->UI_StatusStrip_MouseCoords->Size = System::Drawing::Size(79, 17);
			this->UI_StatusStrip_MouseCoords->Text = L"Mouse Coords";
			// 
			// UI_StatusStrip_ActionsButton
			// 
			this->UI_StatusStrip_ActionsButton->DisplayStyle = System::Windows::Forms::ToolStripItemDisplayStyle::Text;
			this->UI_StatusStrip_ActionsButton->DropDownItems->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(1) {this->UI_StatusStrip_ActionButton_Reset});
			this->UI_StatusStrip_ActionsButton->Image = (cli::safe_cast<System::Drawing::Image^  >(resources->GetObject(L"UI_StatusStrip_ActionsButton.Image")));
			this->UI_StatusStrip_ActionsButton->ImageTransparentColor = System::Drawing::Color::Magenta;
			this->UI_StatusStrip_ActionsButton->Name = L"UI_StatusStrip_ActionsButton";
			this->UI_StatusStrip_ActionsButton->Size = System::Drawing::Size(67, 17);
			this->UI_StatusStrip_ActionsButton->Text = L"Actions...";
			// 
			// UI_StatusStrip_ActionButton_Reset
			// 
			this->UI_StatusStrip_ActionButton_Reset->Name = L"UI_StatusStrip_ActionButton_Reset";
			this->UI_StatusStrip_ActionButton_Reset->Size = System::Drawing::Size(113, 22);
			this->UI_StatusStrip_ActionButton_Reset->Text = L"Reset";
			this->UI_StatusStrip_ActionButton_Reset->Click += gcnew System::EventHandler(this, &Form1::UI_StatusStrip_ActionButton_Reset_Click);
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(425, 400);
			this->Controls->Add(this->UI_StatusStrip);
			this->Name = L"Form1";
			this->Text = L"Lab #2 - Addison Babcock";
			this->Paint += gcnew System::Windows::Forms::PaintEventHandler(this, &Form1::Form1_Paint);
			this->MouseUp += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::Form1_MouseUp);
			this->MouseMove += gcnew System::Windows::Forms::MouseEventHandler(this, &Form1::Form1_MouseMove);
			this->UI_StatusStrip->ResumeLayout(false);
			this->UI_StatusStrip->PerformLayout();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	public:
		/***********************************************
		Function: ColorChanged
		Description: Called from the penSelector dialog
		to update the color of the Pen Size status label 
		***********************************************/
		void ColorChanged (void)
		{
			//update the pen status strip
			this->UI_StatusStrip_PenSize->ForeColor = this->penSelector->p_PenColor;
			this->UI_StatusStrip_PenSize->Text = 
				"Pen (" + this->penSelector->p_iPenSize + ")";
		}
	private: 
		/***********************************************
		Function: DrawSegment (SLineSeg sLineSegment)
		Description: Draws a given line segment to the
		main form.
		Arguments: sLineSegment is the line that should 
		be drawn to the form.
		***********************************************/
		void DrawSegment (SLineSeg sLineSegment)
		{
			//the graphics interface
			Graphics ^ hGr (this->CreateGraphics ());

			//Set up the pen using the color and width passed to the function
			Pen ^ hPen = gcnew Pen (sLineSegment.m_penColor, 
				static_cast <float> (sLineSegment.m_iThickness));

			//smooth the ends of the pen
			hPen->SetLineCap (Drawing2D::LineCap::Round,
				Drawing2D::LineCap::Round, Drawing2D::DashCap::Round);

			//finally, add the line
			hGr->DrawLine (hPen, sLineSegment.m_startingPoint, 
				sLineSegment.m_endingPoint);
		}

		/***********************************************
		Function: UI_StatusStrip_ActionButton_Reset_Click
		Description: Clears the lines that are saved and
		redraws the window.
		***********************************************/
		System::Void UI_StatusStrip_ActionButton_Reset_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			//delete all the saved lines and redraw the window
			lines->Clear ();
			this->Invalidate ();
		}

		/***********************************************
		Function: Form1_MouseMove
		Description: Adds a line to the list if the 
		left mouse button is held down. Draws the line
		that was added, and updates the status strip 
		with the mouse coordinates.
		***********************************************/
		System::Void Form1_MouseMove(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e) 
		{
			//only add lines if the left mouse button is down
			if (e->Button == System::Windows::Forms::MouseButtons::Left)
			{
				//build the new line segment
				SLineSeg newLineSeg;

				if (lines->Count == 0 || !m_bMouseDown)
				{
					//the first segment will start and end at the mouse coords
					newLineSeg.m_startingPoint = e->Location;
					newLineSeg.m_endingPoint = e->Location;

					//the thickness and color are obtained from the dialog
					newLineSeg.m_iThickness = penSelector->p_iPenSize;
					newLineSeg.m_penColor = penSelector->p_PenColor;
					
					lines->AddLast (newLineSeg);
				}
				else
				{
					//the line goes from the end of the last line
					newLineSeg.m_startingPoint = lines->Last->Value.m_endingPoint;
					//the the current mouse location
					newLineSeg.m_endingPoint = e->Location;

					//the thickness and color are obtained from the dialog
					newLineSeg.m_iThickness = penSelector->p_iPenSize;
					newLineSeg.m_penColor = penSelector->p_PenColor;

					//put the new segment into the end of the list
					//lines->AddLast (newLineSeg);
					lines->AddAfter (lines->Last, newLineSeg);
				}

				//next mouse movement, we will connect the segment that was
				//just added to the one that will be created
				m_bMouseDown = true;

				//Now add the line to the screen
				DrawSegment (newLineSeg);
			}

			//update the status strip
			UI_StatusStrip_SegmentCount->Text = "Segment Count: " + lines->Count.ToString ();
			UI_StatusStrip_MouseCoords->Text = "X:" + e->Location.X.ToString ("0000") + 
											   " Y:" + e->Location.Y.ToString ("0000");
		}

		/***********************************************
		Function: Form1_Paint
		Description: Redraws all the stored lines to 
		the screen.
		***********************************************/
		System::Void Form1_Paint(System::Object^  sender, System::Windows::Forms::PaintEventArgs^  e) 
		{
			//exit if the list is empty
			if (lines->Count <= 1)
			{
				return;
			}

			//points at the segment being drawn
			LinkedList<SLineSeg>::Enumerator segment (lines->GetEnumerator ());

			do
			{
				//draw a line
				DrawSegment (segment.Current);
			} while (segment.MoveNext ()); //go the next line
		}

		/***********************************************
		Function: UI_StatusStrip_PenSize_Click
		Description: Re-opens the penSelector dialog
		when the pen size label is clicked.
		***********************************************/
		System::Void UI_StatusStrip_PenSize_Click(System::Object^  sender, System::EventArgs^  e) 
		{
			//bring the selector back up and give it focus
			penSelector->Show ();
			penSelector->Focus ();
		}

		/***********************************************
		Function: Form1_MouseUp
		Description: If the left mouse button is 
		released, it will set m_bMouseDown to indicate
		that so the next line drawn will be seperate
		from the lines previous.
		***********************************************/
		System::Void Form1_MouseUp(System::Object^  sender, System::Windows::Forms::MouseEventArgs^  e) 
		{
			//mark the mouse button as released
			if (e->Button == System::Windows::Forms::MouseButtons::Left)
			{
				m_bMouseDown = false;
			}
		}
	};
}