#include <iostream>
#include <iomanip>
#include <cmath>

using namespace std;

void GetCoefficients (double* pdA, double* pbB, double* pdC);
bool CalculateRoots (double dA, double dB, double dC, double* dRoot1, double* dRoot2);

int main ()
{
	double	a = 0.0,
			b = 0.0,
			c = 0.0;

	double  dRoot1 = 0.0,
			dRoot2 = 0.0;

	cout << "Enter a quadratic equation in the form of ax^2 + bx + c" << endl << endl;

	GetCoefficients (&a, &b, &c);
	if (CalculateRoots (a, b, c, &dRoot1, &dRoot2))
	{
		cout << setprecision (2) << fixed;
		cout << endl << "Root one: " << dRoot1 << endl;
		cout << "Root two: " << dRoot2 << endl << endl;
	}
	else
	{
		cout << endl << "Sorry, the roots could not be calculated.\n\n";
	}

    system ("pause");
	return 0;
}



void GetCoefficients (double* pdA, double* pdB, double* pdC)
{
	double	a = 0.0,
			b = 0.0,
			c = 0.0;

	cout << "Enter a: ";
	cin >> a;
	
	cout << "Enter b: ";
	cin >> b;
	
	cout << "Enter c: ";
	cin >> c;

	*pdA = a;
	*pdB = b;
	*pdC = c;
}

bool CalculateRoots (double dA, double dB, double dC, double* dRoot1, double* dRoot2)
{
    double dDiscriminant = 0.0;

	dDiscriminant = (dB * dB) - (4 * dA * dC);

	if (dDiscriminant >= 0.0)
	{
		*dRoot1 = (-dB + sqrt(dDiscriminant)) / (2 * dA);
		*dRoot2 = (-dB - sqrt(dDiscriminant)) / (2 * dA);

		return true;
	}
	return false;
}