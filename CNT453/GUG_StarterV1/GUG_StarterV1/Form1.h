#pragma once

namespace GUG_StarterV1 {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	// added namespaces to save fingers
	using namespace Microsoft::DirectX::Direct3D;
	using namespace Microsoft::DirectX;

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
		value struct SKeys
		{
			bool m_bW;
			bool m_bA;
			bool m_bS;
			bool m_bD;
		} sKeys;

		int iGridSize;
		Mesh ^ Cube;
	public:
		Form1(void)
		{
			InitializeComponent();

			// device handle
			_d3ddev = nullptr;

			// starting cam pos
			_fCamZPos = -4.0f, _fCamYPos = 2.0f, _fCamXPos = 0;

		}

	private:
		// direct 3d device
		Device ^ _d3ddev;

		// cam x/y/z position
		float _fCamZPos, _fCamYPos, _fCamXPos;

		//light blinking state
		int _iLightState;

		// this code is placed into a function, since the code will be called for initial setup
		//  and if the device is reset.
		void _SetRenderStateStandard (void)
		{
			// turn on some ambient light
			_d3ddev->RenderState->Ambient = Color::FromArgb (0, 0, 32);

			// describe flex. vertex format that this app intends to use
			_d3ddev->VertexFormat = CustomVertex::PositionNormal::Format;

			// turn on a z-buffer
			_d3ddev->RenderState->ZBufferEnable = true;

			//_d3ddev->RenderState->CullMode = Direct3D::Cull::None;
		}

		// this handler is called when the device is reset (window resized, etc...)
		void DevResetHandler (System::Object ^ hDev, System::EventArgs ^ hArgs)
		{
			// device has been reset, so render state lost, reset
			this->_SetRenderStateStandard ();

			// restart the rendering timer
			this->UI_TIM_RENDER->Enabled = true;
		}

