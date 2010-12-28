#include "Dist.h"

int main ()
{// ICA 09 Test Code - add to main, no other code required
	{// ica09 test code
		CDist const cA(5,-9);
		CDist const cB(-2,7);
		CDist const cC(0,0);
		CDist C(0,0), D(0,0), E(0,0);

		// The following tests your declarations by using functional notation, rather than infix,
		//   if your overloaded operators are not defined correctly ( ie, member, global ) these
		//   won't compile
		C = ( C.operator+(cC) ); // + is obviously a member
		C = operator+=( C, cC ); // += is a global here, this cannot test friendship
		C = C.operator-=(cC);    // -= is obviously a member
		C = operator-( C, cC );  // - is a global here, this cannot test friendship
		// end of compile test

		cout << cA << endl; // quick test of global display
		cout << cB << endl;
		cout << C << endl;
		// + test
		D = cA + cB;
		cout << "D = " << D << endl;
		cout << "( cA + cB ) = " << ( cA + cB ) << endl;
		// += test
		C = cC;
		D = cB;
		D += cA;
		cout << D << endl;
		C = ( C += cA ) + cA + cB;;
		cout << C << endl;

		// - test
		D = cA - cB;
		cout << "D = " << D << endl;
		cout << "( cA - cB ) = " << ( cA + cB ) << endl;
		// -= test
		C = cC;
		D = cB;
		D -= cA;
		cout << D << endl;
		C = ( C -= cA ) - cA - cB;;
		cout << C << endl;
		// Supertest
		D = C = cA;
		D += ( D + ((C += cA) + cA + cB)) - ( E -= (( cB - cA ) - cA) - cA );
		cout << "C:" << C << " D:" << D << " E:" << E << endl;
		cin.get();
	}
	return 0;
}