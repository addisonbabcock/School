#include <iostream>
#include "MailBox.h"

// Copy Constructor test function
void PassTest( CMailBox copy )
{
  copy.AddMsg("Four");
  cout << "Copy test:\n" ;
  copy.ShowMsg( cout );
  if( copy.AddMsg("Should be full"))
    cout << "Error on AddMsg(), should be full" << endl;
}

int main( int argc, char* argv[] )
{
  _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF | _CRTDBG_CHECK_ALWAYS_DF);
  { // ICA04 Test code
    CMailBox cBox;
    cBox.ShowMsg( cout ) << endl;
    cin.get();
    if( !cBox.AddMsg("Don't you know who I think I am?"))
      cout << "Error on AddMsg()" << endl;
    cBox.AddMsg("Testing, testing, one");
    cBox.AddMsg("Testing, testing, two");
    cBox.AddMsg("Testing, testing, three");
    cBox.ShowMsg( cout );
    cin.get();
    PassTest( cBox ); //test COPY CTOR via Pass-by-value
    cout << "Back to original\n";
    cBox.ShowMsg( cout );
		cin.get();
    CMailBox const cCopy( cBox ); // explicitly invoke copy ctor
    CMailBox const cOther( cCopy ); // once again based on first const copy, whatsit testing ?
    cOther.ShowMsg( cout );
    cin.get();
  }// end of ICA04 test code
  { // ICA05 Test code
    CMailBox cBox;
    cBox.PutMsg("Chain..").PutMsg("Chaining....").PutMsg("Chainingest..").ShowMsg(cout);
    cBox.Size( cout ) << " bytes\n";
    cin.get();
    CMailBox const cCopy( cBox ); // explicitly invoke copy ctor
    cCopy.Size(cout) << " bytes\n";  // Output should look like "[NN] bytes", use [] and NN = num bytes
    cCopy.ShowMsg( cout ) << "\nEnd of ICA05";
    cin.get();
  }// end of ICA05 test code

  // add to test code of ICA05
  { // ICA06 Test code
    // REPLACE THIS LINE WITH SCOPE RESOLVED GET_TOTAL_SIZE() TO cout
	cout << CMailBox::GetTotalSize () << endl;
    CMailBox cBox;
    cBox.PutMsg("Don't you know who I think I am?").PutMsg("Me").PutMsg("ICA05 - Message 3");
    // REPLACE THIS LINE WITH A CBOX RESOLVED GET_TOTAL_SIZE() TO cout
	cout << cBox.GetTotalSize () << endl;
    cBox.ShowMsg( cout );
    cin.get();
    CMailBox const cOne;
    // REPLACE THIS LINE WITH A CBOX RESOLVED GET_TOTAL_SIZE() TO cout
	cout << cBox.GetTotalSize () << endl;
    CMailBox const cThree( cBox ); // explicitly invoke copy ctor
    CMailBox const cFour( cBox ); // explicitly invoke copy ctor
    CMailBox const cFive( cBox ); // explicitly invoke copy ctor
    // REPLACE THIS LINE WITH A CFIVE RESOLVED GET_TOTAL_SIZE() TO cout
	cout << cFive.GetTotalSize () << endl;
    cFive.Size(cout) << "bytes\n";
    cFive.ShowMsg( cout );
    cin.get();
  }// end of ICA06 test code
  // REPLACE THIS LINE WITH SCOPE RESOLVED GET_TOTAL_SIZE() TO cout
  cout << CMailBox::GetTotalSize () << endl;
  cin.get();
  return 0;
}