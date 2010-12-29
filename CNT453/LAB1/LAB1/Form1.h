/****************************************************
Project: Lab 01 - PicBender
Files: Form1.h
Date: 21 Sept 2007
Author: Addison Babcock		Class: CNT4K
Instructor: Simon Walker	Course: CNT453
****************************************************/

#pragma once
#include <stdlib.h>

//---------
//constants
//---------

//What files are accepted by the open file dialog
char const szFileDialogFilter [] = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg;*.jpeg|PNG (*.png)|*.png|All Files (*.*)|*.*";
//The initial directory of the open file dialog
char const szInitDir [] = "%DOCUMENTS%";
//Error message to be displayed when the file cant be found
char const szFileNotFound [] = "File not found!";
//Error message to be displayed when the file was not an image
char const szBadFileType [] = "Picture could not be loaded, invalid file";
//A string containing error
char const szError [] = "Error";

//Label strings (left and right side of the trackbar, when whichever 
//transform is selected)
char const szLeftContrast [] = "Less";
char const szLeftGrayScale [] = "Color";
char const szLeftTint [] = "Red";
char const szLeftNoise [] = "Clear";
char const szRightContrast [] = "More";
char const szRightGrayScale [] = "Gray";
char const szRightTint [] = "Green";
char const szRightNoise [] = "Noisy";

namespace LAB1 {

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

