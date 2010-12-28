#include <iostream>
#include <iomanip>
#include <cmath>

const double kdPi = 3.14159265358979323846;
const double kdG = 9.81;

using namespace std;

int main ()
{
	double dDistance = 0.0;
	double dAngle = 0.0;
	double dVelocity = 0.0;
	double dAngleRad = 0.0;
	


	cout << "\t\tGeneral Gutz'n Blud's Atomic Cannon" << endl << endl;

	cout << "Enter angle of the cannon in degrees: ";
	cin >> dAngle;
	cout << "Enter the muzzle velocity in meters per second: ";
	cin >> dVelocity;

	dAngleRad = dAngle * (kdPi / 180);
	dDistance = (2 * dVelocity * dVelocity * cos (dAngleRad) * sin (dAngleRad)) / kdG;

	cout << endl;
	cout <<	"Using the angle of " << dAngle << " degrees, and a muzzle velocity of " << dVelocity << " meters/sec.";
	cout << endl << "The shot distance is " << setprecision (2) << fixed << dDistance << " meters\n";

	system("pause");
	return 0;
}