		// this handler is called when the device is lost (three finger salute, etc...)
		void DevLostHandler (System::Object ^ hDev, System::EventArgs ^ hArgs)
		{
			// device has been lost, it could be temporary or permanent
			Device ^ hdev = (Device ^)(hDev);

			// stop the rendering timer
			this->UI_TIM_RENDER->Enabled = false;

			// check to see if the loss is unrecoverable
			// this code will cause an exit if so, the device could be restored
			//  with some more advanced code, but that is beyond starter stuff
			if (false == hdev->CheckCooperativeLevel ())
				Application::Exit ();
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

	private: System::Windows::Forms::Timer^  UI_TIM_RENDER;
	private: System::ComponentModel::IContainer^  components;

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>


#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->components = (gcnew System::ComponentModel::Container());
			this->UI_TIM_RENDER = (gcnew System::Windows::Forms::Timer(this->components));
			this->SuspendLayout();
			// 
			// UI_TIM_RENDER
			// 
			this->UI_TIM_RENDER->Interval = 20;
			this->UI_TIM_RENDER->Tick += gcnew System::EventHandler(this, &Form1::UI_TIM_RENDER_Tick);
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(800, 600);
			this->Name = L"Form1";
			this->Text = L"GUG Starter Application V1.1";
			this->Shown += gcnew System::EventHandler(this, &Form1::Form1_Shown);
			this->KeyUp += gcnew System::Windows::Forms::KeyEventHandler(this, &Form1::Form1_KeyUp);
			this->KeyDown += gcnew System::Windows::Forms::KeyEventHandler(this, &Form1::Form1_KeyDown);
			this->ResumeLayout(false);

		}
#pragma endregion
	private:
		System::Void UI_TIM_RENDER_Tick(System::Object^  sender, System::EventArgs^  e)
		{
			// rendering timer has gone off, so render the scene

			// ensure that device is there
			if (nullptr != this->_d3ddev)
			{
				// frame to frame rotation changes via this angle
				static float fYAngle (0.0f);
				static float fXAngle (0.0f);

				if (sKeys.m_bD)
					fYAngle += 0.05f;

				if (sKeys.m_bA)
					fYAngle -= 0.05f;

				if (sKeys.m_bW)
					fXAngle += 0.05f;

				if (sKeys.m_bS)
					fXAngle -= 0.05f;

				// start the scene
				_d3ddev->Clear (ClearFlags::Target | ClearFlags::ZBuffer, Color::Black, 1.0f, 0);
				_d3ddev->BeginScene ();

				Material mWhite;
				mWhite.Ambient = Color::Blue;
				mWhite.Diffuse = Color::FromArgb (100, 100, 150);
				_d3ddev->Material = mWhite;

				// setup a matrix for rotation/translation
				Matrix matrix;
//				matrix.Scale (0.1f, 0.1f, 0.1f);

				for (float x = -iGridSize * 0.25f; x <= iGridSize * 0.25f; x += 0.5f)
				{
					for (float y = -iGridSize * 0.25f; y <= iGridSize * 0.25f; y += 0.5f)
					{
						for (float z = -iGridSize * 0.25f; z <= iGridSize * 0.25f; z += 0.5f)
						{
							matrix = Matrix::Scaling (0.08f, 0.08f, 0.08f) * 
								Matrix::Translation (x, y, z) *
								Matrix::RotationY (fYAngle) *
								Matrix::RotationX (fXAngle);
							_d3ddev->SetTransform (TransformType::World, matrix);
							Cube->DrawSubset (0);
						}
					}
				}
				
				// reset world
				_d3ddev->SetTransform (TransformType::World, Matrix::Identity);

				bool lightsOn = true;

				//switch (_iLightState)
				//{
				//case 0:
				//case 1:
				//	lightsOn = true;
				//	break;
				//case 2:
				//case 3:
				//case 4:
				//	lightsOn = false;
				//	break;
				//case 5:
				//case 6:
				//case 7:
				//	lightsOn = true;
				//	break;
				//case 8:
				//case 9:
				//case 10:
				//case 11:
				//case 12:
				//case 13:
				//case 14:
				//case 15:
				//	lightsOn = false;
				//	break;
				//default:
				//	lightsOn = false;
				//	_iLightState = 0;
				//}
				//_iLightState++;

				_d3ddev->Lights[0]->Type = LightType::Directional;
				_d3ddev->Lights[0]->Diffuse = Color::Red;
				_d3ddev->Lights[0]->Direction = Vector3 (1, 0, 0); // down and back and to the right
				_d3ddev->Lights[0]->Direction.Normalize ();
				_d3ddev->Lights[0]->Update ();
				_d3ddev->Lights[0]->Enabled = lightsOn;

				_d3ddev->Lights[1]->Type = LightType::Directional;
				_d3ddev->Lights[1]->Diffuse = Color::Green;
				_d3ddev->Lights[1]->Direction = Vector3 (-1, 0, 0);
				_d3ddev->Lights[1]->Direction.Normalize ();
				_d3ddev->Lights[1]->Update ();
				_d3ddev->Lights[1]->Enabled = lightsOn;

				_d3ddev->Lights[2]->Type = LightType::Directional;
				_d3ddev->Lights[2]->Diffuse = Color::Blue;
				_d3ddev->Lights[2]->Direction = Vector3 (0, -1, 0);
				_d3ddev->Lights[2]->Direction.Normalize ();
				_d3ddev->Lights[2]->Update ();
				_d3ddev->Lights[2]->Enabled = lightsOn;

				/////////////////////////////////////////////////////////////////////////////////////////////
				//					 view/projection transforms
				/////////////////////////////////////////////////////////////////////////////////////////////
				Microsoft::DirectX::Matrix matView;
				matView = Matrix::LookAtLH (
					Vector3 (this->_fCamXPos, this->_fCamYPos, this->_fCamZPos), // eye
					Vector3 (0, 0, 0),		// at
					Vector3 (0, 1, 0));	// up
				_d3ddev->SetTransform (TransformType::View, matView);

				Microsoft::DirectX::Matrix matProj;
				matProj = Matrix::PerspectiveFovLH (
					float (Math::PI / 4),					// standard perspective
					float (this->Width) / (this->Height),	// aspect ratio
					0.1f,									// near clip
					1000.0f);								// far clip
				_d3ddev->SetTransform (TransformType::Projection, matProj);

				// end scene
				_d3ddev->EndScene ();

				try
				{
					_d3ddev->Present ();
				}
				catch (DeviceLostException ^ e)
				{
					// could not present due to a lost surface, prob., lost hander will take it from here
					Text = e->Message;
				}
				catch (...)
				{
					// something really bad happened?
				}
			}
		}