			//initialize the file open dialog
			openDlg->InitialDirectory = gcnew String (szInitDir);
			openDlg->FileName = "";
			openDlg->RestoreDirectory = false;
			openDlg->Filter = gcnew String (szFileDialogFilter);
			openDlg->FilterIndex = 0;
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
	private: System::Windows::Forms::PictureBox^  picture;
	private: System::Windows::Forms::ProgressBar^  progressBar;
	private: System::Windows::Forms::Button^  loadButton;
	private: System::Windows::Forms::GroupBox^  effectGroupBox;
	private: System::Windows::Forms::RadioButton^  noiseButton;
	private: System::Windows::Forms::RadioButton^  tintButton;
	private: System::Windows::Forms::RadioButton^  bnwButton;
	private: System::Windows::Forms::RadioButton^  contrastButton;
	private: System::Windows::Forms::TrackBar^  trackBar;
	private: System::Windows::Forms::Button^  transformButton;
	private: System::Windows::Forms::Label^  leftLabel;
	private: System::Windows::Forms::Label^  rightLabel;
	private: System::Windows::Forms::OpenFileDialog^  openDlg;

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
			this->picture = (gcnew System::Windows::Forms::PictureBox());
			this->progressBar = (gcnew System::Windows::Forms::ProgressBar());
			this->loadButton = (gcnew System::Windows::Forms::Button());
			this->effectGroupBox = (gcnew System::Windows::Forms::GroupBox());
			this->noiseButton = (gcnew System::Windows::Forms::RadioButton());
			this->tintButton = (gcnew System::Windows::Forms::RadioButton());
			this->bnwButton = (gcnew System::Windows::Forms::RadioButton());
			this->contrastButton = (gcnew System::Windows::Forms::RadioButton());
			this->trackBar = (gcnew System::Windows::Forms::TrackBar());
			this->transformButton = (gcnew System::Windows::Forms::Button());
			this->leftLabel = (gcnew System::Windows::Forms::Label());
			this->rightLabel = (gcnew System::Windows::Forms::Label());
			this->openDlg = (gcnew System::Windows::Forms::OpenFileDialog());
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->picture))->BeginInit();
			this->effectGroupBox->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->trackBar))->BeginInit();
			this->SuspendLayout();
			// 
			// picture
			// 
			this->picture->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Bottom) 
				| System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->picture->BackColor = System::Drawing::SystemColors::Control;
			this->picture->BorderStyle = System::Windows::Forms::BorderStyle::Fixed3D;
			this->picture->Location = System::Drawing::Point(13, 13);
			this->picture->Name = L"picture";
			this->picture->Size = System::Drawing::Size(664, 397);
			this->picture->SizeMode = System::Windows::Forms::PictureBoxSizeMode::Zoom;
			this->picture->TabIndex = 0;
			this->picture->TabStop = false;
			// 
			// progressBar
			// 
			this->progressBar->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->progressBar->Location = System::Drawing::Point(13, 416);
			this->progressBar->Name = L"progressBar";
			this->progressBar->Size = System::Drawing::Size(664, 23);
			this->progressBar->TabIndex = 1;
			// 
			// loadButton
			// 
			this->loadButton->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left));
			this->loadButton->Location = System::Drawing::Point(13, 445);
			this->loadButton->Name = L"loadButton";
			this->loadButton->Size = System::Drawing::Size(75, 23);
			this->loadButton->TabIndex = 1;
			this->loadButton->Text = L"Load Picture";
			this->loadButton->UseVisualStyleBackColor = true;
			this->loadButton->Click += gcnew System::EventHandler(this, &Form1::loadbutton_Click);
			// 
			// effectGroupBox
			// 
			this->effectGroupBox->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left));
			this->effectGroupBox->Controls->Add(this->noiseButton);
			this->effectGroupBox->Controls->Add(this->tintButton);
			this->effectGroupBox->Controls->Add(this->bnwButton);
			this->effectGroupBox->Controls->Add(this->contrastButton);
			this->effectGroupBox->Location = System::Drawing::Point(95, 445);
			this->effectGroupBox->Name = L"effectGroupBox";
			this->effectGroupBox->Size = System::Drawing::Size(175, 66);
			this->effectGroupBox->TabIndex = 2;
			this->effectGroupBox->TabStop = false;
			this->effectGroupBox->Text = L"Modification Type";
			// 
			// noiseButton
			// 
			this->noiseButton->AutoSize = true;
			this->noiseButton->Enabled = false;
			this->noiseButton->Location = System::Drawing::Point(117, 43);
			this->noiseButton->Name = L"noiseButton";
			this->noiseButton->Size = System::Drawing::Size(52, 17);
			this->noiseButton->TabIndex = 3;
			this->noiseButton->Text = L"Noise";
			this->noiseButton->UseVisualStyleBackColor = true;
			this->noiseButton->CheckedChanged += gcnew System::EventHandler(this, &Form1::noiseButton_CheckedChanged);
			// 
			// tintButton
			// 
			this->tintButton->AutoSize = true;
			this->tintButton->Enabled = false;
			this->tintButton->Location = System::Drawing::Point(117, 20);
			this->tintButton->Name = L"tintButton";
			this->tintButton->Size = System::Drawing::Size(43, 17);
			this->tintButton->TabIndex = 1;
			this->tintButton->Text = L"Tint";
			this->tintButton->UseVisualStyleBackColor = true;
			this->tintButton->CheckedChanged += gcnew System::EventHandler(this, &Form1::tintButton_CheckedChanged);
			// 
			// bnwButton
			// 
			this->bnwButton->AutoSize = true;
			this->bnwButton->Enabled = false;
			this->bnwButton->Location = System::Drawing::Point(6, 43);
			this->bnwButton->Name = L"bnwButton";
			this->bnwButton->Size = System::Drawing::Size(105, 17);
			this->bnwButton->TabIndex = 2;
			this->bnwButton->Text = L"Black And White";
			this->bnwButton->UseVisualStyleBackColor = true;
			this->bnwButton->CheckedChanged += gcnew System::EventHandler(this, &Form1::bnwButton_CheckedChanged);
			// 
			// contrastButton
			// 
			this->contrastButton->AutoSize = true;
			this->contrastButton->Checked = true;
			this->contrastButton->Enabled = false;
			this->contrastButton->Location = System::Drawing::Point(7, 20);
			this->contrastButton->Name = L"contrastButton";
			this->contrastButton->Size = System::Drawing::Size(64, 17);
			this->contrastButton->TabIndex = 0;
			this->contrastButton->TabStop = true;
			this->contrastButton->Text = L"Contrast";
			this->contrastButton->UseVisualStyleBackColor = true;
			this->contrastButton->CheckedChanged += gcnew System::EventHandler(this, &Form1::contrastButton_CheckedChanged);
			// 
			// trackBar
			// 
			this->trackBar->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->trackBar->Enabled = false;
			this->trackBar->Location = System::Drawing::Point(276, 445);
			this->trackBar->Maximum = 100;
			this->trackBar->Name = L"trackBar";
			this->trackBar->Size = System::Drawing::Size(320, 45);
			this->trackBar->TabIndex = 3;
			this->trackBar->TickFrequency = 5;
			// 
			// transformButton
			// 
			this->transformButton->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Right));
			this->transformButton->Enabled = false;
			this->transformButton->Location = System::Drawing::Point(602, 445);
			this->transformButton->Name = L"transformButton";
			this->transformButton->Size = System::Drawing::Size(75, 23);
			this->transformButton->TabIndex = 4;
			this->transformButton->Text = L"Transform!";
			this->transformButton->UseVisualStyleBackColor = true;
			this->transformButton->Click += gcnew System::EventHandler(this, &Form1::transformButton_Click);
			// 
			// leftLabel
			// 
			this->leftLabel->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Left));
			this->leftLabel->AutoSize = true;
			this->leftLabel->Location = System::Drawing::Point(276, 490);
			this->leftLabel->Name = L"leftLabel";
			this->leftLabel->Size = System::Drawing::Size(29, 13);
			this->leftLabel->TabIndex = 5;
			this->leftLabel->Text = L"Less";
			// 
			// rightLabel
			// 
			this->rightLabel->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Right));
			this->rightLabel->AutoSize = true;
			this->rightLabel->Location = System::Drawing::Point(565, 490);
			this->rightLabel->Name = L"rightLabel";
			this->rightLabel->Size = System::Drawing::Size(31, 13);
			this->rightLabel->TabIndex = 7;
			this->rightLabel->Text = L"More";
			// 
			// openDlg
			// 
			this->openDlg->DefaultExt = L"jpg";
			this->openDlg->FileName = L"openFileDialog1";
			this->openDlg->Title = L"Open Image";
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(689, 523);
			this->Controls->Add(this->rightLabel);
			this->Controls->Add(this->leftLabel);
			this->Controls->Add(this->transformButton);
			this->Controls->Add(this->trackBar);
			this->Controls->Add(this->effectGroupBox);
			this->Controls->Add(this->loadButton);
			this->Controls->Add(this->progressBar);
			this->Controls->Add(this->picture);
			this->MinimumSize = System::Drawing::Size(460, 160);
			this->Name = L"Form1";
			this->Text = L"PicBender - Addison Babcock";
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->picture))->EndInit();
			this->effectGroupBox->ResumeLayout(false);
			this->effectGroupBox->PerformLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->trackBar))->EndInit();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: 

