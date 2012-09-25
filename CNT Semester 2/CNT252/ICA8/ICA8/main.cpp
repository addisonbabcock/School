#include <iostream>
#include "GetInput.h"

using namespace std;

int main (int argc, char** argv)
{
	int iTest (0);
	double dTest (0.0);

	//test the int overload

	GetInput (&iTest, "Enter a valid int:");
	cout << iTest << endl;
	GetInput (&iTest, "Enter a positive int :", 0);
	cout << iTest << endl;
	GetInput (&iTest, "Enter an int greater than or equal to 50:", 50);
	cout << iTest << endl;
	GetInput (&iTest, "Enter an int between 50 and 60:", 50, 60);

	//test the double overload

	GetInput (&dTest, "Enter a double:");
	cout << dTest << endl;
	GetInput (&dTest, "Enter a positive double:", 0.00);
	cout << dTest << endl;
	GetInput (&dTest, "Enter a double greater or equal to 50.0:", 50.0);
	cout << dTest << endl;
	GetInput (&dTest, "Enter a double between 50.0 and 60.0:", 50.0, 60.0);
	cout << dTest << endl;

	return 0;
}