		System::Void Form1_Shown(System::Object^  sender, System::EventArgs^  e)
		{
			// this method is called when the form is first displayed (should be only once)

			// enforce device creation only once
			static bool bShownAlready (false);
			if (!bShownAlready)
			{
				bShownAlready = true;

				// try to get a Direct3D device
				try
				{
					PresentParameters ^ pparms = gcnew PresentParameters ();
					pparms->Windowed = true;
					pparms->SwapEffect = SwapEffect::Discard;
					pparms->BackBufferCount = 1;
					pparms->EnableAutoDepthStencil = true;
					pparms->AutoDepthStencilFormat = DepthFormat::D16;
					pparms->BackBufferFormat = Format::Unknown;

					// if you need to render to the backbuffer
					pparms->PresentFlag = PresentFlag::LockableBackBuffer;

					// create the D3D device
					_d3ddev = gcnew Device (
						0,
						DeviceType::Hardware,	// hope you can get this...
						this,
						CreateFlags::MixedVertexProcessing | CreateFlags::FpuPreserve,
						pparms);

					// set standard render state (vertex format, ambient light, etc...)
					_SetRenderStateStandard ();

					// add event handlers for a lost/reset device
					this->_d3ddev->DeviceLost += gcnew System::EventHandler (this, &Form1::DevLostHandler);
					this->_d3ddev->DeviceReset += gcnew System::EventHandler (this, &Form1::DevResetHandler);

					try
					{
						Cube = Mesh::FromFile ("untitled.x",
							MeshFlags::Dynamic, _d3ddev);
					}
					catch (Direct3DXException ^ e)
					{
						Diagnostics::Trace::WriteLine (e->ErrorString);
					}

					// start the rendering timer
					this->UI_TIM_RENDER->Start ();				 
				}
				catch (...)
				{
					this->Text = "Unable to create the Direct3D device!...";
				}
			}
		}
		System::Void Form1_KeyDown(System::Object^  sender, System::Windows::Forms::KeyEventArgs^  e) 
		{
			switch (e->KeyCode)
			{
			case Windows::Forms::Keys::W :
				sKeys.m_bW = true;
				break;
			case Windows::Forms::Keys::A :
				sKeys.m_bA = true;
				break;
			case Windows::Forms::Keys::S :
				sKeys.m_bS = true;
				break;
			case Windows::Forms::Keys::D :
				sKeys.m_bD = true;
				break;
			case Windows::Forms::Keys::Add :
				if (iGridSize < 10)
					++iGridSize;
				break;
			case Windows::Forms::Keys::Subtract :
				if (iGridSize > 0)
					--iGridSize;
				break;
			default: break;
			}
		}
		System::Void Form1_KeyUp(System::Object^  sender, System::Windows::Forms::KeyEventArgs^  e) 
		{
			switch (e->KeyCode)
			{
			case Windows::Forms::Keys::W :
				sKeys.m_bW = false;
				break;
			case Windows::Forms::Keys::A :
				sKeys.m_bA = false;
				break;
			case Windows::Forms::Keys::S :
				sKeys.m_bS = false;
				break;
			case Windows::Forms::Keys::D :
				sKeys.m_bD = false;
				break;
			default: break;
			}
		}
	};
}