// Function name   : loadbutton_Click 
// Description     : Event handler used when the loadButton is clicked
//					 allows the user to select a picture, load it and display
//					 it in the pictureBox control.

		System::Void loadbutton_Click(System::Object^  sender, 
									  System::EventArgs^  e) 
		{
			//dont do anything if the user cancels out of the dialog
			if (System::Windows::Forms::DialogResult::OK == 
				this->openDlg->ShowDialog ())
			{
				try
				{
					//try to load the picture
					this->picture->Load (this->openDlg->FileName);
					this->EnableControls ();
				}
				//The file wasnt a picture
				catch (ArgumentException ^ arg)
				{
					System::Windows::Forms::MessageBox ^ messageBox;
					messageBox->Show (gcnew String (szBadFileType), 
									  gcnew String (szError));
				}
				//The file didnt exist
				catch (System::IO::FileNotFoundException ^ oops)
				{
					System::Windows::Forms::MessageBox ^ messageBox;
					messageBox->Show (gcnew String (szFileNotFound), 
									  gcnew String (szError));
				}
			}
		}

// Function name   : contrastButton_CheckedChanged 
// Description     : Event handler used when the contrastButton is changed
//					 Changes the left and right labels of the trackbar to be
//					 appropriate for adjusting contrast

		System::Void contrastButton_CheckedChanged(System::Object^  sender, 
												   System::EventArgs^  e) 
		{
			//set the labels to desribe contrast
			if (this->contrastButton->Checked)
			{
				this->leftLabel->Text = gcnew String (szLeftContrast);
				this->rightLabel->Text = gcnew String (szRightContrast);
			}
		}

// Function name   : bnwButton_CheckedChanged 
// Description     : Event handler used when the bnwButton is changed
//					 Changes the left and right labels of the trackbar to be
//					 appropriate for adjusting gray scale

		System::Void bnwButton_CheckedChanged(System::Object^  sender, 
											  System::EventArgs^  e) 
		{
			//set the labels to desribe black and white
			if (this->bnwButton->Checked)
			{
				this->leftLabel->Text = gcnew String (szLeftGrayScale);
				this->rightLabel->Text = gcnew String (szRightGrayScale);
			}
		}

