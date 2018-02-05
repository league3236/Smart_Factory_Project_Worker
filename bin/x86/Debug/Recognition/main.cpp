#include "Controller.h"
#define _WIN32_WINNT 0x0500 
#include<Windows.h>

int main(int argc, char* argv[]) {
	/*HWND hWnd = GetConsoleWindow();
	ShowWindow(hWnd, SW_HIDE);*/
	

	system("dir");

	Controller().DoDetect();
	
	return 0;
}

