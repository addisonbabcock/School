#include <iostream>
using namespace std;

int main ()
{
	int iTotalEggs = 0;
	int iPackages = 0;
	int iSingleEggs = 0;
	
	double dPricePerDozen = 0.0;		//in dollars
	double dPricePerEgg = 0.0;
	double dTotalCost = 0.0;
	const double kdSingleEggPremium = 1.20;
	
	cout << "               The Bad Egg Company" << endl << endl;	//display welcome, get input
	cout << "How many eggs to purchase: ";
	cin >> iTotalEggs;
	cout << "Price per package $";
	cin >> dPricePerDozen;

	iPackages = (static_cast<int>(iTotalEggs)) / 12;	//truncated
	iSingleEggs = iTotalEggs % 12;
	dPricePerEgg = (dPricePerDozen / 12) * kdSingleEggPremium;
	dTotalCost = (static_cast<double>(iPackages) * dPricePerDozen) + (static_cast<double>(iSingleEggs) * dPricePerEgg);

	cout << "You will require " << iPackages << " packages and " << iSingleEggs 
		 << " single eggs to complete your order." << endl;
	cout << "The price per single egg will be $" << dPricePerEgg << endl;
	cout << "The total purchase price is $" << dTotalCost << endl;

	system ("pause");

	return 0;
}