// Function name   : tintButton_CheckedChanged 
// Description     : Event handler used when the tintButton is changed
//					 Changes the left and right labels of the trackbar to be
//					 appropriate for adjusting tint

		System::Void tintButton_CheckedChanged(System::Object^  sender, 
											   System::EventArgs^  e) 
		{
			//set the labels to describe tint
			if (this->tintButton->Checked)
			{
				this->leftLabel->Text = gcnew String (szLeftTint);
				this->rightLabel->Text = gcnew String (szRightTint);

				//set the adjust value to the middle
				this->trackBar->Value = 
					(this->trackBar->Maximum - this->trackBar->Minimum) / 2;
			}
		}

// Function name   : noiseButton_CheckedChanged 
// Description     : Event handler used when the noiseButton is changed
//					 Changes the left and right labels of the trackbar to be
//					 appropriate for adjusting noise

		System::Void noiseButton_CheckedChanged(System::Object^  sender, 
												System::EventArgs^  e) 
		{
			//set the labels to describe noise
			if (this->noiseButton->Checked)
			{
				this->leftLabel->Text = gcnew String (szLeftNoise);
				this->rightLabel->Text = gcnew String (szRightNoise);
			}
		}

// Function name   : transformButton_Click 
// Description     : Event handler used when the transformButton is clicked
//					 Sets in motion the transform selected by the user.
//					 Also temporarily disables all controls on the form.

		System::Void transformButton_Click(System::Object^  sender, 
										   System::EventArgs^  e) 
		{
			//Program is doing a long calculation, disable the controls
			this->DisableControls ();
			this->loadButton->Enabled = false;

			//Adjust the appropriate property of the image based on which 
			//button is selected
			if (this->contrastButton->Checked)
				AdjustContrast ();
			if (this->bnwButton->Checked)
				AdjustGrayScale ();
			if (this->tintButton->Checked)
				AdjustTint ();
			if (this->noiseButton->Checked)
				AdjustNoise ();

			//Done calculations, re-enable the controls and reset the progress
			this->loadButton->Enabled = true;
			this->EnableControls ();
			this->progressBar->Value = this->progressBar->Minimum;
		}

// Function name   : EnableControls 
// Description     : Enables all controls on the form except the loadButton
// Return type	   : void

		void EnableControls ()
		{
			//enable all the controls except loadButton
			this->contrastButton->Enabled = true;
			this->bnwButton->Enabled = true;
			this->tintButton->Enabled = true;
			this->noiseButton->Enabled = true;
			this->trackBar->Enabled = true;
			this->transformButton->Enabled = true;
			this->effectGroupBox->Enabled = true;
		}

// Function name   : DisableControls 
// Description     : Disables all controls on the form except the loadButton
// Return type	   : void

		void DisableControls ()
		{
			//disable all the controls except loadButton
			this->contrastButton->Enabled = false;
			this->bnwButton->Enabled = false;
			this->tintButton->Enabled = false;
			this->noiseButton->Enabled = false;
			this->trackBar->Enabled = false;
			this->transformButton->Enabled = false;
			this->effectGroupBox->Enabled = false;
		}

// Function name   : AdjustContrast 
// Description     : Adjusts the contrast of the image by the amount dictated
//					 by the trackBar
// Return type	   : void

		void AdjustContrast ()
		{
			//create a bitmap for manipulating
			System::Drawing::Bitmap ^ hBM = gcnew Bitmap(this->picture->Image);

			//the trackbar will be incremented after every column
			this->progressBar->Value = 0;
			this->progressBar->Minimum = 0;
			this->progressBar->Maximum = hBM->Width;

			//save some time by not going through the Value property lots
			int iValue (this->trackBar->Value);
			
			//holds the pixel currently being worked on
			Drawing::Color pixel;

			//the individual color values of the pixel being worked on
			int iRed (0), iBlue (0), iGreen (0);

			//go through all the columns
			for (int iX (0); iX < hBM->Width; ++iX)
			{
				//go through all the rows
				for (int iY (0); iY < hBM->Height; ++iY)
				{
					//grab a pixel form the buffer
					pixel = hBM->GetPixel (iX, iY);

					//adjust the contrast of the red pixel
					iRed = pixel.R > 128 ? pixel.R + iValue : 
										   pixel.R - iValue;
					//make sure the red value doesnt go over 255
					iRed = iRed > 255 ? 255 : iRed;
					//make sure the red value doesnt go below 0
					iRed = iRed < 0 ? 0 : iRed;

					//adjust the contrast of the blue pixel
					iBlue = pixel.B > 128 ? pixel.B + iValue : 
											pixel.B - iValue;
					//make sure the blue value doesnt go over 255
					iBlue = iBlue > 255 ? 255 : iBlue;
					//make sure the blue value doesnt go below 0
					iBlue = iBlue < 0 ? 0 : iBlue;

					//adjust the contrast of the green pixel
					iGreen = pixel.G > 128 ? pixel.G + iValue : 
											 pixel.G - iValue;
					//make sure the green value doesnt go over 255
					iGreen = iGreen > 255 ? 255 : iGreen;
					//make sure the green value doesnt go below 0
					iGreen = iGreen < 0 ? 0 : iGreen;

					//the pixel is complete, put it back in the image
					hBM->SetPixel (iX, iY, Drawing::Color::FromArgb (iRed, 
																	 iGreen, 
																	 iBlue));
				}
				//update the progress bar
				this->progressBar->Value = iX;
			}
			//update the picture
			this->picture->Image = hBM;
		}

