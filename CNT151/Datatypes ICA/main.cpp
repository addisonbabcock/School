int main ( int argc, char** argv)
{
	bool bTrueBool = true;
	bool bFalseBool = false;
	int iInteger = 2147483647;
	double dDouble = 45.5678912345;
	char cCharacter = '@';
	float fFloat = 45.5678912345;

	unsigned int uiUInteger = 4294967295;
	unsigned char ucUCharacter = '@';

	unsigned int uiBoolSize = sizeof (bool);
	unsigned int uiIntegerSize = sizeof (int);
	unsigned int uiUnsignedIntegerSize = sizeof (unsigned int);
	unsigned int uiDoubleSize = sizeof (double);
	unsigned int uiFloatSize = sizeof (float);
	unsigned int uiCharacterSize = sizeof (char);
	unsigned int uiUnsignedCharacterSize = sizeof (unsigned char);

	return 0;
}
