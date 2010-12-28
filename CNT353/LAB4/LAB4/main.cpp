#include <crtdbg.h>
#include <iostream>
#include <sstream>
#include "Path.h"
//#include "e:\lab04_test.h"

using namespace std;

void Lab4TestAddison ()
{
	try
	{
		CPath path, myPath;
		
		path += 1.001;
		path.Show ();
		cin.get ();

		path += 798.598;
		path += 1.598;
		path += 798.001;
		myPath += path;
		myPath += path;

		CPath const YourPath (myPath);
		//YourPath.Show ();

		myPath = myPath = YourPath;

		if (myPath == YourPath)
			cout << "Good\n";

		if (myPath != YourPath)
			cout << "Bad\n";

		myPath += 5.5;

		if (myPath != YourPath)
			cout << "Good2\n";

		if (myPath == YourPath)
			cout << "Bad2\n";
		
		//path.Show ();
		myPath.Show ();
	}
	catch (string const & error)
	{
		cerr << error << endl;
	}
	cin.get ();
}

int main ()
{
	_CrtSetDbgFlag (_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
//	Lab04Test();
	Lab4TestAddison ();
	return 0;
}