// Function name   : AdjustGrayScale 
// Description     : Adjusts the gray scale of the image by the amount dictated
//					 by the trackBar
// Return type	   : void

		void AdjustGrayScale ()
		{
			//create a bitmap for manipulating
			System::Drawing::Bitmap ^ hBM = gcnew Bitmap(this->picture->Image);

			//the trackbar will be incremented after every column
			this->progressBar->Value = 0;
			this->progressBar->Minimum = 0;
			this->progressBar->Maximum = hBM->Width;

			//save some time by not going through the Value property lots
			//dividing by 100 the convert the int trackBar->value to a %
			double dValue (this->trackBar->Value / 100.0);
			//holds the pixel currently being worked on
			Drawing::Color pixel;
			//the individual color values of the pixel being worked on
			int iRed (0), iBlue (0), iGreen (0);
			//the average value of a given pixel
			int iAverage (0);

			//go through all the columns
			for (int iX (0); iX < hBM->Width; ++iX)
			{
				//go through all the rows
				for (int iY (0); iY < hBM->Height; ++iY)
				{
					//grab a pixel from the buffer
					pixel = hBM->GetPixel (iX, iY);

					//calculate the average of the pixel
					iAverage = (pixel.R + pixel.B + pixel.G) / 3;

					//adjust the contrast of the red pixel
					iRed = static_cast <int> 
						(pixel.R + ((iAverage - pixel.R) * dValue));
					//make sure the red value doesnt go over 255
					iRed = iRed > 255 ? 255 : iRed;
					//make sure the red value doesnt go below 0
					iRed = iRed < 0 ? 0 : iRed;

					//adjust the contrast of the blue pixel
					iBlue = static_cast <int> 
						(pixel.B + ((iAverage - pixel.B) * dValue));
					//make sure the blue value doesnt go over 255
					iBlue = iBlue > 255 ? 255 : iBlue;
					//make sure the blue value doesnt go below 0
					iBlue = iBlue < 0 ? 0 : iBlue;

					//adjust the contrast of the green pixel
					iGreen = static_cast <int> 
						(pixel.G + ((iAverage - pixel.G) * dValue));
					//make sure the green value doesnt go over 255
					iGreen = iGreen > 255 ? 255 : iGreen;
					//make sure the green value doesnt go below 0
					iGreen = iGreen < 0 ? 0 : iGreen;

					//the pixel is complete, put it back in the image
					hBM->SetPixel (iX, iY, Drawing::Color::FromArgb (iRed, 
																	 iGreen, 
																	 iBlue));
				}
				//update the progress bar
				this->progressBar->Value = iX;
			}
			//show the result!
			this->picture->Image = hBM;
		}

