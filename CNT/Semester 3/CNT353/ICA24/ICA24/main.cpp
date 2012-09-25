#include <iostream>
#include <string>
#include "Stack.h"
#include "info.h"

using namespace std;

int main ()
{
	{ // Test Code here, duplicate for int and string tests.
		TStack<int> stk(3); // you will change this to TStack<> stk(3) or whatever type required
		info( cout, stk );  // call info with TStack object
		stk.Push(1).Push(2).Push(3); // Push some values on
		cout << stk;
		// Sample output of above: format [element] : value, newest to oldest
		//[2] : 3
		//[1] : 2
		//[0] : 1
		TStack<int> stk2( stk ); // TStack<>
		while( stk2.Size() )
			cout << '[' << stk2.Pop() << ']'; // pop them off and display them
		cout << endl;
		stk2 = stk = stk;  // assignment test
		cout << stk2 << endl; 
		cin.get();
	}
	{ // Test Code here, duplicate for int and string tests.
		TStack<double> stk(3); // you will change this to TStack<> stk(3) or whatever type required
		info( cout, stk );  // call info with TStack object
		stk.Push(1).Push(2).Push(3); // Push some values on
		cout << stk;
		// Sample output of above: format [element] : value, newest to oldest
		//[2] : 3
		//[1] : 2
		//[0] : 1
		TStack<double> stk2( stk ); // TStack<>
		while( stk2.Size() )
			cout << '[' << stk2.Pop() << ']'; // pop them off and display them
		cout << endl;
		stk2 = stk = stk;  // assignment test
		cout << stk2 << endl; 
		cin.get();
	}
	{ // Test Code here, duplicate for int and string tests.
		TStack<string> stk(3); // you will change this to TStack<> stk(3) or whatever type required
		info( cout, stk );  // call info with TStack object
		stk.Push("1").Push("2").Push("3"); // Push some values on
		cout << stk;
		// Sample output of above: format [element] : value, newest to oldest
		//[2] : 3
		//[1] : 2
		//[0] : 1
		TStack<string> stk2( stk ); // TStack<>
		while( stk2.Size() )
			cout << '[' << stk2.Pop() << ']'; // pop them off and display them
		cout << endl;
		stk2 = stk = stk;  // assignment test
		cout << stk2 << endl; 
		cin.get();
	}
	return 0;
}