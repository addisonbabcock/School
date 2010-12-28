#include <string>
#include <iostream>

#include "And.h"
#include "Nand.h"
#include "Or.h"
#include "Xor.h"

using namespace std;

int main ()
{
	// ICA  17 Test Code
	{ // ICA 17 test code
		// sample output :
		// given : cout << myGate;
		// output as follows : ( a truth table )
		// A  B   OR  << replace OR with appropriate name of Gate
		// 0  0   0
		// 0  1   1
		// 1  0   1
		// 1  1   1

		CNand A;
		CAnd B;
		COr C;
		CXor D;
		cout << A << B << C << D;

		// Code Circuit segment here:
		// REPLACE
		CGate * BGates [10] = {0};
		BGates [0] = new CAnd;
		BGates [1] = new COr;
		BGates [2] = new CNand;
		BGates [3] = new CXor;
		BGates [4] = new CNand;
		BGates [5] = new COr;
		BGates [6] = new CAnd;
		BGates [7] = new CXor;
		BGates [8] = new CAnd;
		BGates [9] = new COr;

		cout << "A B C D  Result\n";

		for (int inputA (0); inputA < 2; ++inputA)
		{
			for (int inputB (0); inputB < 2; ++inputB)
			{
				for (int inputC (0); inputC < 2; ++inputC)
				{
					for (int inputD (0); inputD < 2; ++inputD)
					{
						BGates [0]->Set (inputA, inputA);
						BGates [0]->Latch ();

						BGates [1]->Set (inputA, inputB);
						BGates [1]->Latch ();

						BGates [2]->Set (inputB, inputC);
						BGates [2]->Latch ();

						BGates [3]->Set (BGates [2]->Get (), inputC);
						BGates [3]->Latch ();

						BGates [4]->Set (BGates [0]->Get (), BGates [1]->Get ());
						BGates [4]->Latch ();

						BGates [5]->Set (BGates [0]->Get (), BGates [4]->Get ());
						BGates [5]->Latch ();

						BGates [6]->Set (BGates [1]->Get (), BGates [3]->Get ());
						BGates [6]->Latch ();

						BGates [7]->Set (BGates [3]->Get (), inputD);
						BGates [7]->Latch ();

						BGates [8]->Set (BGates [5]->Get (), BGates [6]->Get ());
						BGates [8]->Latch ();

						BGates [9]->Set (BGates [8]->Get (), BGates [7]->Get ());
						BGates [9]->Latch ();

						cout << inputA << " " << inputB << " " << inputC << " " << inputD 
							 << "  " <<  BGates [9]->Get () << endl;
					}
				}
			}
		}

		for (int i (0); i < 10; ++i)
			delete BGates [i];
	} // end ICA17 test code
	// end of ICA17 test code
	cin.get ();
	return 0;
}