// Function name   : AdjustTint 
// Description     : Adjusts the tint of the image by the amount dictated
//					 by the trackBar
// Return type	   : void

		void AdjustTint ()
		{
			//create a bitmap for manipulating
			System::Drawing::Bitmap ^ hBM = gcnew Bitmap(this->picture->Image);

			//the trackbar will be incremented after every column
			this->progressBar->Value = 0;
			this->progressBar->Minimum = 0;
			this->progressBar->Maximum = hBM->Width;

			//save some time by not going through the Value property lots
			//dividing by 100 the convert the int trackBar->value to a %
			int iValue (this->trackBar->Value);
			//holds the pixel currently being worked on
			Drawing::Color pixel;
			//the individual color values of the pixel being worked on
			int iRed (0), iBlue (0), iGreen (0);
			//the average of the min and max values for this->trackBar
			int iTrackBarMiddle ((trackBar->Minimum + trackBar->Maximum) / 2);

			//go through all the columns
			for (int iX (0); iX < hBM->Width; ++iX)
			{
				//go through all the rows
				for (int iY (0); iY < hBM->Height; ++iY)
				{
					//grab a pixel from the buffer
					pixel = hBM->GetPixel (iX , iY);

					//set the red
					iRed = pixel.R;
					//dont adjust the red if the image is to be green tinted
					if (iValue < iTrackBarMiddle)
					{
						//adjust the contrast of the red pixel
						iRed = iRed + (iTrackBarMiddle - 1 - iValue);
						//make sure the red value doesnt go over 255
						iRed = iRed > 255 ? 255 : iRed;
						//make sure the red value doesnt go below 0
						iRed = iRed < 0 ? 0 : iRed;
					}
					
					//set the blue
					iBlue = pixel.B;

					//set the green
					iGreen = pixel.G;
					//dont adjust the green if the image is to be red tinted
					if (iValue > iTrackBarMiddle)
					{
						//adjust the contrast of the green pixel
						iGreen = iGreen + (iValue - iTrackBarMiddle + 1);
						//make sure the green value doesnt go over 255
						iGreen = iGreen > 255 ? 255 : iGreen;
						//make sure the green value doesnt go below 0
						iGreen = iGreen < 0 ? 0 : iGreen;
					}

					//the pixel is complete, put it back in the image
					hBM->SetPixel (iX , iY, Drawing::Color::FromArgb (iRed, 
																	  iGreen, 
																	  iBlue));
				}
				//update the progress bar
				this->progressBar->Value =  iX ;
			}
			//show the result!
			this->picture->Image = hBM;
		}

// Function name   : AdjustNoise 
// Description     : Adjusts the noise of the image by the amount dictated
//					 by the trackBar
// Return type	   : void

		void AdjustNoise ()
		{
			//create a bitmap for manipulating
			System::Drawing::Bitmap ^ hBM = gcnew Bitmap(this->picture->Image);

			//the trackbar will be incremented after every column
			this->progressBar->Value = 0;
			this->progressBar->Minimum = 0;
			this->progressBar->Maximum = hBM->Width;

			//save some time by not going through the Value property lots
			//dividing by 100 the convert the int trackBar->value to a %
			int iValue (this->trackBar->Value + 1);
			//holds the pixel currently being worked on
			Drawing::Color pixel;
			//the individual color values of the pixel being worked on
			int iRed (0), iBlue (0), iGreen (0);

			//go through all the columns
			for (int iX(0); iX < hBM->Width; ++iX)
			{
				//go through all the rows
				for (int iY (0); iY < hBM->Height; ++iY)
				{
					//grab a pixel from the buffer
					pixel = hBM->GetPixel (iX , iY);

					//adjust the contrast of the red pixel
					iRed = pixel.R + (rand () % (iValue * 2)) - iValue;
					//make sure the red value doesnt go over 255
					iRed = iRed > 255 ? 255 : iRed;
					//make sure the red value doesnt go below 0
					iRed = iRed < 0 ? 0 : iRed;

					//adjust the contrast of the blue pixel
					iBlue = pixel.B + (rand () % (iValue * 2)) - iValue;
					//make sure the blue value doesnt go over 255
					iBlue = iBlue > 255 ? 255 : iBlue;
					//make sure the blue value doesnt go below 0
					iBlue = iBlue < 0 ? 0 : iBlue;

					//adjust the contrast of the green pixel
					iGreen = pixel.G + (rand () % (iValue * 2)) - iValue;
					//make sure the green value doesnt go over 255
					iGreen = iGreen > 255 ? 255 : iGreen;
					//make sure the green value doesnt go below 0
					iGreen = iGreen < 0 ? 0 : iGreen;

					//the pixel is complete, put it back in the image
					hBM->SetPixel (iX, iY, Drawing::Color::FromArgb (iRed, 
																	 iGreen, 
																	 iBlue));
				}
				//update the progress bar
				this->progressBar->Value = iX;
			}
			//show the result!
			this->picture->Image = hBM;
		}
	}; // class Form1
} // namespace LAB1

